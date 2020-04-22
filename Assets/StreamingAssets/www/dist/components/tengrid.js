/**
 * [Component] 10方眼
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class TenGrid {

  /**
   * [p5,p5dom]10方眼
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Array} letters - 10方眼に表示させる10個の内容を持った配列
   * @param {Object} options
   */
  constructor(p, letters, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;

    // 10方眼用のDivを作成
    let tengrid = p.createDiv('');

    letters.forEach((value, index) => {
      tengrid.child(this.createButton(value, index))
    });

    tengrid.id("tengrid")
      .style('position', 'fixed')
      // .style('top', '75vh')
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
    let button = p.createButton(letter);

    button.id(id)
      .style('padding', '0px 0px')
      .style('width', (19 / 100) * this.maxWidth + 'px')
      .style('height', '12vh')
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
      .style('border-radius', this.p5.theme.tengrid.borderRadius)
      .class('ripple')      
      .mousePressed(this.options.mouseClicked)
    ;
    return button;
  }
}

