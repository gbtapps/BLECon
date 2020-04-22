webpackJsonp([2,3],[
/* 0 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
class Training{

  constructor(userPreference) {
    /* ユーザ設定の全体 */
    this.userPreference = userPreference || {};
    this.result = {};
    this.scoreLog = {};
  }
  get uid(){ return null; }

  /** ユーザ設定から個別トレーニング設定を取得 */
  get preference(){
    return this.userPreference[this.uid] || null;
  }
  /** ユーザ設定に個別トレーニング設定を保存 */
  set preference(preference){
    this.userPreference[this.uid] = preference;
  }

  get response(){
    return {trainingResult: this.result, userPreference: this.userPreference, scoreLog: this.scoreLog};
  }
}
/* harmony export (immutable) */ __webpack_exports__["Training"] = Training;



/***/ }),
/* 1 */,
/* 2 */,
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__training_js__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__components_timingscorer_js__ = __webpack_require__(21);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_hourglass_js__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__components_sounds_wrongsound_js__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__components_sounds_correctsound_js__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_circlecircle_js__ = __webpack_require__(22);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__components_pushable_js__ = __webpack_require__(23);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7__config_theme_json__);









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
    this.$p5.theme = __WEBPACK_IMPORTED_MODULE_7__config_theme_json__;
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

    this.hourGlass = new __WEBPACK_IMPORTED_MODULE_2__components_hourglass_js__["a" /* HourGlass */](this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.correctSound = new __WEBPACK_IMPORTED_MODULE_4__components_sounds_correctsound_js__["a" /* CorrectSound */](this.$p5);
    this.wrongSound = new __WEBPACK_IMPORTED_MODULE_3__components_sounds_wrongsound_js__["a" /* WrongSound */](this.$p5);
    this.circleTimingScorer = new __WEBPACK_IMPORTED_MODULE_1__components_timingscorer_js__["a" /* TimingScorer */]();
    this.faceTimingScorer = new __WEBPACK_IMPORTED_MODULE_1__components_timingscorer_js__["a" /* TimingScorer */]();

    /* サークルを生成 */
    let size = (this.$p5.height / 4) > (this.maxWidth * (9 / 10))
      ? maxWidth * (2 / 3)
      : this.$p5.height / 2;
    this.circleArray.push(new __WEBPACK_IMPORTED_MODULE_5__components_circlecircle_js__["a" /* CircleCircle */](this.$p5, size, {x: this.center.circle1.x, y: this.center.circle1.y}, this.level == 1));
    this.pushable = new __WEBPACK_IMPORTED_MODULE_6__components_pushable_js__["a" /* Pushable */](this.$p5, this.image, size, true, this.pushImage);
    if (this.level == 2) {
      this.circleArray.push(new __WEBPACK_IMPORTED_MODULE_5__components_circlecircle_js__["a" /* CircleCircle */](this.$p5, size, {x: this.center.circle2.x, y: this.center.circle2.y}));
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

class NFB011 extends __WEBPACK_IMPORTED_MODULE_0__training_js__["Training"]{

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
/* harmony export (immutable) */ __webpack_exports__["NFB011"] = NFB011;



/***/ }),
/* 4 */,
/* 5 */,
/* 6 */,
/* 7 */,
/* 8 */,
/* 9 */
/***/ (function(module, exports) {

module.exports = {"color":{"primary":"#ffe600","secondary":"#ed7229","positioner":"#ffffff","positionerLight":"#ffffff","positionerText":"#505050","positionerPushedText":"#dfdfdf","domKeyOutline":"#505050","domKeyText":"#505050","domKeyBackground":"#ffffff","pushableFlash":"#ff0000","insideCircle":"#e6e6e6","outsideCircle":"#c8c8c8","accent":"#37b595","primaryText":"#606060","secondaryText":"#424242","accentText":"#000000","background":"#ffffff","empty":"#dfdfdf","shadow":"#c5c5c6","circles":"#ed7769","crosses":"#22587a"},"option":{"vibe":"off","sound":"on","isReturnEnable":false},"tengrid":{"borderRadius":"0px"},"font":{"family":"LocalFont","path":"./assets/font/mplus-1p-regular.woff"},"pulse":{"images":null,"ballImage":null},"NFB001":{"level":"2"},"NFB002":{"level":"2","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB003":{"level":"2","pushList":[["1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"]]},"NFB004":{"level":"3","marker":"●"},"NFB005":{"level":"4"},"NFB006":{"level":"4"},"NFB007":{"level":"1"},"NFB008":{"level":"1","symbolList":["〇","☆","▽","■","◇","×","△","※","◎","□"]},"NFB009":{"level":"1"},"NFB010":{"level":"3","threshold":"500"},"NFB011":{"level":"2","threshold":"500","isUseImage":true,"iconfont":"tag_faces","imagePath":"assets/image/face1.png"},"NFB012":{"level":"1","isUseImage":false,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/push1.png","isPositive":true},"image2":{"iconfont":"tag_faces","imagePath":"./assets/image/unpush1.png","isPositive":false},"image3":{"iconfont":"brightness_high","imagePath":"./assets/image/push2.png","isPositive":true},"image4":{"iconfont":"brightness_high","imagePath":"./assets/image/unpush2.png","isPositive":false},"image5":{"iconfont":"bug_report","imagePath":"./assets/image/push3.png","isPositive":true},"image6":{"iconfont":"bug_report","imagePath":"./assets/image/unpush3.png","isPositive":false}},"NFB013":{"level":"1","threshold":"1000","isUseImage":true,"image1":{"iconfont":"tag_faces","imagePath":"./assets/image/face1.png","isPositive":true},"image2":{"iconfont":"sentiment_very_dissatisfied","imagePath":"./assets/image/face2.png","isPositive":false}},"NFB014":{"level":"1","pushList":[["1","あ","2","い","3","う","4","え","5","お","6","か","7","き","8","く","9","け","10","こ"]]},"NFB015":{"level":"1"},"NFB016":{"level":"1"},"NFB017":{"level":"1"},"NFB018":{"level":"1","threshold":"1000","isUseImage":false,"iconfont":"star","imagePath":"./assets/image/face1.png","isPositive":true},"NFB019":{"level":"1"},"NFB020":{"level":"1"},"NFB021":{"level":"1"},"NFB022":{"level":"1"},"NFB023":{"level":"1","flowingList":[{"name":"犬","image":"./assets/image/dog.png"},{"name":"馬","image":"./assets/image/horse.png"},{"name":"猿","image":"./assets/image/monkey.png"},{"name":"猫","image":"./assets/image/cat.png"},{"name":"兎","image":"./assets/image/rabbit.png"},{"name":"鼠","image":"./assets/image/rat.png"}]},"NFB024":{"level":"1"},"NFB025":{"level":"1"},"NFB026":{"level":"1"},"NFB027":{"level":"1"},"NFB028":{"level":"1"},"NFB029":{"level":"1"},"NFB030":{"level":"1","flowingList":[{"name":"いぬ","image":"./assets/image/dog.png"},{"name":"うま","image":"./assets/image/horse.png"},{"name":"ねこ","image":"./assets/image/cat.png"}]}}

/***/ }),
/* 10 */
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
/* 11 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

/**
 * [Component] スコアラー
 * @property {Array} corrects - 正解の配列
 * @property {number} score - スコア（間違うとペナルティをうけます）
 * @property {number} correctCount - 正答数
 *
 */
class Scorer {

  /**
   * コンストラクタ
   */
  constructor() {
    this.corrects = [];
    this.score = 0;
    this.correctCount = 0;
    this.rapTime = 0;
    this.responseTimes = [];
    this.isCorrects = [];
  }

  /**
   * 問題の正解をcorrectsに追加します
   * void
   * @private
   * @param {any} correct - 問題の正解
   */
  addCorrect(correct) {
    this.corrects.push(correct);
  }

  /**
   * correctsを初期化します
   * void
   */
  formatCorrect() {
    this.corrects = [];
  }

  /**
   * N番目が正解かどうか
   * @param {number} index - Nに相当する値
   * @param {any} answer - 回答
   * @returns {boolean} true: 正解である、false: 間違えてる
   */
  isCorrect(index, answer) {
    return this.corrects[this.corrects.length - (1 + index)] === answer;
  }

  /**
   * 最新が正解かどうか
   * @param {any} answer - 回答
   * @returns {boolean} true: 正解である、false: 間違えてる
   */
  isCorrectLastest(answer) {
    return this.isCorrect(0, answer);
  }

  /**
   * 回答が正解一覧に含まれているかどうか
   * @param {any} answer - 回答
   * @returns {boolean} true: 含まれている、false: 含まれていない
   */
  isIncludeCorrect(answer) {
    return this.corrects.includes(answer);
  }

  /**
   * 総合脳活性度を算出します
   * @returns {number} 総合脳活性度
   */
  calcActiveness() {
    return this.score * 1;
  }

  /**
   * 正答率を算出します
   * @returns {number} 正答率
   */
  calcPerCorrect() {
    if (this.corrects.length === 0) {
      return 0;
    }
    if (this.correctCount != 0) {
      return this.correctCount / this.corrects.length;
    }
    return this.score / this.corrects.length;
  }

  /**
   * NBackトレーニングにおける正答率を算出します
   * @param {number} timeLimit - 制限時間
   * @param {number} interval - 出題間隔
   * @returns {number} 正答率
   */
  calcNbackPerCorrect(timeLimit, interval) {
    var questionCount = timeLimit / interval;
    return this.correctCount / questionCount;
  }

  get numOfCorrect() {
    let count = 0;
    this.isCorrects.forEach((v, i)=>{
      if(v === true){
        count += 1;
      }
    })
    return count;
  }

  addResponseTime() {
    this.responseTimes.push(Date.now() - this.rapTime)
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = Scorer;



/***/ }),
/* 12 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 正解
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {Array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
class CorrectSound {

  /**
   * [p5sound] 再生音定義
   */
  constructor(p) {
    this.p5 = p;
    this.env = new p5.Envelope(0.000001, 0.1, 0.08, 0.1, 0.4, 0);
    this.osc = new p5.Oscillator();
    this.osc.setType('square');
    this.osc.amp(0);
    this.pattern = [2100, 0, 1660];
    this.phrase = new p5.Phrase('correct', (t, p) => {this.makeSound(t, p)}, this.pattern);
    this.part = new p5.Part();
    this.part.addPhrase(this.phrase);
    this.part.setBPM(95);
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
/* harmony export (immutable) */ __webpack_exports__["a"] = CorrectSound;



/***/ }),
/* 13 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 不正解
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
class WrongSound {

  /**
   * [p5sound] 再生音定義
   *
   */
  constructor(p) {
    this.p5 = p;
    this.env = new p5.Envelope(0.00001, 0.1, 0.08, 0.1, 0.00001, 0.000001);
    this.osc = new p5.Oscillator();
    this.osc.setType('square');
    this.osc.amp(0);
    this.pattern = [250, 0, 250, 250, 250, 250];
    this.phrase = new p5.Phrase('wrong',(t, p) => {this.makeSound(t, p)}, this.pattern);
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
/* harmony export (immutable) */ __webpack_exports__["a"] = WrongSound;



/***/ }),
/* 14 */,
/* 15 */,
/* 16 */,
/* 17 */,
/* 18 */,
/* 19 */,
/* 20 */,
/* 21 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__scorer_js__ = __webpack_require__(11);


/**
 * タイミングスコアラー
 * @property {number} pushCount - クリックされた回数
 * @property {number} totalTime - 反応時間の総計
 */
class TimingScorer extends __WEBPACK_IMPORTED_MODULE_0__scorer_js__["a" /* Scorer */] {

  constructor() {
    super();
    this.totalTime = 0;
    this.pushCount = 0;
  }
  /**
   * スコア判定メソッド
   * @param {number} diff - 予測のズレ時間
   */
  addReactionTime(diff) {
    this.pushCount += 1;
    this.totalTime += diff;
  }

  /* 失敗時ペナルティ */
  penalty() {
    this.totalTime += 500;
  }

  /**
   * 平均反応時間を取得
   * @returns {number} 平均反応時間
   */
  get aveReaction() {
    if (this.pushCount === 0){
      this.pushCount = 1;
    }
    return this.totalTime / this.pushCount;
  }

  /**
   * 平均反応時間から求められた結果を取得
   * @return {number} 最大10、最小1の整数値
   */
  get result() {
    let result = 1 / (this.aveReaction / 1000);
    return Math.round(Math.min(Math.max(result, 1), 10));
  }

  /**
   * 2秒から平均反応時間を引いた数値を10段階のスコアにして返します
   * 反射神経系に使えるかもしれません
   * @returns {number} スコア(1~10)
   */
  get result2() {
    return Math.min(
      Math.max(
        20 - Math.floor(this.aveReaction / 100),
        1
      ),
      10
    );
  }

  /* プロパティを初期値に戻します */
  reset() {
    this.pushCount = 0;
    this.totalTime = 0;
  }



}
/* harmony export (immutable) */ __webpack_exports__["a"] = TimingScorer;



/***/ }),
/* 22 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] サークル
 * @property {p5} p5
 * @property {number} distance - 青円赤円を描画する、中心からの距離
 * @property {Object} center - サークルの中心
 * @property {number} overrapTime - 青円と赤円が重なる時間
 * @property {Object} outsideCircle - 外側の円のスプライト
 * @property {Object} insideCircle - 内側の円のスプライト
 * @property {number} blueRadian - 青円の角度
 * @property {Object} bluePoint - 青円のスプライト
 * @property {number} redRadian - 赤円の角度
 * @property {Object} redPoint - 赤円のスプライト
 * @property {boolean} isClockwise - 赤円が時計回りかどうか
 * @property {number} difRadian - 赤円と青円のなす角
 * @property {number} speed - 1フレームに赤円が進む角度
 * @property {number} diff - 反応速度
 */

class CircleCircle {

  /**
   * サークルを作成
   * @param {p5} p
   * @param {number} size - サイズ
   * @param {Object} position - 中心点
   * @param {boolean} [isEasyMode] - 赤円と青円の難易度を下げるかどうか
   */
  constructor(p, size, position, isEasyMode = false) {
    this.p5 = p;
    this.distance = size * (5 / 12);
    this.center = {x:position.x, y:position.y}
    this.overrapTime = 0;
    this.alpha = 0;
    this.size = size;
    this.outsideColor = 200;
    this.image = p.loadImage("./assets/image/blueCircle.svg");
    this.lerp = 0.00;
  
    this.outsideCircle = p.createSprite(this.center.x, this.center.y, size, size);
    this.outsideCircle.draw = function() {
      p.fill(200);
      p.stroke(230);
      p.ellipse(0, 0, size);
    }
    // this.outsideCircle.mouseActive = true;

    this.insideCircle = p.createSprite(this.center.x, this.center.y, size * (2 / 3), size * (2 / 3));
    this.insideCircle.draw = function() {
      p.fill(230);
      p.noStroke();
      p.ellipse(0, 0, size * (2 / 3));
    }
    this.insideCircle.mouseActive = true;

    this.blueRadian = p.radians(isEasyMode ? 270 : p.random(0, 360));
    this.bluePoint = p.createSprite(
      this.center.x + p.cos(this.blueRadian) * this.distance,
      this.center.y + p.sin(this.blueRadian) * this.distance,
      size / 4,
      size / 4
    );
    this.bluePoint.draw = function() {
      p.fill(0, 0, 255);
      p.noStroke();
      p.ellipse(0, 0, size / 4.5);
    }
    this.bluePoint.setCollider("circle", 0, 0, size / 6);
    this.bluePoint.mouseActive = true;
    // this.bluePoint.debug=true;

    // this.redRadian =p.radians(p.random(0,360));
    this.redRadian = this.blueRadian;
    this.redPoint = p.createSprite(
      this.center.x + p.cos(this.redRadian) * this.distance,
      this.center.y + p.sin(this.redRadian) * this.distance,
      size / 4,
      size / 4
    );
    this.redPoint.draw = function() {
      p.fill(255, 0, 0);
      p.noStroke();
      p.ellipse(0, 0, size / 4.5);
    }

    this.isClockwise = isEasyMode
      ? true
      : Math.random() >= 0.5;
    this.difRadian = this.getDifRadian();
    this.speed = this.p5.radians(isEasyMode ? 3 : p.random(1, 4));
    this.diff;
  }

  // start(){
  //   this.overrapTime = this.p5.millis() + (this.difRadian/this.speed/20*1000);
  // }

  /**
   * 二つの円が重なる時間と今の時間との差を取得
   * @returns {number} 二つの円が重なる時間と今の時間との差
   */
  calcDiff() {
    return Math.abs(this.p5.millis() - this.overrapTime);
  }

  /**
   * 赤円と青円が成す角度を取得
   * 向きは赤円の進行方向
   * @returns {number} 赤円と青円が成す角度
   */
  getDifRadian() {
    let difRadian = Math.abs(this.redRadian - this.blueRadian);
    if (
      (this.redRadian > this.blueRadian && this.isClockwise)
      || (this.blueRadian > this.redRadian && !this.isClockwise)
    ){
      difRadian = this.p5.TWO_PI - difRadian;
    }
    return difRadian;
  }

  /**
   * 赤円を移動させる
   */
  movePoint(avefps) {
    if (this.isClockwise) {
      this.redRadian += this.speed;
      if (this.redRadian > this.p5.TWO_PI)
        this.redRadian -= this.p5.TWO_PI;
    } else {
      this.redRadian -= this.speed;
      if (this.redRadian < 0)
        this.redRadian += this.p5.TWO_PI;
    }
    this.redPoint.position = {
      x: this.center.x + Math.cos(this.redRadian) * this.distance,
      y: this.center.y + Math.sin(this.redRadian) * this.distance
    };
    this.difRadian = this.getDifRadian();
  }

  /**
   * 赤円の位置に画像を描画する
   * @param {p5.Image} pic - 表示するp5Imageオブジェクト
   */
  drawRedPointImage(pic) {
    pic.mask(pic);
    this.p5.tint(255, 255);
    this.p5.image(
      pic,
      this.redPoint.position.x - this.redPoint.width / 2,
      this.redPoint.position.y - this.redPoint.height / 2,
      this.redPoint.width,
      this.redPoint.height
    );
  }

  /**
   * 青円の位置に画像を描画する
   * @param {p5.Image} pic - 表示するp5Imageオブジェクト
   */
  drawBluePointImage(pic) {
    pic.mask(pic)
    this.p5.tint(255, 255);
    this.p5.image(
      pic,
      this.bluePoint.position.x - this.bluePoint.width / 2,
      this.bluePoint.position.y - this.bluePoint.height / 2,
      this.bluePoint.width,
      this.bluePoint.height
    );
  }

  /**
   * タッチフィードバック表示
   */
  drawCircle(){
    if (this.alpha > 0)
      this.alpha -= 20;
    if(this.lerp > 0)
      this.lerp -= 0.1;
    this.outsideCircle.draw = ()=>{
      this.p5.fill(this.p5.lerpColor(this.p5.color(200), this.p5.color(this.outsideColor), this.lerp));
      this.p5.stroke(230);
      this.p5.ellipse(0, 0, this.size);
    }
    this.p5.drawSprite(this.outsideCircle);
    this.p5.drawSprite(this.insideCircle);
  
    this.p5.tint(255, this.alpha);
    this.p5.image(
      this.image,
      this.bluePoint.position.x - this.bluePoint.width,
      this.bluePoint.position.y - this.bluePoint.height,
      this.bluePoint.width * 2,
      this.bluePoint.height * 2
    );

    this.p5.drawSprite(this.bluePoint);
    this.p5.drawSprite(this.redPoint);
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = CircleCircle;



/***/ }),
/* 23 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 押せるマーク
 * @property {p5} p5
 * @property {boolean} isPositive - 押すべきマークかどうか
 * @property {number} diff - 押された時間と表示された時間の差
 * @property {number} showTime - 表示された時間
 * @property {Object} sprite - マークのスプライト
 * @property {Any} image - 表示する画像もしくはiconfontの名前
 */

class Pushable {

    /**
     * [p5,p5play]押せるマークを作成
     * @param {p5} p
     * @param {Any} image - 表示する画像もしくはiconfontの名前
     * @param {number} size - 画像のサイズ
     * @param {boolean} isPositive - 押すべき画像かどうか
     */
    constructor(p, image, size, isPositive, pushImage) {
      this.p5 = p;
      this.isPositive = isPositive;
      this.diff;
      this.showTime;
      this.sprite = p.createSprite(0, 0, size * (2 / 3), size * (2 / 3));
      this.sprite.shapeColor = p.color(200, 0);
      this.sprite.setCollider('circle', 0, 0, size * (2 / 6));
      this.sprite.visible = false;
      this.sprite.mouseActive = true;
      this.image = image;
      this.pushCircle = pushImage;
      this.alpha = 0;
      // this.sprite.debug = true;
    }

    /* スプライトを表示する */
    show(position) {
      this.sprite.position.x = position.x
      this.sprite.position.y = position.y;
      this.sprite.visible = true;
      this.showTime = this.p5.millis();
    }

    /* スプライトを隠す */
    hidden() {
      this.sprite.visible = false;
    }

    /* 画像を描画 */
    drawImage() {
      this.image.mask(this.image);
      if (!this.sprite.visible) return;
      this.p5.tint(255, 255);
      this.p5.image(
        this.image,
        this.sprite.position.x - this.sprite.width / 2,
        this.sprite.position.y - this.sprite.height / 2,
        this.sprite.width,
        this.sprite.height
      );
    }

    drawEffect() {
      if(this.alpha > 0)
        this.alpha -= 30;
      this.p5.tint(255, this.alpha);
      this.p5.image(
        this.pushCircle,
        this.sprite.position.x - this.sprite.width / 2,
        this.sprite.position.y - this.sprite.height / 2,
        this.sprite.width,
        this.sprite.height
      );
    }

    /* アイコンフォントを描画 */
    drawIconfont(color = "#FF8C00") {
      this.p5.textFont('Material Icons');
      if (!this.sprite.visible) return;
      this.p5.fill(color);
      this.p5.noStroke();
      this.p5.textSize(this.sprite.width);
      this.p5.textAlign(this.p5.CENTER);

      this.p5.text(
        this.image,
        this.sprite.position.x,
        this.sprite.position.y + this.sprite.height / 2
      );
    }

    /**
     * 表示された時間と今の時間との差を取得
     * @returns {number} 二つの円が重なる時間と今の時間との差
     */
    calcDiff() {
      return this.p5.millis() - this.showTime;
    }

  }
/* harmony export (immutable) */ __webpack_exports__["a"] = Pushable;



/***/ })
]);
//# sourceMappingURL=2.js.map