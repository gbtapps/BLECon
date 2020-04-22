/**
 *  リスト押し管理クラス
 */
export class FlowingList {

  /**
   * コンストラクタ
   * @param {Array} flowingList - リストの配列
   * @property {Array} flowingList - リストの配列
   * @property {string} target - ターゲット要素の名前
   */
  constructor(flowingList) {
    this.flowingList = flowingList;
    this.target = "";
  }

  /**
   * 引数で指定したnameのFlowingListの要素を返します
   * 指定がない場合はランダムにFlowingList要素を返します。
   * @param {String} [name]
   * @return {Object} テーマ内のFlowingList要素（{"name":名前, "image":画像パス}）
   */
  getFlowing(name = null) {
    let flowing = {}
    if (name!==null) {
      this.flowingList.forEach((v, i)=>{
        if(v.name === name){
          flowing = v;
        }
      });
    } else {
      flowing = this.flowingList[Math.floor(Math.random() * this.flowingList.length)];
    }
    return flowing;
  }

  /**
   * リストからランダムなFlowingList要素のnameを返します
   * @returns {string} FlowingListからランダムに取得した要素のname
   */
  getRandomName() {
    return this.flowingList[Math.floor(Math.random()*this.flowingList.length)].name;
  }

  /**
   * リストの名前一覧を取得します
   * @returns {Array} リストの名前一覧の配列
   */
  get nameList() {
    let nameList = []
    this.flowingList.forEach((v)=>{
      nameList.push(v.name);
    })
    return nameList;
  }
}
