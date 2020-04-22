/**
 * [Component] 6パネル
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class SixPanel {

  /**
   * [p5,p5dom]6パネル
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Array} letters - 6パネルに表示させる20個の内容を持った配列
   * @param {Object} [options]
   */
  constructor(p, letters, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.isOver = false;

    // 6パネル用のDivを作成
    let sixPanel = p.createDiv('');

    letters.forEach((value, index) => {
      sixPanel.child(this.createButton(value, index))
    });

    sixPanel.id("sixPanel")
      .style('position', 'fixed')
      // .style('top', '55vh')
      .style('bottom', '5vh')
      .style('width', (95 / 100) * this.maxWidth + 'px')
      .style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + 'px')
      .style('text-align', 'center')
    ;
  }

  /**
   * 各キー作成用
   * [p5dom]createButtonメソッド
   * @param {string} letter - キーに表示する値
   * @param {number} id - id
   * @returns {p5.dom} button dom element
   */
  createButton(letter, id) {
    const p = this.p5;
    let button = p.createButton(letter, letter);

    button.id(id)
      .style('padding', '0px 0px')
      .style('width', (30 / 100) * this.maxWidth + 'px')
      .style('height', '15vh')
      .style('box-sizing', 'content-box')
      .style('font-size', (10 / 100) * this.maxWidth + 'px')
      .style('background-color', this.p5.theme.color.domKeyBackground)
      .style('color', this.p5.theme.color.domKeyText)
      .style('cursor', 'pointer')
      .style('outline', 'solid 3px')
      .style('outline-offset', '-2px')
      .style('outline-color', this.p5.theme.color.domKeyOutline)
      .style('margin', '0px')
      .style('vertical-align', 'bottom')
      .style('font-family', 'LocalFont')
      .style('border', '0px')
      .class('ripple')
      .mousePressed(this.options.mouseClicked)
    ;
    return button;
  }

}

