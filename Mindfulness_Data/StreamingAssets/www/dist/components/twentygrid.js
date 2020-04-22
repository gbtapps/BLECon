/**
 * [Component] 20方眼
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class TwentyGrid {

  /**
   * [p5,p5dom]20方眼
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Array} letters - 20方眼に表示させる20個の内容を持った配列
   * @param {Object} [options]
   */
  constructor(p, letters, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.isOver = false;
    this.isLandscape = p.windowWidth > p.windowHeight;

    // 20方眼用のDivを作成
    let twentygrid = p.createDiv('');

    letters.forEach((value, index) => {
      twentygrid.child(this.createButton(value, index))
    });

    twentygrid.id("twentygrid")
      .style('position', 'fixed')
      .style('text-align', 'center')
    ;
    if(this.isLandscape){
      twentygrid.style('width', '45vw')
        .style('right', '2.5vw')
        .style('bottom', '15vh')
    }else{
      twentygrid.style('bottom', '5vh')
        .style('width', (95 / 100) * this.maxWidth + 'px')
        .style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + 'px')
    }
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
      .style('box-sizing', 'content-box')
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
    if(this.isLandscape){
      button.style('width', '9vw')
        .style('height', '17.5vh')
        .style('line-height', '17.5vh')
        .style('font-size', '10vh')
      } else {
      button.style('width', (19 / 100) * this.maxWidth + 'px')
        .style('height', '11vh')
        .style('line-height', '11vh')
        .style('font-size', (10 / 100) * this.maxWidth + 'px')
    }

    return button;
  }

  /**
   * 正解配列点滅
   * @param {Array} list - 点滅させる要素のidが順番に格納された配列
   * @param {CorrectSound} sound - 点滅毎に鳴らす音
   * @param {string} marker - 表示する文字
   * @param {number} level - 3段階 出題間隔に関与
   */
  flashCorrects(list, sound, marker, level) {

    for (let i = 0; i < list.length; i++) {
      setTimeout(() => {
        if (this.isOver) return;
        this.flashPoint(list[i], 1000 / level, marker);
        sound.playSound();
      }, (1000 / level + 500) * i);
    }
  }

  /**
   * [p5dom]指定id要素の点滅
   * @param {number} id - 点滅させる要素のid
   * @param {number} time - 点灯時間(ミリ秒)
   * @param {string} marker - 表示する文字
   */
  flashPoint(id, time, marker) {
    this.p5.select("#" + id).html(marker);
    setTimeout(() => {
      if (this.isOver) return;
      this.p5.select("#" + id).html("");
    }, time);
  }

}

