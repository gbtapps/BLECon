import {HourGlass} from '../components/hourglass.js';
import *  as theme from '../../config/theme.json';

/**
 * ベースライン測定
 * Libries: p5
 * @property {HourGlass} hourglass - 砂時計
 * @property {string} message - 表示するメッセージ
 * @property {number} alpha - メッセージの透明度
 * @property {number} changeTime - メッセージが変更された時間
 * @property {number} interval - 次のメッセージに変更するまでの時間
 * @property {boolean} isInhaling - 現在のメッセージが「吸う」メッセージであるかどうか
 * @property {number} maxWidth - 描画対象とする横幅
 * @property {Array:JSON data} theme - カスタマイズテーマ
 */
class Countdown {

  /**
   * 安静処理
   * @param {String} mode - "doNF", "noNF"
   * @param {Number} time - 安静時間（秒）
   */
  constructor(p5, mode, time) {
    this.$p5 = p5;
    this.mode = mode;
    this.time = time;
    this.waitingForReady = new Promise((resolve, reject) => {
      this.ready = resolve;
    });
    this.waitingForStopped = new Promise((resolve, reject) => {
      this.stop = resolve;
    });
    this.maxWidth = (p5.windowWidth < p5.windowHeight)
      ? p5.windowWidth
      : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.textSize = this.maxWidth/15;
    this.$p5.theme = theme;
    this.diameter = p5.windowHeight * 0.75;
    this.hourGlass = new HourGlass(this.$p5, time*1000, false);


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
    this.$p5.isReady = true;
    if (this.$p5.theme.font)
      this.$p5.textFont(this.font);

    this.timer = this.$p5.createDiv('')
      .style('position', 'fixed')
      .style('font-size', '15vh')
      .style('width', '12vw')
      .style('height', '20vh')
      .style('color', this.$p5.theme.color.primaryText)
      // .style('background', "#FFFFFF")
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '35vh')
      .style('left', '44vw')
      .style('line-height', '20vh')
      .style('text-shadow', 'white 2px 2px 0px')

    ;
    this.message = this.$p5.createDiv('')
      .style("position", "fixed")
      .style("font-size", "4.5vh")
      .style('width', '60vw')
      .style('height', '20vh')
      .style("color", this.$p5.theme.color.primaryText)
      // .style("background", "#FFFFFF")
      .style("left", "20vw")
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('top', '55vh')
      .style('text-shadow', 'white 1px 1px 0px')
    ;

    this.ready();
  }

  /* [p5] スタートボタン押下 */
  start() {
    this.hourGlass.start();

    switch(this.mode) {
      case "doNF":
        this.message.html("リラックスして、呼吸の数を数えながら、<br>深い呼吸を続けてください。<br>できるだけ青い色をキープしましょう。")
        break;
      case "noNF":
        this.message.html("リラックスして、呼吸の数を数えながら、<br>深い呼吸を続けてください。")
        break;
    }
  }

  /* [p5] draw処理 */
  draw() {
    this.$p5.background(this.$p5.theme.color.background);
    if(this.mode==="doNF"){
      this.$p5.noCanvas();
      this.$p5.createCanvas(this.$p5.windowWidth,this.$p5.windowHeight);
    }

    this.position = {
      left: this.$p5.windowWidth / 2,
      top: this.$p5.windowHeight / 2
    };
    this.$p5.textAlign(this.$p5.CENTER);
    if (!this.hourGlass.isOver) {
      this.hourGlass.renderGlass(false);
      if (!isNaN(this.hourGlass.limitSec)) {
        this.timer.html(this.time - Math.floor(this.hourGlass.limitSec))
      }
      return;
    }
    this.$p5.noLoop();
    this.$p5.noCanvas();
    this.$p5.remove();

    this.stop();

  }



}

export class CountdownGuide {
  async start(time) {
    new p5(($p5) => {
      this.training = new Countdown($p5, "doNF", time);
      $p5.preload = this.training.preload.bind(this.training);
      $p5.setup = this.training.setup.bind(this.training);
      $p5.draw = this.training.draw.bind(this.training);
    }, "content");
    await this.training.waitingForReady;

    this.training.start();

    await this.training.waitingForStopped;
    // if (typeof Android !== "undefined")
    //   Android.startTraining();
  }
}
// new BaselineGuide().start();
// var x = new XMLHttpRequest();
// x.addEventListener("load", () => {
//   App={
//     theme: JSON.parse(x.responseText)
//   };
// });
// x.open("GET", "./config/theme.json");
// x.send();
