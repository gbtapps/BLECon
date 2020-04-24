using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UIExtention;
using Random = UnityEngine.Random;

public class BMBrainMgr
{
	/** 表示させる脳活性度 */
	static double displayedActivenesses = 0.0;

	/** 表示させる脳活性度を更新させるかどうか */
	static bool isupdateActivenesses = false;
	static List<bool> isupdateActivenessesBuffer;

	/*
	TrainingResult registerTrainingResult()
	{
        return TrainingResult(
				id = null,
				trained = Date(),
				score = this.calcActiveness(),
				trainingTime = Date(),
				activenesses = NpodApplication.app.nfb.activenesses,
				start = NpodApplication.app.meta.measured,
				end = NpodApplication.app.stopMeasured
		)

	}*/

	//List

	static public void Start(BrainDataFeedbacker.Type _type =BrainDataFeedbacker.Type.NEURO)
	{
		if ( Hot2gApplication.eMode.Connecting <= Hot2gApplication.Instance.mode )
		{
			Hot2gApplication.Instance.StartRecieve(_type);
		}
		isupdateActivenessesBuffer = new List<bool>();
	}

	static public void End()
	{
		Hot2gApplication.Instance.StopRecieve();

	}




	public class  TrainingResult 
	{
		public enum eColor
		{
			Bule,
			Green,
			Yellow,
			Red,

			Max,
		};

		public string id;                   // id = null いる？
		public DateTime trained;            // 終了日時　いる？
		public DateTime trainingTime;       // 終了日時　いる？
		public int score;                   // 評価点（活性度平均）
		public double[] activenesses;		// 活性度履歴
		public DateTime start;              // 脳波計測開始日時
		public DateTime end;                // 脳波計測終了日時

		public int[] Rates;
		public int[] TotalSeconds;
		public int[] Width;
	}

	public static TrainingResult GetResultData()
	{
		TrainingResult Data = new TrainingResult();
		Data.id = null;
		Data.trained = DateTime.Now;
		Data.trainingTime = DateTime.Now;
		Data.score = calcActiveness();
		Data.activenesses = Hot2gApplication.Instance.GetActivenessesLog();
		Data.start = Hot2gApplication.Instance.GetStartTime();
		Data.end = Hot2gApplication.Instance.GetEndTime();

		float TotalSeconds = (float)( (Data.end - Data.start).TotalSeconds );
		Data.Rates = new int[] { 25, 25, 25, 25 };
		Data.Width = new int[] { 245, 245, 245, 245 };
		Data.TotalSeconds = new int[] { 0, 0, 0, 0 };

		if(Data.activenesses.Length == 0)
		{
			return Data;
		}

		//int Count = 0;
		int[] Counts = new int[] { 0, 0, 0, 0 };
		for( int i=0; i<Data.activenesses.Length; i++ )
		{
			double it = Data.activenesses[i];
			if (it <= 0.2)
			{
				Counts[0]++;

			}
			else if (0.2 < it && it <= 0.5)
			{
				Counts[1]++;
			}
			else if (0.5 < it && it <= 0.8)
			{
				Counts[2]++;

			}
			else
			{
				Counts[3]++;
			}
		}

		for (int i = 0; i < 4; i++)
		{
			float Rate = (float)Counts[i] / (float)Data.activenesses.Length;

			Data.TotalSeconds[i] = Mathf.RoundToInt(TotalSeconds * Rate);
			Data.Rates[i] = Mathf.RoundToInt(Rate*100);
			Data.Width[i] = Mathf.RoundToInt(980.0f * Rate); 
		}

		Data.score = Data.Rates[2] + Data.Rates[3];

		// 幅の計算調整
		int w = Data.Width[0] + Data.Width[1] + Data.Width[2] + Data.Width[3];
		int s = 980 - w;
		Data.Width[0] += s; // 誤差だから青でごまかす


		return Data;
	}





    public static int GetBreathScore()
    {
        return calcActiveness();
    }

    static int calcActiveness()
	{
		//return 
		return Mathf.RoundToInt((float)Hot2gApplication.Instance.ActivenessesAverage() * 100f);
    }

	public static double getCurrentActiveness()
	{
        GyroScope.Stability mStablilty;
        if (Hot2gApplication.Instance.datastore.stability.Count > 0)
            mStablilty = Hot2gApplication.Instance.datastore.stability[Hot2gApplication.Instance.datastore.stability.Count - 1];
        else
            mStablilty = GyroScope.Stability.Stabled;
        
        Debug.Log("getCurrentActiveness: State: " + Hot2gApplication.Instance.mode.ToString() +
            "/ after MGC: " + Hot2gApplication.Instance.afterMGC_counter.ToString() +
            " / Motion: " + mStablilty.ToString() + 
            " / OnHead: " + Hot2gApplication.Instance.state2.ToString());

		if (Hot2gApplication.Instance.datastore == null)
		{
			Debug.Log("Data Store is Null");
			return 0.0;
		}

        if (Hot2gApplication.Instance.afterMGC_counter < 0 && 
             mStablilty == GyroScope.Stability.Stabled)
        {            
            //updateActivenesses();
        }

        if (mStablilty != GyroScope.Stability.Stabled)
        {
            return -10.0 / 12.0;
        }
        else if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting)
		{
			return 0.0; //- 0.0 is for black color

		}
		else if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.GainSetting)
		{
			return -100.0/12.0;
		}


		displayedActivenesses = Hot2gApplication.Instance.calcActivenessFromBuffered();
		return displayedActivenesses;
	}

	/**
     * isupdateActivenessesを管理するメソッド
     * ・心拍数が40～180の範囲を超えている状態が5秒続いたら、画面の色更新を止めます
     * ・心拍数が40～180の範囲に入っている状態が3秒続いたら、画面の色更新をします
     *
     *
     * Revised conditions
     *  sample num: 20
     *
     *  1. HbT 3cm: abs(median - mean)<3
     *  2. HbT 3cm: non zero
     *  3. Hb 1cm: median(v-v(1))<0.3
     *  4. Gyro-Z: median(v-v(1))<7
     *
     */
	static void updateActivenesses()
	{
		if(Hot2gApplication.Instance.datastore.stability.Count<=0)
		{
			return;
		}

		List<double> brain1 = Hot2gApplication.Instance.datastore.l1Ac;
		List<double> brain3 = Hot2gApplication.Instance.datastore.l3Ac;
		List<double> gyroZ = Hot2gApplication.Instance.datastore.gyroZ;
		GyroScope.Stability stability = Hot2gApplication.Instance.datastore.stability[Hot2gApplication.Instance.datastore.stability.Count - 1];

		//Debug.Log("HRTEST State:" + sF(NpodApplication.app.state.toString()) + " / State2:" + sF(NpodApplication.app.state2.toString()))

		int sz = 20;

        if (brain1.Count > sz)
        {
            bool lastStatus = isupdateActivenesses;

            bool cndStability = (stability == GyroScope.Stability.Stabled); //true;//stability.isStable;

            //  1. HbT 3cm: abs(median - mean)<3
            // val cnd1 = (median(brain3.takeLast(sz)) - brain3.takeLast(sz).average()).absoluteValue < 3.0
            double B3_median = median(takeLast(brain3, sz));
            double B3_ave = AnalyseHelper.Average(brain3, brain3.Count - sz, brain3.Count);
            bool cnd1 = (Math.Abs(B3_median - B3_ave) < 3);

            //  2. HbT 3cm: non zero
            //val cnd2 = (median(brain3.takeLast(sz)) - brain3.takeLast(sz).average()).absoluteValue != 0.0
            bool cnd2 = (Math.Abs(B3_median - B3_ave) != 0.0);

            //  3. Hb 1cm: median(v-v(1))<0.3
            bool cnd3 = median(subtract(takeLast(brain1, sz), brain1[brain1.Count - sz])) < 0.3;
            //  4. Gyro-Z: median(v-v(1))<7
            bool cnd4 = median(subtract(takeLast(gyroZ, sz), gyroZ[gyroZ.Count - sz])) < 7;

            //  5. 置いたままの状態を加速度から検出
            bool cnd5 = sd(takeLast(gyroZ, sz)) > 0.02;


            isupdateActivenessesBuffer.Add(cndStability && cnd1 && cnd2 && cnd3 && cnd4 && cnd5);
            if (isupdateActivenessesBuffer.Count > 50)
            {
                isupdateActivenessesBuffer.RemoveAt(0);
            }


            //isupdateActivenesses = !isupdateActivenessesBuffer.takeLast(2).all { !it }

            if (isupdateActivenessesBuffer.Count == 1)
            {
                isupdateActivenesses = isupdateActivenessesBuffer[0];
            }
            else
            {
                int c = isupdateActivenessesBuffer.Count - 1;
                isupdateActivenesses = (isupdateActivenessesBuffer[c] || isupdateActivenessesBuffer[c - 1]);
            }

            Debug.Log("HRTEST 1:" + cnd1 + " 2:" + cnd2 + " 3:" + cnd3 + " 4:" + cnd4 + " 5:" + cnd5);


            if (!isupdateActivenesses)
            /** 非装着検出！ */
            {
                /** 計測中（装着）　→　非装着 */
                if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.RecieveData)
                {
                    Hot2gApplication.Instance.setStateToResetGain(); //- リセット後はルーチン内でWaitingに設定される
                    Hot2gApplication.Instance.setState2ToNotOnHead();

                }
                else if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting)
                { /** 非装着中 */
                    //Hot2gApplication.Instance.setStateToWating();
                    //Hot2gApplication.Instance.setState2ToNotOnHead();
                }
            }
            else
            {
                if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting)
                /** 装着中検出　*/
                {
                    /** 非装着　→　装着  */
                    Hot2gApplication.Instance.setStateToMGC();
                    Hot2gApplication.Instance.setState2ToOnHead();
                }                
                else
                {
                    /** 装着中 */
                    Hot2gApplication.Instance.setState2ToOnHead();
                }
            }

#if false
			if (!isupdateActivenesses)
			{
				/** 装着　→　非装着 */
				if (lastStatus)
				{
					Hot2gApplication.Instance.setStateToResetGain();
					Hot2gApplication.Instance.setState2ToNotOnHead();
				}
				else
				{ /** 非装着中 */
					Hot2gApplication.Instance.setStateToWating();
					Hot2gApplication.Instance.setState2ToNotOnHead();
				}
			}
			else if (!lastStatus)
			{
				/** 非装着　→　装着 */
				Hot2gApplication.Instance.setStateToMGC();
				Hot2gApplication.Instance.setState2ToOnHead();

			}
			else
			{ /** 装着中 */
				Hot2gApplication.Instance.setState2ToOnHead();
			}
#endif

		}


#if false


		/*
		var heartRate = NpodApplication.app.dataStore.heartRate

		if (heartRate.takeLast(30).all { 40 <= it && it <= 180 }) {
			isupdateActivenesses = true
		}
		if (heartRate.takeLast(50).all { it < 40 || 180 < it }) {
			isupdateActivenesses = false
		}
		*/
		List<double> pulseCoef = Hot2gApplication.Instance.datastore.pulseAmplitudeCoeff;
		List<double> brain1 = Hot2gApplication.Instance.datastore.l1Ac;
		List<double> brain3 = Hot2gApplication.Instance.datastore.l3Ac;


		Debug.Log("HRTEST\n" + pulseCoef.Count + "," + Hot2gApplication.Instance.mode.ToString());


		int sz = 20;

		if (pulseCoef.Count > sz)
		{
			List<double> takeLast_sz = new List<double>();
			List<double> takeLast_sz_brain1 = new List<double>();
			List<double> takeLast_sz_brain3 = new List<double>();


			for (int i = pulseCoef.Count - 20; i < pulseCoef.Count; i++)
			{
				takeLast_sz.Add(pulseCoef[i]);
				takeLast_sz_brain1.Add(brain1[i]);
				takeLast_sz_brain3.Add(brain3[i]);
			}


			bool lastStatus = isupdateActivenesses;



			bool cond3 = false;
			double c3buf = sd(takeLast_sz.ToArray());
			if (2 <= c3buf && c3buf <= 1000)
			{
				cond3 = true;
			}

			Debug.Log("c3buf " + c3buf);

			bool cond4 = (brain1[brain1.Count - 1] != double.NaN && brain3[brain3.Count - 1] != double.NaN);
			bool cond5 = max(takeLast_sz_brain1) < 30.0 && max(takeLast_sz_brain3) < 30.0;
			//bool cond6 = (brain1[brain1.Count - 1] != 0.0 && brain3[brain3.Count - 1] != 0.0);
			bool cond6 = true;


			Debug.Log("HRTEST 3:" + cond3 + " 4:" + cond4 + " 5:" + cond5 + " 6:" + cond6);


			isupdateActivenesses = cond3 && cond4 && cond5 && cond6;


			/** 非装着時はStateをWatingにキープ */
			if(!isupdateActivenesses)
			{
				Debug.Log("NotOnHead");
				Hot2gApplication.Instance.setStateToWating();
				Hot2gApplication.Instance.setState2ToNotOnHead();
			}
			/** 非装着　→　装着 */
			if (isupdateActivenesses && !lastStatus)
			{
				Debug.Log("Not To OnHead");
				Hot2gApplication.Instance.setStateToMGC();
				Hot2gApplication.Instance.setState2ToOnHead();
			}
			/** 装着　→　非装着 */
			if (!isupdateActivenesses && lastStatus)
			{
				Debug.Log("OnHead To Not");
				Hot2gApplication.Instance.setStateToResetGain();
				Hot2gApplication.Instance.setState2ToNotOnHead();

			}
		}
#endif
	}


	static string sF( int v){ return v + " "; }
	static string sF( double v ){ return v.ToString("0.00"); }
    static string sF( byte v){ return v + " "; }
    static string sF( string v){ return v + " "; }



    static double sd( double[] numArray)
	{
		double mean = AnalyseHelper.Average(numArray, 0, numArray.Length);

		double n = 0.0;
		for( int i=0; i<numArray.Length; i++ )
		{
			//next = numArray[i] + System.Math.Pow(next - mean, 2.0);
			n = n + System.Math.Pow(numArray[i] - mean, 2.0);
		}

        return System.Math.Sqrt(n / numArray.Length);
    }

	static double max( List<double> l )
	{
		double d = -9999999999999;
		for( int i=0; i<l.Count;i++)
		{
			if( d < l[i] )
			{
				d = l[i];
			}
		}

		return d;
	}

	//fun median(l: List<Double>) = l.sorted().let { (it[it.size / 2] + it[(it.size - 1) / 2]) / 2 }.toDouble()


	static double median(double[] b)
	{
		List<double> l = new List<double>(b);
		l.Sort();

		return (l[l.Count / 2] + l[(l.Count - 1) / 2]) / 2.0;
	}

	static double[] subtract(double[] l, double v)
	{
        double[] l1 = new double[l.Length];
		for ( int i = 0; i < l.Length; i++) 
		{
			l1[i] = (l[i] - l[0]);
        }
		return l1;
    }
	/*
	public static double[] takeLast(double[] Data, int Num)
	{
		if (Data.Length < Num)
		{
			return Data;
		}

		int c = 0;
		double[] r = new double[Num];
		for (int i = Data.Length - Num; i < Data.Length; i++)
		{
			r[c] = Data[i];
			c++;
		}

		return r;
	}*/

	public static double[] takeLast(List<double> Data, int Num)
	{
		if (Data.Count < Num)
		{
			return Data.ToArray();
		}

		int c = 0;
		double[] r = new double[Num];
		for (int i = Data.Count - Num; i < Data.Count; i++)
		{
			r[c] = Data[i];
			c++;
		}

		return r;
	}

    static readonly string[] color_list =
    {
      "#2196F3",
      "#03A9F4",
      "#00BCD4",
      "#009688",
      "#4CAF50",
      "#8BC34A",
      "#CDDC39",
      "#FFEB3B",
      "#FFC107",
      "#FF9800",
      "#FF5722",
      "#F44336"
    };

    static public string GetBrainColor()
    {
        #if UNITY_EDITOR
        var act = Mathf.FloorToInt(Random.value * 12);
        #else
        var act = Mathf.FloorToInt((float)(getCurrentActiveness() * 12.0));
        #endif

        return color_list[act];
    }

    /*
    public static GradationMaterial BrainLine(Gradation gradation, List<int> Rates)
    {
        GradationMaterial gradationMaterial = ScriptableObject.CreateInstance<GradationMaterial>();

        gradation.material = gradationMaterial;
        List<GradationMaterial.Key> keys = new List<GradationMaterial.Key>();
        GradationMaterial.Key key = new GradationMaterial.Key();

        //  最初の青は固定,後でループになおしたい
        key.position = new Vector2(0.0f, 0.0f);
        key.color = Color.blue;
        keys.Add(key);

        key.position = new Vector2(0.0f, 1.0f);
        key.color = Color.blue;
        keys.Add(key);

        key.position = new Vector2((Rates[0] / 2) / 100.0f, 0.0f);
        key.color = Color.blue;
        keys.Add(key);

        key.position = new Vector2((Rates[0] / 2) / 100.0f, 1.0f);
        key.color = Color.blue;
        keys.Add(key);

        key.position = new Vector2((Rates[0]) / 100.0f, 0.0f);
        key.color = Color.green;
        keys.Add(key);

        key.position = new Vector2((Rates[0]) / 100.0f, 1.0f);
        key.color = Color.green;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] / 2) / 100.0f, 0.0f);
        key.color = Color.green;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] / 2) / 100.0f, 1.0f);
        key.color = Color.green;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1]) / 100.0f, 0.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1]) / 100.0f, 1.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2] / 2) / 100.0f, 0.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2] / 2) / 100.0f, 1.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2]) / 100.0f, 0.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2]) / 100.0f, 1.0f);
        key.color = Color.yellow;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2] + Rates[3] / 2) / 100.0f, 0.0f);
        key.color = Color.red;
        keys.Add(key);

        key.position = new Vector2((Rates[0] + Rates[1] + Rates[2] + Rates[3] / 2) / 100.0f, 1.0f);
        key.color = Color.red;
        keys.Add(key);

        // 最後の赤は固定
        key.position = new Vector2(1.0f, 0.0f);
        key.color = Color.red;
        keys.Add(key);

        key.position = new Vector2(1.0f, 1.0f);
        key.color = Color.red;
        keys.Add(key);

        gradationMaterial.keys = keys;

        return gradationMaterial;
    }
    */


    static int Clamp(int val, int min, int max)
    {
        if (val < min) val = min;
        else if (val > max) val = max;
        return val;
    }


    //  課題スコアを年齢で補正する
    public static int CalcScore(int rawScore)
    {
        int[] range = new int[2];
        /*
        if (CommonData.userObject.age < 36)
        {
            range[0] = CommonData.training.scoringSetting.young.min;
            range[1] = CommonData.training.scoringSetting.young.max;
        }
        else if (CommonData.userObject.age < 61)
        {
            range[0] = CommonData.training.scoringSetting.middle.min;
            range[1] = CommonData.training.scoringSetting.middle.max;
        }
        else if (CommonData.userObject.age < 71)
        {
            range[0] = CommonData.training.scoringSetting.senior.min;
            range[1] = CommonData.training.scoringSetting.senior.max;
        }
        else
        {
            range[0] = CommonData.training.scoringSetting.silver.min;
            range[1] = CommonData.training.scoringSetting.silver.max;
        }

    */

        int score = (int)((double)(rawScore - range[0]) / (range[1] - range[0]) * 100);

        return Clamp(score, 0, 100);
    }
}
