using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tr_TraningSetting : SceneBase
{
    //実行するトレーニング
    static ConstData.EnumScene playTraining;
    public static void SetPlayTraining(ConstData.EnumScene _playTraining)
    {
        playTraining = _playTraining;

        if(_playTraining == ConstData.EnumScene.Tr_TrainingNeuro)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Neuro;
        }
        if(_playTraining == ConstData.EnumScene.Tr_TrainingHeartRate)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Heart;
        }
        if(_playTraining == ConstData.EnumScene.Tr_TrainingBreath)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Breath;
        }
    }

    public static ConstData.EnumScene GetPlaytraining()
    {
        return playTraining;
    }

    //トレーニング時間
    public static float timeSec { get; private set; }

    Text textOverview;
    Slider slider;
    ExButton btnStart;



    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);
        textOverview = gameObject.FindDescendant("Text_Notice01").GetComponent<Text>();
        slider = gameObject.FindDescendant("Slider").GetComponent<Slider>();
        btnStart = gameObject.FindDescendant("Btn_Start").AddComponent<ExButton>();

        //初期値5分
        timeSec = 60 * 5;
        slider.onValueChanged.AddListener((float value)=>{
            timeSec = 60 + (value * 9 * 60); //1分から10分
        });


        if (playTraining == ConstData.EnumScene.Tr_TrainingNeuro)
        {
            textOverview.text = "aaaa";
        }
        else if (playTraining == ConstData.EnumScene.Tr_TrainingHeartRate)
        {
            textOverview.text = "bbbb";
        }
        else if (playTraining == ConstData.EnumScene.Tr_TrainingBreath)
        {
            textOverview.text = "cccc";
        }
    }
          
    private void Update()
    {

        if (btnStart.lastHit2)
        {
            SceneFunc.ChangeScene(playTraining, false);
        }
    }
}
