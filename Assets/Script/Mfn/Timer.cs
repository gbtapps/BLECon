using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    //表示種類
    public bool count_down;
    public bool count_up;
    public bool is_on { get; set; }

    //各桁
    public Text textHour;
    public Text textMinute;
    public Text textSecond;
    public Text textMilli;

    //各桁ごと
    int numHour = 0;
    int numMinute = 0;
    int numSecond = 0;
    int numMilli = 0;
    int nowMilliSecond;

    public float time { get { return nowMilliSecond; } }


    public void SetTimeSecond(float _time)
    {
        int _milliSocond = ((int)_time*1000) + (int)((_time%1)*1000);
        SetTimeMilli(_milliSocond);
    }

    public void SetTimeMilli(int _milliSecond)
    {
        nowMilliSecond = _milliSecond;
        _milliSecond /= 1000;
        numMilli = nowMilliSecond % 1000;
        if (textHour)
        {
            //時間表示あり
            //時
            numHour = _milliSecond / 3600;
            _milliSecond -= numHour * 3600;
            //分
            numMinute = _milliSecond / 60;
            //秒
            numSecond = _milliSecond % 60;
        }
        else
        {
            //時間表示なし
            //分
            numMinute = _milliSecond / 60;
            if (numMinute > 999) { numMinute = 999; }
            //秒
            numSecond = _milliSecond % 60;
        }



        //時
        if (textHour)
        {
            string hour = "";
            if (numHour < 10) { hour += "0"; }
            hour += numHour;
            textHour.text = hour;
        }
        //分
        if (textMinute)
        {
            string minute = "";
            if (numMinute < 10) { minute += "0"; }
            minute += numMinute;
            textMinute.text = minute;
        }
        //秒
        if (textSecond)
        {
            string secondS = "";
            if (numSecond < 10) { secondS += "0"; }
            secondS += numSecond;
            textSecond.text = secondS;
        }
        //ミリ秒
        if(textMilli)
        {
            string milli = "";
            int millinum_d2 = numMilli / 10;
            if (millinum_d2 < 10) { milli += "0"; }
            milli += millinum_d2;
            textMilli.text = milli;
        }
    }

    void Update()
    {
        if (!is_on){ return; }

        if (count_down && nowMilliSecond>0)
        {
            nowMilliSecond -= (int)(Time.deltaTime * 1000);
            if (nowMilliSecond<0) { nowMilliSecond = 0; }
        }
        else if (count_up)
        {
            nowMilliSecond += (int)(Time.deltaTime * 1000);
        }

        SetTimeMilli(nowMilliSecond);

    }
}
