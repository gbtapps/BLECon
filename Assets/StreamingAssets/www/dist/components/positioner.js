/**
 * [Component] ランダム配置
 * @property {p5} p5
 * @property {Array} array - 表示する要素の配列
 * @property {number} width - 画面の横幅
 * @property {number} height - 画面の縦幅
 * @property {number} maxWidth - 描画対象とする横幅
 * @property {p5.Group} group - 作られたスプライトを包括するグループ
 * @property {p5.Image} image - 正解時に表示させる画像
 * @property {number} alpha - アニメーション用の透明度
 */
export class Positioner {

  /**
   * [p5play]ランダム配置
   * @param {p5} p
   * @param {Array} letters - 表示する要素の配列
   * @param {string} [category] - カテゴリの指定 "training" or "check"
   * @param {boolean} [isFirstAssist] - 最初に押すべき要素を強調表示するかどうか
   */
  constructor(p, letters, category = "training", isFirstAssist = false) {
    this.p5 = p;
    this.array = letters;
    this.width = p.windowWidth;
    this.height = p.windowHeight;
    this.short = p.min(this.height, this.width);
    this.maxWidth = (p.windowWidth < p.windowHeight)
      ? p.windowWidth
      : p.pow(p.windowHeight, 2) / p.windowWidth;
    this.group = p.Group();
    switch(category){
      case "training":
        this.image = p.loadImage("./assets/image/whiteCircle.svg");
        break;
      case "check":
        this.image = p.loadImage("./assets/image/checkCircle.svg")
        break;
      default:
        this.image = p.loadImage("./assets/image/whiteCircle.svg");
    }
    this.isFirstAssist = isFirstAssist;
    this.alpha = 0;
  }

  /**
   * [p5,p5play]表示する要素の配列をすべて画面に描画する
   */
  createAll(isFullScreen, arrayset = 0) {
    if (this.group.length != 0) {
      this.group = this.p5.Group();
    }
    let collided = 0;
    let newSprite;
    this.diameter = (isFullScreen ?this.short :this.maxWidth) / 8;

    for (let i = 0; i < this.array[arrayset].length; i++) {
      if (isFullScreen) {
        newSprite = this.p5.createSprite(
          this.p5.random(
            (isFullScreen ?this.short :this.maxWidth) / 14,
            this.width - (isFullScreen ?this.short :this.maxWidth) / 14
          ),
          this.p5.random(
            this.height * (2 / 5),
            this.height * (9 / 10)
          ),
          this.diameter,
          this.diameter
        );
      } else {
        newSprite = this.p5.createSprite(
          this.p5.random(
            this.width / 2 - this.maxWidth / 2 + this.diameter / 2,
            this.width / 2 + this.maxWidth / 2 - this.diameter / 2
          ),
          this.p5.random(
            this.height * (2 / 5),
            this.height * (9 / 10)
          ),
          this.diameter,
          this.diameter
        );
      }
      newSprite.setCollider("circle", 0, 0, this.diameter/1.5);

      if (this.group.overlap(newSprite)) {
        newSprite.remove();
        // console.log(collided);
        if (collided > 400)
          break;
        collided += 1;
        i -= 1;
      } else {
        this.group.add(newSprite);
      }
    }
    /* スプライトがすべて揃ったら、マウスアクティブ要素を付与する */
    this.group.forEach((v, i, a) => {
      v.mouseActive = true;
      v.shapeColor = this.p5.color(255, 0);
      // v.debug = true;
    })
  }

  /**
   * [p5]スプライトに対応するラベルを描画する
   * @param {boolean} isFullScreen - 配置位置の制限の有無
   * @param {number} arrayset - 何番目のラベルリストを描画するか
   * @param {number} [index] - 強調表示するスプライトのインデックス
   * @param {boolean} [isCorrect] - 押したスプライトが正解かどうか
   * @param {boolean} [isDeleteCircle] - 押した正解のスプライトの円を消すかどうか
   */
  drawLabel(isFullScreen, arrayset = 0, index = null, isCorrect = true, isDeleteCircle = false) {
    this.alpha += 20;
    this.p5.textAlign(this.p5.CENTER);
    this.p5.textSize(this.maxWidth / 11);
    if (isFullScreen)
      this.p5.textSize(this.short / 11);
    this.group.forEach((v, i, a) => {
      this.p5.noStroke();
      this.p5.fill(this.p5.theme.color.positioner);
      if (this.isFirstAssist && i === 0)
        this.p5.fill(this.p5.theme.color.primary);
      /* クリックしたスプライト */
      if (index === i){
        if (isCorrect) {
          /* 正解時アニメ */
          /* ⑴外円表示 */
          // this.p5.fill(this.p5.theme.color.positionerLight);
          // this.p5.ellipse(
          //   this.group[index].position.x,
          //   this.group[index].position.y,
          //   this.short / 6,
          //   this.short / 6
          // );

          /* ⑵グラデーション画像表示 */
          this.p5.fill(this.p5.theme.color.positionerLight);
          if(this.isFirstAssist && index === 0)
            this.p5.fill(this.p5.theme.color.primaryLight);
          this.p5.imageMode(this.p5.CENTER);
          this.p5.tint(255, 255-this.alpha);
          this.p5.image(
            this.image,
            this.group[index].position.x,
            this.group[index].position.y,
            this.diameter * 2,
            this.diameter * 2
          );

          /* ⑶ホワイトフェード */
          // this.p5.fill(this.p5.theme.color.positioner + this.alpha.toString(16));
        } else {
          /* 不正解時アニメ */
          /* ⑵黒フラッシュ */
          // this.p5.fill(this.p5.theme.color.positioner);
          // if (this.alpha < 80) {
          //   this.p5.fill(this.p5.theme.color.spriteInCorrect);
          // }
        }
      }
      /* スプライトの見た目 */
      if (!isDeleteCircle || v.visible) {
        this.p5.ellipse(v.position.x, v.position.y, this.diameter, this.diameter);
      }

      if (index === i && !isCorrect) {
        /* 失敗時アニメ */
        /* ⑴×印表示 */
        this.p5.fill(this.p5.theme.color.crosses);
        this.p5.rectMode(this.p5.CENTER);
        this.p5.translate(this.group[index].position.x, this.group[index].position.y);
        this.p5.rotate(this.p5.PI / 4);
        this.p5.rect(0, 0, this.diameter * 1.3, this.diameter / 8);
        this.p5.rotate(this.p5.PI / 2);
        this.p5.rect(0, 0, this.diameter * 1.3, this.diameter / 8);
        this.p5.resetMatrix();
      }
      /* ラベル */
      if (v.visible)
        this.p5.fill(this.p5.theme.color.positionerText);
      else
        this.p5.fill(this.p5.theme.color.positionerPushedText);
      this.p5.text(
        this.array[arrayset][i],
        v.position.x,
        v.position.y + (isFullScreen ?this.short :this.maxWidth) / 30
      );
    });
  }

  /**
   * 押されているスプライトのindexを返す
   * @returns 正解のindex。
   */
  pushIndex() {
    for (let index = 0; index < this.group.length; index++) {
      if (this.group[index].mouseIsPressed) return index;
    }
    return null;
  }

  /* 押されたボタンのvisibleをfalseにする */
  invisibleSprite(index) {
    this.group[index].visible = false;
  }

}
