using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * HOT-2G通信インターフェース管理クラス
 */
public class Hot2gInterface
{
    public delegate void DeviceMeasDataReport(Hot2gEnumerations.EnumHot2gMeasResult e, object meas_data, string device_address);
    public delegate void CommStatusChanged(Hot2gEnumerations.EnumHot2gCommStsChangedEvent e, string device_address);

    private const string TAG = "Hot2gInterface";

	private Hot2gDataManager		hot2gDataManager ;

	BLEMgr					        mBluetoothDriver ;

	Hot2gCommands					mCommands ;


	private string				mRequestAddress ;
	private string				mConnectedAddress ;
	private Hot2gEnumerations.EnumHot2gCommType	mSelectedCommType ;
	private bool			mBtServiceConnected ;
	private bool			mBtConnected ;
	private bool			mUsbConnected ;
	private bool			mMeasureActivatedF ;
	private bool			mAutoGainActivatedF ;
	private bool			mOutputLogEnabled ;
	private Hot2gEnumerations.EnumHot2gMsgID		mNeedResponseMsgId ;
	private bool			mReceiveResponseF ;
	private byte[]				mMemMsgBytes = new byte[4096] ;		// [181030.1]
	private int				mMemMsgSize = 0 ;						// [181030.1]

	public Hot2gDataManager		getHot2gDataManager() { return( this.hot2gDataManager ) ; }
	public Hot2gDevInfo			getHot2gDevInfo() { return( this.hot2gDataManager.DevInfo ) ; }
	public Hot2gDevState		getHot2gDevState() { return( this.hot2gDataManager.DevState ) ; }
	public Hot2gLocalInfo		getHot2gLocalInfo() { return( this.hot2gDataManager.LocalInfo ) ; }
	public Hot2gAgcResult		getHot2gAgcResult() { return( this.hot2gDataManager.AgcResult ) ; }
	public BLEMgr	getBluetooth() { return( this.mBluetoothDriver ) ; }

	public string getConnectedAddress() { return(this.mConnectedAddress); }
	public Hot2gEnumerations.EnumHot2gCommType getCommType() { return(this.mSelectedCommType); }
	public bool isUsbConnected() { return(this.mUsbConnected); }
	public bool isBtConnected() { return(this.mBtConnected); }
	public bool isMeasuring() { return(this.mMeasureActivatedF); }
	public bool isAGC() { return(this.mAutoGainActivatedF); }
	public bool isBtServiceConnected() { return(this.mBtServiceConnected); }

	private const int TIMEOUT_RESPONSE = 5 ;			// 応答タイムアウト [seconds]

    DeviceMeasDataReport m_DeviceMeasDataReport = null;
    CommStatusChanged m_CommStatusChanged = null;

    //---------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------

    /**
	 * コンストラクタ
	 */
    public Hot2gInterface(DeviceMeasDataReport deviceMeasDataReport, CommStatusChanged commStatusChanged)
	{
        m_DeviceMeasDataReport = deviceMeasDataReport;
        m_CommStatusChanged = commStatusChanged;

        hot2gDataManager = new Hot2gDataManager() ;

		mSelectedCommType = Hot2gEnumerations.EnumHot2gCommType.NotSelected ;
		mBtConnected = false ;
		mUsbConnected = false ;

        //mBluetoothDriver = newBLEMgr
        mBluetoothDriver = BLEMgr.Instance;


		mCommands = new Hot2gCommands( hot2gDataManager ) ;

        BLEMgr.Instance.Init(onBtStatusChanged, onBtReceiveReport);


        /*
		// Bluetooth通信ドライバ用コールバック登録
		setCallbackEvents(new IBluetoothDriverCallBack() {
			@Override
			public void onBtStatusChanged(Hot2gEnumerations.EnumHot2gDriverCommStsEvent event) {
				outputWorkLog( TAG, "onBtStatusChanged :" + event.ToString() ) ;
				switch( event ){
					case Connected:				// 接続完了イベント
						mBtConnected = true ;
//						if( Events != null ){
//							Events.onCommStatusChanged( Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Connected, mConnectedAddress ) ;	// 18.10.05
//						}
						break ;
					case Disconnected:			// 切断完了イベント
						mBtConnected = false ;
						if( Events != null ){
							Events.onCommStatusChanged( Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Disconnected, mConnectedAddress ) ;	// 18.10.05
						}
						mConnectedAddress = "" ;		// 18.10.05
						break ;
					case ServiceDiscovered:		// GATTサービス発見完了
						mSelectedCommType = Hot2gEnumerations.EnumHot2gCommType.Bluetooth ;
						mConnectedAddress = mBluetoothDriver.getConnectedAddress() ;
						break ;
				}
			}

			@Override
			public void onBtReceiveReport(Hot2gEnumerations.EnumHot2gCommBtMsgEvent event, byte[] bytes) {
				outputWorkLog( TAG, "onBtReceiveReport :" + event.ToString() ) ;
				bool	check_format = false ;

				switch( event ){
					case Notified:
						try{
							outputCommLog( "BT", "R", bytes );

							check_format = Hot2gCommandBluetoothFormat.checkReceiveMessage( bytes, bytes.length ) ;
							if ( check_format ){
								// 受信メッセージ処理分岐
								parserReceiveMessage( Hot2gCommandBluetoothFormat.valCmdID, Hot2gCommandBluetoothFormat.valCmdBytes ) ;
							}
						}
						catch( Exception ex ){
							outputWorkLog( TAG, "onBtReceiveReport: ", ex ) ;
						}
						break ;
					case Read:
						break ;
					case Written:
						break ;
				}
			}

			@Override
			public void onBtStatusReport(string message, Hot2gEnumerations.EnumMsgNotifyType type) {
				outputWorkLog( TAG, "onBtStatusReport :" + message ) ;
			}

			@Override
			public void onBtSearchReport( Hot2gEnumerations.EnumHot2gWatcherEvent event, BluetoothDevice device ) {
				outputWorkLog( TAG, "onBtSearchReport :" + event.ToString() ) ;

				Events.onBtSearchReport( event, device );
			}
		}) ;
        */


    }

    public void onBtStatusChanged(Hot2gEnumerations.EnumHot2gDriverCommStsEvent e, string ConectAddress)
    {
        outputWorkLog(TAG, "onBtStatusChanged :" + e.ToString());
        switch (e)
        {
            case Hot2gEnumerations.EnumHot2gDriverCommStsEvent.Connected:             // 接続完了イベント
                mBtConnected = true;
                mSelectedCommType = Hot2gEnumerations.EnumHot2gCommType.Bluetooth;
                if (m_CommStatusChanged != null)
                {
                    m_CommStatusChanged(Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Connected, ConectAddress); // 18.10.05
                }
                mConnectedAddress = ConectAddress;
                break;

            case Hot2gEnumerations.EnumHot2gDriverCommStsEvent.Disconnected:          // 切断完了イベント
                mBtConnected = false;
                if (m_CommStatusChanged != null)
                {
                    m_CommStatusChanged(Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Disconnected, mConnectedAddress); // 18.10.05
                }
                mConnectedAddress = "";     // 18.10.05
           break;
        }
    }


    public void onBtReceiveReport(Hot2gEnumerations.EnumHot2gCommBtMsgEvent e, byte[] bytes)
    {

        outputWorkLog(TAG, "onBtReceiveReport :" + e.ToString());

        bool check_format = false;

        switch (e)
        {
            case Hot2gEnumerations.EnumHot2gCommBtMsgEvent.Notified:
                try
                {
                    outputCommLog("BT", "R", bytes);

                    check_format = Hot2gCommands.Hot2gCommandBluetoothFormat.checkReceiveMessage(bytes, bytes.Length);
                    if (check_format)
                    {
                        // 受信メッセージ処理分岐
                        parserReceiveMessage(Hot2gCommands.Hot2gCommandBluetoothFormat.valCmdID, Hot2gCommands.Hot2gCommandBluetoothFormat.valCmdBytes);
                    }
                }
                catch (System.Exception ex)
                {
                    outputWorkLog(TAG, "onBtReceiveReport: ", ex);
                }
                break;
            case Hot2gEnumerations.EnumHot2gCommBtMsgEvent.Read:
                break;
            case Hot2gEnumerations.EnumHot2gCommBtMsgEvent.Written:
                break;
        }
    }




    //---------------------------------------------------------------------------------------------

    /**
	 * 受信メッセージ解析
	 * @param id :受信メッセージID[Hot2gEnumerations.EnumHot2gMsgID]
	 * @param bytes :受信メッセージ
	 * @return :処理結果[T/F]
	 */
    private void parserReceiveMessage( Hot2gEnumerations.EnumHot2gMsgID id,  byte[] bytes )
	{
		bool	check = true ;
		Hot2gEnumerations.EnumHot2gMeasResult	meas_type = Hot2gEnumerations.EnumHot2gMeasResult.None;
		object		meas_data = null ;

		try{
			switch( id ){
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceType:			mCommands.Report80.setReportData( bytes ) ;		break ;		// [80]機種情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceID:				mCommands.Report81.setReportData( bytes ) ;		break ;		// [81]機器ID情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceName:			mCommands.Report82.setReportData( bytes ) ;		break ;		// [82]機種名称
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceFirmVer:		mCommands.Report83.setReportData( bytes ) ;		break ;		// [83]バージョン情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceHwInfo:		mCommands.Report84.setReportData( bytes ) ;		break ;		// [84]ハードウェア情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_WaitIrInfo:			mCommands.Report85.setReportData( bytes ) ;		break ;		// [85]待機時IR情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MotionAcc:			mCommands.Report86.setReportData( bytes, hot2gDataManager.LocalInfo.valHwAccEnableBits ) ;		break ;		// [86]モーション加速度情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MotionGyro:			mCommands.Report87.setReportData( bytes, hot2gDataManager.LocalInfo.valHwGyroEnableBits ) ;		break ;		// [87]モーション角速度情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceBattery:		mCommands.Report88.setReportData( bytes ) ;		break ;		// [88]バッテリー情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasSampling:		mCommands.Report89.setReportData( bytes ) ;		break ;		// [89]計測サンプリング情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasTransMode:		mCommands.Report8A.setReportData( bytes ) ;		break ;		// [8A]計測転送モード情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasAnalysisMode:	mCommands.Report8B.setReportData( bytes ) ;		break ;		// [8B]計測解析モード情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_DebugFlag:			mCommands.Report8C.setReportData( bytes ) ;		break ;		// [8C]デバッグ情報
				case Hot2gEnumerations.EnumHot2gMsgID.Report_LotID:				mCommands.Report8D.setReportData( bytes ) ;		break ;		// [8D]ロットID
				case Hot2gEnumerations.EnumHot2gMsgID.Report_ErrorCode:			mCommands.ReportA0.setReportData( bytes ) ;		break ;		// [A0]診断状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryGauge:		mCommands.ReportA1.setReportData( bytes ) ;		break ;		// [A1]電池残量状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryVolt:			mCommands.ReportA2.setReportData( bytes ) ;		break ;		// [A2]電池電圧状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_Temperature:			mCommands.ReportA3.setReportData( bytes ) ;		break ;		// [A3]温度状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_IrLightOutput:		mCommands.ReportA4.setReportData( bytes ) ;		break ;		// [A4]IR光源出力状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_UsbState:				mCommands.ReportA5.setReportData( bytes ) ;		break ;		// [A5]USB検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_BluetoothState:		mCommands.ReportA6.setReportData( bytes ) ;		break ;		// [A6]BT検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_SensorBoardEnabled:	mCommands.ReportA7.setReportData( bytes ) ;		break ;		// [A7]センサモジュール検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_PDEnabled:			mCommands.ReportA8.setReportData( bytes ) ;		break ;		// [A8]PD検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MotionEnabled:		mCommands.ReportA9.setReportData( bytes ) ;		break ;		// [A9]モーションセンサ検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryEnabled:		mCommands.ReportAA.setReportData( bytes ) ;		break ;		// [AA]残量ゲージセンサ検出状態
				case Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoMeasStart:	mCommands.ReportC0.setReportData( bytes ) ;		break ;		// [C0]自動計測開始
				case Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoMeasStop:	mCommands.ReportC1.setReportData( bytes ) ;		break ;		// [C1]自動計測停止
				case Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoGainStart:	mCommands.ReportC2.setReportData( bytes ) ;		break ;		// [C2]ゲイン自動調整開始
				case Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoGainStop:	mCommands.ReportC3.setReportData( bytes ) ;		break ;		// [C3]ゲイン自動調整終了
				case Hot2gEnumerations.EnumHot2gMsgID.Report_PDManuGainIr:		mCommands.ReportC4.setReportData( bytes ) ;		break ;		// [C4]ゲイン手動更新
				case Hot2gEnumerations.EnumHot2gMsgID.Report_PDValueInput:		mCommands.ReportC5.setReportData( bytes ) ;		break ;		// [C5]現在PD値入力
				case Hot2gEnumerations.EnumHot2gMsgID.Report_IrOutputEstablish:	mCommands.ReportC6.setReportData( bytes ) ;		break ;		// [C6]IR光源出力設定
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MotionValueInput:	mCommands.ReportC7.setReportData( bytes ) ;		break ;		// [C7]現在モーション値入力
				case Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryInput:		mCommands.ReportC8.setReportData( bytes ) ; 		break ;		// [C8]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasBothAC:
					mCommands.ReportD0.setReportData( bytes ) ;
					meas_data = mCommands.ReportD0.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Ac2ch ;
					break ;		// [D0]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasSideAC:
					mCommands.ReportD1.setReportData( bytes ) ;
					meas_data = mCommands.ReportD1.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Ac1ch ;
					break ;		// [D1]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataMotion:
					mCommands.ReportD2.setReportData( bytes ) ;
					meas_data = mCommands.ReportD2.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Motion ;
					break ;		// [D2]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataBattery:
					mCommands.ReportD3.setReportData( bytes ) ;
					meas_data = mCommands.ReportD3.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Battery ;
					break ;		// [D3]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataMotBatt:
					mCommands.ReportD4.setReportData( bytes ) ;
					meas_data = mCommands.ReportD4.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.MotionBatt ;
					break ;		// [D4]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataHR:
					mCommands.ReportD5.setReportData( bytes ) ;
					meas_data = mCommands.ReportD5.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.HeartRate ;
					break ;		// [D5]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataHRElement1:			// 18.10.11 ohara
					mCommands.ReportD6.setReportData( bytes ) ;
					meas_data = mCommands.ReportD6.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.HRElement1 ;
					break ;		// [D6]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_MeasDataHRElement2:			// 18.10.11 ohara
					mCommands.ReportD7.setReportData( bytes ) ;
					meas_data = mCommands.ReportD7.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.HRElement2 ;
					break ;		// [D7]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_ProbeParam:		mCommands.ReportATC.setReportData( bytes ) ;		break ;		// [ATC]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasStop:			mCommands.ReportATS.setReportData( bytes ) ;		break ;		// [ATS]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasBothAC:
					mCommands.ReportATR.setReportData( bytes ) ;
					meas_data = mCommands.ReportATR.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Ac2ch ;
					break ;		// [ATR]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasSideAC:
					mCommands.ReportATX.setReportData( bytes ) ;
					meas_data = mCommands.ReportATX.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Ac1ch ;
					break ;		// [ATX]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasDataMoton:
					mCommands.ReportATY.setReportData( bytes ) ;
					meas_data = mCommands.ReportATY.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.Motion ;
					break ;		// [ATY]
				case Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasDataMotBatt:
					mCommands.ReportATZ.setReportData( bytes ) ;
					meas_data = mCommands.ReportATZ.geResultData() ;
					meas_type = Hot2gEnumerations.EnumHot2gMeasResult.MotionBatt ;
					break ;		// [ATZ]
				default:		check = false ;	break;
			}

			if( check ){
				if( mNeedResponseMsgId != Hot2gEnumerations.EnumHot2gMsgID.None ){
					if( id == mNeedResponseMsgId )
                    {
						mNeedResponseMsgId = Hot2gEnumerations.EnumHot2gMsgID.None;
						mReceiveResponseF = true ;
					}
				}
			}
            
			if( meas_data != null ){
				m_DeviceMeasDataReport( meas_type, meas_data, this.mConnectedAddress );
			}
		}
		catch( System.Exception ex )
        {
			outputWorkLog( TAG, "parserReceiveMessage", ex ) ;
		}
	}

	//---------------------------------------------------------------------------------------------

	/**
	 * ログファイル出力有無
	 * @param enabled	:通信ログ出力有無 [T/F]
	 */
	public void setOutputLogEnabled( bool enabled )
	{
		this.mOutputLogEnabled = enabled ;

		/*if( enabled ){
			mWorkLog = new WorkingLogger() ;
			mWorkLog.open() ;

			mCommLog = new CommRawLogger() ;
			mCommLog.open() ;
		}*/
	}

	/**
	 * メッセージで使用する通信ドライバを選択
	 * @param type
	 */
	public void setCommType( Hot2gEnumerations.EnumHot2gCommType type )
	{
		this.mSelectedCommType = type ;
	}

	/**
	 * Bluetooth接続
	 * @param address
	 * @return
	 */
	public Hot2gEnumerations.EnumHot2gResult scanBtDevice(BLEMgr.Callback Success, BLEMgr.Callback Error )
	{
		BLEMgr.Instance.StartScan(Success, Error);
		return Hot2gEnumerations.EnumHot2gResult.Success;
	}

	public List<string> getScanDeviceList()
	{
		return BLEMgr.Instance.GetScanDeviceList();
	}



	/**
	 * Bluetooth接続
	 * @param address
	 * @return
	 */
	public Hot2gEnumerations.EnumHot2gResult connectBtDevice( string Address, BLEMgr.Callback Success, BLEMgr.Callback Error)
	{
        BLEMgr.Instance.StartConnect( Address, Success, Error);
        return Hot2gEnumerations.EnumHot2gResult.Success;
	}

	/**
	 * Bluetooth切断
	 * @return
	 */
	public Hot2gEnumerations.EnumHot2gResult disconnectBtDevice( int timeout )
	{
        /*
		Hot2gEnumerations.EnumHot2gResult		result = Hot2gEnumerations.EnumHot2gResult.Failed ;

		mThreadPool.submit(new Runnable() {
			@Override
			public void run() {
				mBluetoothDriver.disconnect() ;

				while( mBtConnected ) ;
			}
		});

		try{
			mThreadPool.awaitTermination( timeout, TimeUnit.SECONDS ) ;
			result = Hot2gEnumerations.EnumHot2gResult.Success ;
			outputWorkLog( TAG, "disconnectBtDevice: " + result.ToString() ) ;
		}
		catch (InterruptedException ex){
			result = Hot2gEnumerations.EnumHot2gResult.SystemError ;
		}
		catch (Exception ex){
			result = Hot2gEnumerations.EnumHot2gResult.SystemError ;
		}

		return( result ) ;
        */

        return Hot2gEnumerations.EnumHot2gResult.Success;
    }


	/**
	 * HOT1Gコマンド送信
	 * @param bytes
	 */
	public Hot2gEnumerations.EnumHot2gResult sendHot1gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID id, byte[] bytes, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult		result = Hot2gEnumerations.EnumHot2gResult.Failed ;
		bool	ret = false ;

		if( mSelectedCommType == Hot2gEnumerations.EnumHot2gCommType.Bluetooth )
        {
			if( mBluetoothDriver.isConnected ){
				outputCommLog( "BT", "S", bytes );
				/*if( async_mode ){
					// 応答待ちあり
					Future<bool>	future = mThreadPool.submit( new AsyncSendBluetoothCommand( id, bytes ) ) ;
					try {
						ret = future.get( TIMEOUT_RESPONSE, TimeUnit.SECONDS ) ;
						if( ret ){
							result = Hot2gEnumerations.EnumHot2gResult.Success ;
						}
					} catch (TimeoutException e) {
						result = Hot2gEnumerations.EnumHot2gResult.ReadTimeout ;
					} catch (Exception e) {
						result = Hot2gEnumerations.EnumHot2gResult.SystemError ;
					}
				}
				else*/
                {
					// 応答待ちなし
					ret = mBluetoothDriver.SendCommand( bytes ) ;
					if( ret ){
						result = Hot2gEnumerations.EnumHot2gResult.Success ;
					}
				}
			}
		}


		return( result ) ;
	}

	/**
	 * HOT2Gコマンド非同期送信
	 * @param id		:
	 * @param bytes	:
	 */
	public Hot2gEnumerations.EnumHot2gResult sendHot2gRequestCommand(Hot2gEnumerations.EnumHot2gMsgID id, byte[] bytes, bool async_mode, BLEMgr.Callback cc = null)
	{
		Hot2gEnumerations.EnumHot2gResult		result = Hot2gEnumerations.EnumHot2gResult.Failed ;
		bool	ret = false ;
		byte[]		send_bytes ;

		if( mSelectedCommType == Hot2gEnumerations.EnumHot2gCommType.Bluetooth ){
			if( mBluetoothDriver.isConnected ){
				send_bytes = Hot2gCommands.Hot2gCommandBluetoothFormat.createSendMessage( bytes ) ;
				outputCommLog( "BT", "S", send_bytes );
				/*if( async_mode ){
					// 応答待ちあり
					Future<bool>	future = mThreadPool.submit( new AsyncSendBluetoothCommand( id, send_bytes ) ) ;
					try {
						ret = future.get( TIMEOUT_RESPONSE, TimeUnit.SECONDS ) ;
						if( ret ){
							result = Hot2gEnumerations.EnumHot2gResult.Success ;
						}
					} catch (TimeoutException e) {
						result = Hot2gEnumerations.EnumHot2gResult.ReadTimeout ;
					} catch (Exception e) {
						result = Hot2gEnumerations.EnumHot2gResult.SystemError ;
					}
				}
				else*/
                {
					// 応答待ちなし
					ret = mBluetoothDriver.SendCommand( send_bytes, cc ) ;
					if( ret ){
						result = Hot2gEnumerations.EnumHot2gResult.Success ;
					}
				}
			}
		}


		return( result ) ;
	}
#if false

    /**
	 * Bluetooth接続用非同期処理
	 */
    private class AsyncSendBluetoothCommand implements Callable {
		private Hot2gEnumerations.EnumHot2gMsgID		id ;
		private byte[]				bytes ;

		private AsyncSendBluetoothCommand( Hot2gEnumerations.EnumHot2gMsgID id, byte[] bytes ) {
			this.id	 = id ;
			this.bytes = bytes ;
		}
		@Override
		public Object call() throws Exception {
			bool	result ;

			mNeedResponseMsgId = id ;
			mReceiveResponseF = false ;

			result = mBluetoothDriver.write( bytes ) ;
			if( result ) {
				while( ! mReceiveResponseF ) ;
			}

			return(result) ;
		}
	}

	/**
	 * Bluetooth接続用非同期処理
	 */
	private class AsyncSendUsbCommand implements Callable {
		private Hot2gEnumerations.EnumHot2gMsgID		id ;
		private byte[]				bytes ;

		private AsyncSendUsbCommand( Hot2gEnumerations.EnumHot2gMsgID id, byte[] bytes ) {
			this.id	 = id ;
			this.bytes = bytes ;
		}
		@Override
		public Object call(){
			bool	result ;

			mNeedResponseMsgId = id ;
			mReceiveResponseF = false ;

			result = mUsbSerialDriver.write( bytes ) ;
			if( result ) {
				while( ! mReceiveResponseF ) ;
			}

			return(result) ;
		}
	}
#endif
	//---------------------------------------------------------------------------------------------

	/**
	 * [00]機種情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand00_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceType, mCommands.Request00.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valDevType = mCommands.Report80.getDevType() ;
		}

		outputWorkLog( TAG, "sendCommand00_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [01]機器ID情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand01_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceID, mCommands.Request01.getReadReqCmd(), async_mode ) ;
		/*if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valDevId = mCommands.Report81.getDevId() ;
		}*/
		
		//outputWorkLog( TAG, "sendCommand01_Read: " + result.ToString() ) ;

		

		return( result ) ;
	}

	public void UpdateReport81()
	{
		Debug.Log("81:" + mCommands.Report81.getDevId() );
		hot2gDataManager.DevInfo.valDevId = mCommands.Report81.getDevId();
	}

	/**
	 * [02]機器名称 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand02_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceName, mCommands.Request02.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valDevName = mCommands.Report82.getDevName() ;
		}

		outputWorkLog( TAG, "sendCommand02_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [02]機器名称 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand02_Write( string device_name, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request02.createWriteCommand( device_name ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceName, mCommands.Request02.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valDevName = mCommands.Report82.getDevName() ;
		}

		outputWorkLog( TAG, "sendCommand02_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [03]バージョン情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand03_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceFirmVer, mCommands.Request03.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valDevFirmVersion = mCommands.Report83.getFwVersion() ;
		}

		outputWorkLog( TAG, "sendCommand03_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [04]HW情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand04_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceHwInfo, mCommands.Request04.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valHwADEnableBits = mCommands.Report84.getHwADEnableBits() ;
			hot2gDataManager.LocalInfo.valHwADVoltMin = mCommands.Report84.getHwADVoltMin() ;
			hot2gDataManager.LocalInfo.valHwADVoltMax = mCommands.Report84.getHwADVoltMax() ;
			hot2gDataManager.LocalInfo.valHwAccEnableBits = mCommands.Report84.getHwAccEnableBits() ;
			hot2gDataManager.LocalInfo.valHwGyroEnableBits = mCommands.Report84.getHwGyroEnableBits() ;
			hot2gDataManager.DevInfo.valVmaxPd = mCommands.Report84.getADDegitMax() ;
			hot2gDataManager.DevInfo.valResolutePd = mCommands.Report84.getResolutePd() ;
		}

		outputWorkLog( TAG, "sendCommand04_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [05]待機時IR情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand05_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_WaitIrInfo, mCommands.Request05.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valWaitIrSwGains = mCommands.Report85.getWaitIrSwGains() ;
			hot2gDataManager.LocalInfo.valWaitIrPmGains = mCommands.Report85.getWaitIrPmGains() ;
			hot2gDataManager.LocalInfo.valWaitIrIoOuts = mCommands.Report85.getWaitIrIoOuts() ;
			hot2gDataManager.LocalInfo.valWaitIrBrights = mCommands.Report85.getWaitIrBrights() ;
		}

		outputWorkLog( TAG, "sendCommand05_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [05]待機時IR情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand05_Write( int[] iSwGain, int[] iPmGain, int[] iOnOff, int[] iBright, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request05.createWriteCommand( iSwGain, iPmGain, iOnOff, iBright ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_WaitIrInfo, mCommands.Request05.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valWaitIrSwGains = mCommands.Report85.getWaitIrSwGains() ;
			hot2gDataManager.LocalInfo.valWaitIrPmGains = mCommands.Report85.getWaitIrPmGains() ;
			hot2gDataManager.LocalInfo.valWaitIrIoOuts = mCommands.Report85.getWaitIrIoOuts() ;
			hot2gDataManager.LocalInfo.valWaitIrBrights = mCommands.Report85.getWaitIrBrights() ;
		}

		outputWorkLog( TAG, "sendCommand05_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [06]モーション加速度情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand06_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionAcc, mCommands.Request06.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valRangeAcc = mCommands.Report86.getRangeAcc() ;
			hot2gDataManager.DevInfo.valResoluteAcc = mCommands.Report86.getResoluteAcc() ;
			hot2gDataManager.LocalInfo.valAccLPF = mCommands.Report86.getAccLPF() ;
			hot2gDataManager.LocalInfo.valAccODR = mCommands.Report86.getAccODR() ;
		}

		outputWorkLog( TAG, "sendCommand06_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [06]モーション加速度情報 設定要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand06_Write( Hot2gEnumerations.EnumHot2gAccRange AccRange, int AccLpf, int AccOdr, bool async_mode )
	{
		AccOdr = 4;

		Hot2gEnumerations.EnumHot2gResult	result ;

		Debug.Log("06W:" + AccRange.ToString() + "," + AccLpf + "," + AccOdr);

		mCommands.Request06.createWriteCommand( AccRange, AccLpf, AccOdr ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionAcc, mCommands.Request06.getWriteReqCmd(), async_mode, ()=>
		{
			//if (result == Hot2gEnumerations.EnumHot2gResult.Success)
			/*{
				hot2gDataManager.DevInfo.valRangeAcc = mCommands.Report86.getRangeAcc();
				hot2gDataManager.DevInfo.valResoluteAcc = mCommands.Report86.getResoluteAcc();
				hot2gDataManager.LocalInfo.valAccLPF = mCommands.Report86.getAccLPF();
				hot2gDataManager.LocalInfo.valAccODR = mCommands.Report86.getAccODR();
			}*/
		}) ;



		outputWorkLog( TAG, "sendCommand06_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	public void UpdateReport86()
	{
		Debug.Log("86:" + mCommands.Report86.getRangeAcc().ToString() + ","  + mCommands.Report86.getResoluteAcc() .ToString() +","+ mCommands.Report86.getAccLPF() + "," + mCommands.Report86.getAccODR());
		hot2gDataManager.DevInfo.valRangeAcc = mCommands.Report86.getRangeAcc();
		hot2gDataManager.DevInfo.valResoluteAcc = mCommands.Report86.getResoluteAcc();
		hot2gDataManager.LocalInfo.valAccLPF = mCommands.Report86.getAccLPF();
		hot2gDataManager.LocalInfo.valAccODR = mCommands.Report86.getAccODR();
	}

	/**
	 * [07]モーション角速度情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand07_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionGyro, mCommands.Request07.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valRangeGyro = mCommands.Report87.getRangeGyro() ;
			hot2gDataManager.DevInfo.valResoluteGyro = mCommands.Report87.getResoluteGyro() ;
			hot2gDataManager.LocalInfo.valGyroLPF = mCommands.Report87.getGyroLPF() ;
			hot2gDataManager.LocalInfo.valGyroODR = mCommands.Report87.getGyroODR() ;
		}

		outputWorkLog( TAG, "sendCommand07_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [07]モーション角速度情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand07_Write( Hot2gEnumerations.EnumHot2gGyroRange AngRange, int AngLpf, int AngOdr, bool async_mode )
	{
		AngOdr = 4;
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request07.createWriteCommand( AngRange, AngLpf, AngOdr ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionGyro, mCommands.Request07.getWriteReqCmd(), async_mode ) ;
		/*if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valRangeGyro = mCommands.Report87.getRangeGyro() ;
			hot2gDataManager.DevInfo.valResoluteGyro = mCommands.Report87.getResoluteGyro() ;
			hot2gDataManager.LocalInfo.valGyroLPF = mCommands.Report87.getGyroLPF() ;
			hot2gDataManager.LocalInfo.valGyroODR = mCommands.Report87.getGyroODR() ;
		}*/

		outputWorkLog( TAG, "sendCommand07_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	public void UpdateReport87()
	{
		hot2gDataManager.DevInfo.valRangeGyro = mCommands.Report87.getRangeGyro();
		hot2gDataManager.DevInfo.valResoluteGyro = mCommands.Report87.getResoluteGyro();
		hot2gDataManager.LocalInfo.valGyroLPF = mCommands.Report87.getGyroLPF();
		hot2gDataManager.LocalInfo.valGyroODR = mCommands.Report87.getGyroODR();
	}

	/**
	 * [08]バッテリー情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand08_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceBattery, mCommands.Request08.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valBattCapacity = mCommands.Report88.getBattCapacity() ;
		}

		outputWorkLog( TAG, "sendCommand08_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [08]バッテリー情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand08_Write( int Battery, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request08.createWriteCommand( Battery ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DeviceBattery, mCommands.Request08.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valBattCapacity = mCommands.Report88.getBattCapacity() ;
		}

		outputWorkLog( TAG, "sendCommand08_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [09]計測モード情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand09_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasSampling, mCommands.Request09.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valSmplRateBatt = mCommands.Report89.getSmplRateBatt() ;
			hot2gDataManager.DevInfo.valSmplRateMot = mCommands.Report89.getSmplRateMot() ;
		}

		outputWorkLog( TAG, "sendCommand09_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [09]計測モード情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand09_Write( Hot2gEnumerations.EnumHot2gMotSmplRate motion_sampling, Hot2gEnumerations.EnumHot2gBattSmplRate battery_sampling, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request09.createWriteCommand( motion_sampling, battery_sampling ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasSampling, mCommands.Request09.getWriteReqCmd(), async_mode ) ;
		/*if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valSmplRateBatt = mCommands.Report89.getSmplRateBatt() ;
			hot2gDataManager.DevInfo.valSmplRateMot = mCommands.Report89.getSmplRateMot() ;
		}*/

		outputWorkLog( TAG, "sendCommand09_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	public void UpdateReport89()
	{
		hot2gDataManager.DevInfo.valSmplRateBatt = mCommands.Report89.getSmplRateBatt();
		hot2gDataManager.DevInfo.valSmplRateMot = mCommands.Report89.getSmplRateMot();
	}


	/**
	 * [0A]計測転送モード情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0A_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasTransMode, mCommands.Request0A.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valTransRateMode = mCommands.Report8A.getTransRateMode() ;
			hot2gDataManager.LocalInfo.valTransRatePD = mCommands.Report8A.getTransRatePD() ;
			hot2gDataManager.DevInfo.valTransRateBatt = mCommands.Report8A.getTransRateBatt() ;
			hot2gDataManager.DevInfo.valTransRateMot = mCommands.Report8A.getTransRateMot() ;
			hot2gDataManager.DevInfo.valTransRateHR = mCommands.Report8A.getTransRateHR() ;
		}

		outputWorkLog( TAG, "sendCommand0A_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [0A]計測転送モード情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0A_Write( int TransMethod, int PDMeasure, Hot2gEnumerations.EnumHot2gMotTransRate MotionMeasure, Hot2gEnumerations.EnumHot2gBattTransRate BatteryMeasure, Hot2gEnumerations.EnumHot2gHRTransRate HRTransfer, bool async_mode )
	{
		PDMeasure = 4;


		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request0A.createWriteCommand( TransMethod, PDMeasure, MotionMeasure, BatteryMeasure, HRTransfer ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasTransMode, mCommands.Request0A.getWriteReqCmd(), async_mode ) ;
		/*if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valTransRateMode = mCommands.Report8A.getTransRateMode() ;
			hot2gDataManager.LocalInfo.valTransRatePD = mCommands.Report8A.getTransRatePD() ;
			hot2gDataManager.DevInfo.valTransRateBatt = mCommands.Report8A.getTransRateBatt() ;
			hot2gDataManager.DevInfo.valTransRateMot = mCommands.Report8A.getTransRateMot() ;
			hot2gDataManager.DevInfo.valTransRateHR = mCommands.Report8A.getTransRateHR() ;
		}*/

		outputWorkLog( TAG, "sendCommand0A_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	public void UpdateReport8A()
	{
		//if (result == Hot2gEnumerations.EnumHot2gResult.Success)
		{
			hot2gDataManager.LocalInfo.valTransRateMode = mCommands.Report8A.getTransRateMode();
			hot2gDataManager.LocalInfo.valTransRatePD = mCommands.Report8A.getTransRatePD();
			hot2gDataManager.DevInfo.valTransRateBatt = mCommands.Report8A.getTransRateBatt();
			hot2gDataManager.DevInfo.valTransRateMot = mCommands.Report8A.getTransRateMot();
			hot2gDataManager.DevInfo.valTransRateHR = mCommands.Report8A.getTransRateHR();
		}
	}


	/**
	 * [0B]計測解析モード情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0B_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasAnalysisMode, mCommands.Request0B.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valAnalysisHR = mCommands.Report8B.getAnalysisHR() ;
			hot2gDataManager.LocalInfo.valAnalysisAngle = mCommands.Report8B.getAnalysisHR() ;
		}

		outputWorkLog( TAG, "sendCommand0B_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [0B]計測解析モード情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0B_Write( int HRAnalysis, int AngleAnalysis, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request0B.createWriteCommand( HRAnalysis, AngleAnalysis ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MeasAnalysisMode, mCommands.Request0B.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valAnalysisHR = mCommands.Report8B.getAnalysisHR() ;
			hot2gDataManager.LocalInfo.valAnalysisAngle = mCommands.Report8B.getAnalysisHR() ;
		}

		outputWorkLog( TAG, "sendCommand0B_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [0C]デバック情報 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0C_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DebugFlag, mCommands.Request0C.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valEnabledOutputUsb = mCommands.Report8C.getEnabledOutputUsb() ;
		}

		outputWorkLog( TAG, "sendCommand0C_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [0C]デバック情報 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0C_Write( int enabled_output_usb, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request0C.createWriteCommand( enabled_output_usb ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_DebugFlag, mCommands.Request0C.getWriteReqCmd(), async_mode ) ;
		/*if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valEnabledOutputUsb = mCommands.Report8C.getEnabledOutputUsb() ;
		}*/

		outputWorkLog( TAG, "sendCommand0C_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	public void UpdateReport8C()
	{
		hot2gDataManager.DevInfo.valEnabledOutputUsb = mCommands.Report8C.getEnabledOutputUsb();
	}

	/**
	 * [0D]ロットID 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand0D_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_LotID, mCommands.Request0D.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevInfo.valLotId = mCommands.Report8D.getLotId() ;
		}

		outputWorkLog( TAG, "sendCommand0D_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [20]診断状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand20_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_ErrorCode, mCommands.Request20.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valErrorCode = mCommands.ReportA0.getStsErrorCode() ;
		}

		outputWorkLog( TAG, "sendCommand20_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [21]電池残量状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand21_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryGauge, mCommands.Request21.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valBattGauge = mCommands.ReportA1.getBattGauge() ;
			hot2gDataManager.DevState.valConvBattGauge = mCommands.ReportA1.getConvBattGauge() ;
		}

		outputWorkLog( TAG, "sendCommand21_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [22]電池電圧状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand22_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryVolt, mCommands.Request22.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valBattVolt = mCommands.ReportA2.getBattVolt() ;
			hot2gDataManager.DevState.valConvBattVolt = mCommands.ReportA2.getConvBattVolt() ;
		}

		outputWorkLog( TAG, "sendCommand22_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [23]温度状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand23_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_Temperature, mCommands.Request23.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valDevTemps = mCommands.ReportA3.getDevTemps() ;
			hot2gDataManager.DevState.valConvDevTemps = mCommands.ReportA3.getConvDevTemps() ;
		}

		outputWorkLog( TAG, "sendCommand23_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [24]IR光源出力状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand24_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_IrLightOutput, mCommands.Request24.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valStsIrIoOuts = mCommands.ReportA4.getStsIrIoOuts() ;
			hot2gDataManager.LocalInfo.valStsIrBrights = mCommands.ReportA4.getStsIrBrights() ;
		}

		outputWorkLog( TAG, "sendCommand24_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [25]USB検出状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand25_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_UsbState, mCommands.Request25.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledUsbVbus = mCommands.ReportA5.getEnabledUsbVbus() ;
			hot2gDataManager.DevState.valEnabledUsbComm = mCommands.ReportA5.getEnabledUsbComm() ;
		}

		outputWorkLog( TAG, "sendCommand25_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [26]BLE検出状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand26_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_BluetoothState, mCommands.Request26.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledBtModule = mCommands.ReportA6.getEnabledBtModule() ;
			hot2gDataManager.DevState.valEnabledBtComm = mCommands.ReportA6.getEnabledBtComm() ;
		}

		outputWorkLog( TAG, "sendCommand26_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [27]センサモジュール検出状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand27_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_SensorBoardEnabled, mCommands.Request27.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledSensModule = mCommands.ReportA7.getEnabledSensModule() ;
		}

		outputWorkLog( TAG, "sendCommand27_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [28]PD検出状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand28_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_PDEnabled, mCommands.Request28.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledPdLs = mCommands.ReportA8.getEnabledPdLs() ;
			hot2gDataManager.DevState.valEnabledPdRs = mCommands.ReportA8.getEnabledPdRs() ;
		}

		outputWorkLog( TAG, "sendCommand28_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [29]モーションセンサ検出 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand29_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionEnabled, mCommands.Request29.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledMotion = mCommands.ReportA9.getEnabledMotion() ;
		}

		outputWorkLog( TAG, "sendCommand29_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [2A]残量ゲージセンサ検出状態 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand2A_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_BatteryEnabled, mCommands.Request2A.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.DevState.valEnabledGauge = mCommands.ReportAA.getEnabledGauge() ;
		}

		outputWorkLog( TAG, "sendCommand2A_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [40]自動計測開始 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand40_Write( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		hot2gDataManager.LocalInfo.valFirstBloodFlag = true ;
		hot2gDataManager.LocalInfo.valFirstBloodData = new double[4] ;

		mCommands.Request40.createWriteCommand() ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoMeasStart, mCommands.Request40.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			mMeasureActivatedF = true ;
			hot2gDataManager.LocalInfo.valCtrlAutoMeasStartResult = mCommands.ReportC0.getProcessResult() ;
		}

		outputWorkLog( TAG, "sendCommand40_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [41]自動計測停止 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand41_Write( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request41.createWriteCommand() ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoMeasStop, mCommands.Request41.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			mMeasureActivatedF = false ;
			hot2gDataManager.LocalInfo.valCtrlAutoMeasStopResult = mCommands.ReportC1.getProcessResult() ;
		}

		outputWorkLog( TAG, "sendCommand41_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [42]自動PDゲイン調整開始 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand42_Write( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request42.createWriteCommand() ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoGainStart, mCommands.Request42.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			mAutoGainActivatedF = true ;
			hot2gDataManager.LocalInfo.valCtrlAutoGainStartResult = mCommands.ReportC2.getProcessResult() ;
		}

		outputWorkLog( TAG, "sendCommand42_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [43]自動PDゲイン調整停止 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand43_Write( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request43.createWriteCommand() ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_CtrlAutoGainStop, mCommands.Request43.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			mAutoGainActivatedF = false ;
			hot2gDataManager.LocalInfo.valCtrlAutoGainStopResult = mCommands.ReportC3.getProcessResult() ;
		}

		outputWorkLog( TAG, "sendCommand43_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [44]手動PDゲイン設定 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand44_Write( Hot2gEnumerations.EnumHot2gMgcType UpdateClass, int[] iSwGain, int[] iPmGain, int[] iBright, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request44.createWriteCommand( UpdateClass, iSwGain, iPmGain, iBright ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_PDManuGainIr, mCommands.Request44.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valCtrlManuGainResult = mCommands.ReportC4.getProcessResult() ;
		}

		outputWorkLog( TAG, "sendCommand44_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [45]現在PD値入力 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand45_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_PDValueInput, mCommands.Request45.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valCtrlPDResult = mCommands.ReportC5.getProcessResult() ;
			hot2gDataManager.LocalInfo.valCtrlPDValues = mCommands.ReportC5.getCtrlPDValues() ;
		}

		outputWorkLog( TAG, "sendCommand45_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [46]IR光源出力設定 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand46_Write( int[] iOnOff, int[] iBright, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.Request46.createWriteCommand( iOnOff, iBright ) ;
		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_IrOutputEstablish, mCommands.Request46.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valCtrlManuGainResult = mCommands.ReportC6.getProcessResult() ;
			hot2gDataManager.LocalInfo.valCtrlIrOutIos = mCommands.ReportC6.getCtrlIrOutIos() ;
			hot2gDataManager.LocalInfo.valCtrlIrOutBrights = mCommands.ReportC6.getCtrlIrOutBrights() ;
		}

		outputWorkLog( TAG, "sendCommand46_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [47]現在モーション値入力 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand47_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionValueInput, mCommands.Request47.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valCtrlMotionResult = mCommands.ReportC7.getProcessResult() ;
			hot2gDataManager.LocalInfo.valCtrlAccValues = mCommands.ReportC7.getCtrlAccValues() ;
			hot2gDataManager.LocalInfo.valCtrlGyroValues = mCommands.ReportC7.getCtrlGyroValues() ;
			hot2gDataManager.LocalInfo.valCtrlConvAccValues = mCommands.ReportC7.getCtrlConvAccValues() ;
			hot2gDataManager.LocalInfo.valCtrlConvGyroValues = mCommands.ReportC7.getCtrlConvGyroValues() ;
		}

		outputWorkLog( TAG, "sendCommand47_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [48]現在電池状態入力 取得要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommand48_Read( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		result = sendHot2gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_MotionValueInput, mCommands.Request48.getReadReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ) {
			hot2gDataManager.LocalInfo.valCtrlBatteryResult = mCommands.ReportC8.getProcessResult() ;
			hot2gDataManager.DevState.valBattGauge = mCommands.ReportC8.getBattGauge() ;
			hot2gDataManager.DevState.valBattVolt = mCommands.ReportC8.getBattVolt() ;
			hot2gDataManager.DevState.valConvBattGauge = mCommands.ReportC8.getConvBattGauge() ;
			hot2gDataManager.DevState.valConvBattVolt = mCommands.ReportC8.getConvBattVolt() ;
		}

		outputWorkLog( TAG, "sendCommand48_Read: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [ATC]プローブパラメータ設定 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommandATC_Write( Hot2gEnumerations.EnumHot2gMgcType mode, int[] pm_gain, int[] sw_gain, int[] bright, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.RequestATC.createWriteCommand( mode, pm_gain, sw_gain, bright ) ;
		result = sendHot1gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.Report_1g_ProbeParam, mCommands.RequestATC.getWriteReqCmd(), async_mode ) ;

		outputWorkLog( TAG, "sendCommandATC_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [ATR]測定データ要求開始 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommandATR_Write( int mode, bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		hot2gDataManager.LocalInfo.valFirstBloodFlag = true ;
		hot2gDataManager.LocalInfo.valFirstBloodData = new double[4] ;

		mCommands.RequestATR.createWriteCommand( mode ) ;
		result = sendHot1gRequestCommand( Hot2gEnumerations.EnumHot2gMsgID.None, mCommands.RequestATR.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ){
			mMeasureActivatedF = true ;
		}

		outputWorkLog( TAG, "sendCommandATR_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * [ATS]測定データ要求停止 更新要求
	 */
	public Hot2gEnumerations.EnumHot2gResult sendCommandATS_Write( bool async_mode )
	{
		Hot2gEnumerations.EnumHot2gResult	result ;

		mCommands.RequestATS.createWriteCommand() ;
		result = sendHot1gRequestCommand(Hot2gEnumerations.EnumHot2gMsgID.None, mCommands.RequestATS.getWriteReqCmd(), async_mode ) ;
		if( result == Hot2gEnumerations.EnumHot2gResult.Success ){
			mMeasureActivatedF = false ;
		}

		outputWorkLog( TAG, "sendCommandATS_Write: " + result.ToString() ) ;

		return( result ) ;
	}

	/**
	 * 稼働ログ出力
	 * @param tag		:稼働タグ
	 * @param message	:テキスト
	 */
	private void outputWorkLog( string tag, string message )
	{
        Debug.Log(TAG + "\n" + message);
		//Log.d( TAG, message ) ;

		/*if( this.mOutputLogEnabled )
        {
			mWorkLog.write( tag, message ) ;
		}*/
	}

	/**
	 * 稼働ログ出力
	 * @param tag		:稼働タグ
	 * @param message	:テキスト
	 */
	private void outputWorkLog( string tag, string message, System.Exception ex )
	{
        Debug.Log(TAG + "\n" + message + "\n" + ex.ToString() );
        /*   
		Log.d( TAG, message, ex ) ;

		if( this.mOutputLogEnabled ){
			mWorkLog.write( tag, message + ": " + ex.getMessage() ) ;
		}
        */
    }

	/**
	 * 通信ログ出力
	 * @param comm_type	:通信ドライバ名称
	 * @param direction	:通信方向
	 * @param bytes		:通信メッセージRawデータ
	 */
	private void outputCommLog( string comm_type, string direction, byte[] bytes )
	{
		if( this.mOutputLogEnabled )
        {
            
            string dump = CommonFunctions.getHexDump( bytes, " " ) ;
            //mCommLog.write( comm_type, direction, dump ) ;
            Debug.Log(comm_type + "\n" + direction + "\n" + dump);
        }
	}

	/**
	 * 通信ログ出力
	 * @param comm_type	:通信ドライバ名称
	 * @param direction	:通信方向
	 * @param bytes		:通信メッセージRawデータ
	 */
	private void outputCommLog( string comm_type, string direction, byte[] bytes, int size )
	{
		if( this.mOutputLogEnabled ){
			string dump = CommonFunctions.getHexDump( bytes, size, " " ) ;
            //mCommLog.write( comm_type, direction, dump ) ;
            Debug.Log(comm_type + "\n" + direction + "\n" + dump);
        }
	}
}
