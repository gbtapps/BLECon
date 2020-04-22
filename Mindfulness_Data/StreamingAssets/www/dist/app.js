/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	var parentJsonpFunction = window["webpackJsonp"];
/******/ 	window["webpackJsonp"] = function webpackJsonpCallback(chunkIds, moreModules, executeModules) {
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [], result;
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(chunkIds, moreModules, executeModules);
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 	};
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// objects to store loaded and loading chunks
/******/ 	var installedChunks = {
/******/ 		7: 0
/******/ 	};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/ 	// This file contains only the entry chunk.
/******/ 	// The chunk loading function for additional chunks
/******/ 	__webpack_require__.e = function requireEnsure(chunkId) {
/******/ 		var installedChunkData = installedChunks[chunkId];
/******/ 		if(installedChunkData === 0) {
/******/ 			return new Promise(function(resolve) { resolve(); });
/******/ 		}
/******/
/******/ 		// a Promise means "currently loading".
/******/ 		if(installedChunkData) {
/******/ 			return installedChunkData[2];
/******/ 		}
/******/
/******/ 		// setup Promise in chunk cache
/******/ 		var promise = new Promise(function(resolve, reject) {
/******/ 			installedChunkData = installedChunks[chunkId] = [resolve, reject];
/******/ 		});
/******/ 		installedChunkData[2] = promise;
/******/
/******/ 		// start chunk loading
/******/ 		var head = document.getElementsByTagName('head')[0];
/******/ 		var script = document.createElement('script');
/******/ 		script.type = "text/javascript";
/******/ 		script.charset = 'utf-8';
/******/ 		script.async = true;
/******/ 		script.timeout = 120000;
/******/
/******/ 		if (__webpack_require__.nc) {
/******/ 			script.setAttribute("nonce", __webpack_require__.nc);
/******/ 		}
/******/ 		script.src = __webpack_require__.p + "" + chunkId + ".js";
/******/ 		var timeout = setTimeout(onScriptComplete, 120000);
/******/ 		script.onerror = script.onload = onScriptComplete;
/******/ 		function onScriptComplete() {
/******/ 			// avoid mem leaks in IE.
/******/ 			script.onerror = script.onload = null;
/******/ 			clearTimeout(timeout);
/******/ 			var chunk = installedChunks[chunkId];
/******/ 			if(chunk !== 0) {
/******/ 				if(chunk) {
/******/ 					chunk[1](new Error('Loading chunk ' + chunkId + ' failed.'));
/******/ 				}
/******/ 				installedChunks[chunkId] = undefined;
/******/ 			}
/******/ 		};
/******/ 		head.appendChild(script);
/******/
/******/ 		return promise;
/******/ 	};
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "dist/";
/******/
/******/ 	// on error function for async loading
/******/ 	__webpack_require__.oe = function(err) { console.error(err); throw err; };
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 4);
/******/ })
/************************************************************************/
/******/ ({

/***/ 4:
/***/ (function(module, exports, __webpack_require__) {

const isMeasuring = (location.hash.substr(1, 1) === '!'),
      isCircuit   = (location.hash.substr(-1, 1) === '$'),
      uid         = location.hash.replace(/[#!$]/g, ''),
      params      = new URLSearchParams(location.search)
;

window.App = {
  isStarted: false,
  params: params,
  isAndroid: (typeof Android !== "undefined")
};

window.addEventListener('load', async () => {
  const manager= new TrainingContentManager(uid);
  if(isMeasuring)
    await measure(manager);
  else
    await practice(manager);
});


/** 練習；計測しない */
async function practice(manager){
  /* トレーニング; エクササイズ/チェック */
  let result;
  if(manager.isExercise){     /* エクササイズ*/
    const exercise= await manager.getExercise();
    result = await exercise.start();
  }else if(manager.isChecktest){  /* チェックテスト */
    const checktest = await manager.getChecktest();
    result = await checktest.start()
  }else if(manager.isMeasureTimer){
    const measureTimer = await manager.getMeasureTimer();
    await measureTimer.start();
  }
  /* 終了処理 */
  if(window.App.isAndroid)
    Android.finishTraining();
}
/** 本番 */
async function measure(manager){
  const {CountdownGuide} = await __webpack_require__.e/* import() */(6).then(__webpack_require__.bind(null, 6));
  /* ベースライン */
  if(window.App.isAndroid)
    Android.startBaseline();
  if(!manager.isInterlude)
    await new CountdownGuide().start(15);

  if(window.App.isAndroid)
    await Android.startContent();


  /* トレーニング; エクササイズ/チェック */
  let result={};
  if(manager.isExercise){
    // /* ユーザ設定 */
    // let _userPreference = (window.App.isAndroid)
    //   ? JSON.parse(Android.getUserPreference())
    //   : {};
    let _userPreference = {};
    const exercise= await manager.getExercise(_userPreference);
    let {trainingResult, userPreference, scoreLog} = await exercise.start();
    result = trainingResult;
    // /* 設定保存 */
    // if(window.App.isAndroid)
    //   Android.saveUserPreference(JSON.stringify(userPreference));
    // /* スコアログを反映 */
    // if(window.App.isAndroid)
    //   Android.addScoreLog(JSON.stringify(scoreLog));

  }else if(manager.isMeasureTimer){
    const measureTimer = await manager.getMeasureTimer();
    await measureTimer.start();
  }else{    /* インタルード */
    /* ユーザ設定 */
    let _pulse = (window.App.isAndroid)
      ? JSON.parse(Android.getUserPulse())
      : {};
    const interlude = await manager.getInterlude(_pulse);
    await interlude.start();
  }
  if(!manager.isInterlude){
    /* 安静待機 */
    if(window.App.isAndroid)
      Android.startCooldown();
    await new CountdownGuide().start(10);
    if(window.App.isAndroid)
      Android.addResult(JSON.stringify(result));
  }
  /* 終了処理 */
  if(window.App.isAndroid)
    Android.finishTraining();
}
/**
 * トレーニング管理
 */
class TrainingContentManager{
  constructor(uid){
    this.uid = uid;
  }
  get isExercise(){
    return this.uid.startsWith('NFB');
  }
  get isChecktest(){
    return this.uid.startsWith('NCK');
  }
  get isMeasureTimer(){
    return this.uid.startsWith('MES');
  }
  get isInterlude(){
    return !this.uid.startsWith('NFB') && !this.uid.startsWith('NCK') && !this.uid.startsWith('MES');
  }
  /**
   * エクササイズ
   * @param {object} userPreference - ユーザ設定
   */
  async getExercise(userPreference){
    let training = {};
    try{
      const m = await __webpack_require__(5)(`./${this.uid}`);
      return new m[this.uid](userPreference);
    }catch(e){
      return null;
    }
  }
  /** セットナンバー */
  getSetNum(){
    switch(this.uid){
      case 'NCK001':
        return 3;
      case 'NCK002':
        return 3;
      case 'NCK003':
        return 1;
      case 'NCK004':
        return 3;
      case 'NCK005':
        return 2;
      case 'NCK006':
        return 2;
      default:
        return 1;
    }
  }
  // /** チェックテスト */
  // async getChecktest(){
  //   switch(this.uid){
  //     case 'NCK001':
  //       const {Speed} = await import('./checktests/NCK001.js');
  //       return new Speed();
  //     case 'NCK002':
  //       const {WorkingMemory} = await import('./checktests/NCK002.js');
  //       return new WorkingMemory();
  //     case 'NCK003':
  //       const {EpisodeMemory} = await import('./checktests/NCK003.js');
  //       return new EpisodeMemory();
  //     case 'NCK004':
  //       const {Attention} = await import('./checktests/NCK004.js');
  //       return new Attention();
  //     case 'NCK005':
  //       const {Suppression} = await import('./checktests/NCK005.js');
  //       return new Suppression();
  //     case 'NCK006':
  //       const {Persistence} = await import('./checktests/NCK006.js');
  //       return new Persistence();
  //     default:
  //       return null;
  //   }
  // }
  /**　計測間に挟むトレーニング　*/
  async getInterlude(preference){
    switch (this.uid) {
      case "NIL001":
        /* 心拍BF */
        const {PulseGuide} = await __webpack_require__.e/* import() */(5).then(__webpack_require__.bind(null, 7));
        return new PulseGuide(preference);
    }
  }
  /** 1分間の計測 */
  async getMeasureTimer(){
    switch (this.uid) {
      case "MES001":
        /* 計測タイマー */
        const {MeasureTimer} = await __webpack_require__.e/* import() */(4).then(__webpack_require__.bind(null, 8));
        return new MeasureTimer();
    }
  }
}



/***/ }),

/***/ 5:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"./NFB001": [
		1,
		0
	],
	"./NFB001.js": [
		1,
		0
	],
	"./NFB004": [
		2,
		1
	],
	"./NFB004.js": [
		2,
		1
	],
	"./NFB011": [
		3,
		2
	],
	"./NFB011.js": [
		3,
		2
	],
	"./training": [
		0,
		3
	],
	"./training.js": [
		0,
		3
	]
};
function webpackAsyncContext(req) {
	var ids = map[req];
	if(!ids)
		return Promise.reject(new Error("Cannot find module '" + req + "'."));
	return __webpack_require__.e(ids[1]).then(function() {
		return __webpack_require__(ids[0]);
	});
};
webpackAsyncContext.keys = function webpackAsyncContextKeys() {
	return Object.keys(map);
};
webpackAsyncContext.id = 5;
module.exports = webpackAsyncContext;

/***/ })

/******/ });
//# sourceMappingURL=app.js.map