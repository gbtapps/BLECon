/**
 * [Component] 練習中メッセージ表示
 * Libries: p5
 * @property {p5} p5
 * @property {string} message - 表示するメッセージ
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class PracticeMessage {
  /**
   * 練習中メッセージ表示
   * @param {p5} p
   */
  constructor(p) {
    this.p5 = p;
    this.message = "練習中";
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
  }

  /**
   * [p5]描画
   * void
   */
  draw() {
    if (this.p5.frameCount % 15 == 0) {
      if (this.message == "練習中") {
        this.message = "";
      } else {
        this.message = "練習中";
      }
    }
    this.p5.textSize(this.maxWidth / 15);
    this.p5.textStyle(this.p5.BOLD);
    this.p5.textAlign(this.p5.RIGHT);
    this.p5.fill(this.p5.theme.color.primaryText);
    this.p5.text(this.message, this.p5.windowWidth / 2 + this.maxWidth / 2.1, this.p5.windowHeight / 5);
  }
};
