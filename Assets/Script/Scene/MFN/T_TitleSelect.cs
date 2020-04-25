using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_TitleSelect : SceneBase
{
    ExButton btnNeuro;          //ニューロフィードバック

//    Image imgBrainMeter;

    Button btnBlueTooth;

    bool bleConnected = false;

    Text textDebug;

    [SerializeField]Text AppVersion;


    void Start()
    {

        AppVersion.text = Application.version;

        CommonHeaderMfn.Instance.SetView(false);

        //ニューロフィードバック
        btnNeuro = gameObject.FindDescendant("btn_NeuroFeedback").AddComponent<ExButton>();


        //BlueTooth
        btnBlueTooth = transform.Find("btn_Connect").GetComponent<Button>();


        BluetoothMgr.Instance.SetButton(btnBlueTooth);
        btnBlueTooth.onClick.AddListener(() => {
            BluetoothMgr.Instance.Bluetooth();
        });



        textDebug = gameObject.FindDescendant("DebugText").GetComponent<Text>();
        textDebug.gameObject.SetActive(false);

        btnNeuro.SetColor(Color.gray);


        /*             
        var txt_Title = transform.Find("txt_Title_NF").GetComponent<Text>();
        txt_Title.text = ConstData.HeaderType[ConstData.EnumScene.Tr_TrainingNeuro].title; ;
        */

                
    }

    private void Update()
    {
        UpdateBlueTooth();

        //ニューロフィードバック
        if (btnNeuro.lastHit2 && bleConnected)
        {
            Tr_TraningSetting.SetPlayTraining(ConstData.EnumScene.Tr_TrainingNeuro);
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningSetting, true);
        }
        


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
//            btnNeuro.SetColor(Color.white);

        }
        else
        {
//            btnNeuro.SetColor(Color.gray);

        }

//        textDebug.text = Hot2gApplication.Instance.mode.ToString();

    }
}
