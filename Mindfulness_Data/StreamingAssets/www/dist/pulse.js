import {HourGlass} from './components/hourglass.js';
import *  as theme from '../config/theme.json';
const time_limit = 60000; //{number} - トレーニングの制限時間
const is_debug = true; //{boolean} - 動作テスト用モードにするか
/**
 * 心拍BF
 * Libries: p5
 * @property {HourGlass} hourglass - 砂時計
 * @property {Array<p5.Image>} images - 花の画像配列
 * @property {p5.Image} ballImage - 心拍ボールの画像
 * @property {Ground} ground - 画面下部のOKバー
 * @property {HeartbeatBall} heartbeatBall - 心拍ボール
 */
class Pulse {

  constructor(p5) {
    this.$p5 = p5;
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });

    this.$p5.theme = theme;
    this.hourGlass = new HourGlass(this.$p5, time_limit, {isReturnEnable:false, isChangeColor:false});

    this.currentHR = 100;　// テスト用

  }

  /* [p5] preload処理 */
  preload() {
    if (this.$p5.theme.font)
      this.font = this.$p5.loadFont(this.$p5.theme.font.path);

    this.images = [];
    this.$p5.theme.pulse.images.forEach((v, i, a) => {
      this.images.push(this.$p5.loadImage(v));
    });
    this.ballImage = this.$p5.loadImage(this.$p5.theme.pulse.ballImage);
  }

  /* [p5] setup処理 */
  setup() {
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);
    this.$p5.background(this.$p5.theme.color.background);
    this.$p5.frameRate(30);
    this.$p5.noLoop();
    this.$p5.isReady = true;
    if (this.$p5.theme.font)
      this.$p5.textFont(this.font);

    this.ground = new Ground(this.$p5, this.images);
    this.heartbeatBall = new HeartbeatBall(this.$p5, this.ballImage);

    this.$p5.imageMode(this.$p5.CENTER);

    this.ready();
  }

  /* [p5] スタートボタン押下 */
  start() {
    this.hourGlass.start();
    this.vibe();

    this.ground.flowers.forEach((v, i, a) => {
      v.images.forEach((v, i, a) => {
        v.resize(this.$p5.width / 5, 0)
      });
    });
  }

  /**
   * 現在の心拍に応じて定期的にボールを弾ませます
   */
  vibe() {
    // this.heartbeatBall.beatValue = this.getCurrentHR();
    this.heartbeatBall.setBeat();
    this.beatTimer = setTimeout(() => {
      this.vibe();
    }, 1000 / (this.getCurrentHR() / 60));
  }

  /**
   * 動作テスト用
   * 現在の心拍数を取得します
   */
  getCurrentHR() {
    return this.currentHR;
  }

  /* [p5] draw処理 */
  draw() {
    this.$p5.noCanvas();
    this.$p5.createCanvas(this.$p5.windowWidth,this.$p5.windowHeight);
    if (this.hourGlass)
      this.hourGlass.renderGlass();

    /* テスト用 */
    if (
      this.$p5.keyIsDown(this.$p5.UP_ARROW)
      && this.heartbeatBall.basePosition.y > this.$p5.height*(10/100)
    ) {
      this.currentHR += 1;
    }
    if (
      this.$p5.keyIsDown(this.$p5.DOWN_ARROW)
      && this.heartbeatBall.basePosition.y < this.$p5.height*(90/100)
    ) {
      this.currentHR -= 1;
    }
    if (this.$p5.keyIsDown(this.$p5.LEFT_ARROW)) {
      this.heartbeatBall.isUseImage = true;
    }
    if (this.$p5.keyIsDown(this.$p5.RIGHT_ARROW)) {
      this.heartbeatBall.isUseImage = false;
    }
    /* テスト用ここまで */

    /* スプライトの描画 */
    this.heartbeatBall.beat(this.getCurrentHR());
    this.$p5.drawSprite(this.heartbeatBall.sprite);
    this.$p5.drawSprite(this.ground.base);

    /* アニメーション管理 */
    if (this.heartbeatBall.sprite.collide(this.ground.base)) {
      this.ground.flowers[this.ground.targetIndex].goToFrame(this.ground.flowers[this.ground.targetIndex].getLastFrame());
    } else {
      this.ground.flowers[this.ground.targetIndex].goToFrame(0);
    }

    if (
      this.ground.flowers[this.ground.targetIndex].getFrame() === this.ground.flowers[this.ground.targetIndex].getLastFrame()
      && this.ground.targetIndex < 5
    ) {
      this.ground.targetIndex += 1;
    } else if (
      this.ground.flowers[this.ground.targetIndex].getFrame() === 0
      && this.ground.targetIndex > 0
    ) {
      this.ground.targetIndex -= 1;
    }

    [1.5, 3.5, 1, 3, 4, 2].forEach((v, i, a) => {
      this.$p5.animation(this.ground.flowers[i], this.$p5.width * (v/4) - this.$p5.width/7.5, this.$p5.height*(85/100)-this.$p5.width/10);
    });
    /* アニメここまで */

    /* 終了処理 */
    if (!this.hourGlass.isOver)
      return;
    this.$p5.noLoop();
    clearInterval(this.beatTimer);
    this.$p5.noCanvas();
    this.$p5.remove();

    this.stop();
  }
}

/**
 * 心拍ボール
 * @property {number} size - ボールの大きさ
 * @property {p5} p
 * @property {number} beatVal - ベースポジションから弾む距離
 * @property {number} perBeatVal - 弾むボールが移動する1フレームごとの距離
 * @property {number} baseHR - 開始時の心拍
 * @property {Object} basePosition - ボールの基準位置
 * @property {p5.Sprite} sprite - ボールのスプライト
 */
class HeartbeatBall {
  constructor(p, image = null) {
    this.size = p.height /10;
    this.p5 = p;
    this.beatVal = 0;
    this.perBeatVal = 0;
    this.baseHR = 100; //TODO: 初期心拍を取得
    this.basePosition = {
      x: p.width / 2,
      y: p.height * (3 / 10)
    };

    /* テスト用 */
    if (is_debug) {
      this.isUseImage = false;
    }
    /* テスト用ここまで */

    this.sprite = p.createSprite(this.basePosition.x, this.basePosition.y, this.size, this.size);
    this.sprite.draw = () => {

      /* テスト用 */
      if (is_debug) {
        if (this.isUseImage) {
          p.image(
            image,
            0,
            0,
            this.size,
            this.size
          );
          return;
        }
        p.fill("#87CEEB");
        p.noStroke();
        p.ellipse(0, 0, this.size, this.size);
        return;
      }
      /* テスト用ここまで */

      if (image) {
        p.image(
          image,
          0,
          0,
          this.size,
          this.size
        );
        return;
      }
      p.fill("#87CEEB");
      p.noStroke();
      p.ellipse(0, 0, this.size, this.size);
    }

  }

  /* 振動幅を設定 */
  setBeat() {
    this.beatVal = 1-(this.basePosition.y/this.p5.height);
    this.perBeatVal = this.beatVal/10;
  }
  // set beatValue(currentHR) {
  //   this.beatVal = currentHR;
  //   this.perBeatVal = currentHR / 10;
  // }

  /* ボールの位置を更新 */
  beat(currentHR) {
    console.log(currentHR);
    // this.basePosition.y = this.p5.height * (3 /10) - this.p5.height * ( ((currentHR - this.baseHR)/this.baseHR)*this.p5.height*(2/100) / 10);
    let y = this.p5.height*(3/10) - ((currentHR-this.baseHR)/this.baseHR * 5) * (this.p5.height/2);
    this.basePosition.y = this.p5.max(this.p5.height*(10/100), this.p5.min(y, this.p5.height*(90/100)));
    if (this.beatVal > 0) {
      this.beatVal -= this.perBeatVal;
    }
    // this.sprite.position.y = this.basePosition.y - ((this.p5.height*(5/100)) * (this.beatVal/this.baseHR));
    this.sprite.position.y = this.basePosition.y - (this.p5.height*(2.5/100) * this.beatVal);
  }

}

/**
 * OKライン
 * @property {p5.Sprite} base - OKラインのスプライト
 * @property {number} targetIndex - アニメーション対象のインデックス
 * @property {Array<p5.Animation>} flowers - 花ごとのアニメーション配列
 */
class Ground {
  constructor(p, images) {
    this.base = p.createSprite(p.windowWidth / 2, p.windowHeight * (92.5 / 100), p.windowWidth, p.windowHeight * (15 / 100));
    this.base.shapeColor = p.color("#87CEEB");

    this.targetIndex = 0;
    this.flowers = [];
    [1, 2, 3, 4, 5, 6].forEach((v, i, a) => {
      let flower = p.loadAnimation(...images);
      flower.looping = false;
      flower.stop();
      this.flowers.push(flower);
    });
  }
}


export class PulseGuide {
  async start() {
    new p5(($p5) => {
      this.training = new Pulse($p5);
      $p5.preload = this.training.preload.bind(this.training);
      $p5.setup = this.training.setup.bind(this.training);
      $p5.draw = this.training.draw.bind(this.training);
    }, "content");
    await this.training.waitingForReady;

    this.training.start();

    await this.training.waitingForStopped;
  }
}
