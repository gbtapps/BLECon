import { Training } from './training.js';
import { HourGlass } from '../components/hourglass.js';
import { WrongSound } from '../components/sounds/wrongsound.js';
import { CorrectSound } from '../components/sounds/correctsound.js';
import { MemoryScorer } from '../components/memoryscorer.js';
import { TwentyGrid } from '../components/twentygrid.js';
import { FlashSound } from '../components/sounds/flashsound.js';
import *  as theme from '../../config/theme.json';

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
    this.$p5.theme = theme;
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

    this.hourGlass = new HourGlass(this.$p5, time_limit, {isReturnEnable:this.$p5.theme.option.isReturnEnable, isChangeColor:true});
    this.memoryScorer = new MemoryScorer();
    this.wrongSound = new WrongSound(this.$p5);
    this.correctSound = new CorrectSound(this.$p5);
    this.flashSound = new FlashSound(this.$p5);
    this.twentyGrid = new TwentyGrid(
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

export class NFB004 extends Training{

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
