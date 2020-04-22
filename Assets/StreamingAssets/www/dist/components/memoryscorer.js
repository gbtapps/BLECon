import {Scorer} from "./scorer.js";

/**
 * [Component] 順番記憶・数字記憶・音階記憶用スコアラー
 * @property {number} count - 現在対象のindex
 */
export class MemoryScorer extends Scorer {

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
