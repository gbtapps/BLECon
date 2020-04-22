using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainController
{

    /** L1 AC データバッファ */
    List<int> l1Buff;
    /** L3 AC データバッファ */
    List<int> l3Buff;
    /** R1 AC データバッファ */
    List<int> r1Buff;
    /** R3 AC データバッファ */
    List<int> r3Buff;
    /** L1 飽和カウンタ */
    int l1Saturation = 0;
    /** L3 飽和カウンタ */
    int l3Saturation = 0;
    /** R1 飽和カウンタ */
    int r1Saturation = 0;
    /** R3 飽和カウンタ */
    int r3Saturation = 0;
    /** L1GCチャネル */
    GainControllerChannel l1Gcc;
    /** L3GCチャネル */        
    GainControllerChannel l3Gcc;
    /** R1GCチャネル */        
    GainControllerChannel r1Gcc;
    /** R3GCチャネル */        
    GainControllerChannel r3Gcc;

    /** すべてのチャネルがフィックス済みか */
    public bool isFixed = false;     

	/** ゲイン調整失敗フラグ **/
	public bool isAdjustmentFailure = false;

	/** ゲイン調整用　計測値バッファサイズ*/
	int bufferMaxSize = 20;
	/** ゲイン計算　計測値平均化サイズ*/
	int bufferCalcSize = 10;


	/** Retry counter */
	public int retryCounter = 0;

    public GainController()
    {
        l1Buff = new List<int>();
        l3Buff = new List<int>();
        r1Buff = new List<int>();
        r3Buff = new List<int>();

        l1Gcc = new GainControllerChannel( l1Saturation, GainControllerChannel.Channel.L1, 4.0);
        l3Gcc = new GainControllerChannel( l3Saturation, GainControllerChannel.Channel.L3, 6.0);
        r1Gcc = new GainControllerChannel( l1Saturation, GainControllerChannel.Channel.R1, 4.0);
        r3Gcc = new GainControllerChannel( l3Saturation, GainControllerChannel.Channel.R3, 6.0);
    }

    public double getL3Gain()
    {
        return l3Gcc.currentGain;
    }

    public double getL1Gain()
    {
        return l1Gcc.currentGain;
    }

	public bool getIsMGCFailure()
	{
		return (this.l1Gcc.candidateState == GainControllerChannel.GainCandidateState.Failure || this.l3Gcc.candidateState == GainControllerChannel.GainCandidateState.Failure);
	}

	/**
	 * バッファにAC値を追加
	 * @param data AC値データ
	 */
	public bool addData(Hot2gMeasData1chAc data)
    {
        if( data == null )
        {
            Debug.Log("Gain Controller addData1ch data is NULL");
            return false;
        }

        l1Buff.Add(data.valAcValues[0]);
        l3Buff.Add(data.valAcValues[1]);

        Debug.Log("MGC buffer len:" + l1Buff.Count);

		//this.r1Buff.add(data.valAcValues[0])
		//this.r3Buff.add(data.valAcValues[1])

		this.l1Saturation += data.valSaturations[0]; //- TODO OK??
		this.l3Saturation += data.valSaturations[1];
		//this.r1Saturation += data.valSaturations[0]
		//this.r3Saturation += data.valSaturations[1]

		this.l1Gcc.candidate(l1Buff);
		this.l3Gcc.candidate(l3Buff);

		/*
		if (this.l1Buff.Count < 10 && (l1Gcc.candidateState == GainControllerChannel.GainCandidateState.ChangingGain ||
				l3Gcc.candidateState == GainControllerChannel.GainCandidateState.ChangingGain))
		{
			return false;

		}*/

		if (l1Gcc.candidateState == GainControllerChannel.GainCandidateState.ChangingGain ||
			l3Gcc.candidateState == GainControllerChannel.GainCandidateState.ChangingGain)
		{
			return false;
		}


		/* 20件以上蓄積 */
		//if(this.l1Buff.size < 3)
		if (this.l1Buff.Count < bufferMaxSize)
		{ //TK 2018-11-17
            Debug.Log("MGC adding to buffer");
			return true;
		}

		isFixed = (l1Gcc.isFixed ) && (l3Gcc.isFixed );

		return true;
    }

    /**
     * バッファにAC値を追加
     * @param data AC値データ
     */
    public void addData(Hot2gMeasData2chAc data)
    {
        this.l1Buff.Add(data.valAcValues[0]);
        this.l3Buff.Add(data.valAcValues[1]);
        this.r1Buff.Add(data.valAcValues[2]);
        this.r3Buff.Add(data.valAcValues[3]);
        this.l1Saturation += data.valSaturations[0];
        this.l3Saturation += data.valSaturations[1];
        this.r1Saturation += data.valSaturations[2];
        this.r3Saturation += data.valSaturations[3];
        /* 20件以上蓄積 */
        //if(this.l1Buff.size < 3)
        if (this.l1Buff.Count < 20) //TK 2018-11-17
            return;

        this.isFixed =
            (l1Gcc.isFixed )
         && (l3Gcc.isFixed )
         && (r1Gcc.isFixed )
         && (r3Gcc.isFixed );
    }

	public void clearBuffer()
	{
		this.l1Buff.Clear();
		this.l3Buff.Clear();
		this.r1Buff.Clear();
		this.r3Buff.Clear();

		this.l1Saturation = 0; this.l3Saturation = 0;
		this.r1Saturation = 0; this.r3Saturation = 0;

        this.l1Gcc.isFixed = false;
        this.l3Gcc.isFixed = false;
        this.r1Gcc.isFixed = false;
        this.r3Gcc.isFixed = false;
    }

    public void reset()
    {
        this.isFixed = false;
        this.l1Buff.Clear(); this.l3Buff.Clear();
        this.r1Buff.Clear(); this.r3Buff.Clear();
        this.l1Saturation = 0; this.l3Saturation = 0;
        this.r1Saturation = 0; this.r3Saturation = 0;
		//this.l1Gcc.currentGain = 4.0; this.l3Gcc.currentGain = 6.0
		//this.r1Gcc.currentGain = 4.0; this.r3Gcc.currentGain = 6.0
		this.l1Gcc.currentGain = 8.0; this.l3Gcc.currentGain = 12.0;
		this.r1Gcc.currentGain = 8.0; this.r3Gcc.currentGain = 12.0;
	}

	public void resetAll()
	{
		this.reset();
		this.retryCounter = 0;
	}

	/** マスタリング済みゲイン調整値 */
	public Hot2gGainData.ParamGain mastering
    {
        get
        {
			this.isFixed = false;

			Hot2gGainData.ParamGain Data = new Hot2gGainData.ParamGain();
            Data.ch0_L_AmpGain = l1Gcc.mastering(l1Buff, bufferCalcSize);
            Data.ch1_L_AmpGain = l3Gcc.mastering(l3Buff, bufferCalcSize);
            //Data.ch0_R_AmpGain = r1Gcc.mastering(r1Buff, bufferCalcSize);
            //Data.ch1_R_AmpGain = r3Gcc.mastering(r3Buff, bufferCalcSize);

            return Data;
        }
    }

	public Hot2gGainData.ParamGain setCurrentGainAsIS()
	{
		Hot2gGainData.ParamGain r = new Hot2gGainData.ParamGain();
		r.ch0_L_AmpGain = (byte)l1Gcc.currentGain;//.toByte()
		r.ch1_L_AmpGain = (byte)l3Gcc.currentGain;//.toByte()

		r.ch0_R_AmpGain = (byte)r1Gcc.currentGain;//.toByte()
		r.ch1_R_AmpGain = (byte)r3Gcc.currentGain;//.toByte()

		return r;
    }

	public string toString()
	{
		return "Gain L1: " + this.l1Gcc.currentGain.ToString(".0") + ", L3: " + this.l3Gcc.currentGain.ToString(".0") +
			" L1 values = " + TakeLast(this.l1Buff, 3) +
			" L3 values = " + TakeLast(this.l3Buff, 3);
	}

	string TakeLast( List<int> list, int num )
	{
		string ret = "[";

		if( list.Count < num )
		{
			for( int i=0;i<list.Count; i++ )
			{
				if( i!= 0)
				{
					ret += ",";
				}
				ret += list[i];
			}
		}
		else
		{
			int c = 0;
			for (int i = list.Count-num; i < list.Count; i++)
			{
				if (c != 0)
				{
					ret += ",";
				}
				ret += list[i];
				c++;
			}
		}

		ret += "]";

		return ret;
	}
}

/**
 * チャネルごとのゲイン調整値算出の機能を提供するクラス
 */
class GainControllerChannel
{
    public enum  Channel
    {
        L1, L3, R1, R3
    }

	public enum GainCandidateState
	{
		Default, Failure, ChangingGain, Finished
	};

	const int sampling_size = 3;
    const int min_ac = 100;
    const int step_gain = 4;
    const int max_saturated_count = 2 * sampling_size;

    /** チャネル */
    Channel ch;


    /** 飽和データカウントの合計 */
    int saturation;

    /** 調整が完了したかどうか */
    public bool isFixed = false;

    /** 現在のゲイン */
    public double currentGain = 0.0;

	/** 初回調整中の状態 */
	public GainCandidateState candidateState = GainCandidateState.Default;


	public GainControllerChannel( int Saturation, Channel CH, double defaultGainValue)
    {
        ch = CH;
        saturation = Saturation;
		currentGain = defaultGainValue;

	}

    /**
     * 候補値を算出します
     * @return 処理済み値
     */
    public void candidate( List<int>buffer )
    {
		if (this.isFixed)
			candidateState = GainCandidateState.Finished; //-TODO OK?


		int acAverage = 0;
        for (int i = 0; i < buffer.Count; i++)
        {
            acAverage += buffer[i];
        }
        acAverage /= buffer.Count;


		/* 候補値を調整 */
		if (acAverage <= min_ac)
        {
            buffer.Clear();
			/* 平均電圧が規定値以下の場合 */
			this.currentGain += step_gain; //ゲインを上げて再計測

			/* 調整値の妥当性 */
			if (this.currentGain >= maxGain) //もうゲイン上げられないときはFalse
				candidateState = GainCandidateState.Failure; //ゲイン調整失敗            
			else
				candidateState = GainCandidateState.ChangingGain; //再調整ループ
		}
        else if (this.saturation > max_saturated_count)
        {
            buffer.Clear();
            /* 飽和カウンタが規定値を超えていた場合 */
            this.currentGain -= step_gain; //ゲインを下げて再調整
										   /* 調整値の妥当性 */
			if (this.currentGain < 1) //もうゲイン下げられないときはFalse
				candidateState = GainCandidateState.Failure; //ゲイン調整失敗

			else
				candidateState = GainCandidateState.ChangingGain; //再調整ループ
		}
		else
		{
			this.isFixed = true; //調整済み
			candidateState = GainCandidateState.Finished;
		}
    }

    /**
     * マスタリング
     * @param gain
     * @return 処理済み値
     */
    public byte mastering(List<int> buffer, int bufferCalcSize)
    {
        if (!isFixed)
            //return 0
            return 1;

        int acAverage = 0;

        //- Use median instead of averaging
        Debug.Log("MGC buffer sz=" + buffer.Count);
        acAverage = median(new List<int>(buffer.GetRange(buffer.Count - 1 - bufferCalcSize, bufferCalcSize)));
        /*
        int c = 0;
        for (int i = buffer.Count- bufferCalcSize; i < buffer.Count; i++)//- 頭のデータはゲイン調整が終わっていない可能性もあり。
		{
			if (0 <= i)
			{
				acAverage += buffer[i];
				c++;
			}
        }
        acAverage /= buffer.Count;
        */

        var g = this.currentGain * (2048 / acAverage);
        //var g = this.currentGain * 1024.0 / (double)acAverage; //TK 2018-11-17 for XB-01
		if (g > maxGain)
		{
			g = maxGain - 1;
		}

		if (g < 1.0)
		{
			currentGain = 1.0;
		}
		else
		{
			currentGain = (double)((byte)g);
		}

		return (byte)currentGain;

	}

    static int median(List<int> l)
    {        
        l.Sort();
        return (l[l.Count / 2] + l[(l.Count - 1) / 2]) / 2;
    }

    /** ゲイン最大値 */
    int maxGain
    {
        get
        {
            switch (this.ch)
            {
                //Channel.L1, Channel.R1 -> 16 * 4
                //Channel.L3, Channel.R3 -> 16
                case Channel.L1:
                case Channel.R1:
                    return 16;

                case Channel.L3:
                case Channel.R3:
                    return 16 * 4;
            }

            return 0;
        }

    }
}