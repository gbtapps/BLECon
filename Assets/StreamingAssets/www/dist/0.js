webpackJsonp([0,3],[
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
/* 1 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__training_js__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__components_tenkey_js__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_calcurator_js__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__components_target_js__ = __webpack_require__(16);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__components_calcscorer_js__ = __webpack_require__(17);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_hourglass_js__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__components_sounds_correctsound_js__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__components_sounds_wrongsound_js__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_8__config_theme_json__);










const time_limit = 60000; //{number} トレーニングの制限時間

/**
 * [トレーニングNFB001] 単純計算1
 * Libries: p5, p5sound, p5dom
 * @property {HourGlass} hourGlass - 砂時計
 * @property {CalcScorer} calcScorer - 単純計算用スコアラー
 * @property {Target} target - 問題表示エリア
 * @property {Calcrator} calcrator - 計算機
 * @property {Tenkey} tenkey - 10キー
 * @property {CorrectSound} correctSound - 正解音
 * @property {WrongSound} wrongSound - 不正解音
 * @property {number} maxWidth - 描画対象とする横幅
 * @property {number} alpha - 2問次の問題の透明度
 * @property {number} nextPositionY - 1問次の問題のY座標（vw）
 * @property {number} changeNextTimer - 問題を更新する処理を管理するsetInterval処理のID
 * @property {string} nextQuestion - 1問次の問題文
 * @property {string} doubleNextQuestion - 2問次の問題
 */

class SimpleCalculation1 {
  constructor(p5, preference=null) {
    this.$p5 = p5;
    this.$p5.theme = __WEBPACK_IMPORTED_MODULE_8__config_theme_json__;
    this.level = preference ? preference.level : parseInt(p5.theme.NFB001.level);
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    // this.maxWidth = (p5.windowWidth < p5.windowHeight)
    //   ? p5.windowWidth
    //   : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.alpha = 255;
    this.nextPositionY = 43;
    this.animTimer;
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
    this.nextQuestion = "";
    this.doubleNextQuestion = "";

    this.hourGlass = new __WEBPACK_IMPORTED_MODULE_5__components_hourglass_js__["a" /* HourGlass */](this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.target = new __WEBPACK_IMPORTED_MODULE_3__components_target_js__["a" /* Target */](this.$p5, true);
    this.calcurator = new __WEBPACK_IMPORTED_MODULE_2__components_calcurator_js__["a" /* Calcurator */](this.$p5);
    this.calcScorer = new __WEBPACK_IMPORTED_MODULE_4__components_calcscorer_js__["a" /* CalcScorer */](this.level);
    this.correctSound = new __WEBPACK_IMPORTED_MODULE_6__components_sounds_correctsound_js__["a" /* CorrectSound */](this.$p5);
    this.wrongSound = new __WEBPACK_IMPORTED_MODULE_7__components_sounds_wrongsound_js__["a" /* WrongSound */](this.$p5);
    this.tenKey = new __WEBPACK_IMPORTED_MODULE_1__components_tenkey_js__["a" /* TenKey */](this.$p5, {
      mouseClicked: (e) => {
        /* タッチした際の波紋 */
        let x, y;
        let cover = document.createElement('span'); //span作る
        let coversize = document.getElementById(e.currentTarget.id).offsetWidth; //要素の幅を取得
        let loc = document.getElementById(e.currentTarget.id).getBoundingClientRect(); //絶対座標の取得
        if(e.clientX){
          x = e.clientX - loc.left - window.pageXOffset - (coversize / 2);
          y = e.clientY - loc.top - window.pageYOffset - (coversize / 2);
        } else {
          x = e.touches[0].clientX - loc.left - window.pageXOffset - (coversize / 2);
          y = e.touches[0].clientY - loc.top - window.pageYOffset - (coversize / 2);
        }
        let pos = 'top:' + y + 'px; left:' + x + 'px; height:' + coversize + 'px; width:' + coversize + 'px;';
        //spanを追加
        document.getElementById(e.currentTarget.id).appendChild(cover);
        cover.setAttribute('style', pos);
        cover.setAttribute('class', 'rp-effect');//クラス名追加
        // setTimeout(function() {
        //   var list = document.getElementsByClassName( "rp-effect" ) ;
        //   for(var i =list.length-1;i>=0; i--){//末尾から順にすべて削除
        //     list[i].parentNode.removeChild(list[i]);
        // }}, 500);

        this.calcurator.input(e, () => {
          let changeTime = this.$p5.millis();
          let answer = this.$p5.select("#display");
          let isCorrect = this.calcScorer.isCorrect(2, parseInt(answer.html())); //正誤判定
          this.calcScorer.isCorrects.push(isCorrect);
          this.calcScorer.addResponseTime();
          this.calcScorer.rapTime = Date.now();
          if (isCorrect) {
            /* テンキーの色を正解色に */
            Array.prototype.forEach.call(document.getElementsByClassName("ripple"), (button)=>{
              button.style.color = this.$p5.theme.color.accent;
            });
            clearTimeout(this.animTimer);
            this.animTimer = setTimeout(()=>{
              Array.prototype.forEach.call(document.getElementsByClassName("ripple"), (button)=>{
                button.style.color = this.$p5.theme.color.domKeyOutline;
              });
            }, 300);

            this.calcScorer.plusCorrectCount();
            this.correctSound.playSound();
          } else {
            this.wrongSound.playSound();
            this.calcScorer.penalty();
            if (App.isAndroid && this.$p5.theme.option.vibe==="on") {
              Android.vibeWrong();
            }
          }
          this.target.update(this.nextQuestion);
          this.target.updateNext(this.nextQuestion = this.doubleNextQuestion);
          this.target.updateDoubleNext(this.doubleNextQuestion = this.calcScorer.question());
          this.$p5.select("#display").html(""); //入力欄をクリア

          this.alpha = 0;
          this.nextPositionY = 32;
          clearInterval(this.changeNextTimer);
          /* 次以降の問題の位置を更新 */
          this.changeNextTimer = setInterval(() => {
            if (this.nextPositionY >= 43) {
              this.alpha = (this.$p5.millis() - changeTime) * 255 / 500;
              if (this.alpha > 255) return clearInterval(this.changeNextTimer);
            } else {
              this.nextPositionY = 32 + Math.min((this.$p5.millis() - changeTime) / 30, 43);
            }
          }, 33);
        });
      }
    });
    window.App.state = 'ready';
    this.ready();
  }

  mousePressed() {
    return false;
  }

  /* スタートボタン押下 */
  start() {
    this.hourGlass.start();
    /* 問題を更新 */
    this.target.update(this.calcScorer.question());
    this.calcScorer.rapTime = Date.now();
    this.target.updateNext(this.nextQuestion = this.calcScorer.question());
    this.target.updateDoubleNext(this.doubleNextQuestion = this.calcScorer.question());
  }

  /* [p5] draw処理 */
  draw() {
    this.$p5.noCanvas();
    this.$p5.createCanvas(this.$p5.windowWidth,this.$p5.windowHeight);
    if (this.hourGlass) this.hourGlass.renderGlass();

    this.maxWidth = (this.$p5.windowWidth < this.$p5.windowHeight)
      ? this.$p5.windowWidth
      : this.$p5.pow(this.$p5.windowHeight, 2) / this.$p5.windowWidth;

    this.$p5.textAlign(this.$p5.CENTER);
    this.$p5.textSize(this.maxWidth / 6.5);
    this.$p5.smooth();
    this.$p5.fill(97, this.alpha);
    this.$p5.text(
      this.doubleNextQuestion,
      this.$p5.windowWidth * ((this.target.doubleNext.left+7) / 100),
      this.$p5.windowHeight * (32 / 100) + this.maxWidth / 25
    );
    this.$p5.fill("#616161")
    this.$p5.text(
      this.nextQuestion,
      this.$p5.windowWidth * ((this.target.next.left+7) / 100),
      this.$p5.windowHeight * (this.nextPositionY / 100) + this.maxWidth / 25
    );

    if (!this.hourGlass.isOver) return;
    /* 終了処理 */
    clearInterval(this.changeNextTimer);
    this.correctSound.stopSound();
    this.wrongSound.stopSound();
    this.$p5.noLoop();
    /* 難易度を設定 */
    let l = this.level;
    // const bias = this.level * 3;
    // if (this.calcScorer.score >= (35-bias) && this.level < 3) {
    //   l += 1;
    // }
    // if (this.calcScorer.score <= (20-bias) && this.level > 1) {
    //   l -= 1;
    // }

    const scoreLog = {
      isCorrects: this.calcScorer.isCorrects,
      responseTimes: this.calcScorer.responseTimes
    };

    this.$p5.remove();
    this.stop({
      rawScore: this.calcScorer.score,
      level: l,
      scoreLog: scoreLog
    });
  }

}

class NFB001 extends __WEBPACK_IMPORTED_MODULE_0__training_js__["Training"]{

  get uid(){ return 'NFB001'; }

  async start() {
    new p5(($p5) => {
      this.training = new SimpleCalculation1($p5, this.preference);
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
/* harmony export (immutable) */ __webpack_exports__["NFB001"] = NFB001;



/***/ }),
/* 2 */,
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
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__config_theme_json__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__config_theme_json___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0__config_theme_json__);

/**
 * [Component] 10keyキーボード
 * Libries: p5, p5dom
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
class TenKey {

  /**
   * [p5,p5dom] 10keyキーボード
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Object} [options]
   */
  constructor(p, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      // : p.pow(p.windowHeight, 2) / p.windowWidth;
      : p.windowWidth
    this.isLandscape = p.windowWidth > p.windowHeight;

    // 10キー用のDivを作成
    let tenkey = p.createDiv('');
    tenkey.child(this.createButton('', 's1'));
    tenkey.child(this.createButton("1", 1));
    tenkey.child(this.createButton("2", 2));
    tenkey.child(this.createButton("3", 3));
    if (this.options.numOnly) {
      tenkey.child(this.createButton('', 's2'));
    } else {
      tenkey.child(this.createButton('backspace', 'x', true));
    }
    tenkey.child(this.createButton('', 's3'));
    tenkey.child(this.createButton("4", 4));
    tenkey.child(this.createButton("5", 5));
    tenkey.child(this.createButton("6", 6));
    tenkey.child(this.createButton('', 's4'));
    tenkey.child(this.createButton('', 's5'));
    tenkey.child(this.createButton("7", 7));
    tenkey.child(this.createButton("8", 8));
    tenkey.child(this.createButton("9", 9));
    tenkey.child(this.createButton('', 's6'));
    tenkey.child(this.createButton('', 's7'));
    tenkey.child(this.createButton('', 's8'));
    tenkey.child(this.createButton("0", 0));
    tenkey.child(this.createButton('', 's9'));
    if (this.options.numOnly) {
      tenkey.child(this.createButton('', 's0'));
    } else {
      tenkey.child(this.createButton('keyboard_return', 'e', true));
      // tenkey.child(this.createButton('check_circle', 'e', true));
    }


    tenkey.id("tenkey")
      .style('position', 'fixed')
      .style('text-align', 'center')
      .style('margin', 'auto')
    ;
    if(this.isLandscape){
      tenkey.style('width', '45vw')
        .style('right', '2.5vw')
        .style('bottom', '15vh')
    }else{
      tenkey.style('bottom', '5vh')
        .style('width', (95 / 100) * this.maxWidth + 'px')
        .style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + 'px')
    }

    if(this.options.top){
      tenkey.style('top', '30vh');
    }

    if(this.options.dualSetting){
      switch(this.options.dualSetting.whichSide){
        case "right":
          tenkey.style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + this.maxWidth/2 + 'px');
          break;
        case "left":
          tenkey.style('left', p.width / 2 - ((95 / 200) * this.maxWidth) - this.maxWidth/2 + 'px');
          break;
      }
    }
  }

  /**
   * [p5dom] 各キー作成用
   * createButtonメソッド
   * @param {string} letter - キーに表示する値
   * @param {number} id - id
   * @param {boolean} [isMaterialIcons] - マテリアルアイコンを使用するかどうか
   * @returns {p5.dom} button dom element
   */
  createButton(letter, id, isMaterialIcons = false) {
    const p = this.p5;
    let button = p.createButton(letter);

    button.id(id)
      .style('padding', '0px 0px')
      .style('box-sizing', 'content-box')
      .style('background-color', this.p5.theme.color.domKeyBackground)
      .style('color', this.p5.theme.color.domKeyText)
      .style('outline-color', this.p5.theme.color.domKeyOutline)
      .style('cursor', 'pointer')
      .style('outline', 'solid 3px')
      .style('outline-offset', '-2px')
      .style('margin', '0px 0px 0px 0px')
      .style('vertical-align', 'bottom')
      .style(
        'font-family',
        isMaterialIcons
          ? 'Material Icons'
          : 'LocalFont'
      )
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
   * [p5dom]10キーをシャッフル
   * void
   */
  shuffleKey() {
    let keyArray = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    for (let i = 0; i < 10; i++) {
      this.p5.select("#" + i).html(keyArray.splice(Math.floor(Math.random() * keyArray.length), 1)[0])
    }
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = TenKey;




/***/ }),
/* 15 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] 計算器
 * Libries: p5, p5dom
 * @property {p5} p
 * @property {number} maxWidth - 描画対象とする横幅
 */
class Calcurator {
  /**
   * [p5dom] 計算機
   * Componentを生成し、中央上部に描画。
   * @param {p5} p
   */

  constructor(p) {
    this.p5 = p;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.isLandscape = p.windowWidth > p.windowHeight

    let input = p.createDiv('');

    input.id("display")
      .style('position', 'fixed')
      .style('font-size', (18 / 100) * this.maxWidth + 'px')
      .style('background-color', '#212121')
      .style('color', '#fff')
      .style('cursor', 'default')
      .style('width', (40 / 100) * this.maxWidth + 'px')
    //  .style('height', (20 / 100) * this.maxWidth + 'px')
      .style('height', '15vh')
      .style('left', p.windowWidth / 2 - ((20 / 100) * this.maxWidth) + 'px')
      .style('top', '33vh')
      .style('font-family', 'LocalFont')
    //  .style('line-height', (20 / 100) * this.maxWidth + 'px')
      .style('line-height', '15vh')
      .style('text-align', 'center')
    ;

    if(this.isLandscape){
      input.style('top', '50vh')
      .style('left', '35vw')
      .style('width','15vw')
    }

    let equal = p.createDiv('=');
    equal.id("equal")
      .style('position', 'fixed')
      .style('font-size', (18 / 100) * this.maxWidth + 'px')
      .style('color', '#616161')
      .style('width', '5vw')
      .style('height', '15vh')
      .style('top', '50vh')
      .style('font-family', 'LocalFont')
      .style('line-height', '15vh')
      .style('text-align', 'center')
      .style('left', '28vw')

  }

  /**
   * [EventHandler,p5dom] 入力表示用displayメソッド
   * 押下されたボタンの値を入力エリアに追加
   * mousePressedからコールされるため、ここではスコープがp5.Elementに変化しています。
   * @param {event} e
   * @param {function} callback
   * @returns {any}
   */
  input(e, callback = null) {
    const input = this.p5.select("#display");
    const letter = e.currentTarget.innerText;

    switch (letter) {
      case 'backspace':/*文字を消す*/
        if (input.html().length >= 2) {
          return input.html(input.html().substr(0, input.html().length - 1));
        } else {
          return input.html("");
        }
      case "":/* 無視 */
        return;
      case 'keyboard_return':/* 正誤判定 */
        return callback();
      default:/*文字を追加*/
        if (input.html().length == 3) {
          return;
        } else if (input.html() == 0) {
          return input.html(letter);
        } else {
          return input.html(input.html() + letter);
        }
    }
  }

}
/* harmony export (immutable) */ __webpack_exports__["a"] = Calcurator;



/***/ }),
/* 16 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/**
 * [Component] ターゲット
 * Libries: p5,p5dom
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
class Target {

  /**
   * [p5dom] 表示領域の確保
   * @public
   * @param {p5} p
   */
  constructor(p, isDisplayNext = false) {
    this.p5 = p;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.target = p.createDiv('');
    this.next = p.createDiv('');
    this.doubleNext = p.createDiv('');
    this.isLandscape = p.windowWidth > p.windowHeight


    this.target.id("target")
      .style('position', 'fixed')
      .style('width', '100vw')
      .style('height', '15vh')
      .style('top', '15vh')
      .style('font-size', (15 / 100) * this.maxWidth + 'px')
      .style('color', '#616161')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('font-family', 'LocalFont')
      .style('line-height', '15vh')
    ;
    if(this.isLandscape){
      this.target.style('width', '14vw')
      .style('top', '50vh')
      .style('left', '13vw')
      .style('outline', 'solid 2px')
      .style('outline-offset', '-1px')
      .style('outline-color', '#616161')
    }

    if (!isDisplayNext) return;

    this.doubleNext.id('doubleNext')
      .style('position', 'fixed')
      .style('width', (25 / 100) * this.maxWidth + 'px')
      .style('top', '1vh')
      .style('left', p.width / 2 + (23 / 100) * this.maxWidth + 'px')
      .style('color', 'rgba(255, 255, 255, 0)')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('font-family', 'LocalFont')
      .style('padding', '0px 0px')
      .style('height', '10vh')
      .style('box-sizing', 'content-box')
      .style('font-size', (9 / 100) * this.maxWidth + 'px')
      // .style('outline', 'solid 2px')
      // .style('outline-offset', '-1px')
      // .style('outline-color', '#616161')
      .style('line-height', '10vh')
      .style('border', '0px')
    ;

    this.next.id("next")
      .style('position', 'fixed')
      .style('width', (25 / 100) * this.maxWidth + 'px')
      .style('top', '12vh')
      .style('left', '73vw')
      .style('color', 'rgba(255, 255, 255, 0)')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('font-family', 'LocalFont')
      .style('padding', '0px 0px')
      .style('height', '10vh')
      .style('box-sizing', 'content-box')
      .style('font-size', (9 / 100) * this.maxWidth + 'px')
      // .style('outline', 'solid 2px')
      // .style('outline-offset', '-1px')
      // .style('outline-color', '#616161')
      .style('line-height', '10vh')
      .style('border', '0px')
    ;

    if(this.isLandscape){
      this.next.style('top', '37vh')
      .style('height', '12vh')
      .style('width', '14vw')
      .style('left', '13vw')

      this.doubleNext.style('top', '24vh')
      .style('height', '12vh')
      .style('width', '14vw')
      .style('left', '13vw')
    }

  }
  /**
   * 出題するvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  update(value) {
    this.p5.select("#target").html(value);
  }

  /**
   * 1問次のvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  updateNext(value) {
    this.p5.select("#next").html("(" + value + ")");
  }

  /**
   * 2問次のvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  updateDoubleNext(value) {
    this.p5.select("#doubleNext").html("(" + value + ")");
  }


  get setTemplate() {

  }
}
/* harmony export (immutable) */ __webpack_exports__["a"] = Target;



/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__scorer_js__ = __webpack_require__(11);


class CalcScorer extends __WEBPACK_IMPORTED_MODULE_0__scorer_js__["a" /* Scorer */] {
  /**
   * コンストラクタ
   * @param {number} level - 3段階 符号の出題割合と問題桁数に関与
   */
  constructor(level = 1) {
    super();
    this.level = level;
  }

  /**
   * 1桁どうしの四則演算を出題します。
   * 数字はランダムに抽出されます。
   * @param {string} [operator] - 演算子
   * @returns {string} 提案された演算式
   */
  question(operator = null) {

    if (operator === null)
      var operator = this.operator();
    let left = (this.level >= 3)
      ? Math.floor(Math.random() * 20)
      : Math.floor(Math.random() * 10);
    let right = Math.floor(Math.random() * 10);
    if (left === 0 && right === 0 && operator === '/')
      return this.question(operator);
    let answer = eval(left + operator + right);

    /*０除算の防止、０未満・小数点ありの答えの防止 */
    if (
      operator === '/' && (right === 0 || left === 0)
      || answer < 0
      || !(Number.isInteger(answer))
    ) {
      return this.question(operator);
    }
    super.addCorrect(answer);
    /* 計算式用に記号を変化 */
    switch (operator) {
      case '*':
        operator = '×';
        break;
      case '/':
        operator = '÷';
        break;
      default:
        break;
    }
    return left + operator + right ;
  }

  /**
   * Nバック用の計算式出力
   * 1桁同士の加減算を出題します
   * @returns {string} 提案された演算式
   */
  questionNBack() {
    var operator = this.operator();

    let left = Math.floor(Math.random() * 10);
    let right = Math.floor(Math.random() * 10);
    if (operator === '/' || operator === '*') //乗除算はNG
      return this.questionNBack();
    let answer = eval(left + operator + right);

    if (answer < 0 || answer >= 10) //0未満・2桁の答えの防止
      return this.questionNBack();

    super.addCorrect(answer);
    return left + operator + right ;
  }

  /**
   * 演算子をランダムに抽出します。
   * @return {string} 演算子
   */
  operator() {
    /* 重みづけした演算子配列 */
    const opes = [['+', 1], ['-', 1], ['*', 4], ['/', 4]];
    if (this.level >= 2) {
    /* 重みづけの上演算子を抽出する */
    let arr = [];
      opes.forEach((v, i, a) => {
        for (let index = 0; index < v[1]; index++) {
          arr.push(v[0]);
        }
      });
      return arr[Math.floor(Math.random() * arr.length)];
    }
    return opes[Math.floor(Math.random() * 4)][0];
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.score += 1;
    this.correctCount += 1;
  }

  /* 不正解時ペナルティ */
  penalty() {
    if (this.score <= 0)
      return;
    this.score -= 1;
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

}
/* harmony export (immutable) */ __webpack_exports__["a"] = CalcScorer;



/***/ })
]);
//# sourceMappingURL=0.js.map