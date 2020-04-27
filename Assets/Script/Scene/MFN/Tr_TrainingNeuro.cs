using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//ニューロフィードバック
//脳活動を上げ下げするやつ！
public class Tr_TrainingNeuro : SceneBase
{

    //    const float AIR_PLANE_HEIGHT = 100;
    //    const float AREA_HEIGHT = 1920 - AIR_PLANE_HEIGHT;

    //    const float SCREEN_EDGE_DISTANCE = 0.2f; //画面端の位置 脳活動値での距離
    //    const float RING_DISTANCE = 0.05f; //リングの位置 脳活動値での距離(要らないかも)

    enum STATE
    {
        START,
        ADD,
        MOVE,
        FIN
    }

    STATE state;


    /*
    Timer timer;
    */

    //--------------------------------------------------------
    //脳波関連
    float debugXbValue = 0.2f; //よくわからんので仮脳波
    float avgXbValue = 0.2f;


    //タイムマネジメント
    float cnt = 0;
    [SerializeField] Text SpentTime;
    [SerializeField] Text cntValue;

    //取得データマネジメント
    List<float> BloodFlows = new List<float>();
        //デバッグ
    [SerializeField] Text DataCount;
    [SerializeField] Text BrainFlow;
    [SerializeField] Text XbValue;
    [SerializeField] Text AvgXbValue;

    int level = 0;
    bool firstStart = true;





    void Start()
    {
        CommonHeaderMfn.Instance.SetView(false);

#if !BLUE_DEBUG
        BrainDataMgr.Start(BrainDataFeedbacker.Type.NEURO);
#endif

        level = 9;


        //-------------------------------------
        //開始！
        state = STATE.START;


        firstStart = true;



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
                UpdateMove();
                break;
            case STATE.FIN:
                break;
        }






    }


    //紙飛行機登場。この隙に5秒間の脳波の平均値を取得
    //↑よくわからん

    void UpdateStart()
    {


        //OnHead状態で5秒間に変更
        if (Hot2gApplication.Instance.state2 == Hot2gApplication.eState.OnHead)
        {

            if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.RecieveData)//- Measureing data in stable status
            {
                cnt += Time.deltaTime;
                SpentTime.text = cnt.ToString();

            }
            else
            {
                cnt = 0;//- counter reset because of unstable situation

            }


        }
        else//- NOT On the head
        {
            cnt = 0;//- counter reset because of unstable situation
            

        }


        //デルタタイムで5秒経過した処理
        if (cnt >= 5)
        {

            cntValue.text = cnt.ToString();


            //着けている時間が5秒越えた以降に5秒毎にavgXBValueの値を更新


            //avgXbValue = GetXBValue();//- Average from 4sec to 5sec in buffer
            avgXbValue = (float)Hot2gApplication.Instance.m_nfb.calcActivenessFromBufferedUsingLastData(10);//- ave last 1 sec (10points)


            //
            cnt = 0;
            state = STATE.ADD;//- 



        }


    }



    void UpdateMove()
    {


        //- Height of the air plane
        cnt += Time.deltaTime;
        if (cnt > 0.01f)
        {
            cnt = 0;

            float xbValue = GetXBValue();
            float _length = xbValue - avgXbValue;

            XbValue.text += xbValue.ToString()+"\n";
            AvgXbValue.text += avgXbValue.ToString() + "\n";


            BrainFlow.text += _length + "\n";
            BloodFlows.Add(_length);

            DataCount.text = BloodFlows.Count.ToString();

            if ((BloodFlows.Count % 50) == 0)
            {
                BrainFlow.text = "";
                XbValue.text = "";
                AvgXbValue.text = "";
            }

        }

        /*
        if (timer.time <= 0)
        {
            BrainDataMgr.End();


        }
        */


    }





            float GetXBValue()
    {
#if !BLUE_DEBUG

        float _value = BrainDataMgr.GetValue();
#else
        float _value = debugXbValue;
#endif

        return _value;
    }


    float GetXBValue(int startIdx)
    {
#if !BLUE_DEBUG

        float _value = BrainDataMgr.GetValue(startIdx);
#else
        float _value = debugXbValue;
#endif

        return _value;
    }

    



}
