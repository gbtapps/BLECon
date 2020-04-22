/**
 * [Component] ビー音
 * Libries:p5sound
 * @property {p5.Env} env エンベロープ
 * @property {p5.Oscillator} osc オシレータ
 * @property {array} pattern フレーズに登録する楽譜
 * @property {p5.Phrase} phrase patternに基づいた再生フレーズ
 * @property {p5.Part} part パート（再生フレーズ,BPM）
 */
export class BeepSound {

  /**
   * [p5sound] 再生音定義
   *
   */
  constructor(p) {
    this.p5 = p;
    this.env = new p5.Envelope(0.00001, 0.1, 0.40, 0.1, 0.00001, 0.000001);
    this.osc = new p5.Oscillator();
    this.osc.setType('square');
    this.osc.amp(0);
    this.pattern = [250];
    this.phrase = new p5.Phrase('beep',(t, p) => {this.makeSound(t, p)}, this.pattern);
    this.part = new p5.Part();
    this.part.addPhrase(this.phrase);
    this.part.setBPM(100);
  }

  /**
   * [p5sound] 音声生成
   * @param {Number} time
   * @param {Array} playbackRate 音高
   */
  makeSound(time, playbackRate) {
    this.osc.freq(playbackRate);
    this.osc.start();
    this.env.play(this.osc);
  }

  /**
   * [p5sound] 音声再生
   * トレーニングからはこれを呼び出す
   */
  playSound(option = null) {
    if (this.p5.theme.option.sound === "off" && option === null) return;
    this.part.start();
  }

  /**
   * [p5sound] 音声停止
   * トレーニングからはこれを呼び出す
   */
  stopSound() {
    this.part.stop();
  }
}
