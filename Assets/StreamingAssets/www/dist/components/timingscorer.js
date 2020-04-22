import { Scorer } from './scorer.js';

/**
 * タイミングスコアラー
 * @property {number} pushCount - クリックされた回数
 * @property {number} totalTime - 反応時間の総計
 */
export class TimingScorer extends Scorer {

  constructor() {
    super();
    this.totalTime = 0;
    this.pushCount = 0;
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

  /**
   * 平均反応時間から求められた結果を取得
   * @return {number} 最大10、最小1の整数値
   */
  get result() {
    let result = 1 / (this.aveReaction / 1000);
    return Math.round(Math.min(Math.max(result, 1), 10));
  }

  /**
   * 2秒から平均反応時間を引いた数値を10段階のスコアにして返します
   * 反射神経系に使えるかもしれません
   * @returns {number} スコア(1~10)
   */
  get result2() {
    return Math.min(
      Math.max(
        20 - Math.floor(this.aveReaction / 100),
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



}
