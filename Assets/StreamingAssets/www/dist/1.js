webpackJsonp([1,3],[
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
/* 2 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__training_js__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__components_hourglass_js__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_sounds_wrongsound_js__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__components_sounds_correctsound_js__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__components_memoryscorer_js__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_twentygrid_js__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__components_sounds_flashsound_js__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7__config_theme_json__);









/**
 * [トレーニングNFB004] 順番記憶
 * Libries: p5, p5sound, p5dom
 * @property {HourGlass} hourGlass - 砂時計
 * @property {MemoryScorer} memoryScorer - 順番記憶・数字記憶・音階記憶用スコアラー
 * @property {TwentyGrid} twentyGrid - 20方眼
 * @property {WrongSound} wrongSound - 不正解音
 * @property {CorrectSound} correctSound - 正解音
 * @property {FlashSound} flashSound - 記号点滅音
 * @property {number} maxWidth - 描画する横幅
 * @property {boolean} isPointing - 出題中かどうか
 * @property {boolean} isInputBlock - 入力を無視するかどうか
 * @property {number} questionTimer - 出題を行うsetTimeout処理のID
 * @property {number} inputTimer - 入力の無視を解除するsetTimeout処理のID
 * @property {number} clickedTimer - クリックした後に行う処理のsetTimeoutのID
 * @property {number} textSize - 文字サイズ
 * @property {p5.Element} message - 問題メッセージ表示用p5エレメント
 * @property {p5.Element} judgeMessage - 状況メッセージ表示用p5エレメント
 */

const time_limit = 60000; //{number} トレーニングの制限時間

class SequentialMemory {
  constructor(p5, preference=null) {
    this.$p5 = p5;
    this.$p5.theme = __WEBPACK_IMPORTED_MODULE_7__config_theme_json__;
    this.level = preference ? preference.level : parseInt(p5.theme.NFB004.level);
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    this.maxWidth = (p5.windowWidth < p5.windowHeight)
      ? p5.windowWidth
      : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.isPointing = true;
    this.isInputBlock = true;
    this.textSize = this.maxWidth / 15;
  }
  /* [p5] preload処理 */
  preload() {
    if (this.$p5.theme.font)
      this.font = this.$p5.loadFont(this.$p5.theme.font.path);
  }

  /* [p5] setup処理 */
  setup() {
    this.$p5.createCanvas(this.$p5.windowWidth, this.$p5.windowHeight);
    this.$p5.frameRate(30);
    this.$p5.noLoop();
    if (this.$p5.theme.font)
      this.$p5.textFont(this.font);

    this.hourGlass = new __WEBPACK_IMPORTED_MODULE_1__components_hourglass_js__["a" /* HourGlass */](this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.memoryScorer = new __WEBPACK_IMPORTED_MODULE_4__components_memoryscorer_js__["a" /* MemoryScorer */]();
    this.wrongSound = new __WEBPACK_IMPORTED_MODULE_2__components_sounds_wrongsound_js__["a" /* WrongSound */](this.$p5);
    this.correctSound = new __WEBPACK_IMPORTED_MODULE_3__components_sounds_correctsound_js__["a" /* CorrectSound */](this.$p5);
    this.flashSound = new __WEBPACK_IMPORTED_MODULE_6__components_sounds_flashsound_js__["a" /* FlashSound */](this.$p5);
    this.twentyGrid = new __WEBPACK_IMPORTED_MODULE_5__components_twentygrid_js__["a" /* TwentyGrid */](
      this.$p5,
      ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""],
      {
        mouseClicked: (e) => {
          if (this.isInputBlock) return;
          this.twentyGridPressed(Number(e.currentTarget.id));
        }
      }
    );

    this.message = this.$p5.createDiv("")
      .style("position", "fixed")
      .style("font-size", this.textSize + 'px')
      .style('width', '50vw')
      .style("color", this.$p5.theme.color.primaryText)
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '30vh')
      .html("点滅した順に<br>タッチしてください")
    ;
    this.judgeMessage = this.$p5.createDiv("")
      .style("position", "fixed")
      .style("font-size", this.textSize*2 + 'px')
      .style('width', '50vw')
      .style('height', '10vh')
      .style('color', this.$p5.theme.color.primaryText)
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '50vh')
      // .style('font-weight', 'bold')
      .html("出題中です")
    ;

    window.App.state = 'ready';
    this.ready();
  }

  mousePressed() {
    return false;
  }

  /* スタートボタン押下 */
  start() {
    this.hourGlass.start();
    /* 最初の問題を生成 */
    this.memoryScorer.addCorrect(Math.floor(Math.random() * 20));
    this.isInputBlock = true;
    /* 1秒後に出題 */
    this.questionTimer = setTimeout(() => {
      this.isPointing = true;
      this.twentyGrid.flashCorrects(this.memoryScorer.corrects, this.flashSound, this.$p5.theme.NFB004.marker, this.level);
    }, 1000);
    /* 問題の点滅が終わるとクリック可能に */
    this.inputTimer = setTimeout(() => {
      this.judgeMessage.html("GO！！");
      this.isPointing = false;
      this.isInputBlock = false;
      this.memoryScorer.rapTime = Date.now();
    }, 750 * this.level + (this.memoryScorer.corrects.length * (1000 / this.level + 500)));
  }

  /* [p5] draw処理 */
  draw() {
    this.$p5.noCanvas();
    this.$p5.createCanvas(this.$p5.windowWidth,this.$p5.windowHeight);
    if (this.hourGlass) this.hourGlass.renderGlass();

    /* 入力可能かどうかにあわせて20方眼の色を変更 */
    this.$p5.select("#twentygrid").child().forEach((value,index) => {
      if (this.isPointing) {
        this.$p5.select("#" + value.id).style('background-color', '#aaa');
      } else {
        this.$p5.select("#" + value.id).style('background-color', '#fff');
      }
    });

    /* メッセージの表示 */
    this.$p5.textAlign(this.$p5.CENTER);
    this.$p5.textSize(this.textSize);
    this.$p5.textStyle(this.$p5.BOLD);
    this.$p5.fill(80);

    if (!this.hourGlass.isOver || this.memoryScorer.isPerfect) return;
    /* 終了処理 */
    this.correctSound.stopSound();
    this.wrongSound.stopSound();
    this.flashSound.stopSound();
    clearTimeout(this.questionTimer);
    clearTimeout(this.inputTimer);
    clearTimeout(this.clickedTimer);
    this.$p5.noLoop();
    this.twentyGrid.isOver = true;
    /* 難易度を設定 */
    let l = this.level;
    // if (this.memoryScorer.score >= 10 && this.level < 3) {
    //   l += 1;
    // }
    // if (this.memoryScorer.score <= 5 && this.level > 1) {
    //   l -= 1;
    // }

    const scoreLog = {
      isCorrects: this.memoryScorer.isCorrects,
      responseTimes: this.memoryScorer.responseTimes
    };

    this.$p5.remove();
    this.stop({
      rawScore: this.memoryScorer.score,
      level: l,
      scoreLog: scoreLog
    });
  }

  twentyGridPressed(letter) {
    this.twentyGrid.flashPoint(letter, 300, this.$p5.theme.NFB004.marker);
    /* 正誤判定 */
    const isCorrect = this.memoryScorer.isCorrectCount(letter);
    this.memoryScorer.addResponseTime();
    this.memoryScorer.isCorrects.push(isCorrect);
    this.memoryScorer.rapTime = Date.now();

    if (isCorrect) {
      /* 正解の場合 */
      this.memoryScorer.plusCorrectCount();
      this.flashSound.playSound();
      if (!this.memoryScorer.isAllAnswered()) return;
      /* 出題された点をすべてをクリックし終わった場合 */
      this.memoryScorer.addCorrect(Math.floor(Math.random() * 20)); //問題の数を1つ増やす
      this.memoryScorer.resetCount();
      this.clickedTimer = setTimeout(() => {
        this.correctSound.playSound();
        /* 方眼の色を正解色に */
        Array.prototype.forEach.call(document.getElementsByClassName("ripple"), (button)=>{
          button.style.outlineColor = this.$p5.theme.color.accent;
          button.style.color = this.$p5.theme.color.accent;
        });
        clearTimeout(this.animTimer);
        this.animTimer = setTimeout(()=>{
          Array.prototype.forEach.call(document.getElementsByClassName("ripple"), (button)=>{
            button.style.outlineColor = this.$p5.theme.color.domKeyOutline;
            button.style.color = this.$p5.theme.color.domKeyOutline;
          });
        }, 300);
      }, 350);
      /* 問題を出題 */
      this.isInputBlock = true;
      this.questionTimer = setTimeout(() => {
        this.judgeMessage.html("出題中です").style("font-size", this.textSize*2 + 'px');
        this.isPointing = true;
        this.twentyGrid.flashCorrects(this.memoryScorer.corrects, this.flashSound, this.$p5.theme.NFB004.marker, this.level);
      }, 1500);
      /* 問題の点滅が終わるとクリック可能に */
      this.inputTimer = setTimeout(() => {
        this.judgeMessage.html("GO！！");
        this.isPointing = false;
        this.isInputBlock = false;
        this.memoryScorer.rapTime = Date.now();
      }, 1000 * this.level + (this.memoryScorer.corrects.length * (1000 / this.level + 500)));
    } else {
      /* 不正解の場合 */
      this.wrongSound.playSound();
      this.judgeMessage.html("×").style("font-size", '10vh');
      this.clickedTimer = setTimeout(() => {
        this.memoryScorer.isPerfect = false;
      }, 500);
      if (App.isAndroid && this.$p5.theme.option.vibe==="on") {
        Android.vibeWrong();
      }
      if (this.hourGlass.isOver)
        return;
      /* 問題を破棄し、新たに1つの問題を生成 */
      this.memoryScorer.formatCorrect();
      this.memoryScorer.addCorrect(Math.floor(Math.random() * 20));
      this.memoryScorer.resetCount();
      this.isInputBlock = true;
      /* 問題を出題 */
      this.questionTimer = setTimeout(() => {
        this.judgeMessage.html("出題中です").style("font-size", this.textSize*2 + 'px');
        this.isPointing = true;
        this.twentyGrid.flashCorrects(this.memoryScorer.corrects, this.flashSound, this.$p5.theme.NFB004.marker, this.level);
      }, 1500);
      /* 問題の点滅が終わるとクリック可能に */
      this.inputTimer = setTimeout(() => {
        this.judgeMessage.html("GO！！");
        this.isPointing = false;
        this.isInputBlock = false;
        this.memoryScorer.rapTime = Date.now();
      }, 1000 * this.level + (this.memoryScorer.corrects.length * (1000 / this.level + 500)));
    }
  }
}

class NFB004 extends __WEBPACK_IMPORTED_MODULE_0__training_js__["Training"]{

  get uid(){ return 'NFB004'; }

  async start() {
    new p5(($p5) => {
      this.training = new SequentialMemory($p5, this.preference);
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
/* harmony export (immutable) */ __webpack_exports__["NFB004"] = NFB004;



/***/ }),
/* 3 */,
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
/* 18 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__scorer_js__ = __webpack_require__(11);


/**
 * [Component] 順番記憶・数字記憶・音階記憶用スコアラー
 * @property {number} count - 現在対象のindex
 */
class MemoryScorer extends __WEBPACK_IMPORTED_MODULE_0__scorer_js__["a" /* Scorer */] {

  /**
   * 順番記憶・数字記憶・音階記憶用スコアラー
   * @param {p5} p
   * @param {Array} letters - 順序配列
   */
  constructor(p, letters) {
    super();
    this.count = 0;
    this.isPerfect = true;
  }

  /**
   * キー押下時数字キーか空白キーかを判別
   * @param {event} e
   * @param {function} callback
   * @returns {any}
   */
  pressKey(e, callback) {
    const answer = e.currentTarget.innerText;

    switch (answer) {
      case "":/* 空白キーは無視 */
        return;
      default:/* 数字キーの場合 */
        return callback(answer);
    }
  }

  /**
   * 回答が正解かどうか
   * @param {string} answer - 回答
   * @returns {boolean} true:正解 false:不正解
   */
  isCorrectCount(answer) {
    return super.isCorrect(this.corrects.length - this.count - 1, answer);
  }

  /**
   * 逆スパンの回答が正解かどうか
   * @param {string} answer - 回答
   * @returns {boolean} true:正解 false:不正解
   */
  isCorrectCountReverse(answer) {
    return super.isCorrect(this.count, answer)
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.count += 1;
    if (this.score < this.count)
      this.score = this.count;
  }

  /**
   * 出題された数字をすべて答えたかどうか
   * @returns true:すべて押された false:まだ押されていないスプライトがある
   */
  isAllAnswered() {
    return this.count == this.corrects.length;
  }

  /* カウントの初期化 */
  resetCount() {
    this.count = 0;
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = MemoryScorer;



/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 20方眼
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
class TwentyGrid {

  /**
   * [p5,p5dom]20方眼
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Array} letters - 20方眼に表示させる20個の内容を持った配列
   * @param {Object} [options]
   */
  constructor(p, letters, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.isOver = false;
    this.isLandscape = p.windowWidth > p.windowHeight;

    // 20方眼用のDivを作成
    let twentygrid = p.createDiv('');

    letters.forEach((value, index) => {
      twentygrid.child(this.createButton(value, index))
    });

    twentygrid.id("twentygrid")
      .style('position', 'fixed')
      .style('text-align', 'center')
    ;
    if(this.isLandscape){
      twentygrid.style('width', '45vw')
        .style('right', '2.5vw')
        .style('bottom', '15vh')
    }else{
      twentygrid.style('bottom', '5vh')
        .style('width', (95 / 100) * this.maxWidth + 'px')
        .style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + 'px')
    }
  }

  /**
   * 各キー作成用
   * [p5dom]createButtonメソッド
   * @param {string} letter - キーに表示する値
   * @param {number} id - id
   * @returns {p5.dom} button dom element
   */
  createButton(letter, id) {
    const p = this.p5;
    let button = p.createButton(letter);

    button.id(id)
      .style('padding', '0px 0px')
      .style('box-sizing', 'content-box')
      .style('background-color', this.p5.theme.color.domKeyBackground)
      .style('color', this.p5.theme.color.domKeyText)
      .style('cursor', 'pointer')
      .style('outline', 'solid 3px')
      .style('outline-offset', '-2px')
      .style('outline-color', this.p5.theme.color.domKeyOutline)
      .style('margin', '0px')
      .style('vertical-align', 'bottom')
      .style('font-family', 'LocalFont')
      .style('border', '0px')
      .class('ripple')
      .mousePressed(this.options.mouseClicked)
    ;
    if(this.isLandscape){
      button.style('width', '9vw')
        .style('height', '17.5vh')
        .style('line-height', '17.5vh')
        .style('font-size', '10vh')
      } else {
      button.style('width', (19 / 100) * this.maxWidth + 'px')
        .style('height', '11vh')
        .style('line-height', '11vh')
        .style('font-size', (10 / 100) * this.maxWidth + 'px')
    }

    return button;
  }

  /**
   * 正解配列点滅
   * @param {Array} list - 点滅させる要素のidが順番に格納された配列
   * @param {CorrectSound} sound - 点滅毎に鳴らす音
   * @param {string} marker - 表示する文字
   * @param {number} level - 3段階 出題間隔に関与
   */
  flashCorrects(list, sound, marker, level) {

    for (let i = 0; i < list.length; i++) {
      setTimeout(() => {
        if (this.isOver) return;
        this.flashPoint(list[i], 1000 / level, marker);
        sound.playSound();
      }, (1000 / level + 500) * i);
    }
  }

  /**
   * [p5dom]指定id要素の点滅
   * @param {number} id - 点滅させる要素のid
   * @param {number} time - 点灯時間(ミリ秒)
   * @param {string} marker - 表示する文字
   */
  flashPoint(id, time, marker) {
    this.p5.select("#" + id).html(marker);
    setTimeout(() => {
      if (this.isOver) return;
      this.p5.select("#" + id).html("");
    }, time);
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = TwentyGrid;




/***/ }),
/* 20 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] フラッシュサウンド
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
class FlashSound {

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
    this.pattern = [1660];
    this.phrase = new p5.Phrase('correct', (t, p) => {this.makeSound(t, p)}, this.pattern);
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
/* harmony export (immutable) */ __webpack_exports__["a"] = FlashSound;



/***/ })
]);
//# sourceMappingURL=1.js.map