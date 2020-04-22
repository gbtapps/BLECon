/**
 * [Comprent] サウンドボード音
 * Libries:p5sound
 * @property {p5} p5
 * @property {p5.SoundFile} do - 「ド」の音ファイル
 * @property {p5.SoundFile} re - 「レ」の音ファイル
 * @property {p5.SoundFile} mi - 「ミ」の音ファイル
 * @property {p5.SoundFile} fa - 「ファ」の音ファイル
 * @property {p5.SoundFile} so - 「ソ」の音ファイル
 * @property {p5.SoundFile} ra - 「ラ」の音ファイル
 * @property {number} vol - 再生する音量
 */
export class SoundBoardSound {
  /**
   * 再生音読み込み
   * @param {p5} p
   */
  constructor(p) {
    this.p5 = p;
    p.soundFormats('wav');
    this.do = p.loadSound('./assets/sounds/piano/do.wav');
    this.re = p.loadSound('./assets/sounds/piano/re.wav');
    this.mi = p.loadSound('./assets/sounds/piano/mi.wav');
    this.fa = p.loadSound('./assets/sounds/piano/fa.wav');
    this.so = p.loadSound('./assets/sounds/piano/so.wav');
    this.ra = p.loadSound('./assets/sounds/piano/ra.wav');
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
          case 0 : this.do.play(0, 1, this.vol, 0, 0.75); break;
          case 1 : this.re.play(0, 1, this.vol, 0, 0.75); break;
          case 2 : this.mi.play(0, 1, this.vol, 0, 0.75); break;
          case 3 : this.fa.play(0, 1, this.vol, 0, 0.75); break;
          case 4 : this.so.play(0, 1, this.vol, 0, 0.75); break;
          case 5 : this.ra.play(0, 1, this.vol, 0, 0.75); break;
          default : break;
        }
      }, (1000 / level + 400) * i);
    }
  }

  /**
   * 再生している音声のインデックスを返す
   * @returns {number} 音階に対応するインデックス
   */
  playIndex() {
    if (this.do.isPlaying()) return 0;
    if (this.re.isPlaying()) return 1;
    if (this.mi.isPlaying()) return 2;
    if (this.fa.isPlaying()) return 3;
    if (this.so.isPlaying()) return 4;
    if (this.ra.isPlaying()) return 5;
  }

  /**
   * 読み上げている音声を止める
   */
  stop() {
    this.vol = 0;
  }

}
