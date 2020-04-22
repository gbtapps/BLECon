using System.Collections.Generic;
using UnityEngine;


//  固定データを置いておくためのクラス
public class ConstData : MonoBehaviour
{
    //  パスワードの暗号化キー
    static public readonly string CryptKey = "abcPaskey!0#";

    static public readonly string[] ButtonList = {
        "training",
        "brain",
//        "check",
        "record",
        "learn",
     };

    static public readonly EnumScene[] ButtonScene = {
//        EnumScene.T_1_TrainingTop,
//        EnumScene.B_1_BrainMeterTop,
////        EnumScene.C_1_Check,
//        EnumScene.R_1_RecordTop,
//        EnumScene.L_1_LearnTop,
    };

    public enum EnumScene
    {
        T_TitleSelect,
        C_SelfCheck,
        R_DailyTrainingResult,
        R_ResultLog,

        Tr_TraningSetting,

        Tr_TrainingNeuro,
        Tr_TrainingHeartRate,
        Tr_TrainingBreath,

        Tr_TraningResult,

        Max,
    };

    public enum EnumHeaderButton
    {
        Menu,
        Close,
        Back
    }

    public enum EnumHeaderAlign
    {
        Left,
        Center,
    }

    public struct HeaderData
    {
        public HeaderData(string n, EnumHeaderButton b, EnumHeaderAlign a, bool l)
        {
            title = n;
            button = b;
            align = a;
            line = l;
        }

        public string title;
        public EnumHeaderButton button;
        public EnumHeaderAlign align;
        public bool line;
    }

    static public readonly Dictionary<EnumScene, HeaderData> HeaderType = new Dictionary<EnumScene, HeaderData>()
    {
        { EnumScene.T_TitleSelect,new HeaderData(
            "セルフコントロール\nトレーニング\nSelf cotrol training",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.C_SelfCheck,new HeaderData(
            "セルフチェック",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.R_DailyTrainingResult,new HeaderData(
            "日々のトレーニングの結果",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.R_ResultLog,new HeaderData(
            "履歴",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.Tr_TraningSetting,new HeaderData(
            "トレーニング時間",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.Tr_TrainingNeuro,new HeaderData(
            "ブレインコントロール",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.Tr_TrainingHeartRate,new HeaderData(
            "心拍コントロール",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.Tr_TrainingBreath,new HeaderData(
            "数息観瞑想",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
        { EnumScene.Tr_TraningResult,new HeaderData(
            "リザルト",
            EnumHeaderButton.Close,
            EnumHeaderAlign.Left,
            false
            )
        },
    };

    /** 共通ドメイン */
    public const string ec_domain = "https://www.active-brain-club.com";
    /** PassWord忘れ */
    public const string password_url = "/application/user/pass/";
    /** 利用規約 */
    public const string agreement_url = "/application/kiyaku/";
    /** データの利用についての規約 */
    public const string agreement_data_url = "/application/riyou/";
    /** よくある質問 */
    public const string faq_url = "/application/faq/";
    /** 学ぶ(川島チャンネル) */
    public const string learn_url = "/application/kawashima/standard/";
    /** 脳トレ説明 */
    public const string explain_url = "/application/training/";

    //  ユーザー種別
    public enum EnumUserRole
    {
        None,
        Basic,      //  brainmeterのみ
        Standard,   //  XB-01があれば全部できる
        Develop,    //  開発用、XB-01なくても全部できる
    };

    public static readonly Dictionary<string, ConstData.EnumUserRole> UserRoleTable = new Dictionary<string, ConstData.EnumUserRole>()
    {
        {"basic",EnumUserRole.Basic},
        {"standard",EnumUserRole.Standard},
        {"tester",EnumUserRole.Develop},
    };

    //  トレーニングIDとシーンの対応
    static public readonly Dictionary<int, EnumScene> TrainingScene = new Dictionary<int, EnumScene>()
    {
    };

    //  トレーニングの種類
    public static readonly string[] TrainingName =
    {
        "",
        "スピード計算",                   //  NFB001
        "シンボルタッチ",                 //  NFB002
        "頭の回転「ナンバータッチ」",     //  NFB003
        "順番記憶",                       //  NFB004
        "数字記憶",                       //  NFB005
        "ドレミ記憶",                     //  NFB006
        "記憶力「計算記憶」",             //  NFB007
        "記号記憶",                       //  NFB008
        "超スピード計算",                 //  NFB009
        "タイミング",                     //  NFB010
        "タイミングスマイル",             //  NFB011
        "赤押して",                       //  NFB012
        "タイミングスマイル",             //  NFB013
        "数字ひらがなタッチ",             //  NFB014
        "トンネルゴー",                   //  NFB015
        "スカッシュ",                     //  NFB016
        "目隠し射的",                     //  NFB017
        "スカッシュ★ゲーム",             //  NFB018
        "",
        "",
        "スピード俳句",                   //  NFB020
        "スピード福笑い",                 //  NFB021
        "アニマル順番記憶",               //  NFB023
        "アニマル記憶",                   //  NFB024
        "アニマル名前記憶",               //  NFB025
        "",
        "",
        "",
        "",
        "注意力「アニマルナンバー」",     //  NFB030
    };
}
