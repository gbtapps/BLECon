using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class C_SelfCheck : MonoBehaviour
{
    //  今何番目か
    int m_q = 0;
    Text m_QNum;
    Text m_QText;
    Slider m_slider;


    class TItem
    {
        public string name;
        public string uid;
        public string[] courses;
        public string _id;
        public string createdAt;
        public string updatedAt;
        public int __v;
        public string id;
        public string trainings;
    }

    // Start is called before the first frame update
    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);

        transform.Find("Btn_Next").GetComponent<Button>().onClick.AddListener(() => {
            Next();
        });

        string[] qtext = {
                    "Q1仮テキスト",
                    "Q2仮テキスト",
                    "Q3仮テキスト",
                    "Q4仮テキスト",
                    "Q5仮テキスト",
                    "Q6仮テキスト",
                    "Q7仮テキスト",
                    "Q8仮テキスト",
                    "Q9仮テキスト",
                    "Q10仮テキスト"
                };

        ChecksObject[] debugData = new ChecksObject[10];

        for (int j = 0; j < 10; j++)
        {
            debugData[j] = new ChecksObject();
            debugData[j].id = j.ToString();
            debugData[j].name = qtext[j];
        }
        CommonData.selfCheckObject = debugData;
        CommonData.selfCheckValue = new int[CommonData.selfCheckObject.Length];

        var q = transform.Find("Question");
        m_QNum = q.Find("Text_QuestionNo").GetComponent<Text>();
        m_QText = q.Find("Text_Question").GetComponent<Text>();
        m_slider = q.Find("Slider").GetComponent<Slider>();
        m_slider.minValue = 0;
        m_slider.maxValue = 100;
        m_slider.value = 50;
        m_slider.gameObject.AddComponent<C_2_Slider>();

        var button = transform.Find("Btn_Next").GetComponent<Button>();
        button.interactable = false;
        m_QNum.text = "";
        m_QText.text = "";

        //  項目の取得はC_1でやってる
        Disp();
        CommonData.selfCheckStartTime = DateTime.Now;

    }

    void Disp()
    {
        m_QNum.text = "Q" + (m_q + 1).ToString();
        m_QText.text = CommonData.selfCheckObject[m_q].name;
        var button = transform.Find("Btn_Next").GetComponent<Button>();
        button.interactable = false;
    }

    void Next()
    {
        CommonData.selfCheckValue[m_q] = Mathf.FloorToInt(m_slider.value);
        m_q++;
        if (m_q < CommonData.selfCheckObject.Length)
        {
            m_slider.value = 50;
            Disp();
        }
        else
        {
            CommonData.selfCheckEndTime = DateTime.Now;
            SceneFunc.ChangeScene(ConstData.EnumScene.R_DailyTrainingResult,false);
            Save();
        }
    }

    void Save()
    {
        //ローカルに保存
        SelfCheckSaveData _data = new SelfCheckSaveData();

        _data.startDate = CommonData.selfCheckStartTime;
        _data.endDate = CommonData.selfCheckEndTime;

        _data.name = new string[10];
        _data.value = new int[10];
        for (int i=0;i<10;i++)
        {
            _data.name[i] = CommonData.selfCheckObject[i].name;
            _data.value[i] = CommonData.selfCheckValue[i];
        }
        string _filename = "selfcheck_" + _data.startDate.Year+ _data.startDate.Month+ _data.startDate.Day+ _data.startDate.Hour+ _data.startDate.Minute+_data.startDate.Second;
        SaveData.Save(_filename, LitJson.JsonMapper.ToJson(_data), CommonData.selfCheckDir +"/");
        Debug.Log("Save:" + _filename);
        CommonData.selfCheckSaveDataList.Add(_data);

        //サーバに保存
        var _postObj = new SelfCheckResultPostObject();
        _postObj.xb01id = Hot2gApplication.Instance.API.getHot2gIf().getHot2gDevInfo().valDevId;
        for (int i = 0; i < 10; i++)
        {
            _postObj.checkname.Add(CommonData.selfCheckObject[i].name);
            _postObj.checkscore.Add(CommonData.selfCheckValue[i]);
        }
        WebAPI.Instance.SelfCheckResultPost(_postObj);

    }
}


