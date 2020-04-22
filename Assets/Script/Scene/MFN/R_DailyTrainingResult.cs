using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class R_DailyTrainingResult : MonoBehaviour
{
    //トレーニング後から来た場合は別処理
    public static bool fromTraining = false;

    // Start is called before the first frame update
    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);
        transform.Find("Btn_WatchTheRecord").GetComponent<Button>().onClick.AddListener(() => {
            SceneFunc.ChangeScene(ConstData.EnumScene.T_TitleSelect);
        });

        if (fromTraining)
        {
            InitTrainingData();
            fromTraining = false;
        }
        else
        {
            InitSelfCheckData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitSelfCheckData()
    {
        if (CommonData.selfCheckSaveDataList.Count<=0)
        {
            return;
        }

        //今回のデータ
        SelfCheckSaveData latestData = CommonData.selfCheckSaveDataList.OrderByDescending(d=>d.startDate).ToArray()[0];

        //前回のデータ
        SelfCheckSaveData compareData = null;
        if (CommonData.selfCheckSaveDataList.Count > 1)
        {
            compareData = CommonData.selfCheckSaveDataList.OrderByDescending(d => d.startDate).ToArray()[1];
        }

        //  平均ポイント
        var atext = transform.Find("Text_ScoreNum").GetComponent<Text>();
        atext.text = latestData.value.Average().ToString();

        var cc = transform.Find("CheckContent");
        for (int i = 0; i < 10; i++)
        {
            var obj = cc.Find("Check_content_" + (i + 1).ToString("00"));
            //  問題
            var text = obj.Find("Text_Question").GetComponent<Text>();
            text.text = latestData.name[i];

            var numBase = obj.Find("GraphNumColor");
            //  点数の色
            var image = numBase.GetComponent<Image>();
            if (latestData.value[i] < 25)
            {
                image.color = Color.blue;
            }
            else if (latestData.value[i] < 50)
            {
                image.color = Color.green;
            }
            else if (latestData.value[i] < 75)
            {
                image.color = Color.yellow;
            }
            else
            {
                image.color = Color.red;
            }
            //   点数表示
            var numText = numBase.Find("Text_GraphNum").GetComponent<Text>();
            numText.text = latestData.value[i].ToString();

            //  矢印表示
            var arrow = obj.Find("Arrow");
            string objName;
            arrow.Find("Image_NoChange").gameObject.SetActive(false);
            arrow.Find("Image_Down").gameObject.SetActive(false);
            arrow.Find("Image_Up").gameObject.SetActive(false);

            //  TODO:前回のデータと比較
            if(compareData ==null )
            {
                if (latestData.value[i] == 50)
                {
                    objName = "Image_NoChange";
                }
                else if (latestData.value[i] < 50)
                {
                    objName = "Image_Down";
                }
                else
                {
                    objName = "Image_Up";
                }
            }
            else
            {
                if (latestData.value[i] == compareData.value[i])
                {
                    objName = "Image_NoChange";
                }
                else if (latestData.value[i] < compareData.value[i])
                {
                    objName = "Image_Down";
                }
                else
                {
                    objName = "Image_Up";
                }
            }
            arrow.Find(objName).gameObject.SetActive(true);
        }
    }

    void InitTrainingData()
    {
        string[] _name = {"スコア","☆","パーフェクト","やった日時","やった時間の長さ"};
        int[] _value = 
        {
            CommonData.resultScore,
            CommonData.resultStar,
            CommonData.resultPerfect,
            CommonData.resultDate,
            CommonData.resultTime
        };

        GameObject cc = GameObject.Find("CheckContent");
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = cc.FindDescendant("Check_content_" + (i + 1).ToString("00"));
            //  問題
            Text text = obj.FindDescendant("Text_Question").GetComponent<Text>();

            GameObject numBase = obj.FindDescendant("GraphNumColor");
            Text numText = numBase.FindDescendant("Text_GraphNum").GetComponent<Text>();
            GameObject arrow = obj.FindDescendant("Arrow");


            if (i >= _name.Length)
            {
                obj.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                numBase.gameObject.SetActive(false);
                numText.gameObject.SetActive(false);
                arrow.gameObject.SetActive(false);
                continue;
            }

            text.text = _name[i];

            //  点数の色
            var image = numBase.GetComponent<Image>();
            if (_value[i] < 25)
            {
                image.color = Color.blue;
            }
            else if (_value[i] < 50)
            {
                image.color = Color.green;
            }
            else if (_value[i] < 75)
            {
                image.color = Color.yellow;
            }
            else
            {
                image.color = Color.red;
            }
            //   点数表示
            numText.text = _value[i].ToString();

            //  矢印表示
            string objName;
            arrow.FindDescendant("Image_NoChange").gameObject.SetActive(false);
            arrow.FindDescendant("Image_Down").gameObject.SetActive(false);
            arrow.FindDescendant("Image_Up").gameObject.SetActive(false);
            //  TODO:前回のデータと比較
            if (_value[i] == 50)
            {
                objName = "Image_NoChange";
            }
            else if (_value[i] < 50)
            {
                objName = "Image_Down";
            }
            else
            {
                objName = "Image_Up";
            }
            arrow.FindDescendant(objName).gameObject.SetActive(true);
        }
    }


}
