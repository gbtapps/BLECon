/**
 * [Component] 数字読み上げ
 * Libries:p5sound
 * @property {p5} p5
 * @property {p5.SoundFile} zero - 「0」の音ファイル
 * @property {p5.SoundFile} one - 「1」の音ファイル
 * @property {p5.SoundFile} two - 「2」の音ファイル
 * @property {p5.SoundFile} three - 「3」の音ファイル
 * @property {p5.SoundFile} four - 「4」の音ファイル
 * @property {p5.SoundFile} five - 「5」の音ファイル
 * @property {p5.SoundFile} six - 「6」の音ファイル
 * @property {p5.SoundFile} seven - 「7」の音ファイル
 * @property {p5.SoundFile} eight - 「8」の音ファイル
 * @property {p5.SoundFile} nine - 「9」の音ファイル
 * @property {number} vol - 再生する音量
 */
export class Speaker {
  /**
   * 再生音読み込み
   * @param {p5} p
   */
  constructor(p) {
    this.p5 = p;
    p.soundFormats('wav');
    this.zero = p.loadSound('./assets/sounds/number/0.wav');
    this.one = p.loadSound('./assets/sounds/number/1.wav');
    this.two = p.loadSound('./assets/sounds/number/2.wav');
    this.three = p.loadSound('./assets/sounds/number/3.wav');
    this.four = p.loadSound('./assets/sounds/number/4.wav');
    this.five = p.loadSound('./assets/sounds/number/5.wav');
    this.six = p.loadSound('./assets/sounds/number/6.wav');
    this.seven = p.loadSound('./assets/sounds/number/7.wav');
    this.eight = p.loadSound('./assets/sounds/number/8.wav');
    this.nine = p.loadSound('./assets/sounds/number/9.wav');
    this.vol = 0.8;
  }

  /**
   * 再生
   * @param {Array} array - 再生する数字が格納された配列
   */
  play(array, level) {
    for (let i = 0; i < array.length; i++) {
      setTimeout(() => {
        if (this.vol == 0) return;
        switch (array[i]) {
          case 0 : this.zero.play(0, 1.2, this.vol, 0, 0.75); break;
          case 1 : this.one.play(0, 1.2, this.vol, 0, 0.75); break;
          case 2 : this.two.play(0, 1.2, this.vol, 0, 0.75); break;
          case 3 : this.three.play(0, 1.2, this.vol, 0, 0.75); break;
          case 4 : this.four.play(0, 1.2, this.vol, 0, 0.75); break;
          case 5 : this.five.play(0, 1.2, this.vol, 0, 0.75); break;
          case 6 : this.six.play(0, 1.2, this.vol, 0, 0.75); break;
          case 7 : this.seven.play(0, 1.2, this.vol, 0, 0.75); break;
          case 8 : this.eight.play(0, 1.2, this.vol, 0, 0.75); break;
          case 9 : this.nine.play(0, 1.2, this.vol, 0, 0.75); break;
          default : break;
        }
      }, (1000/level + 500) * i);
    }
  }

  /**
   * 読み上げている音声を止める
   */
  stop() {
    this.vol = 0;
  }

}
