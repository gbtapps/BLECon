import {Scorer} from "./scorer.js";

export class CalcScorer extends Scorer {
  /**
   * コンストラクタ
   * @param {number} level - 3段階 符号の出題割合と問題桁数に関与
   */
  constructor(level = 1) {
    super();
    this.level = level;
  }

  /**
   * 1桁どうしの四則演算を出題します。
   * 数字はランダムに抽出されます。
   * @param {string} [operator] - 演算子
   * @returns {string} 提案された演算式
   */
  question(operator = null) {

    if (operator === null)
      var operator = this.operator();
    let left = (this.level >= 3)
      ? Math.floor(Math.random() * 20)
      : Math.floor(Math.random() * 10);
    let right = Math.floor(Math.random() * 10);
    if (left === 0 && right === 0 && operator === '/')
      return this.question(operator);
    let answer = eval(left + operator + right);

    /*０除算の防止、０未満・小数点ありの答えの防止 */
    if (
      operator === '/' && (right === 0 || left === 0)
      || answer < 0
      || !(Number.isInteger(answer))
    ) {
      return this.question(operator);
    }
    super.addCorrect(answer);
    /* 計算式用に記号を変化 */
    switch (operator) {
      case '*':
        operator = '×';
        break;
      case '/':
        operator = '÷';
        break;
      default:
        break;
    }
    return left + operator + right ;
  }

  /**
   * Nバック用の計算式出力
   * 1桁同士の加減算を出題します
   * @returns {string} 提案された演算式
   */
  questionNBack() {
    var operator = this.operator();

    let left = Math.floor(Math.random() * 10);
    let right = Math.floor(Math.random() * 10);
    if (operator === '/' || operator === '*') //乗除算はNG
      return this.questionNBack();
    let answer = eval(left + operator + right);

    if (answer < 0 || answer >= 10) //0未満・2桁の答えの防止
      return this.questionNBack();

    super.addCorrect(answer);
    return left + operator + right ;
  }

  /**
   * 演算子をランダムに抽出します。
   * @return {string} 演算子
   */
  operator() {
    /* 重みづけした演算子配列 */
    const opes = [['+', 1], ['-', 1], ['*', 4], ['/', 4]];
    if (this.level >= 2) {
    /* 重みづけの上演算子を抽出する */
    let arr = [];
      opes.forEach((v, i, a) => {
        for (let index = 0; index < v[1]; index++) {
          arr.push(v[0]);
        }
      });
      return arr[Math.floor(Math.random() * arr.length)];
    }
    return opes[Math.floor(Math.random() * 4)][0];
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.score += 1;
    this.correctCount += 1;
  }

  /* 不正解時ペナルティ */
  penalty() {
    if (this.score <= 0)
      return;
    this.score -= 1;
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

}
