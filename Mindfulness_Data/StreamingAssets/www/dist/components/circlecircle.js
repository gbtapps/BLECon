/**
 * [Component] サークル
 * @property {p5} p5
 * @property {number} distance - 青円赤円を描画する、中心からの距離
 * @property {Object} center - サークルの中心
 * @property {number} overrapTime - 青円と赤円が重なる時間
 * @property {Object} outsideCircle - 外側の円のスプライト
 * @property {Object} insideCircle - 内側の円のスプライト
 * @property {number} blueRadian - 青円の角度
 * @property {Object} bluePoint - 青円のスプライト
 * @property {number} redRadian - 赤円の角度
 * @property {Object} redPoint - 赤円のスプライト
 * @property {boolean} isClockwise - 赤円が時計回りかどうか
 * @property {number} difRadian - 赤円と青円のなす角
 * @property {number} speed - 1フレームに赤円が進む角度
 * @property {number} diff - 反応速度
 */

export class CircleCircle {

  /**
   * サークルを作成
   * @param {p5} p
   * @param {number} size - サイズ
   * @param {Object} position - 中心点
   * @param {boolean} [isEasyMode] - 赤円と青円の難易度を下げるかどうか
   */
  constructor(p, size, position, isEasyMode = false) {
    this.p5 = p;
    this.distance = size * (5 / 12);
    this.center = {x:position.x, y:position.y}
    this.overrapTime = 0;
    this.alpha = 0;
    this.size = size;
    this.outsideColor = 200;
    this.image = p.loadImage("./assets/image/blueCircle.svg");
    this.lerp = 0.00;
  
    this.outsideCircle = p.createSprite(this.center.x, this.center.y, size, size);
    this.outsideCircle.draw = function() {
      p.fill(200);
      p.stroke(230);
      p.ellipse(0, 0, size);
    }
    // this.outsideCircle.mouseActive = true;

    this.insideCircle = p.createSprite(this.center.x, this.center.y, size * (2 / 3), size * (2 / 3));
    this.insideCircle.draw = function() {
      p.fill(230);
      p.noStroke();
      p.ellipse(0, 0, size * (2 / 3));
    }
    this.insideCircle.mouseActive = true;

    this.blueRadian = p.radians(isEasyMode ? 270 : p.random(0, 360));
    this.bluePoint = p.createSprite(
      this.center.x + p.cos(this.blueRadian) * this.distance,
      this.center.y + p.sin(this.blueRadian) * this.distance,
      size / 4,
      size / 4
    );
    this.bluePoint.draw = function() {
      p.fill(0, 0, 255);
      p.noStroke();
      p.ellipse(0, 0, size / 4.5);
    }
    this.bluePoint.setCollider("circle", 0, 0, size / 6);
    this.bluePoint.mouseActive = true;
    // this.bluePoint.debug=true;

    // this.redRadian =p.radians(p.random(0,360));
    this.redRadian = this.blueRadian;
    this.redPoint = p.createSprite(
      this.center.x + p.cos(this.redRadian) * this.distance,
      this.center.y + p.sin(this.redRadian) * this.distance,
      size / 4,
      size / 4
    );
    this.redPoint.draw = function() {
      p.fill(255, 0, 0);
      p.noStroke();
      p.ellipse(0, 0, size / 4.5);
    }

    this.isClockwise = isEasyMode
      ? true
      : Math.random() >= 0.5;
    this.difRadian = this.getDifRadian();
    this.speed = this.p5.radians(isEasyMode ? 3 : p.random(1, 4));
    this.diff;
  }

  // start(){
  //   this.overrapTime = this.p5.millis() + (this.difRadian/this.speed/20*1000);
  // }

  /**
   * 二つの円が重なる時間と今の時間との差を取得
   * @returns {number} 二つの円が重なる時間と今の時間との差
   */
  calcDiff() {
    return Math.abs(this.p5.millis() - this.overrapTime);
  }

  /**
   * 赤円と青円が成す角度を取得
   * 向きは赤円の進行方向
   * @returns {number} 赤円と青円が成す角度
   */
  getDifRadian() {
    let difRadian = Math.abs(this.redRadian - this.blueRadian);
    if (
      (this.redRadian > this.blueRadian && this.isClockwise)
      || (this.blueRadian > this.redRadian && !this.isClockwise)
    ){
      difRadian = this.p5.TWO_PI - difRadian;
    }
    return difRadian;
  }

  /**
   * 赤円を移動させる
   */
  movePoint(avefps) {
    if (this.isClockwise) {
      this.redRadian += this.speed;
      if (this.redRadian > this.p5.TWO_PI)
        this.redRadian -= this.p5.TWO_PI;
    } else {
      this.redRadian -= this.speed;
      if (this.redRadian < 0)
        this.redRadian += this.p5.TWO_PI;
    }
    this.redPoint.position = {
      x: this.center.x + Math.cos(this.redRadian) * this.distance,
      y: this.center.y + Math.sin(this.redRadian) * this.distance
    };
    this.difRadian = this.getDifRadian();
  }

  /**
   * 赤円の位置に画像を描画する
   * @param {p5.Image} pic - 表示するp5Imageオブジェクト
   */
  drawRedPointImage(pic) {
    pic.mask(pic);
    this.p5.tint(255, 255);
    this.p5.image(
      pic,
      this.redPoint.position.x - this.redPoint.width / 2,
      this.redPoint.position.y - this.redPoint.height / 2,
      this.redPoint.width,
      this.redPoint.height
    );
  }

  /**
   * 青円の位置に画像を描画する
   * @param {p5.Image} pic - 表示するp5Imageオブジェクト
   */
  drawBluePointImage(pic) {
    pic.mask(pic)
    this.p5.tint(255, 255);
    this.p5.image(
      pic,
      this.bluePoint.position.x - this.bluePoint.width / 2,
      this.bluePoint.position.y - this.bluePoint.height / 2,
      this.bluePoint.width,
      this.bluePoint.height
    );
  }

  /**
   * タッチフィードバック表示
   */
  drawCircle(){
    if (this.alpha > 0)
      this.alpha -= 20;
    if(this.lerp > 0)
      this.lerp -= 0.1;
    this.outsideCircle.draw = ()=>{
      this.p5.fill(this.p5.lerpColor(this.p5.color(200), this.p5.color(this.outsideColor), this.lerp));
      this.p5.stroke(230);
      this.p5.ellipse(0, 0, this.size);
    }
    this.p5.drawSprite(this.outsideCircle);
    this.p5.drawSprite(this.insideCircle);
  
    this.p5.tint(255, this.alpha);
    this.p5.image(
      this.image,
      this.bluePoint.position.x - this.bluePoint.width,
      this.bluePoint.position.y - this.bluePoint.height,
      this.bluePoint.width * 2,
      this.bluePoint.height * 2
    );

    this.p5.drawSprite(this.bluePoint);
    this.p5.drawSprite(this.redPoint);
  }

}
