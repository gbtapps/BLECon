import {Scorer} from "./scorer.js";

export class TelephoneScorer extends Scorer {
  /**
   * コンストラクタ
   * @param {number} level -
   */
  constructor(level = 1) {
    super();
    this.level = level;
    this.number = "";
    this.count = 0;
  }

  start() {
    this.prevTime = Date.now();
  }

  /**
   * 11桁の番号を返します。
   * @returns {string} 提案された電話番号
   */
  question(digitNum = 11) {
    let telnums = [];
    [1,2,3,4,5,6,7,8,9,10,11].forEach((v, i)=>{
      telnums.push(Math.floor(Math.random() * 10))
    });
    telnums.forEach((v, i, a)=>{
      super.addCorrect(v);
    });
    this.number = telnums.join("");
    return this.number;
  }

  /**
   * 正答数のカウント
   */
  plusCorrectCount() {
    this.isCorrects.push(true);
    this.responseTimes.push(Date.now()-this.prevTime);
    this.prevTime = Date.now();
    this.score += 1;
    this.count += 1;
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
   * 出題された数字をすべて答えたかどうか
   * @returns true:すべて押された false:まだ押されていないスプライトがある
   */
  isAllAnswered() {
    return this.count == this.corrects.length;
  }

  /* カウントの初期化 */
  resetCount() {
    this.count = 0;
    super.formatCorrect();
  }

  /* 不正解時ペナルティ */
  penalty() {
    if (this.score <= 0)
      return;
    this.score -= 1;
    this.isCorrects.push(false);
    this.responseTimes.push(Date.now()-this.prevTime);
    this.prevTime = Date.now();
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

  /* 何回正解の番号を押せたかを返します */
  get numOfCorrect() {
    let count = 0;
    this.isCorrects.forEach((v, i)=>{
      if(v === true){
        count += 1;
      }
    })
    return count;
  }

}
