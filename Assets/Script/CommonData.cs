using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


//  共通のデータを置いておくためのクラス
public class CommonData : SingletonMonoBehaviour<CommonData>
{
    //  debug用開始シーンの設定
    [SerializeField] private ConstData.EnumScene m_startScene = ConstData.EnumScene.T_TitleSelect;
    //  現在のシーン(SceneManagerから取れるけど利便性のため
    public static ConstData.EnumScene NowScene = ConstData.EnumScene.T_TitleSelect;
    //  バックボタン用のヒストリ
    public static Stack<ConstData.EnumScene> SceneHistory = new Stack<ConstData.EnumScene>();

    //  ユーザー種別
    [SerializeField] ConstData.EnumUserRole m_UserRole = ConstData.EnumUserRole.Basic;
    public static ConstData.EnumUserRole UserRole;

    //  実行するトレーニング種別
    public static int TrainingID = 1;

    //  brainmeterのカテゴリ関連
    public static List<string> BMCategoryRecent = new List<string>();
    public static int BMCategoryNew = 0;

    public static bool logout = false;

    //  トレーニングのデータ
    public static int trainingNum;

    //  紛らわしい、全トレーニングのデータ取ってくるときと微妙に型が違うので注意

    public static int level = 1;
    public static int correctNum;
    public static int failedNum;

    //  brainmeterの自由入力
    public static string FreeSceneName="";
    public static string SceneID = "";

    //  セルフチェックのデータ(今回の分だけ)
    public static int[] selfCheckValue;
    public static Dictionary<string, int> selfCheckDic = new Dictionary<string, int>();


    public static bool firstCheck = false;

    //-------------------------------------------------------------------
    //リザルト用
    public static int resultScore;
    public static int resultStar;
    public static int resultPerfect;
    public static int resultDate;
    public static int resultTime;
    public static int resultType;
    public static double resultLFHF;

    //ローカル保存用
    public const string selfCheckDir = "SelfCheck";
    public static DateTime selfCheckStartTime;
    public static DateTime selfCheckEndTime;
    public static List<SelfCheckSaveData> selfCheckSaveDataList = new List<SelfCheckSaveData>();

    public const string trainingDir = "Training";
    public static DateTime trainingStartTime;
    public static DateTime trainingEndTime;
    public static List<TrainingSaveData> trainingSaveData = new List<TrainingSaveData>();


    //ロード用
    public static bool isLoaded=false;


    private void Start()
    {
        UserRole = m_UserRole;
        if(NowScene != m_startScene)
        {
            NowScene = m_startScene;
            SceneFunc.ChangeScene(m_startScene);
        }

        //履歴ロード
        if(isLoaded == false)
        {
            //セルフチェック
            string[] _data = SaveData.LoadDirectory(selfCheckDir +"/");
            if (_data != null)
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    SelfCheckSaveData _obj = LitJson.JsonMapper.ToObject<SelfCheckSaveData>(_data[i]);
                    selfCheckSaveDataList.Add(_obj);
                }
            }
            //トレーニング
            string[] _data2 = SaveData.LoadDirectory(trainingDir + "/");
            if(_data2 != null)
            {
                for (int i = 0; i < _data2.Length; i++)
                {
                    TrainingSaveData _obj = LitJson.JsonMapper.ToObject<TrainingSaveData>(_data2[i]);
                    trainingSaveData.Add(_obj);
                }
            }
        }
    }
}

public class SelfCheckSaveData
{
    public string[] name { get; set; }
    public int[] value { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}

public class TrainingSaveData
{
    public int type { get; set; }
    public int score { get; set; }
    public int star { get; set; }
    public int perfect { get; set; }
    public double lfhf { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}
