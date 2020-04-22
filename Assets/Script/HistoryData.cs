//  トレーニング記録
using System;
using System.Collections.Generic;

class TrainingHistoryData
{
    public int id;
    public int date;
    public int brainPoint;
    public int taskPoint;
}

//  brainmeter記録
class BrainmeterHistoryData
{
    public DateTime startDate;
    public DateTime endDate;
    public string category;
    public List<int> rates;
    public int point;
}

//  セルフチェック記録
class SelfCheckHistoryData
{
    public DateTime date;
    //  質問内容が固定ならこれだけで充分？
    public int[] point;
}

//  脳年齢チェック記録
class BrainCheckHistoryData
{
    public DateTime date;
    public int point;
}
