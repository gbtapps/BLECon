using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tr_TraningResult : SceneBase
{
    const int STAR_NUM = 5;

    Text textScore;
    Text textPerfect;
    ExButton btnNext;
    //星
    GameObject[] evaluationStars = new GameObject[5];

    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);
        textScore = gameObject.FindDescendant("Text_ScoreNum").GetComponent<Text>();
        textPerfect = gameObject.FindDescendant("Text_PerfectNum").GetComponent<Text>();
        btnNext = gameObject.FindDescendant("Btn_Next").AddComponent<ExButton>();

        for(int i=0; i< STAR_NUM; i++)
        {
            evaluationStars[i] = gameObject.FindDescendant("EvaluationStar_"+(i+1).ToString());
        }


        for(int i=0;i< 5; i++)
        {
            if(i<CommonData.resultStar)
            {
                evaluationStars[i].SetActive(true);
            }
            else
            {
                evaluationStars[i].SetActive(false);
            }
        }

        textScore.text = CommonData.resultScore.ToString();
        textPerfect.text = CommonData.resultPerfect.ToString()+"個";

        //数息トレーニングは特殊処理
        if (CommonData.resultType == (int)R_ResultLog.ContentType.Breath)
        {
            //パーフェクト無しでLF/HFを表示1
            //textPerfect.text = CommonData.resultLFHF.ToString("F1");
            //gameObject.FindDescendant<Text>("Text_Perfect").text ="LF/HF";

            //星無し
            for (int i = 0; i < 5; i++)
            {
                evaluationStars[i].SetActive(false);
            }
        }

        Save();

    }

    private void Update()
    {
        if (btnNext.lastHit2)
        {
//            R_DailyTrainingResult.fromTraining = true;
//            SceneFunc.ChangeScene(ConstData.EnumScene.R_DailyTrainingResult, false);
           SceneFunc.ChangeScene(ConstData.EnumScene.T_TitleSelect, false);
        }
    }

    void Save()
    {
        //ローカルに保存
        TrainingSaveData _data = new TrainingSaveData();

        _data.startDate = CommonData.trainingStartTime;
        _data.endDate = CommonData.trainingEndTime;

        _data.type = CommonData.resultType;
        _data.score = CommonData.resultScore;
        _data.star = CommonData.resultStar;
        _data.perfect = CommonData.resultPerfect;

        string _filename = "training_" + _data.startDate.Year + _data.startDate.Month + _data.startDate.Day + _data.startDate.Hour + _data.startDate.Minute + _data.startDate.Second;
        SaveData.Save(_filename, LitJson.JsonMapper.ToJson(_data),CommonData.trainingDir+"/");
        Debug.Log("Save:" + _filename);
        CommonData.trainingSaveData.Add(_data);


    }
}
