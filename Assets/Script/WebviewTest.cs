using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using Newtonsoft.Json.Converters;

public class JSFunc
{
    public string vibeWrong(string msg)
    {
        return msg;
    }
    public string redo(string msg)
    {
        return msg;
    }

    public string finishTraining(string msg)
    {
       WebviewTest.Instance.Destroy();
        return msg;
    }

    public string startBaseLine(string msg)
    {
        return msg;
    }

    public string startContent(string msg)
    {
        return msg;
    }

    public string startCooldown(string msg)
    {
        return msg;
    }

    public string startTraining(string msg)
    {
        return msg;
    }
}

public class WebviewTest : MonoBehaviour
{
    static WebviewTest m_instance = null;
    public static WebviewTest Instance
    {
        get
        {
            return m_instance;
        }
        private set
        {
            m_instance = value;
        }
    }

    static WebViewObject webViewObject = null;

    string[] m_jsfunc_list =
    {
        "vibeWrong",
        "redo",
        "finishTraining",
        "startBaseLine",
        "startContent",
        "startCooldown",
        "startTraining",
        "addResult:",
        "addScoreLog:",
        "saveUserPreference:",

        "getPulse",
        "getUserPreference",
        "getUserPulse",
    };

    string finishTraining(string msg)
    {
        Destroy(webViewObject.gameObject);
        return null;
    }

    private void OnDestroy()
    {
        Destroy(webViewObject.gameObject);
        webViewObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(webViewObject != null || Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) => {
            var jsfObject = new JSFunc();
            Type t = jsfObject.GetType();

            Debug.Log(msg);
            for(int i=0;i < m_jsfunc_list.Length; i++)
            {
                if(msg.IndexOf(m_jsfunc_list[i]) == 0)
                {
                    var mi = t.GetMethod(m_jsfunc_list[i]);
                    var res = mi.Invoke(jsfObject,new object[] { msg.Substring(m_jsfunc_list[i].Length) });
                }
            }
        },
        ld: (msg) =>
        {
#if UNITY_EDITOR_OSX || !UNITY_ANDROID
            // NOTE: IOSの場合はこれがないと Unity.call が使えないので注意
            // NOTE: depending on the situation, you might prefer
            // the 'iframe' approach.
            // cf. https://github.com/gree/unity-webview/issues/189
            webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#endif
            //            webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            //  ラッパーを入れる
            /*  HTML側で読ませるので不要
            try
            {
                var fi = Application.streamingAssetsPath + "/Wrapper.js";
                var wr = UnityWebRequest.Get(fi);
                wr.SendWebRequest();
                var js = wr.downloadHandler.text;
                
                webViewObject.EvaluateJS(js);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            */

        });
        //  WebViewで使用するパスはUnityのものと違うので注意(普通のアプリと同じパス)
        string filePath = System.IO.Path.Combine("file:///android_asset/", "www/index.html#NFB001");
        //string filePath = System.IO.Path.Combine("file:///android_asset/", "www/test.html");
//        transform.Find("DebugText").GetComponent<Text>().text = filePath;

        webViewObject.LoadURL(filePath.Replace(" ", "%20"));
        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
