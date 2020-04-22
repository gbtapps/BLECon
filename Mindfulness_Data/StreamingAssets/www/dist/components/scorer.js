
/**
 * [Component] スコアラー
 * @property {Array} corrects - 正解の配列
 * @property {number} score - スコア（間違うとペナルティをうけます）
 * @property {number} correctCount - 正答数
 *
 */
export class Scorer {

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
