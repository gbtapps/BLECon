using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BrainDataFeedbacker
{
    //デバッグモードフラグここです！
    //bool DEBUG_MODE = false;
   bool DEBUG_MODE = true;


    public enum Type {
        NEURO,
        BIO,
    }
    public Type type;

    const int BUF1_MAX = 10;
    public const int BUF2_MAX = 50;
    const int sampleLength = 30;    

    List<double> activenesses;
    DataStore store;
    private bool isstoredActiveness = false;

    public System.DateTime startTime;
    public System.DateTime endTime;

    List<double> buffer1 = new List<double>();
    List<double> buffer2 = new List<double>();
    List<double> buffer_breath = new List<double>(); //数息用バッファ
    public List<double> GetBreathBuffer() { return buffer_breath; }

    GameObject _debugObject;
    Text _debugText1; 
    Text _debugText2;
    Text _debugText3;
        
    public BrainDataFeedbacker(DataStore s)
    {
        activenesses = new List<double>();
        store = s;
        buffer1 = new List<double>();
        buffer2 = new List<double>();
    }

    /** 脳活性度の蓄積を開始します */
    public void start(Type _type)
    {
        type = _type;
        startTime = System.DateTime.Now;
        isstoredActiveness = true;
        buffer1.Clear();
        buffer2.Clear();
        buffer_breath.Clear();

        _debugObject = GameObject.Find("XbBufViewArea");
        if (DEBUG_MODE)
        {
            _debugText1 = _debugObject.FindDescendant<Text>("TextBuffer1");
            _debugText2 = _debugObject.FindDescendant<Text>("TextBuffer2");
            _debugText3 = _debugObject.FindDescendant<Text>("TextActiveness");
        }
        else
        {
            _debugObject.SetActive(false);
        }
    }

    /** 脳活性度の蓄積を終了します */
    public void stop()
    {
        endTime = System.DateTime.Now;
        isstoredActiveness = false;
    }

    public void bufferActiveness()
    {
        Debug.Log("1******************************************");
        //装着チェックが完了していない状態でも値が帰ってきてしまう
        //OnHeadが、一度でもNOTになったら、バッファをクリア
        if (Hot2gApplication.Instance.state2 != Hot2gApplication.eState.OnHead)
        {
            buffer1.Clear();
            buffer2.Clear();
            buffer_breath.Clear();
            return;
        }

        Debug.Log("2******************************************");
        //-Gain調整中はデータを貯めずに、戻す
        if (Hot2gApplication.Instance.mode != Hot2gApplication.eMode.RecieveData)
        {
            return;
        }

        //データの平滑化：データバッファの中央値を代表値にする＝平滑化。代表値をフィルタデータバッファに追加
        int len = store.l3Ac.Count;
        if (len < 12) {
            Debug.Log("6qqq:waiting");
            return;
        }

        //--------------------------------------------------------------------------------
        if (type == Type.NEURO)
        {
            //ニューロフィードバックと数息

            //仕様書p20の1 ①(AC_L3) – (AC_L1)をバッファ１に追加
            double l3AcM = AnalyseHelper.median(sliceArray(store.l3Ac, len - sampleLength - 1, len - 1));
            double l1AcM = AnalyseHelper.median(sliceArray(store.l1Ac, len - sampleLength - 1, len - 1));

            if (buffer1.Count >= BUF1_MAX) { buffer1.RemoveAt(0); } //循環
            buffer1.Add(l3AcM - l1AcM);

        }
        else if(type == Type.BIO)
        {
            //バイオフィードバック
            //Debug.Log("**** HR count" + store.heartRate.Count);

            //len = store.heartRate.Count;
            //if (len < sampleLength + 1) return;

            //double currentHrM = AnalyseHelper.median(sliceArray(store.heartRate, len - sampleLength - 1, len - 1));

            double currentHrM = store.heartRate[store.heartRate.Count-1];
            Debug.Log("6qqq-1: "+currentHrM.ToString());
            //引き算？値が１個しかないので出来ないのでそのまま
            if (buffer1.Count >= BUF1_MAX) { buffer1.RemoveAt(0); }
            buffer1.Add(currentHrM);
            Debug.Log("6qqq-2: " + buffer1[0]);

            /*
            for (int i = 0; i < 6; i++)
            {
                if (buffer1.Count >= BUF1_MAX) { buffer1.Dequeue(); }
                buffer1.Enqueue(store.pulseHRList[store.pulseHRList.Count() - 1].valConvHR_L[i]);
            }
            */
        }

        //--------------------------------------------------------------------------------
        //仕様書p20の2 ②バッファ１の中央値をバッファ２に追加
        //if (buffer1.Count >= BUF1_MAX)
        if (buffer1.Count >= 1)
        {            
            //double _data2 = AnalyseHelper.median(buffer1.ToArray());
            double _data2 = AnalyseHelper.Average80(buffer1.ToArray());
            //double _data2 = buffer1[buffer1.Count-1];

            if (buffer2.Count >= BUF2_MAX) { buffer2.RemoveAt(0); } //循環
            buffer2.Add(_data2);
            buffer_breath.Add(_data2);//LF/HF用
        }

        //デバッグ表示
        if (DEBUG_MODE)
        {
            _debugText1.text = "buffer1\n";
            double[] _buf1 = buffer1.ToArray();

            for (int i = 0; i < _buf1.Length; i++)
            {
                _debugText1.text += "buf1[" + i + "]" + _buf1[i].ToString("F2") + "\n";
            }

            _debugText2.text = "buffer2\n";
            double[] _buf2 = buffer2.ToArray();
            for (int i = 0; i < _buf2.Length; i++)
            {
                _debugText2.text += "buf2[" + i + "]" + _buf2[i].ToString("F2") + "\n";
            }

            //_debugText3.text = "activeness:" + calcActivenessFromBuffered().ToString("F2");
            _debugText3.text = "a:" + calcActivenessFromBuffered().ToString("F2") + " b:";
        }
    }

    public double calcActivenessFromBuffered()
    {
        return calcActivenessFromBuffered(0);
    }

    public double calcActivenessFromBuffered(int startIdx)
    {
        //仕様書p20の3 ③バッファ２の平均値を飛行機の高さに使用
        ///バッファ2をAnalyseHelper.movingAverage(filteredL, filteredL.Length - 1)を使って平均化した値が飛行機の位置Yになります。 
        double activeness = 0;
        if (buffer2.Count > 0)
        {
            //平均値
            activeness = AnalyseHelper.Average(buffer2.ToArray(), startIdx, buffer2.Count - 1);
        }

        if (!isstoredActiveness)
        {
            return activeness;
        }

        //activenesses.Add(activeness);// 呼び出し毎にログ取ってるけどいいのかなぁ…

        return activeness;
    }

    public double calcActivenessFromBufferedUsingLastData(int indexLastTo)
    {


        //仕様書p20の3 ③バッファ２の平均値を飛行機の高さに使用
        ///バッファ2をAnalyseHelper.movingAverage(filteredL, filteredL.Length - 1)を使って平均化した値が飛行機の位置Yになります。 
        double activeness = double.NaN;
        if (buffer2.Count > 0)
        {
            if (buffer2.Count >= indexLastTo)
            {
                activeness = AnalyseHelper.Average(buffer2.ToArray(), buffer2.Count - indexLastTo, buffer2.Count - 1);
            }
            else
            {
                activeness = AnalyseHelper.Average(buffer2.ToArray(), 0, buffer2.Count - 1);
            }
        }

        if (!isstoredActiveness)
        {
            return activeness;
        }

        //activenesses.Add(activeness);// 呼び出し毎にログ取ってるけどいいのかなぁ…

        return activeness;
    }



    /**　脳活性度積算の平均値 */
    public double activenessAverage{get{return activenesses.Average(); }}
	public double[] activenessLog{get{return activenesses.ToArray();}}

    public void resetBrainCals()
    {
//        buffer1.Clear();
  //      buffer2.Clear();
    }

    public static double[] sliceArray(List<double> dList, int start, int end)
    {
        List<double> d = new List<double>();
        for (int i = start; i <= end; i++)
        {
            d.Add(dList[i]);
        }
        return d.ToArray();
    }
}