using System;
using System.Collections.Generic;


public class SceneListObject
{
    public string name;
    public string _id;
    public int __v;
    public string id;
}


/** 性別 */
public class GenderObject
{
    public string id;
    public string name;
}


/**
 * ユーザ
 */
public class UserObject
{
    /** 固有ID */
    public string uid;
    /** プラン */
    public string plan;
    /** ニックネーム */
    public string name;
    /** 誕生日 */
    public string dateOfBirth;
    /** 住所 */
    public string address;
    /** 会員有効期限 */
    public string expire;
    /** コース */
    public string course;
    /** 性別 */
    public GenderObject gender;

    public int sine;
    public int age;
}

public class AuthenticationObject
{
    public string jwt;
    public UserObject user;    
}

/** ユーザ設定；トレ難易度など  */
public class TrainingSettingObject
{
    public string uid;
    public int level;
}

public class SettingsObject
{
    public List<string> currentTrainingUids;
    public List<string> suspendedTrainingUids;
    public List<TrainingSettingObject> trainingSettings;
    public string lastCheckUid;
}

public class UserPreferencesObject
{
    public SettingsObject settings { get; set; }
    public string pulse { get; set; }
    public string uid { get; set; }
    public string _id { get; set; }
//    public DateTime createdAt { get; set; }
//    public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string id { get; set; }
}


/** トレーニング実施結果 */
public class TrainingResult
{
    public string uid { get; set; }
    public int age { get; set; }
    public string address { get; set; }
    public DateTime trained { get; set; }
    public object start { get; set; }
    public int rawScore { get; set; }
    public int score { get; set; }
    public double activeness { get; set; }
    public object activenessRank { get; set; }
    public List<double> activenesses { get; set; }
    public int level { get; set; }
    public string advertisingId { get; set; }
    public List<int> scoreLog { get; set; }
    public object scoreRank { get; set; }
    public string description { get; set; }
    public string _id { get; set; }
    public SceneListObject scene { get; set; }
    public GenderObject gender { get; set; }
    public Training training { get; set; }
//    public DateTime createdAt { get; set; }
//    public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public object log { get; set; }
    public string id { get; set; }
}

public class TrainingresultsPostObject
{
    public string id = null;
    public string uid { get; set; }
    public string gender { get; set; }
    public int age { get; set; }
    public string address { get; set; }
    public double activeness { get; set; }
    public double[] activenesses { get; set; }
    public int level { get; set; }
    public string trained { get; set; }
    public int rawScore { get; set; }
    public int score { get; set; }
    public int[] scoreLog { get; set; }
    public string training { get; set; }
    public string description { get; set; }
    public string scene { get; set; }
}


//  TrainingResultObjectから必要なデータを抜き出したもの
public class TrainingResultData
{
    public string name;
    //  日がキー
    public Dictionary<int, int> score;
    public Dictionary<int, double> activeness;
}

//  brainmeter用
public class BrainResultData
{
    public DateTime trained;
    public string scene;
    public int score;
    public List<int> rate;
}


public class CoursObject
{
    public string name { get; set; }
//    public string _id { get; set; }
//    public DateTime createdAt { get; set; }
//    public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string id { get; set; }
    public object categories { get; set; }
    public object checks { get; set; }
}

//  チェックの項目データ
public class ChecksObject
{
    public string name { get; set; }
    public string uid { get; set; }
    //  CheckResultに入ってる奴はコースの部分がID(List<string>)になってるので使う場合は注意
//    public List<CoursObject> courses { get; set; }
//    public string _id { get; set; }
//    public DateTime createdAt { get; set; }
//    public DateTime updatedAt { get; set; }
//    public int __v { get; set; }
    public string id { get; set; }
}


//  チェック記録用のシーケンスID取得リクエスト
public class CheckSequencesRequestObject
{
    public string id;
    public string uid;
}

//  チェック記録用のシーケンスID取得結果、時間が欲しいので別にする
public class CheckSequencesObject
{
    public string id;
    public string uid;
    public DateTime createdAt;
}


/**
 * チェック結果
 */
public class CheckResultObject
{
    public string id;
    /** 固有ID */
    public string uid;
    /** 年齢 */
    public int age;
    /** 性別 */
    public GenderObject gender;
    /** 住所 */
    public string address;
    /** チェック */
    public ChecksObject check;
    /** 実施日時 */ //  予約語…
    public string @checked;
    /** スコア */
    public int score;
    /** シーケンス */
    public CheckSequencesObject checksequence;
}

/**
 * チェック結果、送る時用
 */
public class CheckResultRequestObject
{
    public string id;
    /** 固有ID */
    public string uid;
    /** 年齢 */
    public int age;
    /** 性別 */
    public string gender;
    /** 住所 */
    public string address;
    /** チェック */
    public string check;
    /** 実施日時 */ //  予約語…
    public string @checked;
    /** スコア */
    public int score;
    /** シーケンス */
    public string checksequence;
}


public class PointHistoryPostObject
{
    public int point;
//    public string expired;
    public string acquired;
//    public string reason;
//   public string user;
    public string training;
}

public class PointHistoryObject
{
    public string id;
    public int point;
    public DateTime acquired;
//    public Training training;
}

//-----------------------------------------------------------------------------
//ここから下追加
//  送る場合
public class TrainingResultPostObject
{
    public string xb01id { get; set; }
    public string training_id { get; set; }
    public DateTime trained { get; set; }   //  いらない気がする・・・created_atで充分？
    public int time { get; set; }
    public int score { get; set; }
}

public class ResponseObject
{
}

public class SelfCheckResultPostObject
{
    public string xb01id { get; set; }
    public List<string> checkname { get; set; } = new List<string>();
    public List<int> checkscore { get; set; } = new List<int>();
}
