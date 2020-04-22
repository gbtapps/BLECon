/**
 * [Component] 計算器
 * Libries: p5, p5dom
 * @property {p5} p
 * @property {number} maxWidth - 描画対象とする横幅
 */
export class Calcurator {
  /**
   * [p5dom] 計算機
   * Componentを生成し、中央上部に描画。
   * @param {p5} p
   */

  constructor(p) {
    this.p5 = p;
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.isLandscape = p.windowWidth > p.windowHeight

    let input = p.createDiv('');

    input.id("display")
      .style('position', 'fixed')
      .style('font-size', (18 / 100) * this.maxWidth + 'px')
      .style('background-color', '#212121')
      .style('color', '#fff')
      .style('cursor', 'default')
      .style('width', (40 / 100) * this.maxWidth + 'px')
    //  .style('height', (20 / 100) * this.maxWidth + 'px')
      .style('height', '15vh')
      .style('left', p.windowWidth / 2 - ((20 / 100) * this.maxWidth) + 'px')
      .style('top', '33vh')
      .style('font-family', 'LocalFont')
    //  .style('line-height', (20 / 100) * this.maxWidth + 'px')
      .style('line-height', '15vh')
      .style('text-align', 'center')
    ;

    if(this.isLandscape){
      input.style('top', '50vh')
      .style('left', '35vw')
      .style('width','15vw')
    }

    let equal = p.createDiv('=');
    equal.id("equal")
      .style('position', 'fixed')
      .style('font-size', (18 / 100) * this.maxWidth + 'px')
      .style('color', '#616161')
      .style('width', '5vw')
      .style('height', '15vh')
      .style('top', '50vh')
      .style('font-family', 'LocalFont')
      .style('line-height', '15vh')
      .style('text-align', 'center')
      .style('left', '28vw')

  }

  /**
   * [EventHandler,p5dom] 入力表示用displayメソッド
   * 押下されたボタンの値を入力エリアに追加
   * mousePressedからコールされるため、ここではスコープがp5.Elementに変化しています。
   * @param {event} e
   * @param {function} callback
   * @returns {any}
   */
  input(e, callback = null) {
    const input = this.p5.select("#display");
    const letter = e.currentTarget.innerText;

    switch (letter) {
      case 'backspace':/*文字を消す*/
        if (input.html().length >= 2) {
          return input.html(input.html().substr(0, input.html().length - 1));
        } else {
          return input.html("");
        }
      case "":/* 無視 */
        return;
      case 'keyboard_return':/* 正誤判定 */
        return callback();
      default:/*文字を追加*/
        if (input.html().length == 3) {
          return;
        } else if (input.html() == 0) {
          return input.html(letter);
        } else {
          return input.html(input.html() + letter);
        }
    }
  }

}
