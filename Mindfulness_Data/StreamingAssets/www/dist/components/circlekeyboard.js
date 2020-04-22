export class CircleKeyboard {
  /**
   * 円鍵盤
   * @param {p5} p
   * @param {number} centerY - 配置する中心のY座標(0~100)
   * @param {p5.Image} image - 表示するマークの画像
   * @param {boolean} isMini - maxWidthを考慮するかどうか
   */
  constructor(p, centerY, image, isMini = false) {
    this.p5 = p;
    this.size = p.min(p.height, p.width) * (9 / 10);
    if (isMini) {
      this.size = p.min(p.height, p.width) * (7 / 10);
      if (p.width < p.height) {
        this.size = p.min(p.height * (75 / 100), p.width) * (9 / 10);
      }
    }
    this.center = {
      x: p.width / 2,
      y: p.height * (centerY / 100)
    };
    this.label = {
      /* 中心から22.5度をなす位置にある鍵盤のx座標 */
      degree22: {
        X: this.size * (3 / 8) * p.cos(p.radians(22.5)),
        Y: this.size * (3 / 8) * p.sin(p.radians(22.5))
      },
      /* 中心から67.5度をなす位置にある鍵盤の座標 */
      degree67: {
        X: this.size * (3 / 8) * p.cos(p.radians(67.5)),
        Y: this.size * (3 / 8) * p.sin(p.radians(67.5))
      }
    };
    this.sprite = p.createSprite(this.center.x, this.center.y, this.size, this.size);
    this.sprite.setCollider('circle');
    this.sprite.mouseActive = true;

    this.centerCircle = p.createSprite(this.center.x, this.center.y, this.size / 2, this.size /2);
    this.centerCircle.setCollider('circle');
    this.centerCircle.mouseActive = true;

    this.image = image;
    this.showTime;
    /* 円の中心に表示するメッセージ用Divを生成 */
    let target = p.createDiv('');
    target.id("centerMessage")
      .style('position', 'fixed')
      .style('width', '100vw')
      .style('height', '20vh')
      .style('margin', 'auto')
      .style('top', centerY - 10 + 'vh')
      .style('font-size', (20 / 100) * this.size + 'px')
      .style('color', this.p5.theme.color.primaryText)
      .style('background-color', 'rgba(255, 255, 255, 0)')
      .style('font-family', 'LocalFont')
      .style('line-height', '20vh')
      .style('text-align', 'center')
    ;
    this.alpha = 0;
  }

  /**
   * 鍵盤を描画
   * @param {boolean} isBlocking - 出題中かどうか
   * @param {number} [markIndex] - マークを表示する鍵盤のインデックス
   * @param {number} [pushIndex] - 押した鍵盤のインデックス
   * @param {boolean} [isCorrect] - 押した鍵盤が正解かどうか
   */
  drawSoundBoard(isBlocking, markIndex = null, pushIndex = null, isCorrect = true) {
    this.alpha += 20;
    this.p5.ellipseMode(this.p5.CENTER);
    /* 各鍵盤用の扇型を描画 */
    let radianList = [-90, -45, 0, 45, 90, 135, 180, 225, 270];
    this.p5.strokeWeight(this.size / 70);
    for (let i = 0; i < 8; i++) {
      this.p5.fill(isBlocking ?this.p5.theme.color.primaryDark :this.p5.theme.color.primary);
      this.p5.stroke(this.p5.theme.color.background);
      /* クリックした鍵盤 */
      if (pushIndex === i) {
        if (isCorrect) {
          /* 正解時アニメ */
          /* ⑴ホワイトフラッシュ */
          this.p5.fill(this.p5.theme.color.primary + this.alpha.toString(16));

          /* ⑵外枠表示 */
          // this.p5.stroke(this.p5.theme.color.secondary + this.alpha.toString(16));
        } else {
          /* 不正解アニメ */
          /* 黒フラッシュ */
          this.p5.fill(this.p5.theme.color.primary);
          if (this.alpha < 80) {
            this.p5.fill(this.p5.theme.color.spriteInCorrect);
          }
        }
      }
      this.p5.arc(
        this.center.x,
        this.center.y,
        this.size,
        this.size,
        /* 外枠表示用 */
        // this.p5.radians(radianList[i] + 1.5),
        // this.p5.radians(radianList[i + 1] - 1.5),
        this.p5.radians(radianList[i]),
        this.p5.radians(radianList[i + 1]),
        this.p5.PIE
      );

    }
    /* 中心の円を描画 */
    this.p5.fill(this.p5.theme.color.background);
    this.p5.stroke(this.p5.theme.color.background);
    this.p5.ellipse(this.center.x, this.center.y, this.size / 2);

    /* 対応する鍵盤の位置にマークを表示 */
    this.p5.imageMode(this.p5.CENTER);
    switch(markIndex) {
      case 0: {
        this.p5.image(
          this.image,
          this.center.x + this.label.degree67.X,
          this.center.y - this.label.degree67.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 1: {
        this.p5.image(
          this.image,
          this.center.x + this.label.degree22.X,
          this.center.y - this.label.degree22.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 2: {
        this.p5.image(
          this.image,
          this.center.x + this.label.degree22.X,
          this.center.y + this.label.degree22.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 3: {
        this.p5.image(
          this.image,
          this.center.x + this.label.degree67.X,
          this.center.y + this.label.degree67.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 4: {
        this.p5.image(
          this.image,
          this.center.x - this.label.degree67.X,
          this.center.y + this.label.degree67.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 5: {
        this.p5.image(
          this.image,
          this.center.x - this.label.degree22.X,
          this.center.y + this.label.degree22.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 6: {
        this.p5.image(
          this.image,
          this.center.x - this.label.degree22.X,
          this.center.y - this.label.degree22.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      case 7: {
        this.p5.image(
          this.image,
          this.center.x - this.label.degree67.X,
          this.center.y - this.label.degree67.Y,
          this.size / 5,
          this.size / 5
        );
        return;
      }
      default: {
        return;
      }
    }
  }

  /**
   * マウスがどの鍵盤の方向にあるのかを返す
   * @returns {number} 各鍵盤に対応するインデックス
   */
  pushIndex() {
    let degrees = this.p5.degrees(
      this.p5.atan2(
        this.p5.mouseY - this.center.y,
        this.p5.mouseX - this.center.x
      )
    );
    if (-90 < degrees && degrees < -45)   return 0;
    if (-45 < degrees && degrees < 0)     return 1;
    if (0 < degrees && degrees < 45)      return 2;
    if (45 < degrees && degrees < 90)     return 3;
    if (90 < degrees && degrees < 135)    return 4;
    if (135 < degrees && degrees < 180)   return 5;
    if (-180 < degrees && degrees < -135) return 6;
    if (-135 < degrees && degrees < -90)  return 7;
  }

  /**
   * 表示するvalueを更新します。
   * @public
   * @param {string} value ターゲットとなる値（メッセージや計算式など）
   */
  update(value) {
    this.p5.select("#centerMessage").html(value);
  }

  /**
   * サークルの中央に〇×を表示します
   * @param {boolean} judge - 回答が正解かどうか
   */
  indicateJudge(judge) {
    switch(judge) {
      case null:
        return;
      case true: {
        this.p5.fill("#00000000");
        this.p5.strokeWeight(this.size / 30);
        this.p5.stroke(this.p5.theme.color.circles);
        this.p5.ellipseMode(this.p5.CENTER);
        this.p5.ellipse(
          this.p5.width / 2,
          this.center.y,
          this.size / 2.8,
          this.size / 2.8
        );
        return;
      }
      case false: {
        this.p5.fill(this.p5.theme.color.crosses);
        this.p5.noStroke();
        this.p5.rectMode(this.p5.CENTER);
        this.p5.translate(this.p5.width / 2, this.center.y);
        this.p5.rotate(this.p5.PI / 4);
        this.p5.rect(0, 0, this.size / 2.5, this.size / 30);
        this.p5.rotate(this.p5.PI / 2);
        this.p5.rect(0, 0, this.size / 2.5, this.size / 30);
        return;
      }
    }
  }


}
