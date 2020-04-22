function UnityCall(msg) {
	console.log(msg);
	Unity.call(msg);
}

if (window.Android === undefined) {
    window.Android = function() {};
}

//  java側で定義されているものを上書きしないように個別設定してある
window.Android.vibeWrong = function () {
    UnityCall("vibeWrong");
};
window.Android.redo = function () {
    UnityCall("redo");
};
window.Android.finishTraining = function () {
    UnityCall("finishTraining");
};
window.Android.startBaseLine = function () {
    UnityCall("startBaseLine");
};
window.Android.startContent = function () {
    UnityCall("startContent");
};
window.Android.startCooldown = function () {
    UnityCall("startCooldown");
};
window.Android.startTraining = function () {
    UnityCall("startTraining");
};
window.Android.addResult = function (json) {
    UnityCall("addResult" + json);
};
window.Android.addScoreLog = function (json) {
    UnityCall("addScoreLog" + json);
};
window.Android.saveUserPreference = function (json) {
    UnityCall("saveUserPreference" + json);
};
/*	get系は別途
getPulse
getUserPreference
getUserPulse
*/

//Android.vibeWrong();
