import {Scorer} from "./scorer.js";

/**
 * [Component] 集中持続力チェック用スコアラー
 * @property {number} setCount - 定義された文字セットの数
 * @property {number} count - 現在対象のindex
 */
export class PersistenceCheckScorer extends Scorer {

  /**
   * チェックテスト版順番押し・交互押し用スコアラー
   * @param {p5} p
   */
  constructor(p) {
    super();

    this.answers = [];
  }

  addMetrics(isCorrect, isTimeout = false) {
    this.isCorrects.push(isCorrect);

    if (isTimeout) {
      this.responseTimes.push(-1);
      return;
    }
    this.responseTimes.push(Date.now()-this.prevTime);
  }

  /* 開始時処理 */
  start() {
    this.prevTime = Date.now();
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
