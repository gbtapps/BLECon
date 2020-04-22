﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//心拍バイオフィードバック
//心拍数を上げ下げするやつ！
public class Tr_TrainingHeartRate : SceneBase
{
    const float AIR_PLANE_HEIGHT = 100;
    const float AREA_HEIGHT = 1920 - AIR_PLANE_HEIGHT;

    const float SCREEN_EDGE_DISTANCE = 5f; //画面端の位置 心拍数での距離
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

    int level = 0;

    Image good;
    Image fade;

    int totalClearNum = 0;
    int fullComboClearNum = 0;

    bool firstStart = true;

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
        BrainDataMgr.Start(BrainDataFeedbacker.Type.BIO);
#endif
        timer = gameObject.FindDescendant("Timer").GetComponent<Timer>();
        if (Tr_TraningSetting.timeSec > 1)
        {
            timer.SetTimeSecond(Tr_TraningSetting.timeSec);
        }
        else
        {
            timer.SetTimeSecond(60);
        }

        timer.is_on = true;

        airPlane = gameObject.FindDescendant("AirPlane").GetComponent<AirPlane>();
        airPlane.Init();
        airPlane.maxHalfRange = AREA_HEIGHT / 2;

        good = gameObject.FindDescendant("Good").GetComponent<Image>();
        good.gameObject.SetActive(false);
        fade = gameObject.FindDescendant("Fade").GetComponent<Image>();

        ringMgr = new RingMgr();
        ringMgr.Init();

        level = 9;

        totalClearNum = 0;
        fullComboClearNum = 0;

        //-------------------------------------
        //開始！
        state = STATE.START;
        airPlane.StartAppear();
        firstStart = true;

        //ダイアログ
        dialog = gameObject.FindDescendant("DialogToTop");
        btnDialogYes = dialog.FindDescendant<ExButton>("btnYes");
        btnDialogNo = dialog.FindDescendant<ExButton>("btnNo");
        btnDialogCloseCol = dialog.FindDescendant<ExButton>("btnDialogCloseCol");
        dialog.gameObject.SetActive(false);

        //デバッグ
        debugArea = gameObject.FindDescendant("DebugArea");
        btnDebugNext = gameObject.FindDescendant("BtnDebugNext").AddComponent<ExButton>();
        btnDebugUp = gameObject.FindDescendant("BtnDebugUp").AddComponent<ExButton>();
        btnDebugDown = gameObject.FindDescendant("BtnDebugDown").AddComponent<ExButton>();
        textDebug = gameObject.FindDescendant("DebugText").GetComponent<Text>();
        textDebug.text = "Activeness:0";
        debugArea.gameObject.SetActive(false);

        //-
        txtRingNumberCounter = gameObject.FindDescendant("TXT_RING_NUMBER_COUNTER").GetComponent<Text>();

        CommonData.trainingStartTime = System.DateTime.Now;
    }

    private void Update()
    {
        UpdateDialog();

        //Debug.Log("77770: " + state);

        //- Update ring num counter
        txtRingNumberCounter.text = ringMgr.clearNum.ToString() + "/10";

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
                UpdateMove();
                break;
            case STATE.GOOD:
                UpdateGood();
                break;
            case STATE.FADE_OUT:
                UpdateFadeOut();
                break;
            case STATE.FADE_IN:
                UpdateFadeIn();
                break;
            case STATE.FIN:
                break;
        }
    }

    //紙飛行機登場。この隙に5秒間の脳波の平均値を取得
    void UpdateStart()
    {
        Debug.Log("7777a: " + airPlane.isAppear + ": " + cnt);

        //紙飛行機
        airPlane.UpdateApper();
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

            Debug.Log("77779: " + avgXbValue);
                        
            //avgXbValue = GetXBValue(40);//- Average from 4sec to 5sec in buffer
            avgXbValue =  (float)Hot2gApplication.Instance.m_nfb.calcActivenessFromBufferedUsingLastData(10);//- ave last 1 sec (10points)

            Debug.Log("77779: " + avgXbValue);

            cnt = 0;
            state = STATE.ADD;//- 
            ringMgr.SetNewGame(level);
        }

        if (btnDebugNext.lastHit2)
        {
#if !BLUE_DEBUG
            BrainDataMgr.End();
#endif
            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
        }

        if (timer.time <= 0)
        {
            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
            state = STATE.FIN;
        }
    }

    void UpdateMove()
    {
        UpdateDebug();

        ringMgr.HitCheck(airPlane.transform.localPosition);
        ringMgr.UpdateExec();

        if (btnDebugNext.lastHit2)
        {
#if !BLUE_DEBUG
            BrainDataMgr.End();
#endif
            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
        }

        //- Height of the air plane
        cnt += Time.deltaTime;
        if (cnt > 0.01f)
        {
            cnt = 0;

            float xbValue = GetXBValue();

            float _length = xbValue - avgXbValue;
            float _plane = _length * (AREA_HEIGHT / 2) / SCREEN_EDGE_DISTANCE;


            //           _plane = Mathf.Clamp(_plane, -AREA_HEIGHT/2, AREA_HEIGHT/2);
            airPlane.SetXbValue(_plane);
            airPlane.UpdateExec();

            if (airPlane.pos.y <= -AREA_HEIGHT / 2 + AIR_PLANE_HEIGHT / 2
                || airPlane.pos.y >= AREA_HEIGHT / 2 - AIR_PLANE_HEIGHT / 2)
            {
                cnt = 0;
                state = STATE.FADE_OUT;
            }

#if !BLUE_DEBUG
            textDebug.text = "Av:" + avgXbValue.ToString("F3");
            textDebug.text += "Ac:" + xbValue.ToString("F3");
            textDebug.text += "Ne:" + _plane.ToString("F3");
#endif
        }

        if (timer.time <= 0)
        {
#if !BLUE_DEBUG
            BrainDataMgr.End();
#endif
            SetResultData();
            SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
            state = STATE.FIN;
        }

        //1セット終了
        if (ringMgr.oneSetClear)
        {
            cnt = 0;
            level--;
            if (level < 0) { level = 0; }
            totalClearNum++;
            if (ringMgr.comboNum >= RingMgr.ONE_SET)
            {
                fullComboClearNum++;
            }

            good.gameObject.SetActive(true);
            state = STATE.GOOD;
        }
    }

    void UpdateDebug()
    {
        if (btnDebugUp.isPress)
        {
            debugXbValue += Time.deltaTime / 10;
        }
        if (btnDebugDown.isPress)
        {
            debugXbValue -= Time.deltaTime / 10;
        }

    }

    void UpdateGood()
    {
        cnt += Time.deltaTime;
        if (cnt >= 0.7f)
        {
            cnt = 0;
            good.gameObject.SetActive(false);
            state = STATE.FADE_OUT;
        }
    }

    void UpdateFadeOut()
    {
        cnt += Time.deltaTime;
        fade.color = new Color(0, 0, 0, cnt / (1.3f / 2));
        if (cnt >= 1.3f / 2)
        {
            cnt = 1.3f / 2;
            fade.color = new Color(0, 0, 0, 1);
            airPlane.ResetPosY();
            ringMgr.Destroy();
            debugXbValue = 0.2f;

            airPlane.SetVisibleTextCnt(true);

            state = STATE.FADE_IN;
            txtRingNumberCounter.text = "0/10";
        }
    }

    void UpdateFadeIn()
    {
        cnt -= Time.deltaTime;
        fade.color = new Color(0, 0, 0, cnt / (1.3f / 2));
        if (cnt <= 0)
        {
            cnt = 0;
            fade.color = new Color(0, 0, 0, 0);
            state = STATE.START;
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


    void UpdateDialog()
    {
        if (dialog.activeSelf)
        {
            if (btnDialogYes.lastHit2)
            {
#if !BLUE_DEBUG
            BrainDataMgr.End();
#endif
                SceneFunc.ChangeScene(ConstData.EnumScene.T_TitleSelect, false);
                state = STATE.FIN;
                return;
            }
            if (btnDialogNo.lastHit2 || btnDialogCloseCol.lastHit2)
            {
                dialog.SetActive(false);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialog.SetActive(true);
            }
        }
    }


    void SetResultData()
    {
        if (totalClearNum >= 5)
        {
            CommonData.resultStar = (int)Mathf.Floor(((float)fullComboClearNum / (float)totalClearNum) / 0.2f);
        }
        else
        {
            CommonData.resultStar = 0;
        }

        CommonData.resultScore = ringMgr.score;
        CommonData.resultPerfect = ringMgr.totalperfect;
        CommonData.resultDate = 0;
        CommonData.resultTime = 0;
        CommonData.trainingEndTime = System.DateTime.Now;

    }
}
