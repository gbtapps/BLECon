using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_TitleSelect : SceneBase
{
    ExButton btnNeuro;          //ニューロフィードバック
    ExButton btnHeartRate;      //バイオフィードバック
    ExButton btnBrainMeter;     //数息トレーニング(ブレインメーター)
    ExButton btnResultSummary;  //日々のトレーニング結果サマリー画面
    ExButton btnSelfCheck;      //セルフチェック

    Image imgBrainMeter;

    Button btnBlueTooth;

    bool bleConnected = false;

    Text textDebug;

    void Start()
    {
        CommonHeaderMfn.Instance.SetView(false);

        //ニューロフィードバック
        btnNeuro = gameObject.FindDescendant("btn_NeuroFeedback").AddComponent<ExButton>();

        /*
        //バイオフィードバック
        btnHeartRate = gameObject.FindDescendant("btn_BioFeedback").AddComponent<ExButton>();
        //数息トレーニング
        btnBrainMeter = gameObject.FindDescendant("btn_NumberBreath").AddComponent<ExButton>();
        //結果
        btnResultSummary = gameObject.FindDescendant("btn_result").AddComponent<ExButton>();
        //セルフチェック
        btnSelfCheck = gameObject.FindDescendant("btn_SelfCheck").AddComponent<ExButton>();
        */


        //BlueTooth
        btnBlueTooth = transform.Find("btn_Connect").GetComponent<Button>();

        BluetoothMgr.Instance.SetButton(btnBlueTooth);
        btnBlueTooth.onClick.AddListener(() => {
            BluetoothMgr.Instance.Bluetooth();
        });


        //数息トレーニング
//        imgBrainMeter = gameObject.FindDescendant("btn_NumberBreath").GetComponent<Image>();

        textDebug = gameObject.FindDescendant("DebugText").GetComponent<Text>();
        textDebug.gameObject.SetActive(false);

        btnNeuro.SetColor(Color.gray);


//        btnHeartRate.SetColor(Color.gray);
//        btnBrainMeter.SetColor(Color.gray);
                     
        var txt_Title = transform.Find("txt_Title_NF").GetComponent<Text>();
        txt_Title.text = ConstData.HeaderType[ConstData.EnumScene.Tr_TrainingNeuro].title; ;
                
        /*
        txt_Title = transform.Find("txt_Title_BF").GetComponent<Text>();
        txt_Title.text = ConstData.HeaderType[ConstData.EnumScene.Tr_TrainingHeartRate].title; ;
                
        txt_Title = transform.Find("txt_Title_BR").GetComponent<Text>();
        txt_Title.text = ConstData.HeaderType[ConstData.EnumScene.Tr_TrainingBreath].title;
          */
          

        txt_Title = transform.Find("txt_Title_Start").GetComponent<Text>();
        txt_Title.text = ConstData.HeaderType[ConstData.EnumScene.T_TitleSelect].title;
    

        //        WebAPI.Instance.TrainingResultGet();
        //        WebAPI.Instance.SelfCheckResultGet();
    }

    private void Update()
    {
        UpdateBlueTooth();

        //ニューロフィードバック
//        if (btnNeuro.lastHit2)
        if (btnNeuro.lastHit2 && bleConnected)
        {
            Tr_TraningSetting.SetPlayTraining(ConstData.EnumScene.Tr_TrainingNeuro);
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningSetting, true);
        }

        /*
        //心拍数バイオフィードバック
        if (btnHeartRate.lastHit2 && bleConnected)
        {
            Tr_TraningSetting.SetPlayTraining(ConstData.EnumScene.Tr_TrainingHeartRate);
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningSetting, true);
        }
        */

        /*
        //数息トレーニング
        if (btnBrainMeter.lastHit2 && bleConnected)
        {
            Tr_TraningSetting.SetPlayTraining(ConstData.EnumScene.Tr_TrainingBreath);
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningSetting, true);
        }
        */

        /*
        //履歴
        if (btnResultSummary.lastHit2)
        {
            SceneFunc.ChangeScene(ConstData.EnumScene.R_ResultLog, true);
        }
        */

        /*
        //セルフチェック
        if (btnSelfCheck.lastHit2)
        {
            SceneFunc.ChangeScene(ConstData.EnumScene.C_SelfCheck, true);

//# if !UNITY_EDITOR
//#else
//            SceneFunc.ChangeScene(ConstData.EnumScene.C_SelfCheck, false);
//#endif
        }
        */

    }

    void UpdateBlueTooth()
    {
#if !BLUE_DEBUG
        if(Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting || Hot2gApplication.Instance.mode == Hot2gApplication.eMode.RecieveData || Hot2gApplication.Instance.mode == Hot2gApplication.eMode.GainSetting)
        {
            bleConnected = true;
        }
#else
        bleConnected = true;
#endif
        if (bleConnected)
        {
            btnNeuro.SetColor(Color.white);
//            btnHeartRate.SetColor(Color.white);
//            btnBrainMeter.SetColor(Color.white);
        }
        else
        {
            btnNeuro.SetColor(Color.gray);
//            btnHeartRate.SetColor(Color.gray);
//            btnBrainMeter.SetColor(Color.gray);
        }

//        textDebug.text = Hot2gApplication.Instance.mode.ToString();

    }
}
