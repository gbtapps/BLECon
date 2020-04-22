/**
 * [Component] スライダー
 * @property {p5} p5
 * @property {number} distance - 青円赤円を描画する、中心からの距離
 */

export class Slider {

  /**
   * スライダーを作成
   * @param {p5} p
   * @param {number} top - 配置する高さ
   * @param {number} behind - さかのぼる日数
   */
  constructor(p, top, behind, margin, isXMode = false) {
    this.p5 = p;
    this.isXMode = isXMode;
    this.margin = margin;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.behind = behind;
    this.top = top;
    this.size = this.maxWidth * (7 / 10) + (this.p5.width - this.maxWidth) / 4;
    /* スライダー */
    this.elm = p.createSlider(0, 100, 0)
      .id("slider")
      .class("input-range")
      .attribute("list", "exlist")
      .position(0, this.top)
      .size(this.size)
      .style("height", "4vh")
      .style("margin-left", this.p5.width / 2 - this.size / 2 + "px")
      .style("margin-top", this.p5.height*(this.margin/ 100) + "px")
    ;
    /* 値を表示 */
    // this.view = p.createSpan("")
    //   .position(
    //     this.maxWidth * (8.7 / 10) + (this.p5.width - this.maxWidth) * (6.5 / 10),
    //     this.top + this.p5.height * (this.margin / 100)
    //   )
    //   .style("position", "fixed")
    //   .style("width", (this.maxWidth * (10 / 100) + (this.p5.windowWidth - this.maxWidth) / 80) + "px")
    //   .style("height", "4vh")
    //   .style("margin-top", "-1px")
    //   .style("font-size", "2.5vh")
    //   .style("font-family", "LocalFont")
    //   .style("line-height", "4vh")
    //   .style("border-radius", "5px")
    //   .style("text-align", "center")
    //   .style("background", this.p5.theme.color.primary)
    //   .style("color", this.p5.theme.color.primaryText)
    // ;
  }

  /* ラベルテキストを描画します */
  drawText() {
    this.p5.noStroke();
    this.p5.fill(this.p5.theme.color.primaryText);
    this.p5.text(
      "全く思いだせない",
      this.p5.width / 2 - this.maxWidth * (7 / 20) - (this.p5.width - this.maxWidth) / 8,
      this.top
    );
    this.p5.text(
      "完全に思いだせる",
      this.p5.width / 2 + this.maxWidth * (7 / 20) + (this.p5.width - this.maxWidth) / 8,
      this.top
    );
    if (!this.isXMode) {
      this.p5.text(
        (this.behind === 1)
          ? "昨夜"
          : this.behind + "日前",
          this.p5.width / 2 - this.maxWidth * (9 / 20) - (this.p5.width - this.maxWidth) / 8,
        this.top - this.p5.height / 40
      );
    }
  }

  /**
   * 目盛を描画します
   */
  drawScale() {
    let bottom = this.top + this.p5.height * ((2.5 + this.margin) / 100);
    this.p5.stroke(this.p5.theme.color.shadow);
    this.p5.fill(this.p5.theme.color.shadow);

    this.p5.line(
      this.p5.width / 2,
      bottom,
      this.p5.width / 2,
      bottom + this.p5.height / 50
    );
    this.p5.line(
      this.p5.width / 2 + this.size / 4 - this.p5.height * (1 / 100),
      bottom + this.p5.height / 100,
      this.p5.width / 2 + this.size / 4 - this.p5.height * (1 / 100),
      bottom + this.p5.height / 50
    );
    this.p5.line(
      this.p5.width / 2 - this.size / 4 + this.p5.height * (1 / 100),
      bottom + this.p5.height / 100,
      this.p5.width / 2 - this.size / 4 + this.p5.height * (1 / 100),
      bottom + this.p5.height / 50
    );
    this.p5.line(
      this.p5.width / 2 + this.size / 2 - this.p5.height * (1 / 50),
      bottom,
      this.p5.width / 2 + this.size / 2 - this.p5.height * (1 / 50),
      bottom + this.p5.height / 50
    );
    this.p5.line(
      this.p5.width / 2 - this.size / 2 + this.p5.height * (1 / 50),
      bottom,
      this.p5.width / 2 - this.size / 2 + this.p5.height * (1 / 50),
      bottom + this.p5.height / 50
    );
  }

  /**
   * スコアを取得
   * @return {number} スコア
   */
  get score() {
    return this.elm.value() * (2 ** (this.behind - 1)) / 100;
  }

  /**
   * スライダーの値を取得(0~100)
   * @return スライダーの値
   */
  get value() {
    return this.elm.value();
  }

  /**
   * スライダーに値を入力
   * ページ移動時の初期表示用
   * @param {number} letter
   */
  setValue(letter) {
    this.elm.value(letter);
  }

}
