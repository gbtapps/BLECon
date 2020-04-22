import { Scorer } from './scorer.js';

/**
 * タイミングスコアラー
 * @property {number} pushCount - クリックされた回数
 * @property {number} totalTime - 反応時間の総計
 * @property {Array} aveReactionArray - セッションごとの平均反応時間を格納する配列
 * @property {Array} resultArray - セッションごとの結果を格納する配列
 */
export class CheckTimingScorer extends Scorer {

  constructor() {
    super();
    this.totalTime = 0;
    this.pushCount = 0;
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

  /**
   * スコア判定メソッド
   * @param {number} diff - 予測のズレ時間
   */
  addReactionTime(diff) {
    this.pushCount += 1;
    this.totalTime += diff;
  }

  /* 失敗時ペナルティ */
  penalty() {
    this.totalTime += 500;
  }

  /**
   * 平均反応時間を取得
   * @returns {number} 平均反応時間
   */
  get aveReaction() {
    if (this.pushCount === 0){
      this.pushCount = 1;
    }
    return this.totalTime / this.pushCount;
  }

  // /**
  //  * 平均反応時間の逆数をスコアとして取得
  //  * @return {number} 最大10、最小1の整数値
  //  */
  // get result() {
  //   let result = 1 / (this.aveReaction / 1000);
  //   return Math.round(Math.min(Math.max(result, 1), 10));
  // }

  // /**
  //  * 1秒から平均反応時間を引いた数値を10段階のスコアにして返します
  //  * 反射神経系に使えるかもしれません
  //  * @returns {number} スコア(1~10)
  //  */
  // get result2() {
  //   return Math.min(
  //     Math.max(
  //       10 - Math.floor(this.aveReaction / 100),
  //       1
  //     ),
  //     10
  //   );
  // }

  /**
   * 2秒から平均反応時間を引いた数値を10段階のスコアにして返します
   * @param {number} 平均反応時間
   * @returns {number} スコア(1~10)
   */
  getResult(aveReaction){
    return Math.min(
      Math.max(
        20 - Math.floor(aveReaction / 100),
        1
      ),
      10
    );
  }

  /* プロパティを初期値に戻します */
  reset() {
    this.pushCount = 0;
    this.totalTime = 0;
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
