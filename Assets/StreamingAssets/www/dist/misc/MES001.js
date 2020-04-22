import { HourGlass } from "../components/hourglass.js";
import * as theme    from "../../config/theme.json";
import {HitWallSound}     from '../components/sounds/hitwallsound.js';
import {Alert}       from '../components/sounds/alert.js';
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
    this.$p5.theme = theme;
    this.hourGlass = new HourGlass(this.$p5, time_limit, {isReturnEnable:false, isChangeColor:true});
    this.maxWidth = (p5.windowWidth < p5.windowHeight)
    ? p5.windowWidth
    : p5.pow(p5.windowHeight, 2) / p5.windowWidth;
    this.textSize = this.maxWidth / 7;
    this.hitWallSound = new HitWallSound(this.$p5);
    this.alert = new Alert(this.$p5);
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

export class MeasureTimer {
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
