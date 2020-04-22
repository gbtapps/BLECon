import {Scorer} from "./scorer.js";

/**
 * [Component] 順番記憶・数字記憶・音階記憶用スコアラー
 * @property {number} count - 現在対象のindex
 */
export class MemoryCheckScorer extends Scorer {

  /**
   * 順番記憶・数字記憶・音階記憶用スコアラー
   * @param {p5} p
   * @param {Array} letters - 順序配列
   */
  constructor(p, letters) {
    super();
    this.count = 0;
  }

  start() {
    this.prevTime = Date.now();
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
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.count += 1;
  }

  plusCount(isCorrect) {
    this.isCorrects.push(isCorrect);
    this.responseTimes.push(Date.now()-this.prevTime);
  }

  /**
   * 出題された数字をすべて答えたかどうか
   * @returns {boolean} true:すべて押された false:まだ押されていないスプライトがある
   */
  isAllAnswered() {
    return this.count == this.corrects.length;
  }

  /* カウントの初期化 */
  resetCount() {
    this.count = 0;
  }

  /* スコアに今回のカウントを反映 */
  addScore(isMiss = true) {
    this.score += this.corrects.length - (isMiss ?1 :0);
  }

  /**
   * 全試行のスコアの平均値を求める
   * @param {number} tryNum - 試行回数
   */
  calcResult(tryNum) {
    return Math.round(this.score / tryNum);
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
