import *  as theme from '../../config/theme.json';
/**
 * [Component] 10keyキーボード
 * Libries: p5, p5dom
 * @property {p5} p5
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class TenKey {

  /**
   * [p5,p5dom] 10keyキーボード
   * Componentを生成し、画面下部に描画します。
   * @public
   * @param {p5} p
   * @param {Object} [options]
   */
  constructor(p, options = null) {
    this.p5 = p;
    this.options = options;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      // : p.pow(p.windowHeight, 2) / p.windowWidth;
      : p.windowWidth
    this.isLandscape = p.windowWidth > p.windowHeight;

    // 10キー用のDivを作成
    let tenkey = p.createDiv('');
    tenkey.child(this.createButton('', 's1'));
    tenkey.child(this.createButton("1", 1));
    tenkey.child(this.createButton("2", 2));
    tenkey.child(this.createButton("3", 3));
    if (this.options.numOnly) {
      tenkey.child(this.createButton('', 's2'));
    } else {
      tenkey.child(this.createButton('backspace', 'x', true));
    }
    tenkey.child(this.createButton('', 's3'));
    tenkey.child(this.createButton("4", 4));
    tenkey.child(this.createButton("5", 5));
    tenkey.child(this.createButton("6", 6));
    tenkey.child(this.createButton('', 's4'));
    tenkey.child(this.createButton('', 's5'));
    tenkey.child(this.createButton("7", 7));
    tenkey.child(this.createButton("8", 8));
    tenkey.child(this.createButton("9", 9));
    tenkey.child(this.createButton('', 's6'));
    tenkey.child(this.createButton('', 's7'));
    tenkey.child(this.createButton('', 's8'));
    tenkey.child(this.createButton("0", 0));
    tenkey.child(this.createButton('', 's9'));
    if (this.options.numOnly) {
      tenkey.child(this.createButton('', 's0'));
    } else {
      tenkey.child(this.createButton('keyboard_return', 'e', true));
      // tenkey.child(this.createButton('check_circle', 'e', true));
    }


    tenkey.id("tenkey")
      .style('position', 'fixed')
      .style('text-align', 'center')
      .style('margin', 'auto')
    ;
    if(this.isLandscape){
      tenkey.style('width', '45vw')
        .style('right', '2.5vw')
        .style('bottom', '15vh')
    }else{
      tenkey.style('bottom', '5vh')
        .style('width', (95 / 100) * this.maxWidth + 'px')
        .style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + 'px')
    }

    if(this.options.top){
      tenkey.style('top', '30vh');
    }

    if(this.options.dualSetting){
      switch(this.options.dualSetting.whichSide){
        case "right":
          tenkey.style('left', p.width / 2 - ((95 / 200) * this.maxWidth) + this.maxWidth/2 + 'px');
          break;
        case "left":
          tenkey.style('left', p.width / 2 - ((95 / 200) * this.maxWidth) - this.maxWidth/2 + 'px');
          break;
      }
    }
  }

  /**
   * [p5dom] 各キー作成用
   * createButtonメソッド
   * @param {string} letter - キーに表示する値
   * @param {number} id - id
   * @param {boolean} [isMaterialIcons] - マテリアルアイコンを使用するかどうか
   * @returns {p5.dom} button dom element
   */
  createButton(letter, id, isMaterialIcons = false) {
    const p = this.p5;
    let button = p.createButton(letter);

    button.id(id)
      .style('padding', '0px 0px')
      .style('box-sizing', 'content-box')
      .style('background-color', this.p5.theme.color.domKeyBackground)
      .style('color', this.p5.theme.color.domKeyText)
      .style('outline-color', this.p5.theme.color.domKeyOutline)
      .style('cursor', 'pointer')
      .style('outline', 'solid 3px')
      .style('outline-offset', '-2px')
      .style('margin', '0px 0px 0px 0px')
      .style('vertical-align', 'bottom')
      .style(
        'font-family',
        isMaterialIcons
          ? 'Material Icons'
          : 'LocalFont'
      )
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
   * [p5dom]10キーをシャッフル
   * void
   */
  shuffleKey() {
    let keyArray = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    for (let i = 0; i < 10; i++) {
      this.p5.select("#" + i).html(keyArray.splice(Math.floor(Math.random() * keyArray.length), 1)[0])
    }
  }

}

