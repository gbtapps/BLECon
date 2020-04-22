webpackJsonp([4],{

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

/***/ 25:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 壁ヒット
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
class HitWallSound {

  /**
   * [p5sound] 再生音定義
   *
   */
  constructor(p) {
    this.p5 = p;
    this.env = new p5.Envelope(0.000001, 0.1, 0.05, 0.1, 0.00001, 0);
    this.osc = new p5.Oscillator();
    this.osc.setType('square');
    this.osc.amp(0);
    this.pattern = [880];
    this.phrase = new p5.Phrase('correct',(t, p) => {this.makeSound(t, p)}, this.pattern);
    this.part = new p5.Part();
    this.part.addPhrase(this.phrase);
    this.part.setBPM(180);
  }

  /**
   * [p5sound] 音声生成
   * @param {Number} time
   * @param {Array} playbackRate 音高
   */
  makeSound(time, playbackRate) {
    this.osc.freq(playbackRate);
    this.osc.start();
    this.env.play(this.osc);
  }

  /**
   * [p5sound] 音声再生
   * トレーニングからはこれを呼び出す
   */
  playSound(option = null) {
    if (this.p5.theme.option.sound === "off" && option === null) return;
    this.part.start();
  }

    /**
   * [p5sound] 音声停止
   * トレーニングからはこれを呼び出す
   */
  stopSound() {
    this.part.stop();
  }
}
/* harmony export (immutable) */ __webpack_exports__["a"] = HitWallSound;



/***/ }),

/***/ 26:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] ビー音
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
class Alert {

  /**
   * [p5sound] 再生音定義
   *
   */
  constructor(p) {
    this.p5 = p;
    this.env = new p5.Envelope(0.00001, 0.1, 0.5, 0.1, 0.00001, 0.000001);
    this.osc = new p5.Oscillator();
    this.osc.setType('square');
    this.osc.amp(0);
    this.pattern = [1760];
    this.phrase = new p5.Phrase('beep',(t, p) => {this.makeSound(t, p)}, this.pattern);
    this.part = new p5.Part();
    this.part.addPhrase(this.phrase);
    this.part.setBPM(100);
  }

  /**
   * [p5sound] 音声生成
   * @param {Number} time
   * @param {Array} playbackRate 音高
   */
  makeSound(time, playbackRate) {
    this.osc.freq(playbackRate);
    this.osc.start();
    this.env.play(this.osc);
  }

  /**
   * [p5sound] 音声再生
   * トレーニングからはこれを呼び出す
   */
  playSound(option = null) {
    if (this.p5.theme.option.sound === "off" && option === null) return;
    this.part.start();
  }

  /**
   * [p5sound] 音声停止
   * トレーニングからはこれを呼び出す
   */
  stopSound() {
    this.part.stop();
  }
}
/* harmony export (immutable) */ __webpack_exports__["a"] = Alert;



/***/ }),

/***/ 8:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__components_hourglass_js__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1__config_theme_json__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_sounds_hitwallsound_js__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__components_sounds_alert_js__ = __webpack_require__(26);




const time_limit = 60000; //{number} - トレーニングの制限時間
/**
 * 計測タイマー
 * Libries: p5
 * @property {HourGlass} hourglass - 砂時計
 */
class Timer {
  /**
   *
   * @param {p5} p5
   */
  constructor(p5) {
    this.$p5 = p5;
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    this.$p5.theme = __WEBPACK_IMPORTED_MODULE_1__config_theme_json__;
    this.hourGlass = new __WEBPACK_IMPORTED_MODULE_0__components_hourglass_js__["a" /* HourGlass */](this.$p5, time_limit, {isReturnEnable:false, isChangeColor:true});
    this.maxWidth = (p5.windowWidth < p5.windowHeight)
    ? p5.windowWidth
    : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.textSize = this.maxWidth / 7;
    this.hitWallSound = new __WEBPACK_IMPORTED_MODULE_2__components_sounds_hitwallsound_js__["a" /* HitWallSound */](this.$p5);
    this.alert = new __WEBPACK_IMPORTED_MODULE_3__components_sounds_alert_js__["a" /* Alert */](this.$p5);
    this.counter = 5;
  }

  /* [p5] preload処理 */
  preload() {
    if (this.$p5.theme.font)
      this.font = this.$p5.loadFont(this.$p5.theme.font.path);
  }

  /* [p5] setup処理 */
  setup() {
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);
    this.$p5.background(this.$p5.theme.color.background);
    this.$p5.frameRate(30);
    this.$p5.noLoop();

    this.message = this.$p5.createDiv("")
      // .parent('defaultCanvas0')
      .style("position", "fixed")
      .style("font-size", this.textSize + 'px')
      .style('width', '100vw')
      .style('height', '100vh')
      .style("color", this.$p5.theme.color.primaryText)
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '0vh')
      .style('line-height', '100vh')
      .html("ゲームを始めてください")
    ;

    this.$p5.isReady = true;
    if (this.$p5.theme.font) this.$p5.textFont(this.font);
    this.ready();
  }

  /* [p5] スタートボタン押下 */
  start() {
    this.hourGlass.start();
  }
  /* [p5] draw処理 */
  draw() {
    if (this.hourGlass)
      this.hourGlass.renderGlass();

    [...Array(6)].forEach((v, i)=>{
      if(this.hourGlass.limit<i+1&&this.counter===i){
        if(i===0) {
          this.alert.playSound("on");
        } else {
          this.hitWallSound.playSound("on");
        }
        this.counter-=1;
      }
    })

    /* 終了処理 */
    if (!this.hourGlass.isOver)
      return;
    this.$p5.noLoop();
    this.$p5.noCanvas();
    this.$p5.remove();

    this.stop();
  }
}

class MeasureTimer {
  constructor(){
  }
  async start() {
    new p5($p5 => {
      this.training = new Timer($p5);
      $p5.preload = this.training.preload.bind(this.training);
      $p5.setup = this.training.setup.bind(this.training);
      $p5.draw = this.training.draw.bind(this.training);
    }, "content");
    await this.training.waitingForReady;

    this.training.start();

    await this.training.waitingForStopped;
  }
}
/* harmony export (immutable) */ __webpack_exports__["MeasureTimer"] = MeasureTimer;



/***/ }),

/***/ 9:
/***/ (function(module, exports) {

module.exports = {"color":{"primary":"#ffe600","secondary":"#ed7229","positioner":"#ffffff","positionerLight":"#ffffff","positionerText":"#505050","positionerPushedText":"#dfdfdf","domKeyOutline":"#505050","domKeyText":"#505050","domKeyBackground":"#ffffff","pushableFlash":"#ff0000","insideCircle":"#e6e6e6","outsideCircle":"#c8c8c8","accent":"#37b595","primaryText":"#606060","secondaryText":"#424242","accentText":"#000000","background":"#ffffff","empty":"#dfdfdf","shadow":"#c5c5c6","circles":"#ed7769","crosses":"#22587a"},"option":{"vibe":"off","sound":"on","isReturnEnable":false},"tengrid":{"borderRadius":"0px"},"font":{"family":"LocalFont","path":"./assets/font/mplus-1p-regular.woff"},"pulse":{"images":null,"ballImage":null},"NFB001":{"level":"2"},"NFB002":{"level":"2","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB003":{"level":"2","pushList":[["1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"]]},"NFB004":{"level":"3","marker":"●"},"NFB005":{"level":"4"},"NFB006":{"level":"4"},"NFB007":{"level":"1"},"NFB008":{"level":"1","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB009":{"level":"1"},"NFB010":{"level":"3","threshold":"500"},"NFB011":{"level":"2","threshold":"500","isUseImage":true,"iconfont":"tag_faces","imagePath":"assets/image/face1.png"},"NFB012":{"level":"1","isUseImage":false,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/push1.png","isPositive":true},"image2":{"iconfont":"tag_faces","imagePath":"./assets/image/unpush1.png","isPositive":false},"image3":{"iconfont":"brightness_high","imagePath":"./assets/image/push2.png","isPositive":true},"image4":{"iconfont":"brightness_high","imagePath":"./assets/image/unpush2.png","isPositive":false},"image5":{"iconfont":"bug_report","imagePath":"./assets/image/push3.png","isPositive":true},"image6":{"iconfont":"bug_report","imagePath":"./assets/image/unpush3.png","isPositive":false}},"NFB013":{"level":"1","threshold":"1000","isUseImage":true,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/face1.png","isPositive":true},"image2":{"iconfont":"sentiment_very_dissatisfied","imagePath":"./assets/image/face2.png","isPositive":false}},"NFB014":{"level":"1","pushList":[["1","あ","2","い","3","う","4","え","5","お","6","か","7","き","8","く","9","け","10","こ"]]},"NFB015":{"level":"1"},"NFB016":{"level":"1"},"NFB017":{"level":"1"},"NFB018":{"level":"1","threshold":"1000","isUseImage":false,"iconfont":"star","imagePath":"./assets/image/face1.png","isPositive":true},"NFB019":{"level":"1"},"NFB020":{"level":"1"},"NFB021":{"level":"1"},"NFB022":{"level":"1"},"NFB023":{"level":"1","flowingList":[{"name":"犬","image":"./assets/image/dog.png"},{"name":"馬","image":"./assets/image/horse.png"},{"name":"猿","image":"./assets/image/monkey.png"},{"name":"猫","image":"./assets/image/cat.png"},{"name":"兎","image":"./assets/image/rabbit.png"},{"name":"鼠","image":"./assets/image/rat.png"}]},"NFB024":{"level":"1"},"NFB025":{"level":"1"},"NFB026":{"level":"1"},"NFB027":{"level":"1"},"NFB028":{"level":"1"},"NFB029":{"level":"1"},"NFB030":{"level":"1","flowingList":[{"name":"いぬ","image":"./assets/image/dog.png"},{"name":"うま","image":"./assets/image/horse.png"},{"name":"ねこ","image":"./assets/image/cat.png"}]}}

/***/ })

});
//# sourceMappingURL=4.js.map