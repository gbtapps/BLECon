using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class R_ResultLog : MonoBehaviour
{
    public enum ContentType {
        Neuro,
        Heart,
        Breath,
        SelfCheck
    }

    //現在選択中
    ContentType currentType;

    string[] titleArray =
    {
        "ブレインコントロール",
        "心拍コントロール",
        "数息観メディテーション",
        "セルフチェック"
    };


    //表示中のタイトル
    Text textTitle;

    //左右の矢印ボタン
    ExButton btnLeft;
    ExButton btnRight;


    //親
    ScrollRect[] scrollRects = new ScrollRect[4];
    GameObject[] resultContentParents = new GameObject[4];

    //インスペクターで指定しとく
    [SerializeField]
    GameObject prefabTraining;
    [SerializeField] 
    GameObject prefabSelfCheck;
    [SerializeField]
    Sprite[] spriteIcon;


    // Start is called before the first frame update
    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);

        currentType = ContentType.SelfCheck;

        textTitle = gameObject.FindDescendant("Text_LogTitle").GetComponent<Text>();
        textTitle.text = titleArray[(int)ContentType.Neuro];

        btnLeft  = gameObject.FindDescendant("Btn_arrowL").AddComponent<ExButton>();
        btnRight = gameObject.FindDescendant("Btn_arrowR").AddComponent<ExButton>();

        resultContentParents[(int)ContentType.Neuro] = gameObject.FindDescendant("ResultContent_Neuro");
        resultContentParents[(int)ContentType.Heart] = gameObject.FindDescendant("ResultContent_HeartRate");
        resultContentParents[(int)ContentType.Breath] = gameObject.FindDescendant("ResultContent_Breath");
        resultContentParents[(int)ContentType.SelfCheck] = gameObject.FindDescendant("ResultContent_SelfCheck");

        scrollRects[(int)ContentType.Neuro] = gameObject.FindDescendant<ScrollRect>("Scroll_View_Neuro");
        scrollRects[(int)ContentType.Heart] = gameObject.FindDescendant<ScrollRect>("Scroll_View_HeartRate");
        scrollRects[(int)ContentType.Breath] = gameObject.FindDescendant<ScrollRect>("Scroll_View_Breath");
        scrollRects[(int)ContentType.SelfCheck] = gameObject.FindDescendant<ScrollRect>("Scroll_View_Self");

        CreateContentsTab();
        ChangeContent(currentType);
    }

    void CreateContentsTab()
    {
        //トレーニング
        List<TrainingSaveData> _trainingDatas = CommonData.trainingSaveData.OrderByDescending(d => d.startDate).ToList();
        for (int i = 0; i < _trainingDatas.Count; i++)
        {
            TrainingSaveData _data = _trainingDatas[i];
            GameObject _obj = Instantiate(prefabTraining, resultContentParents[_data.type].transform);

            _obj.FindDescendant<Text>("Text_MonthDay").text = _data.startDate.Month + "月" + _data.startDate.Day + "日";
            _obj.FindDescendant<Text>("Text_Time").text = _data.startDate.Hour + ":" + _data.startDate.Minute.ToString("00") + "ー" + _data.endDate.Hour + ":" + _data.endDate.Minute.ToString("00");
            _obj.FindDescendant<Text>("Text_ScoreNum").text = _data.score.ToString();
            _obj.FindDescendant<Text>("Text_ParfectNum").text = _data.perfect.ToString();

            _obj.FindDescendant<Image>("Image_Icon").sprite = spriteIcon[(int)_data.type];

            if (_data.type == (int)ContentType.Breath)
            {
                _obj.FindDescendant("Text_Perfect").SetActive(false);
                _obj.FindDescendant("Text_ParfectNum").SetActive(false);
            }

            for(int j=0;j<5;j++)
            {
                if (j>=_data.star || _data.type == (int)ContentType.Breath)
                {
                    _obj.FindDescendant("Star" + (j + 1).ToString()).SetActive(false);
                }
            }

            //縞々にする
            if(resultContentParents[_data.type].transform.childCount % 2==0)
            {
                _obj.GetComponent<Image>().color = Color.white;
            }

        }


        //セルフチェック
        List<SelfCheckSaveData>_selfDatas = CommonData.selfCheckSaveDataList.OrderByDescending(d=>d.startDate).ToList();
        for (int i=0;i< _selfDatas.Count;i++)
        {
            SelfCheckSaveData _data = _selfDatas[i];
            GameObject _obj = Instantiate(prefabSelfCheck,resultContentParents[(int)ContentType.SelfCheck].transform);

            _obj.FindDescendant("NewIcon").SetActive(false);
            _obj.FindDescendant<Text>("Text_MonthDay").text = _data.startDate.Month + "月" + _data.startDate.Day + "日";
            _obj.FindDescendant<Text>("Text_Time").text = _data.startDate.Hour+":"+ _data.startDate.Minute.ToString("00") + "ー"+ _data.endDate.Hour + ":" + _data.endDate.Minute.ToString("00");

            for(int j=0;j<10;j++)
            {
                _obj.FindDescendant("Q" + (j + 1).ToString()).FindDescendant<Text>("Text_QuestionPointNum").text = _data.value[j].ToString();
            }
            if (resultContentParents[(int)ContentType.SelfCheck].transform.childCount % 2 == 0)
            {
                _obj.GetComponent<Image>().color = Color.white;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (btnLeft.lastHit2)
        {
            currentType--;
            if (currentType < 0) { currentType = ContentType.SelfCheck; }
            ChangeContent(currentType);
        }
        if (btnRight.lastHit2)
        {
            currentType++;
            if (currentType > ContentType.SelfCheck) { currentType = ContentType.Neuro; }
            ChangeContent(currentType);
        }
    }

    void ChangeContent(ContentType _contentType)
    {
        textTitle.text = titleArray[(int)_contentType];
        for(int i=0;i<resultContentParents.Length;i++)
        {
            resultContentParents[i].gameObject.SetActive((i==(int)_contentType));
            scrollRects[i].gameObject.SetActive((i == (int)_contentType));
            scrollRects[i].verticalNormalizedPosition = 1f;
        }
    }

}
