import { Training } from './training.js';
import { TenKey } from '../components/tenkey.js';
import { Calcurator } from '../components/calcurator.js';
import { Target } from '../components/target.js';
import { CalcScorer } from '../components/calcscorer.js';
import { HourGlass } from '../components/hourglass.js';
import { CorrectSound } from '../components/sounds/correctsound.js';
import { WrongSound } from '../components/sounds/wrongsound.js';
import *  as theme from '../../config/theme.json';

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
    this.$p5.theme = theme;
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

    this.hourGlass = new HourGlass(this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.target = new Target(this.$p5, true);
    this.calcurator = new Calcurator(this.$p5);
    this.calcScorer = new CalcScorer(this.level);
    this.correctSound = new CorrectSound(this.$p5);
    this.wrongSound = new WrongSound(this.$p5);
    this.tenKey = new TenKey(this.$p5, {
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

export class NFB001 extends Training{

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
