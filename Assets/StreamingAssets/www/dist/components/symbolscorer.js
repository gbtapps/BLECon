import {Scorer} from "./scorer.js";

/**
 * [Component] 記号探し用スコアラー
 * @property {Array} symbolList - 使用する記号の一覧
 */
export class SymbolScorer extends Scorer {

  /**
   * [p5dom]記号探し用スコアラー
   * @param {p5} p
   * @param {Array} symbolList - 表示する記号が格納された配列
   */
  constructor(p, symbolList) {
    super();
    this.p5 = p;
    this.symbolList = symbolList;
  }

  /**
   * 問題出題
   * symbolListからランダムで記号を抽出し、
   * Scorerのcorrectsに追加する。
   * @param {number} length - 出題する記号の数
   * @returns {Array} 出題された記号の配列
   */
  question(length) {
    let tempList = [];
    this.symbolList.forEach((v, i, a) => {
      tempList[i] = v;
    });
    let correctSymbols = (new Array(length)).fill("");
    correctSymbols.forEach((v, i, a) => {
      correctSymbols[i] = tempList.splice(Math.floor(Math.random() * tempList.length), 1)[0];
      this.addCorrect(correctSymbols[i]);
    });
    return correctSymbols;
  }

  /**
   * [p5dom]出題記号が方眼内にいくつ存在しているか
   * @param {number} length - 方眼の数
   * @returns {number} 残りの記号数
   */
  remainingSymbols(length) {
    let count = 0;
    for (let index = 0; index < length; index++) {
      if (this.isIncludeCorrect(this.p5.select("#" + index).html())) {
        count += 1;
      }
    }
    return count;
  }

  /**
   * [p5dom]方眼内の記号を更新
   * @param length 方眼の数
   */
  shuffleSymbols(length) {
    /* symbolListを元に、ランダムな20個の記号配列を生成 */
    let symbolArray = (new Array(length)).fill("");
    symbolArray.forEach((v, i, a) => {
      symbolArray[i] = this.symbolList[Math.floor(Math.random() * this.symbolList.length)];
    });
    /* 記号配列を方眼に入れる */
    for (let index = 0; index < length; index++) {
      this.p5.select("#" + index).html(symbolArray[index]).style('background-color', '#FFFFFF');
    }
    /* 方眼内に出題記号が5つ以上ない場合、作り直し */
    if (this.remainingSymbols(length) < 5){
      this.shuffleSymbols(length);
    }
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.score += 1;
    this.correctCount += 1;
  }

  /* 失敗時ペナルティ */
  penalty() {
    if (this.score <= 0)
      return;
    this.score--;
  }

}
