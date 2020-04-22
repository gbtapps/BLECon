/**
 * [Component] ターゲット
 * Libries: p5,p5dom
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class Target {

  /**
   * [p5dom] 表示領域の確保
   * @public
   * @param {p5} p
   */
  constructor(p, isDisplayNext = false) {
    this.p5 = p;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.target = p.createDiv('');
    this.next = p.createDiv('');
    this.doubleNext = p.createDiv('');
    this.isLandscape = p.windowWidth > p.windowHeight


    this.target.id("target")
      .style('position', 'fixed')
      .style('width', '100vw')
      .style('height', '15vh')
      .style('top', '15vh')
      .style('font-size', (15 / 100) * this.maxWidth + 'px')
      .style('color', '#616161')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('margin', 'auto')
      .style('font-family', 'LocalFont')
      .style('line-height', '15vh')
    ;
    if(this.isLandscape){
      this.target.style('width', '14vw')
      .style('top', '50vh')
      .style('left', '13vw')
      .style('outline', 'solid 2px')
      .style('outline-offset', '-1px')
      .style('outline-color', '#616161')
    }

    if (!isDisplayNext) return;

    this.doubleNext.id('doubleNext')
      .style('position', 'fixed')
      .style('width', (25 / 100) * this.maxWidth + 'px')
      .style('top', '1vh')
      .style('left', p.width / 2 + (23 / 100) * this.maxWidth + 'px')
      .style('color', 'rgba(255, 255, 255, 0)')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('font-family', 'LocalFont')
      .style('padding', '0px 0px')
      .style('height', '10vh')
      .style('box-sizing', 'content-box')
      .style('font-size', (9 / 100) * this.maxWidth + 'px')
      // .style('outline', 'solid 2px')
      // .style('outline-offset', '-1px')
      // .style('outline-color', '#616161')
      .style('line-height', '10vh')
      .style('border', '0px')
    ;

    this.next.id("next")
      .style('position', 'fixed')
      .style('width', (25 / 100) * this.maxWidth + 'px')
      .style('top', '12vh')
      .style('left', '73vw')
      .style('color', 'rgba(255, 255, 255, 0)')
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('text-align', 'center')
      .style('font-family', 'LocalFont')
      .style('padding', '0px 0px')
      .style('height', '10vh')
      .style('box-sizing', 'content-box')
      .style('font-size', (9 / 100) * this.maxWidth + 'px')
      // .style('outline', 'solid 2px')
      // .style('outline-offset', '-1px')
      // .style('outline-color', '#616161')
      .style('line-height', '10vh')
      .style('border', '0px')
    ;

    if(this.isLandscape){
      this.next.style('top', '37vh')
      .style('height', '12vh')
      .style('width', '14vw')
      .style('left', '13vw')

      this.doubleNext.style('top', '24vh')
      .style('height', '12vh')
      .style('width', '14vw')
      .style('left', '13vw')
    }

  }
  /**
   * 出題するvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  update(value) {
    this.p5.select("#target").html(value);
  }

  /**
   * 1問次のvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  updateNext(value) {
    this.p5.select("#next").html("(" + value + ")");
  }

  /**
   * 2問次のvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（記号や計算式など）
   */
  updateDoubleNext(value) {
    this.p5.select("#doubleNext").html("(" + value + ")");
  }


  get setTemplate() {

  }
}
