using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// 各種脳波アプリ用のHot2gラッパ
public class Hot2gApplication : SingletonMonoBehaviour<Hot2gApplication>
{
    //public class DataLogger { };

    //public delegate void Hot2gAppCB();

    public enum eMode
    {
        None,
        Scan,
        ScanOK,
        Connecting,             // 
        InitSetting,			// 初回設定
        GainSetting,			// OnMgc
        RecieveData,            // OnMeasure
        ResetGain,
        Waiting,                // 非装着や、計測開始前
        //Moved,                  // 体動検出後の状態

        ConnectError = -2,
        ScanError = -3,
    }

    public enum eState
    {
        OnHead,
        NotOnHead,
    }

    public eMode mode
    {
        get
        {
            return m_Mode;
        }
    }

    public DataStore datastore
    {
        get
        {
            return dataStore;
        }
    }

    /** 設定値：xbログに書き込む */
#if RELEASE_BUILD
	public bool isOnLogging = false;
#else
    public bool isOnLogging = true;
#endif

    public Hot2gApi API
    {
        get
        {
            return m_API;
        }
    }

    Hot2gApi m_API;
    public eMode m_Mode;
    public BrainDataFeedbacker m_nfb;
    DataStore dataStore;
    GainController m_gc;
    DataLogger m_Logger = null;
    int m_DebugLogCount = 0;
    Hot2gMeta m_Meta;

    public int afterMGC_counter = 0;
    int afterMGC_counter_MAX = 20;//- wait for 20 sec


    public eState state2 = eState.NotOnHead;

    /** 強制MGC用状態変更関数 */
    public void setStateToMGC()
    {
        this.m_Mode = eMode.GainSetting;
    }
    /** 強制ゲインリセット用状態変更関数 */
    public void setStateToResetGain()
    {
        this.m_Mode = eMode.ResetGain;

    }
    /** 状態変更関数 Wating*/
    public void setStateToWating()
    {
        this.m_Mode = eMode.Waiting;
    }

    public void setState2ToOnHead()
    {
        this.state2 = eState.OnHead;
    }
    public void setState2ToNotOnHead()
    {
        this.state2 = eState.NotOnHead;
    }

    public List<bool> statusOnHead_buffer = new List<bool>();

    private void Start()
    {
        m_API = new Hot2gApi();
        m_API.initAPI(false);
        m_Mode = eMode.None;

    }

    BLEMgr.Callback m_ScanSuccess = null;
    BLEMgr.Callback m_ScanError = null;


    BLEMgr.Callback m_ConnectSuccess = null;
    BLEMgr.Callback m_ConnectError = null;

    BLEMgr.Callback m_DisConnectCallback = null;

    Time m_StartTime;
    Time m_EndTime;

    string m_connectAddress;

    public List<string> GetScanDeviceList()
    {
        return m_API.getScanDeviceList();
    }

    public void Scan(BLEMgr.Callback Success, BLEMgr.Callback Error)
    {
        m_Mode = eMode.Scan;
        m_ScanSuccess = Success;
        m_ScanError = Error;

        m_API.scanBtDevice(() =>
        {
            m_Mode = eMode.ScanOK;
            if (m_ScanSuccess != null)
            {
                m_ScanSuccess();
            }

            m_ScanSuccess = null;
            m_ScanError = null;
        }, () =>
        {
            m_Mode = eMode.ScanError;
            if (m_ScanError != null)
            {
                m_ScanError();
            }

            m_ScanSuccess = null;
            m_ScanError = null;
        });

    }

    void onCommStsChanged(Hot2gEnumerations.EnumHot2gCommStsChangedEvent e, string device_address)
    {
        if (e == Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Disconnected)
        {
            m_Mode = eMode.None;
            if (m_DisConnectCallback != null)
            {
                m_DisConnectCallback();
            }
        }
    }


    // デバイスをスキャンして接続
    public void Connecting(string Address, BLEMgr.Callback Success, BLEMgr.Callback Error, BLEMgr.Callback DisConnectCallback)
    {
        m_ConnectSuccess = Success;
        m_ConnectError = Error;
        m_DisConnectCallback = DisConnectCallback;

        m_Mode = eMode.Connecting;

        m_API.connectBtDevice(Address, () =>
       {
           m_Mode = eMode.InitSetting;
           StartCoroutine(InitSetting());

           m_API.setCommStsChangedEventHandler(onCommStsChanged);

       }, () =>
       {
           m_Mode = eMode.ConnectError;
           if (m_ConnectError != null)
           {
               m_ConnectError();
           }

           m_ConnectSuccess = null;
           m_ConnectError = null;
       });
        m_connectAddress = Address;
    }

    const float SettingWait = 0.25f;
    IEnumerator InitSetting()
    {
        yield return new WaitForSeconds(SettingWait);

		// 初期化コマンド群

		Hot2gApplication.Instance.API.getDeviceInfo( Hot2gEnumerations.EnumHot2gDevInfo.DevId);
		yield return new WaitForSeconds(SettingWait);

		/**/
		Hot2gApplication.Instance.API.setMotionAccRange(Hot2gEnumerations.EnumHot2gAccRange.e16G);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport86();

		Hot2gApplication.Instance.API.setMotionGyroRange(Hot2gEnumerations.EnumHot2gGyroRange.e2000DPS);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport87();

		Hot2gApplication.Instance.API.setMotionSamplingInfo_1(Hot2gEnumerations.EnumHot2gMotSmplRate.e10HZ, Hot2gEnumerations.EnumHot2gMotTransRate.e10HZ);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport89();

		Hot2gApplication.Instance.API.setMotionSamplingInfo_2(Hot2gEnumerations.EnumHot2gMotSmplRate.e10HZ, Hot2gEnumerations.EnumHot2gMotTransRate.e10HZ);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport8A();

		Hot2gApplication.Instance.API.setHRSamplingInfo(Hot2gEnumerations.EnumHot2gHRTransRate.e10HZ);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport8A();

		Hot2gApplication.Instance.API.setUsbRTOutputMode(false);
		yield return new WaitForSeconds(SettingWait);
		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport8C();

		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport86();

		//Hot2gApplication.Instance.API.setBatterySamplingInfo(Hot2gEnumerations.EnumHot2gBattTransRate.e1HZ);
		//Hot2gApplication.Instance.API.set

		Hot2gApplication.Instance.API.getHot2gIf().UpdateReport81();

		/**/

		if (m_Mode == eMode.InitSetting)
        {
            m_Mode = eMode.Waiting;
            //m_Mode = eMode.Connecting;
            if (m_ConnectSuccess != null)
            {
                m_ConnectSuccess();
            }
            m_ConnectSuccess = null;
            m_ConnectError = null;
        }
    }

    // 切断
    public void DisConnect()
    {
        m_API.connectBtDevice(m_connectAddress,() => { },() => { });
        m_Mode = eMode.None;//- TK 2019-05-22
    }

    // 受信開始
    public void StartRecieve(BrainDataFeedbacker.Type _type)
    {
        dataStore = new DataStore();
        m_nfb = new BrainDataFeedbacker(dataStore);
        m_nfb.start(_type);

        if (isOnLogging)
        {
            m_Logger = new DataLogger();
            m_DebugLogCount = 0;
        }


        /* ゲイン調整 */
        m_gc = new GainController();

        this.m_Mode = eMode.Waiting;
        //this.m_Mode = eMode.GainSetting;

        m_API.setMeasureDataEventHandler(OnMeasureDataEvent);
        m_API.startMeasure();

        StartCoroutine(GainSetDeray());


    }

    IEnumerator GainSetDeray()
    {
        yield return new WaitForSeconds(0.3f);

        //- ゲイン値の初期化設定
        resetGain(m_nfb, m_gc);
    }

    void OnMeasureDataEvent(Hot2gEnumerations.EnumHot2gMeasResult e, object meas_data, string device_address)
    {
        //- 注意：3種類のBLEパケットが来るのでここは21Hzくらい。10Hz(Ac1) ,10Hz(Gyro), 1Hz(HR)...

        Debug.Log("1===:"+e+"::"+this.m_Mode);

        if (this.m_Mode == eMode.GainSetting)
        {
            /* 手動ゲイン調整 */
            adjustGain(m_nfb, m_gc, e, meas_data, m_Logger);//- -> State will change to RecieveData after this.
        }
        else if (this.m_Mode == eMode.RecieveData)
        {
            /* データ受信 */
            receiveData(m_nfb, e, meas_data, m_Logger);
        }
        else if (this.m_Mode == eMode.ResetGain)
        {
            /* ゲインリセット */
            resetGain(m_nfb, m_gc); //- -> State will change to Waiting after this.
        }
        else if (this.m_Mode == eMode.Waiting)
        {
            /* データ受信 */
            receiveData(m_nfb, e, meas_data, m_Logger);
        }


        //- Check status
        GyroScope.Stability st_stability = this.dataStore.stability[this.dataStore.stability.Count - 1];

        Debug.Log("Check2  " + "stability: " + st_stability.ToString() + " / state2:" + state2.ToString() +
            " / mode: " + Hot2gApplication.Instance.mode.ToString());

        //- RecieveData to Waiting
        if (Hot2gApplication.Instance.mode == eMode.RecieveData)
        {
            if (st_stability != GyroScope.Stability.Stabled || state2 != eState.OnHead)
            {
                resetGain(m_nfb, m_gc);
                //GainSetDeray();
                Hot2gApplication.instance.setStateToWating();//- to Waiting
                Debug.Log("Check2  " + "GainReset!!!***************************");
            }
        }else if (Hot2gApplication.Instance.mode == eMode.Waiting)
        {
            if (st_stability == GyroScope.Stability.Stabled && state2 == eState.OnHead)
            {
                Hot2gApplication.instance.setStateToMGC();//- --> MGC --> RecieveData
                Debug.Log("Check2  " + "MGC Start >>>>>>>>>>>>>>>>>>>>>>>>>>");
            }
        }

    }

    // 受信停止
    public void StopRecieve()
    {
        if (m_nfb != null)
        {
            m_nfb.stop();
        }

        if (m_Logger != null)
        {
            m_Logger.close();
        }

        if(m_API != null)
        {
            m_API.setMeasureDataEventHandler(null);
            if (Hot2gApplication.eMode.Connecting <= Hot2gApplication.Instance.mode)
            {
                m_API.stopMeasure();
            }
        }
    }

    void adjustGain(BrainDataFeedbacker nfb, GainController gc, Hot2gEnumerations.EnumHot2gMeasResult e, object data, DataLogger logger)
    {
        switch (e)
        {
            case Hot2gEnumerations.EnumHot2gMeasResult.Ac1ch:
                if (!gc.addData(data as Hot2gMeasData1chAc))
                {
                    //- changing gain
                    m_API.setMgcGain(gc.setCurrentGainAsIS());
                    gc.clearBuffer();
                    afterMGC_counter = afterMGC_counter_MAX;
                }
                break;

            case Hot2gEnumerations.EnumHot2gMeasResult.Ac2ch:
                gc.addData(data as Hot2gMeasData1chAc);
                break;

            default:
                return;
        }
        if (gc.getIsMGCFailure())
        {
            if (gc.retryCounter < 3)
            {
                //- reset gain parameter and peripheral's setting
                gc.retryCounter++;

                gc.reset();

                m_API.setMgcGain(gc.setCurrentGainAsIS());
                afterMGC_counter = afterMGC_counter_MAX;
                //
                Debug.Log("TEST\nMGC: Retrying...");

            }
            else
            {
                //- give up MGC
                gc.resetAll();
                m_API.setMgcGain(gc.setCurrentGainAsIS());
                afterMGC_counter = afterMGC_counter_MAX;

                m_Mode = eMode.Waiting;
                //
                Debug.Log("TEST\nMGC: Stop retry.");

            }
        }

        if (!gc.isFixed)
        {
            Debug.Log("Hot2gApp gc is Not Fixed");
            //nfb.bufferUnstable();    //黒背景にする
            return;
        }
        Debug.Log("MGC Fixed.");

        /* [CMD] ゲイン設定 */
        //if (m_Mode != eMode.GainSetting) return;

        m_API.setMgcGain(gc.mastering);
        afterMGC_counter = afterMGC_counter_MAX;

        /* ゲイン設定完了後の接続状態管理 */
        this.m_Mode = eMode.RecieveData;
        nfb.resetBrainCals();
        this.dataStore.addReaction("MGC (L1:" + m_gc.getL1Gain().ToString("N0") +
            " L3:" + m_gc.getL3Gain().ToString("N0") + ")");

        if (m_Meta == null)
        {
            m_Meta = new Hot2gMeta();
            m_Meta.measured = System.DateTime.Now.ToString("yyyy-MM-dd.HH.mm.ss");
            m_Meta.mac = m_API.getConnectedAddress();
            m_Meta.type = "XB-01";
            m_Meta.gender = "null";
            m_Meta.age = 0;
            m_Meta.designId = "null";


            if (logger != null)
            {
                logger.write(m_Meta.ToCSV());
                logger.write(Xb01data.csvHeader);
                logger.write(gc.toString());
            }
        }             
    }

    /**
     * ゲイン調整済みデータ受信時の動作
     */
    private void receiveData(BrainDataFeedbacker nfb, Hot2gEnumerations.EnumHot2gMeasResult e, object data, DataLogger logger)
    {        
        switch (e)
        {
            /*XB-01*/
            case Hot2gEnumerations.EnumHot2gMeasResult.Ac1ch:
                //Debug.Log("Ac1ch:" + LitJson.JsonMapper.ToJson(data));

                //- check afterGainCounter
                checkAfterGainCounter();


                if (Hot2gApplication.Instance.datastore.stability.Count > 1) {
                    Debug.Log("Ac1chRec: [ " + Hot2gApplication.Instance.mode.ToString() +
                    " ]/[ " + Hot2gApplication.Instance.datastore.stability[Hot2gApplication.Instance.datastore.stability.Count - 1] +
                    " ]/[: " + Hot2gApplication.Instance.state2.ToString() +
                    " ]/[G1: "+Hot2gApplication.Instance.m_gc.getL1Gain() + "][G3: "+Hot2gApplication.Instance.m_gc.getL3Gain() +                    
                    " ]/[Raw 1: "+Hot2gApplication.Instance.datastore.l1Ac[Hot2gApplication.Instance.datastore.l1Ac.Count -1]+
                    " ]/[Raw 3: " + Hot2gApplication.Instance.datastore.l3Ac[Hot2gApplication.Instance.datastore.l3Ac.Count - 1] +
                    " ]/ after MGC: " + Hot2gApplication.Instance.afterMGC_counter.ToString()
                    );
                }                

                Xb01data xbData = this.dataStore.add(data as Hot2gMeasData1chAc, nfb.calcActivenessFromBuffered());

                state2 = updateStatusOnHead();//- check XB-01 On head status

                nfb.bufferActiveness();//- calc Brain activation level


                if (logger != null)
                {
                    logger.write(xbData.ConvCSV());
                }
                
                break;
#if false
            case Hot2gEnumerations.EnumHot2gMeasResult.Ac2ch:
                {
            when(Hot2gDevInfo().valDevType) {
                /* HOT-2000 */
                Hot2gEnumerations.EnumHot2gDevType.HOT2000->{
                    val hot2Data = this.dataStore.add(data as Hot2gMeasData2chAc)
                        logger?.write(hot2Data)
                    }
                /* HOT-1000 */
                Hot2gEnumerations.EnumHot2gDevType.HOT1000-> {
                    val hot1data = this.dataStore.add(data as Hot2gMeasData2chAc)
                        logger?.write(hot1data)
                    }
            }
#endif
            /* 心拍数 */
            case Hot2gEnumerations.EnumHot2gMeasResult.HeartRate:
                this.dataStore.add(data as Hot2gMeasDataHR);
                break;

            /* 脈波振幅関連係数 */
            case Hot2gEnumerations.EnumHot2gMeasResult.HRElement1:
                this.dataStore.add(data as Hot2gMeasDataHRElement);
                break;

            /* 姿勢変化のチェック */
            case Hot2gEnumerations.EnumHot2gMeasResult.MotionBatt:
                GyroScope.Stability s_pre = this.dataStore.stability[this.dataStore.stability.Count - 1];
                GyroScope.Stability s = this.dataStore.add(data as Hot2gMeasDataMotBatt);
                break;
        }
    }


    eState updateStatusOnHead()
    {
        //if (Hot2gApplication.Instance.datastore.stability.Count <= 0)
        //{
        //return eState.NotOnHead;
        //}
        
        List<double> brain1 = Hot2gApplication.Instance.datastore.l1Ac;
        List<double> brain3 = Hot2gApplication.Instance.datastore.l3Ac;
        List<double> gyroZ = Hot2gApplication.Instance.datastore.gyroZ;
        GyroScope.Stability stability = Hot2gApplication.Instance.datastore.stability[Hot2gApplication.Instance.datastore.stability.Count - 1];

        //Debug.Log("HRTEST State:" + sF(NpodApplication.app.state.toString()) + " / State2:" + sF(NpodApplication.app.state2.toString()))

        int sz = 20;
        int sz2 = 20;

        if (brain1.Count > sz)
        {
            bool cndStability = (stability == GyroScope.Stability.Stabled); //true;//stability.isStable;

            //  1. HbT 3cm: abs(median - mean)<3
            // val cnd1 = (median(brain3.takeLast(sz)) - brain3.takeLast(sz).average()).absoluteValue < 3.0
            double B3_median = median(takeLast(brain3, sz));
            double B3_ave = AnalyseHelper.Average(brain3, brain3.Count - sz, brain3.Count);
            bool cnd1 = (Math.Abs(B3_median - B3_ave) < 3);
            Debug.Log("HRTEST 2:" + B3_ave.ToString("N3")+"/"+B3_median.ToString("N3") + (B3_median-B3_ave).ToString("N3"));

            //  2. HbT 3cm: non zero
            //val cnd2 = (median(brain3.takeLast(sz)) - brain3.takeLast(sz).average()).absoluteValue != 0.0
            bool cnd2 = (Math.Abs(B3_median - B3_ave) != 0.0);

            //  3. Hb 1cm: median(v-v(1))<0.3
            bool cnd3 = median(subtract(takeLast(brain1, sz), brain1[brain1.Count - sz])) < 0.3;
            Debug.Log("HRTEST 3:" + median(subtract(takeLast(brain1, sz), brain1[brain1.Count - sz])).ToString("N3"));

            //  4. Gyro-Z: median(v-v(1))<7
            bool cnd4 = median(subtract(takeLast(gyroZ, sz), gyroZ[gyroZ.Count - sz])) < 7;
            Debug.Log("HRTEST 4:" + median(subtract(takeLast(gyroZ, sz), gyroZ[gyroZ.Count - sz])).ToString("N3"));

            //  5. 置いたままの状態を加速度から検出
            bool cnd5 = sd(takeLast(gyroZ, sz)) > 0.02*5;
            Debug.Log("HRTEST 5:" + sd(takeLast(gyroZ, sz)).ToString("N3"));


            Debug.Log("HRTEST 1:" + cnd1 + " 2:" + cnd2 + " 3:" + cnd3 + " 4:" + cnd4 + " 5:" + cnd5);

            //this.statusOnHead_buffer.Add(cndStability && cnd1 && cnd2 && cnd3 && cnd4 && cnd5);
            this.statusOnHead_buffer.Add(cndStability && cnd1 && cnd2 && cnd3 && cnd5);
            if (this.statusOnHead_buffer.Count > sz2)
            {
                this.statusOnHead_buffer.RemoveAt(0);
            }
            
        }

        //isupdateActivenesses = !isupdateActivenessesBuffer.takeLast(2).all { !it }

        if (statusOnHead_buffer.Count < sz2)
        {
            return eState.NotOnHead;
        }
        else
        {            
            int ngCount = 0;
            statusOnHead_buffer.ForEach(val =>
            {
                if (!val) ngCount++;
            });
            if (ngCount > sz2 / 2) return eState.NotOnHead;//- if more than half status in buffer are NOT on head, status will be NotOnHead
            else return eState.OnHead;            
        }

        /*
        if (!isupdateActivenesses)
        // 非装着検出！
        {
            // 計測中（装着）　→　非装着 
            if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.RecieveData)
            {
                Hot2gApplication.Instance.setStateToResetGain(); //- リセット後はルーチン内でWaitingに設定される
                Hot2gApplication.Instance.setState2ToNotOnHead();

            }
            else if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting)
            { // 非装着中 
                //Hot2gApplication.Instance.setStateToWating();
                //Hot2gApplication.Instance.setState2ToNotOnHead();
            }
        }
        else
        {
            if (Hot2gApplication.Instance.mode == Hot2gApplication.eMode.Waiting)
            // 装着中検出　
            {
                // 非装着　→　装着  
                Hot2gApplication.Instance.setStateToMGC();
                Hot2gApplication.Instance.setState2ToOnHead();
            }
            else
            {
                // 装着中 
                Hot2gApplication.Instance.setState2ToOnHead();
            }
        }       

        return;
        */


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


    /**
     * 強制ゲインリセット
     */
    private void resetGain(BrainDataFeedbacker nfb, GainController gc)
    {
        nfb.resetBrainCals();

        gc.reset();

        m_API.setMgcGain(gc.setCurrentGainAsIS());
        afterMGC_counter = afterMGC_counter_MAX;

        //val ret = this@XbApplication.xb.setMgcGain(gc.setCurrentGainAsIS()) // set gain parameters to the default value.

        dataStore.add(gc.setCurrentGainAsIS());

        m_Mode = eMode.Waiting;
    }

    // 現在の脳活性度を取得
    public double calcActivenessFromBuffered()
    {
        return m_nfb.calcActivenessFromBuffered();
    }
    public double calcActivenessFromBuffered(int startIdx)
    {
        return m_nfb.calcActivenessFromBuffered(startIdx);
    }



    // 現在の脳活性度を取得
    public List<double> GetBreathBuffer()
    {
        return m_nfb.GetBreathBuffer();
    }

    public double ActivenessesAverage()
    {
        if (m_nfb == null)
        {
            return 0.5;
        }

        return m_nfb.activenessAverage;
    }

    public double[] GetActivenessesLog()
    {
        if (m_nfb == null)
        {
            return new double[] { 0, 0, 0.3, 0.3, 0.6, 0.6, 0.9, 0.3, 0.6, 0.9, 0.3, 0.6, 0.9 };
        }

        return m_nfb.activenessLog;
    }

    private void checkAfterGainCounter()
    {
        this.afterMGC_counter--;
        if (this.afterMGC_counter < 0) this.afterMGC_counter = 0;
    }


    public System.DateTime GetStartTime()
    {
        if (m_nfb == null)
        {
            return new System.DateTime(2018, 1, 1, 9, 0, 0);
        }
        return m_nfb.startTime;
    }

    public System.DateTime GetEndTime()
    {
        if (m_nfb == null)
        {
            return new System.DateTime(2018, 1, 1, 9, 5, 0);
        }
        return m_nfb.endTime;
    }

    public void DoubleTap()
    {
        if (m_Logger != null)
        {
            m_Logger.write("DoubleTap_" + m_DebugLogCount.ToString());
            m_DebugLogCount++;
        }
    }


    static string sF(int v) { return v + " "; }
    static string sF(double v) { return v.ToString("0.00"); }
    static string sF(byte v) { return v + " "; }
    static string sF(string v) { return v + " "; }

    static double sd(double[] numArray)
    {
        double mean = AnalyseHelper.Average(numArray, 0, numArray.Length);

        double n = 0.0;
        for (int i = 0; i < numArray.Length; i++)
        {
            //next = numArray[i] + System.Math.Pow(next - mean, 2.0);
            n = n + System.Math.Pow(numArray[i] - mean, 2.0);
        }

        return System.Math.Sqrt(n / numArray.Length);
    }
    static double max(List<double> l)
    {
        double d = -9999999999999;
        for (int i = 0; i < l.Count; i++)
        {
            if (d < l[i])
            {
                d = l[i];
            }
        }

        return d;
    }
    static double median(double[] b)
    {
        List<double> l = new List<double>(b);
        l.Sort();

        return (l[l.Count / 2] + l[(l.Count - 1) / 2]) / 2.0;
    }
    static double[] subtract(double[] l, double v)
    {
        double[] l1 = new double[l.Length];
        for (int i = 0; i < l.Length; i++)
        {
            l1[i] = (l[i] - l[0]);
        }
        return l1;
    }
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

}

