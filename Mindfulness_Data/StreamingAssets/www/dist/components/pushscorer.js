import {Scorer} from "./scorer.js";

/**
 * [Component] 順番押し・交互押し用スコアラー
 * @property {number} count - 現在対象のindex
 */
export class PushScorer extends Scorer {

  /**
   * 順番押し・交互押し用スコアラー
   * @param {p5} p
   * @param {Array} letters - 順序配列
   */
  constructor(p, letters) {
    super();
    for (let i = letters.length - 1; i >= 0; i--) {
      super.addCorrect(letters[i]);
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
    this.score += 1;
    this.count += 1;
  }

  /* 失敗時ペナルティ */
  penalty() {
    if (this.score <= 0)
      return;
    this.score -= 1;
  }

  /* カウントの初期化 */
  resetCount() {
    this.count = 0;
  }

}
