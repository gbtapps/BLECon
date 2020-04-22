export class Training{

  constructor(userPreference) {
    /* ユーザ設定の全体 */
    this.userPreference = userPreference || {};
    this.result = {};
    this.scoreLog = {};
  }
  get uid(){ return null; }

  /** ユーザ設定から個別トレーニング設定を取得 */
  get preference(){
    return this.userPreference[this.uid] || null;
  }
  /** ユーザ設定に個別トレーニング設定を保存 */
  set preference(preference){
    this.userPreference[this.uid] = preference;
  }

  get response(){
    return {trainingResult: this.result, userPreference: this.userPreference, scoreLog: this.scoreLog};
  }
}
