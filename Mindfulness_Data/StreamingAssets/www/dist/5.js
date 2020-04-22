webpackJsonp([5],{

/***/ 10:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
const fully = '#66CC33';
const half = '#FFD400';
const scanty = '#FF3300';
const elapsed = '#848484';

/**
 * 砂時計
 * Libries: p5
 * @property {number} startMillis - 開始タイミング
 * @property {number} timeLimit - 制限時間
 * @property {number} vh - ディスプレイの高さの1/100
 * @property {number} vw - ディスプレイの幅の1/100
 * @property {number} short - vhとvwの短い方
 * @property {boolean} isStart - スタートボタンが押されたかどうか
 *
 */
class HourGlass {

  /**
   * 初期表示
   * @param {p5} p
   * @param {number} timeLimit - 制限時間
   * @param {boolean} [isReturnEnable] - 戻るボタンの表示の有無
   */
  constructor(p, timeLimit, option = {isReturnEnable:false, isChangeColor:true}) {
    this.p5 = p;
    this.timeLimit = timeLimit;
    this.option = option;
    let shortView = p.windowHeight > p.windowWidth
      ? 'vw'
      : 'vh';
    this.secGlass = new Date(this.timeLimit + this.startMillis - this.p5.millis());

    // let div = p.createDiv('');
    // div.id("hourglass")
    //   .style('position', 'fixed')
    //   .style('top','10vw')
    //   // .style('top', '7vw')
    //   .style('left', '7vw')
    //   .style('font-size', '3.5vw')
    //   // .style('background-color', '#fff')
    //   // .style('color', '#616161')
    //   .style('width', '11vw')
    //   .style('height', '11vw')
    //   .style('border-radius', '50%')
    //   .style('display', 'flex')
    //   .style('align-items', 'center')
    //   .style('justify-content', 'center')
    //   .html("戻る")
    // ;
    if (option.isReturnEnable) {
      this.div = p.createButton('戻る')
        .id("hourglass")
        .style('position', 'fixed')
        // .style('top', '10vw')
        .style('top', '4' + shortView)
        .style('left', '4' + shortView)
        .style('font-size', '3.5' + shortView)
        .style('background-color', 'transparent')
        // .style('color', '#616161')
        .style('width', '17' + shortView)
        .style('height', '17' + shortView)
        // .style('border-radius', '50%')
        .style('display', 'flex')
        .style('align-items', 'center')
        .style('justify-content', 'center')
        .style('padding-top', '7' + shortView)
        .style('border-style', 'none')
        .mousePressed(() => {
          if (App.isAndroid) Android.redo();
        })
      ;
    }
    this.isStart = false;
  }

  /**
   * [p5] トレーニング開始時処理
   */
  start() {
    this.startMillis = this.p5.millis();
    this.p5.loop();
    this.isStart = true;
  }

  /**
   * [p5] 砂時計を描画
   */
  renderGlass(isVisible = true) {
    this.vh = this.p5.windowHeight / 100;
    this.vw = this.p5.windowWidth / 100;
    this.short = this.p5.min(this.vh, this.vw);

    // const hourglass = this.p5.select("#hourglass");
    this.secGlass = new Date(this.timeLimit + this.startMillis - this.p5.millis());
    // let nowSec = ("0" + this.secGlass.getSeconds()).slice(-2);
    // hourglass.html("戻る");
    /** 外枠部分 */
    this.limitSec = this.timeLimit / 1000 - this.secGlass.getSeconds() - (this.secGlass.getMilliseconds() / 1000);
    if (!isVisible)
      return;
    this.p5.noStroke();
    this.p5.fill(fully);
    if (this.option.isChangeColor && (this.timeLimit / 1000) / 2 > this.secGlass.getSeconds()){
      this.p5.fill(half);
    }
    if (this.option.isChangeColor && (this.timeLimit / 1000) / 4 > this.secGlass.getSeconds()){
      this.p5.fill(scanty);
    }
    this.p5.ellipseMode(this.p5.CORNER);
    this.p5.ellipse(4 * this.short, 4 * this.short, 17 * this.short, 17 * this.short);
    // this.p5.fill('#212121');
    this.p5.fill(elapsed);
    let rad = (360 / (this.timeLimit / 1000)) * this.limitSec * Math.PI / 180;
    let s = -90 * Math.PI / 180;
    if (this.isOver) {
      this.p5.arc(4 * this.short, 4 * this.short, 17 * this.short, 17 * this.short, s, s - 0.001 )
    } else {
      this.p5.arc(4 * this.short, 4 * this.short, 17 * this.short, 17 * this.short, s, s + rad + 0.001);
    }
    // if (!this.isStart){
    //   this.p5.select("#hourglass").html(this.timeLimit / 1000);
    // }
    this.p5.ellipseMode(this.p5.CENTER);
  }

  /**
   * [p5] タイムオーバー判定
   * @returns {boolean} 経過した：true 経過していない:false
   */
  get isOver() {
    this.endTime = this.p5.millis();
    return (this.p5.millis() > (this.timeLimit + this.startMillis));
  }

  get limit() {
    return this.secGlass.getSeconds() + (this.secGlass.getMilliseconds() / 1000)
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = HourGlass;










/***/ }),

/***/ 24:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * 空
 * @property {number} sunHeight - 日の位置の高さ。一番下になると日没になります。
 * @property {array<Start>} stars - 星々
 */
class Sky{
  /**
   * @param {p5}
   * @param {number} height - 空領域の高さ
   */
  constructor($p, height){
    this.$p5 = $p;
    this.stars = [];
    for (var i = 0; i < 50; i++) {
      let s = new Star(this.$p5);
      this.stars.push(s);
    }
    this.sunHeight = 0;
    this.height = height;
  }
  /**
   * 太陽を落とす
   * HSB色のsaturation/brightnessを太陽の高さと同期させています。
   * 完全に落下したとき、星々がまたたきます。
   */
  letSunGoesDown() {
    const pos = this.sunHeight/this.height;  //上からの位置割合
    let f = [182,86,70],
        t = [23,51,89];
    let from = this.$p5.color(`hsb(${f[0]},${f[1]*pos}%,${f[2]*(1.6-pos)}%)`),
        to   = this.$p5.color(`hsb(${t[0]},${t[1]*pos}%,${t[2]*(1.6-pos)}%)`);
    this.$p5.background(220);
    this._setGradient(from, to);

    if(pos>0.95)
      this.letStartsTwink();
    this.$p5.colorMode();
  }
  /**
   * 星々をまたたかせる
   */
  letStartsTwink(){
    this.stars.forEach(s => {
      s.twink();
    });
  }
  /** 空にグラデーションを描画するメソッド  */
  _setGradient(from, top) {
    this.$p5.noFill();
    for (let i = 0; i <= this.$p5.windowHeight; i++) {
      var inter = this.$p5.map(i, 0, this.$p5.windowHeight, 0, 1);
      var c = this.$p5.lerpColor(from, top, inter);
      this.$p5.stroke(c);
      this.$p5.line(0, i, this.$p5.windowWidth, i);
    }
  }
}
/* harmony export (immutable) */ __webpack_exports__["a"] = Sky;

/**
 * 瞬く星
 */
class Star{
  constructor($p){
    this.$p5 = $p
    this.x = this.$p5.random(this.$p5.windowWidth);
    this.y = this.$p5.random(this.$p5.windowHeight - 200);
    this.w = 2;
    this.h = 2;
    setInterval(()=>{
      this.x += (this.$p5.random(10) - 5);
      this.y += (this.$p5.random(10) - 5);
      if (this.w == 2) {
        this.w = 3;
        this.h = 3;
      } else {
        this.w = 2;
        this.h = 2;
      }
    },6000);
  }
  /**
   * またたく
   * 前回値を更新するため、定期的にコールされることを想定しています。
   */
  twink(){
    this.$p5.noStroke();
    this.$p5.fill(255, 255, 0);
    this.$p5.ellipse(this.x, this.y, this.w, this.h);
  }
}


/***/ }),

/***/ 7:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__components_hourglass_js__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__components_sky_js__ = __webpack_require__(24);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2__config_theme_json__);



const time_limit = 60000; //{number} - トレーニングの制限時間
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
  /**
   *
   * @param {p5} p5
   * @param {object} preference - {"min": number, "max": number}
   */
  constructor(p5, preference) {
    this.$p5 = p5;
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    this.max = preference.max || 75;
    this.min = preference.min || 60;
    this.$p5.theme = __WEBPACK_IMPORTED_MODULE_2__config_theme_json__;
    this.hourGlass = new __WEBPACK_IMPORTED_MODULE_0__components_hourglass_js__["a" /* HourGlass */](this.$p5, time_limit, {isReturnEnable:false, isChangeColor:false});
    this.isInitialized = false;
  }

  /* [p5] preload処理 */
  preload() {
    if (this.$p5.theme.font)
      this.font = this.$p5.loadFont(this.$p5.theme.font.path);

    this.images = this.$p5.theme.pulse.images
      ? this.$p5.theme.pulse.images.map(i => this.$p5.loadImage(i))
      : null;
    this.ballImage = this.$p5.theme.pulse.ballImage
      ? this.$p5.loadImage(this.$p5.theme.pulse.ballImage)
      : null;
  }

  /* [p5] setup処理 */
  setup() {
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);
    this.$p5.background(this.$p5.theme.color.background);
    this.$p5.frameRate(30);
    this.$p5.noLoop();
    this.$p5.isReady = true;
    if (this.$p5.theme.font) this.$p5.textFont(this.font);

    this.ground = new Ground(this.$p5, this.images);
    this.sky = new __WEBPACK_IMPORTED_MODULE_1__components_sky_js__["a" /* Sky */](this.$p5, this.$p5.windowHeight-this.ground.height);

    this.heartbeatBall = new HeartbeatBall(this.$p5, this.ballImage );

    this.$p5.imageMode(this.$p5.CENTER);

    this.ready();
  }

  /* [p5] スタートボタン押下 */
  start() {
    this.hourGlass.start();
    /* バウンス */
    this.bounce();

    /* 花 */
    if(!this.ground.flowers)return;
    this.ground.flowers.forEach(f => {
      f.images.forEach(i => {
        i.resize(this.$p5.width / 5, 0);
      });
    });
  }

  /**
   * 現在の心拍に応じて定期的にボールを弾ませます
   * @returns {number} 現在心拍数
   */
  bounce() {
    this.currentHR = this.getCalcedPulse();
    let interval = 1000;
    if(this.currentHR>0){
      this.heartbeatBall.setBeat();
      interval = 1000 / (this.currentHR / 60);
    }
    this.beatTimer = setTimeout(()=>{
      this.bounce()
    },interval);
    return this.currentHR;
  }
  getCalcedPulse(){
    return (typeof Android !== "undefined") ? Android.getPulse() : 75;  //75は標準値
  }
  /* [p5] draw処理 */
  draw() {
    /* For Standalone */
    if (typeof Android === "undefined") {
      /* テスト用キー操作 */
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
    }

    /* スプライトの描画 */
    if(this.currentHR>30){ //不正値チェック
      if(!this.isInitialized)/*初期化済み*/
        this._initializeByHr(this.currentHR);
      this.sky.sunHeight = this.heartbeatBall.beat(this.currentHR);
    }
    this.sky.letSunGoesDown();
    if (this.hourGlass)
      this.hourGlass.renderGlass();

    this.$p5.drawSprite(this.heartbeatBall.sprite);
    this.$p5.drawSprite(this.ground.base);

    /* アニメーション管理 */
    if(this.ground.flowers){
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
        this.$p5.animation(
          this.ground.flowers[i],
          this.$p5.width * (v / 4) - this.$p5.width / 7.5,
          this.$p5.height * (85 / 100) - this.$p5.width / 10
        );
      });
    }

    /* 終了処理 */
    if (!this.hourGlass.isOver)
      return;
    this.$p5.noLoop();
    clearInterval(this.beatTimer);
    this.$p5.noCanvas();
    this.$p5.remove();

    this.stop();
  }
  /** 初期HRによる初期化 */
  _initializeByHr(hr){
    /* max/min補正 */
    if(this.max<hr)
      this.max=hr;
    else if((this.max-hr) > hr*0.1)
      this.max=hr*1.1;
    if(this.min>hr)
      this.min=hr;
    else if((hr-this.min) > hr*0.1)
      this.min=hr*0.9;
    /** 心拍⇒上からの位置を計算する関数 */
    this.heartbeatBall.calcHeightByHr = (hr)=>{
      if(hr>this.max)
        return this.$p5.windowHeight*0.1;
      if(hr<this.min)
        return this.$p5.windowHeight*0.9;
      let x = this.$p5.windowHeight*0.8 / (this.max - this.min);  // window8割/心拍幅
      return  this.$p5.windowHeight*0.1 + (this.max-hr) * x;
    }
    this.isInitialized=true;
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
    this.size = p.height / 10;
    this.p5 = p;
    this.beatVal = 0;
    this.perBeatVal = 0;
    this.basePosition = {
      x: p.width / 2,
      y: p.height * 0
    };
    this.calcHeightByHr = null;//function
    /* For Standalone */
    if (typeof Android === "undefined") {
      this.isUseImage = false;
    }

    this.sprite = p.createSprite(
      this.basePosition.x,
      this.basePosition.y,
      this.size,
      this.size
    );
    this.sprite.draw = () => {
      /* For Standalone */
      if (typeof Android !== "undefined") {
        if (this.isUseImage) {
          p.image(image, 0, 0, this.size, this.size);
          return;
        }
        p.fill("#FFA000");
        p.noStroke();
        p.ellipse(0, 0, this.size, this.size);
        return;
      }
      if (image) {
        p.image(image, 0, 0, this.size, this.size);
        return;
      }
      p.fill("#FF6D00");
      p.noStroke();
      p.ellipse(0, 0, this.size, this.size);
    };
  }

  /* 振動幅を設定 */
  setBeat() {
    this.beatVal = 1 - this.basePosition.y / this.p5.height;
    this.perBeatVal = this.beatVal / 10;
  }
  /* ボールの位置を更新 */
  beat(currentHR) {
    /* 不正値チェック */
    if(currentHR<1)return;
    if(!this.baseHR)
      this.baseHR=currentHR;
    this.basePosition.y = this.calcHeightByHr(currentHR);
    // let y = this.p5.height*(3/10) - ((currentHR-this.baseHR)/this.baseHR * 5) * (this.p5.height/2);
    // this.basePosition.y = Math.max(
    //   this.p5.height*(10/100),
    //   Math.min(y, this.p5.height*(90/100))
    // );
    if (this.beatVal > 0) {
      this.beatVal -= this.perBeatVal;
    }
    this.sprite.position.y = this.basePosition.y - (this.p5.height*(5/100) * this.beatVal);
    return this.sprite.position.y;
  }
}

/**
 * OKライン
 * @property {p5.Sprite} base - OKラインのスプライト
 * @property {number} targetIndex - アニメーション対象のインデックス
 * @property {Array<p5.Animation>} flowers - 花ごとのアニメーション配列
 */
class Ground {
  constructor(p, images=null) {
    this.height = p.windowHeight * (15 / 100);
    this.base = p.createSprite(
      p.windowWidth / 2,
      p.windowHeight * (92.5 / 100),
      p.windowWidth,
      this.height
    );
    this.base.shapeColor = p.color("#5D4037");

    this.targetIndex = 0;
    if(!images)return;
    /* 花 */
    this.flowers = [];
    [1, 2, 3, 4, 5, 6].forEach((v, i, a) => {
      let flower = p.loadAnimation(...images);
      flower.looping = false;
      flower.stop();
      this.flowers.push(flower);
    });
  }
}

class PulseGuide {
  constructor(pulse){
    this.range = JSON.stringify(pulse);
  }
  async start() {
    new p5($p5 => {
      this.training = new Pulse($p5, this.range);
      $p5.preload = this.training.preload.bind(this.training);
      $p5.setup = this.training.setup.bind(this.training);
      $p5.draw = this.training.draw.bind(this.training);
    }, "content");
    await this.training.waitingForReady;

    this.training.start();

    await this.training.waitingForStopped;
  }
}
/* harmony export (immutable) */ __webpack_exports__["PulseGuide"] = PulseGuide;



/***/ }),

/***/ 9:
/***/ (function(module, exports) {

module.exports = {"color":{"primary":"#ffe600","secondary":"#ed7229","positioner":"#ffffff","positionerLight":"#ffffff","positionerText":"#505050","positionerPushedText":"#dfdfdf","domKeyOutline":"#505050","domKeyText":"#505050","domKeyBackground":"#ffffff","pushableFlash":"#ff0000","insideCircle":"#e6e6e6","outsideCircle":"#c8c8c8","accent":"#37b595","primaryText":"#606060","secondaryText":"#424242","accentText":"#000000","background":"#ffffff","empty":"#dfdfdf","shadow":"#c5c5c6","circles":"#ed7769","crosses":"#22587a"},"option":{"vibe":"off","sound":"on","isReturnEnable":false},"tengrid":{"borderRadius":"0px"},"font":{"family":"LocalFont","path":"./assets/font/mplus-1p-regular.woff"},"pulse":{"images":null,"ballImage":null},"NFB001":{"level":"2"},"NFB002":{"level":"2","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB003":{"level":"2","pushList":[["1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"]]},"NFB004":{"level":"3","marker":"●"},"NFB005":{"level":"4"},"NFB006":{"level":"4"},"NFB007":{"level":"1"},"NFB008":{"level":"1","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB009":{"level":"1"},"NFB010":{"level":"3","threshold":"500"},"NFB011":{"level":"2","threshold":"500","isUseImage":true,"iconfont":"tag_faces","imagePath":"assets/image/face1.png"},"NFB012":{"level":"1","isUseImage":false,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/push1.png","isPositive":true},"image2":{"iconfont":"tag_faces","imagePath":"./assets/image/unpush1.png","isPositive":false},"image3":{"iconfont":"brightness_high","imagePath":"./assets/image/push2.png","isPositive":true},"image4":{"iconfont":"brightness_high","imagePath":"./assets/image/unpush2.png","isPositive":false},"image5":{"iconfont":"bug_report","imagePath":"./assets/image/push3.png","isPositive":true},"image6":{"iconfont":"bug_report","imagePath":"./assets/image/unpush3.png","isPositive":false}},"NFB013":{"level":"1","threshold":"1000","isUseImage":true,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/face1.png","isPositive":true},"image2":{"iconfont":"sentiment_very_dissatisfied","imagePath":"./assets/image/face2.png","isPositive":false}},"NFB014":{"level":"1","pushList":[["1","あ","2","い","3","う","4","え","5","お","6","か","7","き","8","く","9","け","10","こ"]]},"NFB015":{"level":"1"},"NFB016":{"level":"1"},"NFB017":{"level":"1"},"NFB018":{"level":"1","threshold":"1000","isUseImage":false,"iconfont":"star","imagePath":"./assets/image/face1.png","isPositive":true},"NFB019":{"level":"1"},"NFB020":{"level":"1"},"NFB021":{"level":"1"},"NFB022":{"level":"1"},"NFB023":{"level":"1","flowingList":[{"name":"犬","image":"./assets/image/dog.png"},{"name":"馬","image":"./assets/image/horse.png"},{"name":"猿","image":"./assets/image/monkey.png"},{"name":"猫","image":"./assets/image/cat.png"},{"name":"兎","image":"./assets/image/rabbit.png"},{"name":"鼠","image":"./assets/image/rat.png"}]},"NFB024":{"level":"1"},"NFB025":{"level":"1"},"NFB026":{"level":"1"},"NFB027":{"level":"1"},"NFB028":{"level":"1"},"NFB029":{"level":"1"},"NFB030":{"level":"1","flowingList":[{"name":"いぬ","image":"./assets/image/dog.png"},{"name":"うま","image":"./assets/image/horse.png"},{"name":"ねこ","image":"./assets/image/cat.png"}]}}

/***/ })

});
//# sourceMappingURL=5.js.map