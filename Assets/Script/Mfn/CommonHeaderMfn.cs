using System;
using System.Collections;
using System.Collections.Generic;
//using UnityCipher;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Callback = System.Action;

//  共通ヘッダの処理
public class CommonHeaderMfn : SingletonMonoBehaviour<CommonHeaderMfn>
{
    string m_id;
    string m_password;

    GameObject UI;
    Text title;

    static bool m_LoginWait = true;
    public static bool LoginWait
    {
        get
        {
            return m_LoginWait;
        }
    }

    public void SetView(bool value)
    {
        UI.SetActive(value);
    }

    // Start is called before the first frame update
    void Awake()
    {

        /* moritomi

        title = GameObject.Find("Text_Title").GetComponent<Text>();

        var id = PlayerPrefs.GetString("id", null);
        var password = PlayerPrefs.GetString("password", null);
        if (password != null && password != "")
        {
            string decrypt;
            decrypt = RijndaelEncryption.Decrypt(password, ConstData.CryptKey);
            m_LoginWait = false;
        }
        */

        UI = gameObject.FindDescendant("UI");

#if DEBUG
        if (SceneManager.GetActiveScene().name != ConstData.EnumScene.T_TitleSelect.ToString())
        {
            //  スタートシーン以外ならどこから始まったか調べて番号をセットする
            ConstData.EnumScene i;
            for (i = 0; i < ConstData.EnumScene.Max; i++)
            {
                if (SceneManager.GetActiveScene().name == i.ToString())
                {
                    break;
                }
            }
            if (i == ConstData.EnumScene.Max)
            {
                Debug.Log(SceneManager.GetActiveScene().name + ":未定義のシーン");
            }
            CommonData.NowScene = i;
        }
#endif

        //  親のキャンバスを分離して残す
        //        transform.parent.parent = null;
        DontDestroyOnLoad(transform.parent);
//        CommonData.SceneHistory.Push(CommonData.NowScene);
        ChangeScene(CommonData.NowScene);



    }

    //  シーンに対応した内容に書き換える
    public void ChangeScene(ConstData.EnumScene scene)
    {

//        title.text = GetTitle(scene);

        return;


        /*
        var header = ConstData.HeaderType[scene];

        var textLeft = transform.Find("TitleText_Left").GetComponent<Text>();
        var textCenter = transform.Find("TitleText_Center").GetComponent<Text>();

        var line = transform.Find("Line").gameObject;
        if (header.line)
        {
            line.SetActive(true);
        }
        else
        {
            line.SetActive(false);
        }

        switch (header.align)
        {
            case ConstData.EnumHeaderAlign.Center:
                textLeft.gameObject.SetActive(false);
                textCenter.gameObject.SetActive(true);
                textCenter.text = GetTitle(scene);
                break;

            case ConstData.EnumHeaderAlign.Left:
                textLeft.gameObject.SetActive(true);
                textCenter.gameObject.SetActive(false);
                textLeft.text = GetTitle(scene);
                break;
        }

        var buttonMenu = transform.Find("btn_menu").gameObject;
        var buttonClose = transform.Find("btn_close").gameObject;
        var buttonBack = transform.Find("btn_back").gameObject;

        switch (header.button)
        {
            case ConstData.EnumHeaderButton.Menu:
                buttonMenu.SetActive(true);
                buttonClose.SetActive(false);
                buttonBack.SetActive(false);
                //  メニューなら戻ることはないのでシーンヒストリをクリアしておく
                //  戻るケースがあった・・・
//                CommonData.SceneHistory.Clear();
                break;

            case ConstData.EnumHeaderButton.Close:
                buttonMenu.SetActive(false);
                buttonClose.SetActive(true);
                buttonBack.SetActive(false);
                break;

            case ConstData.EnumHeaderButton.Back:
                buttonMenu.SetActive(false);
                buttonClose.SetActive(false);
                buttonBack.SetActive(true);
                break;
        }

        //  T_1から戻ることはないのでヒストリを消しておく
        if (scene == ConstData.EnumScene.T_TitleSelect)
        {
            CommonData.SceneHistory.Clear();
        }


        */



    }

    /* moritomi
    string GetTitle(ConstData.EnumScene scene)
    {
        //return "test";

        string ret;
        if (scene == ConstData.EnumScene.Tr_TraningSetting || scene == ConstData.EnumScene.Tr_TraningResult)
        {
            ret = ConstData.HeaderType[Tr_TraningSetting.GetPlaytraining()].title;
        }
        else if(scene >= ConstData.EnumScene.Tr_TrainingNeuro && scene <= ConstData.EnumScene.Tr_TrainingBreath)
        {
            ret = title.text;
        }
        else
        {
            ret = ConstData.HeaderType[scene].title;
        }
        return ret;
    }
    */


        /*
    public void ErrorDialog(string message,Callback cb = null )
    {
        var bg = transform.Find("ErrorCanvas");
        bg.gameObject.SetActive(true);
        var dialog = bg.Find("ErrorBG").Find("ErrorDialog");
        dialog.Find("Text").GetComponent<Text>().text = message;

        //  エラー用のボタン
        var okButton = dialog.Find("Ok").GetComponent<Button>();
        okButton.onClick.AddListener(() => {
            bg.gameObject.SetActive(false);
            okButton.onClick.RemoveAllListeners();
            cb?.Invoke();
        });
    }
    */



    // Update is called once per frame
    void Update()
    {
        
    }
}
