/**
 * 空
 * @property {number} sunHeight - 日の位置の高さ。一番下になると日没になります。
 * @property {array<Start>} stars - 星々
 */
export class Sky{
  /**
   * @param {p5}
   * @param {number} height - 空領域の高さ
   */
  constructor($p, height){
    this.$p5 = $p;
    this.stars = [];
    for (var i = 0; i < 50; i++) {
      let s = new Star(this.$p5);
      this.stars.push(s);
    }
    this.sunHeight = 0;
    this.height = height;
  }
  /**
   * 太陽を落とす
   * HSB色のsaturation/brightnessを太陽の高さと同期させています。
   * 完全に落下したとき、星々がまたたきます。
   */
  letSunGoesDown() {
    const pos = this.sunHeight/this.height;  //上からの位置割合
    let f = [182,86,70],
        t = [23,51,89];
    let from = this.$p5.color(`hsb(${f[0]},${f[1]*pos}%,${f[2]*(1.6-pos)}%)`),
        to   = this.$p5.color(`hsb(${t[0]},${t[1]*pos}%,${t[2]*(1.6-pos)}%)`);
    this.$p5.background(220);
    this._setGradient(from, to);

    if(pos>0.95)
      this.letStartsTwink();
    this.$p5.colorMode();
  }
  /**
   * 星々をまたたかせる
   */
  letStartsTwink(){
    this.stars.forEach(s => {
      s.twink();
    });
  }
  /** 空にグラデーションを描画するメソッド  */
  _setGradient(from, top) {
    this.$p5.noFill();
    for (let i = 0; i <= this.$p5.windowHeight; i++) {
      var inter = this.$p5.map(i, 0, this.$p5.windowHeight, 0, 1);
      var c = this.$p5.lerpColor(from, top, inter);
      this.$p5.stroke(c);
      this.$p5.line(0, i, this.$p5.windowWidth, i);
    }
  }
}
/**
 * 瞬く星
 */
class Star{
  constructor($p){
    this.$p5 = $p
    this.x = this.$p5.random(this.$p5.windowWidth);
    this.y = this.$p5.random(this.$p5.windowHeight - 200);
    this.w = 2;
    this.h = 2;
    setInterval(()=>{
      this.x += (this.$p5.random(10) - 5);
      this.y += (this.$p5.random(10) - 5);
      if (this.w == 2) {
        this.w = 3;
        this.h = 3;
      } else {
        this.w = 2;
        this.h = 2;
      }
    },6000);
  }
  /**
   * またたく
   * 前回値を更新するため、定期的にコールされることを想定しています。
   */
  twink(){
    this.$p5.noStroke();
    this.$p5.fill(255, 255, 0);
    this.$p5.ellipse(this.x, this.y, this.w, this.h);
  }
}
