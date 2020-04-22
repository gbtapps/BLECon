import {Scorer} from "./scorer.js";

/**
 * [Component] チェックテスト版順番押し・交互押し用スコアラー
 * @property {number} setCount - 定義された文字セットの数
 * @property {number} count - 現在対象のindex
 */
export class FacilityPushScorer extends Scorer {

  /**
   * チェックテスト版順番押し・交互押し用スコアラー
   * @param {p5} p
   * @param {Array} letters - 順序配列
   */
  constructor(p, letters) {
    super();
    this.setCount = 0;
    for (let j = letters.length - 1; j >= 0; j--) {
      for (let i = letters[j].length - 1; i >= 0; i--) {
        super.addCorrect(letters[j][i]);
      }
      this.setCount += 1;
    }

    this.count = 0;
  }

  /**
   * 回答が正解かどうか
   * @param {string} answer - 回答
   * @returns true:正解 false:不正解
   */
  isCorrectCount(answer) {
    return super.isCorrect(this.count, answer);
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.count += 1;
  }

  plusCount(isCorrect) {
    if(isCorrect) {
      this.count += 1;
    }
    this.isCorrects.push(isCorrect);
    // this.responseTimes.push(Date.now()-this.prevTime);
    var now = Date.now();
    this.responseTimes.push(now - this.prevTime);
    this.prevTime = now;
  }

  /* カウントの初期化 */
  resetCount() {
    this.count = 0;
  }

  /* 開始時処理 */
  start(p5) {
    this.prevTime = Date.now();
    this.startTime = p5.millis();
  }

  /* 終了時処理 */
  end(p5) {
    this.totalTime = p5.millis() - this.startTime;
  }

  /* 総合タイムを秒で返します */
  get result() {
    return Math.floor(this.totalTime / 1000);
  }


  /** 根拠値 */
  get metric(){
    return {
      /** 回答〇× */
      answers: this.isCorrects,
      /** 反応時間 */
      responseTimes: this.responseTimes,
    };
  }


}
