import {Scorer} from "./scorer.js";

/**
 * [Component] エピソード記憶用スコアラー
 * @property {Array} list - 各項目の値を管理する配列
 * @property {number} currentPage - 現在のページ数
 */
export class EpisodeMemoryScorer extends Scorer {

  /**
   * エピソード記憶用スコアラー
   * @param {p5} p
   */
  constructor(p) {
    super();
    this.list = [0, 0, 0, 0, 0, 0, 0];
    this.currentPage = 1;
    this.responseTimes = [0, 0, 0, 0, 0, 0, 0];
  }

  start() {
    this.prevTime = Date.now();
  }

  /**
   * リストの値を更新します
   * @param {number} value -更新する値
   */
  update(value) {
    let tmpScore = 0;
    this.list[this.currentPage - 1] = value;
    this.list.forEach((v, i, a) => {
      tmpScore += v * (2 ** i) / 100;
    });
    this.score = tmpScore;

    var now = Date.now();
    this.responseTimes[this.currentPage-1] += (now - this.prevTime);
    this.prevTime = now;
  }

  /** 根拠値 */
  get metric(){
    return {
      /** 各スコア */
      answers: this.list,
      /** 反応時間 */
      responseTimes: this.responseTimes
    };
  }

}
