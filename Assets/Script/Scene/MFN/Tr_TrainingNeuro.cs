using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//ニューロフィードバック
//脳活動を上げ下げするやつ！
public class Tr_TrainingNeuro : SceneBase
{
    const float AIR_PLANE_HEIGHT = 100;
    const float AREA_HEIGHT = 1920 - AIR_PLANE_HEIGHT;

    const float SCREEN_EDGE_DISTANCE = 0.2f; //画面端の位置 脳活動値での距離
    const float RING_DISTANCE = 0.05f; //リングの位置 脳活動値での距離(要らないかも)

    enum STATE
    {
        START,
        ADD,
        MOVE,
        GOOD,
        FADE_OUT,
        FADE_IN,
        FIN
    }

    STATE state;

    Timer timer;
    AirPlane airPlane;

    //--------------------------------------------------------
    //脳波関連
    float debugXbValue = 0.2f; //よくわからんので仮脳波
    float avgXbValue = 0.2f;


    float cnt = 0;

    RingMgr ringMgr;

    int level=0;

    Image good;
    Image fade;

    int totalClearNum = 0;
    int fullComboClearNum = 0;

    bool firstStart=true;

    //ダイアログ
    GameObject dialog;
    ExButton btnDialogCloseCol;
    ExButton btnDialogYes;
    ExButton btnDialogNo;

    //デバッグ
    GameObject debugArea;
    ExButton btnDebugNext;
    ExButton btnDebugUp;
    ExButton btnDebugDown;
    Text textDebug;

    Text txtRingNumberCounter;

    void Start()
    {
        CommonHeaderMfn.Instance.SetView(false);

#if !BLUE_DEBUG
        BrainDataMgr.Start(BrainDataFeedbacker.Type.NEURO);
#endif
        
        level = 9;

        totalClearNum = 0;
        fullComboClearNum = 0;

        //-------------------------------------
        //開始！
        state = STATE.START;
        airPlane.StartAppear();
        firstStart = true;

        

        //-
        txtRingNumberCounter = gameObject.FindDescendant("TXT_RING_NUMBER_COUNTER").GetComponent<Text>();


#if !BLUE_DEBUG
        debugArea.gameObject.SetActive(false);
#endif

        CommonData.trainingStartTime = System.DateTime.Now;
    }

    private void Update()
    {
        

        switch (state)
        {
            case STATE.START:
                UpdateStart();
                break;
            case STATE.ADD:
//                ringMgr.AddOneSet(level);
                state = STATE.MOVE;
                break;
            case STATE.MOVE:
//                UpdateMove();
                break;
            case STATE.GOOD:
//                UpdateGood();
                break;
            case STATE.FADE_OUT:
//                UpdateFadeOut();
                break;
            case STATE.FADE_IN:
//                UpdateFadeIn();
                break;
            case STATE.FIN:
                break;
        }

//#if !BLUE_DEBUG
//            textDebug.text = "Av:" + avgXbValue.ToString("F3");
//            textDebug.text += "Ac:" + xbValue.ToString("F3");
//            textDebug.text += "Ne:" + _plane.ToString("F3");
//            textDebug.text += "st2:" + Hot2gApplication.Instance.state2.ToString();
//#endif

    }




    //紙飛行機登場。この隙に5秒間の脳波の平均値を取得
    void UpdateStart()
    {

        //紙飛行機
//        airPlane.UpdateApper();
        
        //脳波
        //OnHead状態で5秒間に変更
        if (Hot2gApplication.Instance.state2 == Hot2gApplication.eState.OnHead)
        {
            if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.RecieveData)//- Measureing data in stable status
            {
                cnt += Time.deltaTime;
                airPlane.SetTextCnt((5 - cnt).ToString("F0"));
            }
            else
            {
                cnt = 0;//- counter reset because of unstable situation
                airPlane.SetTextCnt("Oo.");
            }
        }
        else//- NOT On the head
        {
            cnt = 0;//- counter reset because of unstable situation
            airPlane.SetTextCnt("*");
        }

        //if (airPlane.isAppear == true && cnt >= 5)
        if (cnt >= 5)
        {
            airPlane.SetVisibleTextCnt(false);

            //Debug.Log("77779: " + avgXbValue);
            //avgXbValue = GetXBValue();//- Average from 4sec to 5sec in buffer
            avgXbValue = (float)Hot2gApplication.Instance.m_nfb.calcActivenessFromBufferedUsingLastData(10);//- ave last 1 sec (10points)
            //Debug.Log("77779: " + avgXbValue);

            cnt = 0;
            state = STATE.ADD;//- 
            ringMgr.SetNewGame(level);
        }


        if (btnDebugNext.lastHit2)
        {
#if !BLUE_DEBUG
            BrainDataMgr.End();
#endif
//            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
        }

        if (timer.time <= 0)
        {
//            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
            state = STATE.FIN;
        }
    }

    


    float GetXBValue()
    {
#if !BLUE_DEBUG
        //            float _value = (float)BMBrainMgr.getCurrentActiveness();
        float _value = BrainDataMgr.GetValue();
#else
        float _value = debugXbValue;
#endif

        return _value;
    }

    float GetXBValue(int startIdx)
    {
#if !BLUE_DEBUG
        //            float _value = (float)BMBrainMgr.getCurrentActiveness();
        float _value = BrainDataMgr.GetValue(startIdx);
#else
        float _value = debugXbValue;
#endif

        return _value;
    }

    



}
