/**
 * 流れるターゲットクラス
 * @property {number} speed - 移動速度
 * @property {Object} sprite - スプライト
 * @property {number} diff - 反応時間のズレ
 */
export class FlowingTarget {
  /**
   * 流れるターゲットを作成
   * @param {p5} p
   */
  constructor(p, flowing, position, maxWidth, sizeScale = 1){
    this.p5 = p;
    this.exitTime = 0;
    this.diff = 0;
    this.alpha = 255;
    this.isPushed = false;
    this.overlapTime = null;
    this.name = flowing.name;
    this.sprite = p.createSprite(
      position.x,
      position.y,
      maxWidth*sizeScale/4,
      maxWidth*sizeScale/4
    );
    this.sprite.setCollider(
      "rectangle",
      0,
      0,
      this.sprite.width,
      this.sprite.height
    )
    // this.sprite.debug = true;
    this.sprite.shapeColor = p.color(150, 0);
    this.sprite.setSpeed(maxWidth/140, 0);
    this.image = p.loadImage(flowing.image);
    this.image.resize(this.sprite.width, 0);
  }

  drawImage() {
    // this.p5.tint(0,this.alpha);
    this.p5.image(
      this.image,
      this.sprite.position.x-this.sprite.width/2,
      this.sprite.position.y-this.sprite.height/2,
      this.sprite.width,
      this.sprite.height
    );
  }

}
