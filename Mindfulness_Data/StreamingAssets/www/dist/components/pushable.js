/**
 * [Component] 押せるマーク
 * @property {p5} p5
 * @property {boolean} isPositive - 押すべきマークかどうか
 * @property {number} diff - 押された時間と表示された時間の差
 * @property {number} showTime - 表示された時間
 * @property {Object} sprite - マークのスプライト
 * @property {Any} image - 表示する画像もしくはiconfontの名前
 */

export class Pushable {

    /**
     * [p5,p5play]押せるマークを作成
     * @param {p5} p
     * @param {Any} image - 表示する画像もしくはiconfontの名前
     * @param {number} size - 画像のサイズ
     * @param {boolean} isPositive - 押すべき画像かどうか
     */
    constructor(p, image, size, isPositive, pushImage) {
      this.p5 = p;
      this.isPositive = isPositive;
      this.diff;
      this.showTime;
      this.sprite = p.createSprite(0, 0, size * (2 / 3), size * (2 / 3));
      this.sprite.shapeColor = p.color(200, 0);
      this.sprite.setCollider('circle', 0, 0, size * (2 / 6));
      this.sprite.visible = false;
      this.sprite.mouseActive = true;
      this.image = image;
      this.pushCircle = pushImage;
      this.alpha = 0;
      // this.sprite.debug = true;
    }

    /* スプライトを表示する */
    show(position) {
      this.sprite.position.x = position.x
      this.sprite.position.y = position.y;
      this.sprite.visible = true;
      this.showTime = this.p5.millis();
    }

    /* スプライトを隠す */
    hidden() {
      this.sprite.visible = false;
    }

    /* 画像を描画 */
    drawImage() {
      this.image.mask(this.image);
      if (!this.sprite.visible) return;
      this.p5.tint(255, 255);
      this.p5.image(
        this.image,
        this.sprite.position.x - this.sprite.width / 2,
        this.sprite.position.y - this.sprite.height / 2,
        this.sprite.width,
        this.sprite.height
      );
    }

    drawEffect() {
      if(this.alpha > 0)
        this.alpha -= 30;
      this.p5.tint(255, this.alpha);
      this.p5.image(
        this.pushCircle,
        this.sprite.position.x - this.sprite.width / 2,
        this.sprite.position.y - this.sprite.height / 2,
        this.sprite.width,
        this.sprite.height
      );
    }

    /* アイコンフォントを描画 */
    drawIconfont(color = "#FF8C00") {
      this.p5.textFont('Material Icons');
      if (!this.sprite.visible) return;
      this.p5.fill(color);
      this.p5.noStroke();
      this.p5.textSize(this.sprite.width);
      this.p5.textAlign(this.p5.CENTER);

      this.p5.text(
        this.image,
        this.sprite.position.x,
        this.sprite.position.y + this.sprite.height / 2
      );
    }

    /**
     * 表示された時間と今の時間との差を取得
     * @returns {number} 二つの円が重なる時間と今の時間との差
     */
    calcDiff() {
      return this.p5.millis() - this.showTime;
    }

  }
