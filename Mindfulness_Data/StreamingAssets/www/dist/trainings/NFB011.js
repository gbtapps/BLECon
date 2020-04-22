import { Training } from './training.js';
import { TimingScorer } from '../components/timingscorer.js';
import { HourGlass } from '../components/hourglass.js';
import { WrongSound } from '../components/sounds/wrongsound.js';
import { CorrectSound } from '../components/sounds/correctsound.js';
import { CircleCircle } from '../components/circlecircle.js';
import { Pushable } from '../components/pushable.js';
import *  as theme from '../../config/theme.json';

const time_limit = 60000; //{number} - トレーニングの制限時間
const is_debug = false; //{boolean} - デバッグモードで実行するかどうか

/**
 * [トレーニングNFB011] タイミングあわせ２
 * Libries: p5, p5sound, p5play
 * @property {Array} circleArray - サークルのスプライトを格納した配列
 * @property {Pushable} pushable - 顔のスプライト
 * @property {p5.Image} image - 顔の画像
 * @property {TimingScorer} circleTimingScorer - 円用のタイミングスコアラー
 * @property {TimingScorer} faceTimingScorer - 顔用のタイミングスコアラー
 * @property {HourGlass} hourGlass - 砂時計
 * @property {CorrectSound} correctSound - 成功音
 * @property {WrongSound} wrongSound - 失敗音
 * @property {boolean} mousePress - マウスが押されているかどうか
 * @property {number} maxWidth - 描画対象とする横幅
 * @property {number} numCircle - 表示するサークルの数
 * @property {number} avefps - 平均実FPS
 * @property {number} overTimerArray - 赤円と青円が重なってから1秒後に呼ばれるsetTimeout処理のIDを管理する配列
 * @property {number} fpsMeasurer - 直近2秒間の平均FPSを計算するsetInterval処理のID
 * @property {number} textSize - 文字サイズ
 * @property {Object} centerY - 各サークルのy座標
 */

class AdjustTiming2 {
  constructor(p5, preference={}) {
    this.$p5 = p5;
    this.$p5.theme = theme;
    this.level = preference ? preference.level : parseInt(p5.theme.NFB011.level);
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    this.circleArray = [];
    this.mousePress = false;
    this.maxWidth = (p5.windowWidth < p5.windowHeight)
      ? p5.windowWidth
      : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.avefps = 20;
    this.overTimerArray = [];
    this.textSize = this.maxWidth / 15;
    this.center = {
      circle1: {x: p5.windowWidth *(1/3), y:p5.windowHeight * 2/3},
      circle2: {x: p5.windowWidth * (2/3), y:p5.windowHeight * 2/3}
    };
  }
  /* [p5] preload処理 */
  preload() {
    if (this.$p5.theme.font)
      this.font = this.$p5.loadFont(this.$p5.theme.font.path);
    this.pushImage = this.$p5.loadImage('./assets/image/blackCircle.svg');
  }

  /* [p5] setup処理 */
  setup() {
    if (this.$p5.theme.NFB011.isUseImage) {
      this.image = this.$p5.loadImage(this.$p5.theme.NFB011.imagePath);
    } else {
      this.image = this.$p5.theme.NFB011.iconfont;
    }
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);
    this.$p5.background(255, 0);
    this.$p5.frameRate(20);
    this.$p5.noLoop();

    this.hourGlass = new HourGlass(this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.correctSound = new CorrectSound(this.$p5);
    this.wrongSound = new WrongSound(this.$p5);
    this.circleTimingScorer = new TimingScorer();
    this.faceTimingScorer = new TimingScorer();

    /* サークルを生成 */
    let size = (this.$p5.height / 4) > (this.maxWidth * (9 / 10))
      ? maxWidth * (2 / 3)
      : this.$p5.height / 2;
    this.circleArray.push(new CircleCircle(this.$p5, size, {x: this.center.circle1.x, y: this.center.circle1.y}, this.level == 1));
    this.pushable = new Pushable(this.$p5, this.image, size, true, this.pushImage);
    if (this.level == 2) {
      this.circleArray.push(new CircleCircle(this.$p5, size, {x: this.center.circle2.x, y: this.center.circle2.y}));
    }

    this.message = this.$p5.createDiv("")
      .style("position", "fixed")
      .style("font-size", this.textSize + 'px')
      .style('width', '100vw')
      .style("color", this.$p5.theme.color.primaryText)
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '7vh')
      .html("赤と青が重なった瞬間にタッチします<br>顔が出たらタッチします")
    ;
    window.App.state = 'ready';
    this.ready();
  }

  mousePressed() {
    return false;
  }

  /* [p5]スタートボタン押下 */
  start() {
    this.hourGlass.start();
    /* 直近2秒間の平均実FPSを設定 */
    let fpsArray = [];
    this.fpsMeasurer = setInterval(() => {
      fpsArray.push(this.$p5.frameRate());
      if (fpsArray.length > 10) fpsArray.shift();
      this.avefps = fpsArray.reduce((pre, curr, i) => {
        return pre + curr;
      }, 0) / fpsArray.length;
    }, 200);

    setTimeout(() => {
      this.showPushable();
    }, this.randomTime);
  }

  /**
   * 次にpushableを表示させるタイミングを決めるための数値
   * @returns 2000~5000のランダムな整数値
   */
  get randomTime() {
    return this.$p5.random(2000, 5000);
  }

  /**
   * 呼ばれてから1秒経つと消えるマークを表示
   * 消えてから数秒後に自身を呼び出す
   */
  showPushable() {
    if (this.circleArray.length == 2 && Math.random() > 0.5) { //どちらのサークルに表示させるか
      this.pushable.show({
        x: this.center.circle2.x,
        y: this.center.circle2.y
      });
    } else {
      this.pushable.show({
        x: this.center.circle1.x,
        y: this.center.circle1.y
      });
    }
    this.overTimerArray.push(setTimeout(() => {
      if (this.pushable.sprite.visible == true) {
        this.faceTimingScorer.isCorrects.push(false);
        this.faceTimingScorer.responseTimes.push(-1);
        this.pushable.hidden();
        this.wrongSound.playSound();
        if (App.isAndroid && this.$p5.theme.option.vibe==="on") Android.vibeWrong();
        this.faceTimingScorer.penalty();
      }
      this.overTimerArray.push(setTimeout(() => {
        this.showPushable();
      },this.randomTime));
    }, 1000));
  }

  /* [p5,p5play] draw処理 */
  draw() {
    this.$p5.noCanvas();
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);

    this.$p5.textAlign(this.$p5.CENTER);
    this.$p5.textSize(this.textSize);
    this.$p5.textStyle(this.$p5.BOLD);
    if (this.$p5.theme.font)
      this.$p5.textFont(this.font);


    if (this.hourGlass) this.hourGlass.renderGlass();
    /* 赤点を移動させる */
    this.circleArray.forEach((v, i, a) => {
      v.movePoint(this.avefps);
      /* 角度が90度となった場合、次に青円と赤円が重なる時間を計算して保持させる */
      if (v.difRadian < 1.5708 && v.difRadian > 0.785398 && v.overrapTime < this.$p5.millis()){
        v.overrapTime = this.$p5.millis() + (v.difRadian / v.speed / this.avefps * 1000);
        /* 重なってから1秒間押されなかった場合、反応時間に0.5秒のペナルティ */
        this.overTimerArray.push(setTimeout(() => {
          if (v.overrapTime != 0) {
            this.wrongSound.playSound();
            this.circleTimingScorer.isCorrects.push(false);
            this.circleTimingScorer.responseTimes.push(-1);
            this.circleTimingScorer.penalty();
            if (App.isAndroid && this.$p5.theme.option.vibe==="on") Android.vibeWrong();
            v.overrapTime = 0;
          }
        }, v.overrapTime - this.$p5.millis() + 1000));
      }
    });

    /* 顔か青円が押されたとき */
    if (this.$p5.mouseIsPressed && this.mousePress == false) {
      this.mousePress = true;
      if (this.pushable.sprite.visible && this.pushable.sprite.mouseIsPressed) {
        this.pushFace(this.pushable);
      } else if (this.circleArray[0].bluePoint.mouseIsPressed) {
        this.pushBlue(this.circleArray[0]);
      } else if (this.circleArray.length == 2 && this.circleArray[1].bluePoint.mouseIsPressed) {
        this.pushBlue(this.circleArray[1]);
      } else if (
        this.circleArray[0].insideCircle.mouseIsPressed
        || (this.circleArray.length == 2 && this.circleArray[1].insideCircle.mouseIsPressed)
      ) {
        this.faceTimingScorer.responseTimes.push(-1);
        this.faceTimingScorer.isCorrects.push(false);
        this.faceTimingScorer.penalty();
        this.wrongSound.playSound();
        if (App.isAndroid && this.$p5.theme.option.vibe==="on") Android.vibeWrong();
      }
    }

    if (!this.$p5.mouseIsPressed) this.mousePress = false;

    // this.$p5.drawSprites();
    this.circleArray.forEach((v, i, a)=>{
      v.drawCircle();
    })

    this.pushable.drawEffect();

    if (this.$p5.theme.NFB011.isUseImage) {
      this.pushable.drawImage();
    } else {
      this.pushable.drawIconfont();
    }

    if (!this.hourGlass.isOver) return;
    /* 終了処理 */
    this.correctSound.stopSound();
    this.wrongSound.stopSound();
    this.$p5.noLoop();
    this.overTimerArray.forEach((v, i, a) => {
      clearTimeout(v);
    })
    clearTimeout(this.fpsMeasurer);
    if (this.circleTimingScorer.pushCount == 0) {
      this.circleTimingScorer.addReactionTime(1000);
    }
    if (this.faceTimingScorer.pushCount == 0) {
      this.faceTimingScorer.addReactionTime(1000);
    }
    this.circleTimingScorer.score = this.circleTimingScorer.result;
    this.faceTimingScorer.score = this.faceTimingScorer.result2;

    const score = Math.floor((this.circleTimingScorer.score + this.faceTimingScorer.score) / 2);

    /* 難易度を設定 */
    let l = this.level;
    const aveReaction = (this.circleTimingScorer.aveReaction + this.faceTimingScorer.aveReaction) / 2
    // if (
    //   aveReaction <= JSON.parse(this.$p5.theme.NFB011.threshold)
    //   && this.level < 2
    // ) {
    //   l += 1;
    // }
    // if (
    //   aveReaction >= 1000
    //   && this.level > 1
    // ) {
    //   l -= 1;
    // }

    const scoreLog = [{
      isCorrects: this.faceTimingScorer.isCorrects,
      responseTimes: this.faceTimingScorer.responseTimes
    },{
      isCorrects: this.circleTimingScorer.isCorrects,
      responseTimes: this.circleTimingScorer.responseTimes
    }];

    this.$p5.remove();
    this.stop({
      // score: this.timingScorer.result,
      rawScore: score,
      level: l,
      scoreLog: scoreLog
    });
  }

  /* 青円をクリックした際の処理 */
  pushBlue(circle) {
    circle.alpha = 255;
    circle.diff = circle.calcDiff();
    const isCorrect = circle.diff < JSON.parse(this.$p5.theme.NFB011.threshold)
    this.circleTimingScorer.isCorrects.push(isCorrect);
    this.circleTimingScorer.responseTimes.push(isCorrect ?circle.diff :-1);
    if (isCorrect) {
      circle.lerp = 1.00;
      this.correctSound.playSound();
      circle.outsideColor = this.$p5.theme.color.accent;
    } else {
      this.wrongSound.playSound();
      if (App.isAndroid && this.$p5.theme.option.vibe==="on") Android.vibeWrong();
    }
    if (circle.diff > 500) {
      this.circleTimingScorer.addReactionTime(500);
    } else {
      this.circleTimingScorer.addReactionTime(circle.diff);
    }
    circle.overrapTime = 0;
  }

  /* 顔をクリックした際の処理 */
  pushFace(pushable) {
    let index = pushable.sprite.position.x === this.center.circle1.x
      ? 0
      : 1
    this.circleArray[index].lerp = 1.00;
    this.circleArray[index].outsideColor = this.$p5.theme.color.pushableFlash;

    this.correctSound.playSound();
    const diff = pushable.calcDiff();
    this.faceTimingScorer.addReactionTime(diff);
    this.faceTimingScorer.responseTimes.push(diff);
    this.faceTimingScorer.isCorrects.push(true);
    // pushable.alpha = 255;
    pushable.hidden();
  }

}

export class NFB011 extends Training{

  get uid(){ return 'NFB011'; }

  async start() {
    new p5(($p5) => {
      this.training = new AdjustTiming2($p5, this.preference);
      $p5.preload = this.training.preload.bind(this.training);
      $p5.setup = this.training.setup.bind(this.training);
      $p5.draw = this.training.draw.bind(this.training);
      $p5.mousePressed = this.training.mousePressed.bind(this.training);
    }, "main");
    await this.training.waitingForReady;

    this.training.start();
    let {rawScore,level,scoreLog} = await this.training.waitingForStopped;
    this.result = {rawScore};
    this.preference = {level};
    this.scoreLog = scoreLog;
    return this.response;
  }
}
