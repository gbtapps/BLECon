/**
 * [Component] 電話番号表示
 * Libries: p5,p5dom
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
const startTag = "<span style=\"color:#FFFFFF\">";
const endTag   = "</span>";

export class TelephoneNumber {

  /**
   * [p5dom] 表示領域の確保
   * @public
   * @param {p5} p
   */
  constructor(p, options = null) {
    this.p5 = p;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.options = options;
    let target = p.createDiv('');

    target.id("telnum")
      .style('position', 'fixed')
      .style('width', this.maxWidth + 'px')
      .style('height', '15vh')
      .style('top', '15vh')
      .style('left', p.width / 2 - (this.maxWidth/2) + 'px')
      .style('font-size', (15 / 100) * this.maxWidth + 'px')
      .style('color', p.theme.color.primaryText)
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('font-family', 'LocalFont')
    ;
    if (this.options.dualSetting) {
      switch(this.options.dualSetting.whichSide){
        case "right":
          target.style('left', p.width / 2 - (this.maxWidth/2) + this.maxWidth/2 + 'px');
          break;
        case "left":
          target.style('left', p.width / 2 - (this.maxWidth/2) - this.maxWidth/2 + 'px');
          break;
      }
    }
    if (this.options.fontSize) {
      target.style('font-size', (this.options.fontSize / 100) * this.maxWidth + 'px')
    }

  }
  /**
   * 出題するvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  update(value, count = 0) {
    let values = value.split('');
    values[2] = values[2] + "-";
    values[6] = values[6] + "-";
    values.unshift(startTag);
    values.splice(count+1, 0, endTag);
    let telNum = values.join("");
    this.p5.select("#telnum").html(telNum);
  }

  get setTemplate() {

  }
}
