﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//数息トレーニング
//墜落しないように頑張るやつ！
public class Tr_TrainingBreath : SceneBase
{
    const float AIR_PLANE_HEIGHT = 100;
    const float AREA_HEIGHT = 1920 - AIR_PLANE_HEIGHT;

    const float SCREEN_EDGE_DISTANCE = 0.2f; //画面端の位置 脳活動値での距離
    const float RING_DISTANCE = 0.05f; //リングの位置 脳活動値での距離(要らないかも)

    enum STATE
    {
        START,
        MOVE,
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
    Image fade;

    float totalTime; //ゲームのトータル時間
    float goodTime;  //下部にいた時間

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

    Text txtScore;

    float timerKeepBottom = 0;

    void Start()
    {
        CommonHeaderMfn.Instance.SetView(false);

#if !BLUE_DEBUG
        BrainDataMgr.Start(BrainDataFeedbacker.Type.NEURO);
#endif
        timer = gameObject.FindDescendant("Timer").GetComponent<Timer>();
        if (Tr_TraningSetting.timeSec > 1)
        {
            timer.SetTimeSecond(Tr_TraningSetting.timeSec);
        }
        else
        {
            timer.SetTimeSecond(10);
        }

        timer.is_on = true;

        airPlane = gameObject.FindDescendant("AirPlane").GetComponent<AirPlane>();
        airPlane.Init();
        airPlane.maxHalfRange = AREA_HEIGHT / 2;

        fade = gameObject.FindDescendant("Fade").GetComponent<Image>();

        //-------------------------------------
        //開始！
        state = STATE.START;
        airPlane.StartAppear();

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
        CommonData.trainingStartTime = System.DateTime.Now;

        txtScore = gameObject.FindDescendant("txtScore").GetComponent<Text>();
    }

    private void Update()
    {
        UpdateDialog();

        if (timer.time <= 0 || btnDebugNext.lastHit2)
        {
            //state = STATE.FADE_OUT;
            //state = STATE.FIN;
            airPlane.StartBreathFadeOut();
            Finish();
        }

        switch (state)
        {
            case STATE.START:
                UpdateStart();
                break;
            case STATE.MOVE:
                UpdateMove();
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
        //紙飛行機
        airPlane.UpdateApper();

        timerKeepBottom = Time.realtimeSinceStartup;

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
            //avgXbValue = GetXBValue();//- Average from 4sec to 5sec in buffer
            avgXbValue = (float)Hot2gApplication.Instance.m_nfb.calcActivenessFromBufferedUsingLastData(10);//- ave last 1 sec (10points)
            cnt = 0;
            state = STATE.MOVE;
        }

        if (btnDebugNext.lastHit2)
        {
            airPlane.StartBreathFadeOut();
            state = STATE.FADE_OUT;
        }
    }

    void UpdateMove()
    {
        UpdateDebug();

        cnt += Time.deltaTime;
        totalTime += Time.deltaTime;
        if (airPlane.pos.y < -1920 / 4)
        {
            goodTime += Time.deltaTime;
            txtScore.text = goodTime.ToString("#");
        }

        if (cnt > 0.01f)
        {
            cnt = 0;

            float xbValue = GetXBValue();

            float _length = xbValue - avgXbValue;
            float _plane = _length * (AREA_HEIGHT / 2) / SCREEN_EDGE_DISTANCE;

            //- check if Plane hight is too low
            //-
            if (_plane <= -AREA_HEIGHT / 2 )
            {
                if (Time.realtimeSinceStartup - timerKeepBottom > 5)
                {
                    avgXbValue = xbValue;
                    timerKeepBottom = Time.realtimeSinceStartup;
                }
            }
            else
            {
                timerKeepBottom = Time.realtimeSinceStartup;
            }
            //-----

            _plane = Mathf.Clamp(_plane, -AREA_HEIGHT/2, AREA_HEIGHT/2);
            airPlane.SetXbValue(_plane);
            airPlane.UpdateExec();

            //if (airPlane.pos.y <= -AREA_HEIGHT / 2 + AIR_PLANE_HEIGHT / 2)|| airPlane.pos.y >= AREA_HEIGHT / 2 - AIR_PLANE_HEIGHT / 2)
            if (airPlane.pos.y >= AREA_HEIGHT / 2 - AIR_PLANE_HEIGHT / 2)
            {
                cnt = 0;
                state = STATE.FADE_OUT;
            }
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

    void UpdateFadeOut()
    {
        cnt += Time.deltaTime;
        fade.color = new Color(0, 0, 0, cnt / (1.3f / 2));
        if (cnt >= 1.3f / 2)
        {
            cnt = 1.3f / 2;
            fade.color = new Color(0, 0, 0, 1);
            airPlane.ResetPosY();
            debugXbValue = 0.2f;

            airPlane.SetVisibleTextCnt(true);

            state = STATE.FADE_IN;
        }
    }

    void UpdateFadeIn()
    {
        cnt -= Time.deltaTime;
        fade.color = new Color(0,0,0,cnt/ (1.3f / 2));
        if (cnt <= 0)
        {
            cnt = 0;
            fade.color = new Color(0,0,0,0);
            state = STATE.START;
        }
    }


    float GetXBValue()
    {
#if !BLUE_DEBUG
            float _value = (float)BrainDataMgr.GetValue();
#else
        float _value = debugXbValue;
#endif

        return _value;
    }

    //トップ画面に戻るダイアログ
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

    void Finish()
    {
#if !BLUE_DEBUG
        BrainDataMgr.End();
        //LF/HF計算用ダミー関数はここです。使ってないパーフェクトを代用
        CommonData.resultLFHF = LFHF_DUMMY(Hot2gApplication.Instance.GetBreathBuffer());
#else
        CommonData.resultLFHF = 0.1;
#endif
        // スコア：（下部の1 / 4に居た時間）/（全時間）*100​
        if (totalTime > 0)
        {
            CommonData.resultScore = (int)((goodTime / totalTime) * 100f);
        }
        else
        {
            CommonData.resultScore = 0;
        }
        CommonData.trainingEndTime = System.DateTime.Now;

        SceneFunc.ChangeScene(ConstData.EnumScene.Tr_TraningResult, false);
        state = STATE.FIN;
    }

    //-----------------------------------------------------------
    //LF/HF計算用ダミー関数です！
    int LFHF_DUMMY(List<double> _breathBuffer)
    {
        return 0;
    }
}
