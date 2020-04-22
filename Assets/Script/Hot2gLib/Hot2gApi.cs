using System.Collections.Generic;
using UnityEngine;

/**
 * HOT-2G通信インターフェース管理クラス
 */
public class Hot2gApi
{
    public delegate void MeasureDataEventHandler(Hot2gEnumerations.EnumHot2gMeasResult e, object meas_data, string device_address );
    public delegate void CommStsChangedEventHandler(Hot2gEnumerations.EnumHot2gCommStsChangedEvent e, string device_address);

    private readonly string TAG = "Hot2gApi";

	private static string mApiVersion = "181214.1";
	public static string getApiVersion() 
	{
		return mApiVersion;
	}
    /*
	private static BluetoothManager	gBluetoothManager ;
	public static BluetoothManager  getBluetoothManager()	{ return( gBluetoothManager ) ; }
    */

	private MeasureDataEventHandler			mMeasDataEvent ;				// 計測データ通知イベントハンドラ
	//private BluetoothSearchReportEventHandler	mBtSearchReportEvent ;		// Bluetooth検索通知イベントハンドラ
	private CommStsChangedEventHandler			mCommStsChangeEvent ;			// 接続状態変化通知イベントハンドラ		// 18.10.05
    

	private bool				mApiEnabled ;
	public bool IsApiEnabled() { return(this.mApiEnabled); }

	// 1台通信機器
	private Hot2gInterface			mHot2gIf ;
	public Hot2gInterface getHot2gIf() {
		return mHot2gIf;
	}
	public string getConnectedAddress() { return(this.mHot2gIf.getConnectedAddress()); }
	public Hot2gEnumerations.EnumHot2gCommType getCommType() { return(this.mHot2gIf.getCommType()); }
	public bool IsBtConnected() { return(this.mHot2gIf.isBtConnected()); }
	public bool IsMeasuring() { return(this.mHot2gIf.isMeasuring()); }
	public bool IsAGC() { return(this.mHot2gIf.isAGC()); }
	public bool IsBtServiceConnected()	{ return( this.mHot2gIf.isBtServiceConnected() ) ; }

	//public ServiceConnection getServiceConnection()	 { return( this.mHot2gIf.getServiceConnection() ) ; }
	//public BroadcastReceiver getBroadcastReceiver()	 { return( this.mHot2gIf.getBluetooth().getDeviceFoundReceiver() ) ; }
	//public ArrayList<BluetoothDevice> getBluetoothKnownDevices() { return( this.mHot2gIf.getBluetooth().getScanResultDevices() ); }


	// 複数台通信機器
	private List<Hot2gInterface>	mHot2gIfList ;
	public string getConnectedAddress(int index) { return(this.mHot2gIfList[index].getConnectedAddress()); }
	public Hot2gEnumerations.EnumHot2gCommType getCommType(int index) { return Hot2gEnumerations.EnumHot2gCommType.Bluetooth; }
	public bool IsBtConnected(int index) { return(this.mHot2gIfList[index].isBtConnected()); }

	public bool IsMeasuring(int index) { return(this.mHot2gIfList[index].isMeasuring()); }
	public bool IsAGC(int index) { return(this.mHot2gIfList[index].isAGC()); }

	private Hot2gGainData.ParamGain oldParamGain = null;		// ParamGainクラスの前回値
	private Hot2gGainData.ParamLed oldParamLed  = null;		// ParamLedクラスの前回値

	private const int TIMEOUT_CONNECT = 10 ;				// 接続タイムアウト [seconds]
	private const int TIMEOUT_DISCONNECT = 3 ;			// 切断タイムアウト [seconds]
	private const int TIMEOUT_USB_CONNECT = 1 ;				// 接続タイムアウト [seconds]
	private const int TIMEOUT_USB_DISCONNECT = 1 ;			// 切断タイムアウト [seconds]

	/**
	 * コンストラクタ
	 */
	public Hot2gApi()
	{
		this.mApiEnabled = false ;
		this.mHot2gIfList = new List<Hot2gInterface>() ;

		this.mHot2gIf = new Hot2gInterface(onDeviceMeasDataReport, onCommStatusChanged);
        this.mHot2gIfList.Add(this.mHot2gIf);
        /*
		setCallbackEvents( new IHot2gIfCallBack() {
			@Override

			@Override
			public void onBtSearchReport(final Hot2gEnumerations.EnumHot2gWatcherEvent event, BluetoothDevice device) {
				Log.d( TAG, "onBtSearchReport :" + event.tostring() ) ;
				switch( event ){
					case Added:
					case Updated:
						break ;
				}
				if( mBtSearchReportEvent != null ){
					mBtSearchReportEvent.onBluetoothSearchReportEvent( event, device );
				}
			}

			@Override
			public void onCommStatusChanged( Hot2gEnumerations.EnumHot2gCommStsChangedEvent event, string device_address ) {
				Log.d( TAG, "onCommStatusChanged :" + event.tostring() ) ;
				switch( event ){
					case Disconnected:
						int index = getHot2gIfListIndex( device_address ) ;
						if( index != -1 )	mHot2gIfList.remove( index ) ;
						break ;
					case Connected:
						break ;
				}
				if( mCommStsChangeEvent != null ){
					mCommStsChangeEvent.onCommStsChangedEvent( event, device_address );
				}
			}
		}) ;*/
    }

    //---------------------------------------------------------------------------------------------
    /**
	 * 計測データ通知イベントハンドラ登録
	 * @param events
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
    public void setMeasureDataEventHandler(MeasureDataEventHandler events)
    {
        mMeasDataEvent = events;
	}

    public void onDeviceMeasDataReport(Hot2gEnumerations.EnumHot2gMeasResult e, object meas_data, string device_address)
    {
        Debug.Log(TAG + "\nonDeviceMeasDataReport :" + e.ToString());
        switch (e)
        {
            case Hot2gEnumerations.EnumHot2gMeasResult.Ac1ch:
            case Hot2gEnumerations.EnumHot2gMeasResult.Ac2ch:
            case Hot2gEnumerations.EnumHot2gMeasResult.Motion:
            case Hot2gEnumerations.EnumHot2gMeasResult.Battery:
            case Hot2gEnumerations.EnumHot2gMeasResult.MotionBatt:
            case Hot2gEnumerations.EnumHot2gMeasResult.HeartRate:
            case Hot2gEnumerations.EnumHot2gMeasResult.HRElement1:          // 18.10.11 ohara
            case Hot2gEnumerations.EnumHot2gMeasResult.HRElement2:            // 18.10.11 ohara
                if (mMeasDataEvent != null)
                {
                    mMeasDataEvent(e, meas_data, device_address);
                }
            break;
        }
    }

    /**
	 * 接続状態変化通知イベントハンドラ登録
	 * @param events			:説ぞ状態変化通知イベント
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
    public void setCommStsChangedEventHandler(CommStsChangedEventHandler events)
    {
        mCommStsChangeEvent = events;
	}

    public void onCommStatusChanged(Hot2gEnumerations.EnumHot2gCommStsChangedEvent e, string device_address ) 
    {
        Debug.Log(TAG + "onCommStatusChanged :" + e.ToString() ) ;

        switch ( e )
        {
		    case Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Disconnected:
            {
                /*int index = getHot2gIfListIndex(device_address);
                if (index != -1)
                    mHot2gIfList.RemoveAt(index);*/
            }
            break;
			case Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Connected:
            break;
        }
		if(mCommStsChangeEvent != null )
        {
		    mCommStsChangeEvent( e, device_address );
        }
    }


    //---------------------------------------------------------------------------------------------

    /**
     * API初期化
     * @param manager		:Bluetoothマネージャー
     * @param log_flag	:デバッグ用ログ出力有無 [T/F]
     * @return				:処理結果[Hot2gEnumerations.EnumHot2gResult]
     * @throws Hot2gException	:ドライバ上の例外発生
     */
    public Hot2gEnumerations.EnumHot2gResult initAPI( bool log_flag )
	{
		this.mHot2gIf.setOutputLogEnabled( log_flag ) ;

		mApiEnabled = true ;
		return Hot2gEnumerations.EnumHot2gResult.Success ;
	}


	/**
	 * API終了
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public Hot2gEnumerations.EnumHot2gResult exitAPI()
	{
		Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;

		try{
			if( mHot2gIfList.Count > 0 ){
				for( int i=0; i<mHot2gIfList.Count; i++ )
                {
                    Hot2gInterface hot2g = mHot2gIfList[i];

                    string	address = hot2g.getConnectedAddress() ;
					if( hot2g.getCommType() == Hot2gEnumerations.EnumHot2gCommType.Bluetooth )
                    {
						disconnectBtDevice( address ) ;
					}
				}
			}

			mApiEnabled = false ;
			result = Hot2gEnumerations.EnumHot2gResult.Success ;
		}
		catch( System.Exception ex )
        {
			//throw new Hot2gException( ex ) ;
		}

		return( result ) ;
	}

	/**
	 * Bluetoothペアリング済み機器検索
	 * @param keyword :検索文字列
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void searchBtDevice( string keyword, int search_span )
	{

	}

	/**
	 * Bluetoothペアリング済み機器検索
	 * @param keyword :検索文字列
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void searchBondedBtDevice( string keyword )
	{

	}

	public void scanBtDevice(BLEMgr.Callback Success, BLEMgr.Callback Error)
	{
		mHot2gIf.scanBtDevice(Success, Error);
	}


	public List<string> getScanDeviceList()
	{
		return mHot2gIf.getScanDeviceList();
	}

	/**
	 * Bluetooth接続
	 * @param address :接続先機器アドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void connectBtDevice( string Address, BLEMgr.Callback Success, BLEMgr.Callback Error)
    {
        mHot2gIf.connectBtDevice( Address, Success, Error);
        /*
		int	index = getHot2gIfListIndex( address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;
			final Hot2gInterface	hot2gif ;
			// Bluetooth接続
			if( index == -1 )		hot2gif = mHot2gIf ;
			else					hot2gif = mHot2gIfList[index] ;

			result = hot2gif.connectBtDevice( address, TIMEOUT_CONNECT ) ;
			if( result == Hot2gEnumerations.EnumHot2gResult.Success ){
				if( index == -1 ){
					mHot2gIfList.add( hot2gif ) ;
				}
			}
			else	return( result ) ;

			try{
				// デバイス情報をすべて取得
				result = getDeviceInfo( address, Hot2gEnumerations.EnumHot2gDevInfo.All ).get() ;
				if( result == Hot2gEnumerations.EnumHot2gResult.Success ){
					// デバイス情報を取得出来たら通信接続完了とみなす
					if( mCommStsChangeEvent != null ){
						mCommStsChangeEvent.onCommStsChangedEvent( Hot2gEnumerations.EnumHot2gCommStsChangedEvent.Connected, address ) ;
					}
				}
				if( result != Hot2gEnumerations.EnumHot2gResult.Success ){
					disconnectBtDevice( address ) ;
				}
			}
			catch( Exception e ){
				result = Hot2gEnumerations.EnumHot2gResult.SystemError ;
			}

			return( result ) ;
		}) ;

		return( cf ) ;*/

    }

	/**
	 * Bluetooth切断
	 * @param address :接続先機器アドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void disconnectBtDevice( string address )
	{
        /*
		int	index = getHot2gIfListIndex( address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;
			final Hot2gInterface	hot2gif ;
			// Bluetooth接続
			if( index == -1 )		hot2gif = mHot2gIf ;
			else					hot2gif = mHot2gIfList[index] ;

			result = hot2gif.disconnectBtDevice( TIMEOUT_DISCONNECT ) ;
			if( result == Hot2gEnumerations.EnumHot2gResult.Success ){
				if( index != -1 ){
					mHot2gIfList.remove( hot2gif ) ;
				}
			}

			return( result ) ;
		}) ;

		return( cf ) ;*/
	}


	/**
	 * デバイス情報取得
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void getDeviceInfo( Hot2gEnumerations.EnumHot2gDevInfo type )
	{
		getDeviceInfo(0, type);
		/*
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( getDeviceInfo( 0, type ) ) ;
		}) ;

		return( cf ) ;*/

	}

	/**
	 * デバイス情報取得
	 * @param dev_address :デバイスアドレス
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void getDeviceInfo( string dev_address, Hot2gEnumerations.EnumHot2gDevInfo type ) 
	{
        /*
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( getDeviceInfo( index, type ) ) ;
		}) ;

		return( cf ) ;*/
        
	}

	/**
	 * デバイス情報取得
	 * @param index :通信対象の通信IFリスト番号
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult getDeviceInfo( int index, Hot2gEnumerations.EnumHot2gDevInfo type )
	{
		Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;

		/*if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.DevType )){
			result = mHot2gIfList[index].sendCommand00_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}*/
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.DevId )){
			result = mHot2gIfList[index].sendCommand01_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}/*
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.DevName )){
			result = mHot2gIfList[index].sendCommand02_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.LotId )){
			result = mHot2gIfList[index].sendCommand0D_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.PDResolution )){
			result = mHot2gIfList[index].sendCommand04_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.MotAccRange )){
			result = mHot2gIfList[index].sendCommand06_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.MotGyroRange )){
			result = mHot2gIfList[index].sendCommand07_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.SamplingRate )){
			result = mHot2gIfList[index].sendCommand09_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.TransRate )){
			result = mHot2gIfList[index].sendCommand0A_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.AnalysisMode )){
			result = mHot2gIfList[index].sendCommand0B_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.UsbRTOutput )){
			result = mHot2gIfList[index].sendCommand0C_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevInfo.All ) || ( type == Hot2gEnumerations.EnumHot2gDevInfo.Version )){
			result = mHot2gIfList[index].sendCommand03_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}*/

		return( Hot2gEnumerations.EnumHot2gResult.Success ) ;
	}

	/**
	 * デバイス状態取得
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void getDeviceState( Hot2gEnumerations.EnumHot2gDevState type )
	{
        /*
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( getDeviceState( 0, type ) ) ;
		}) ;

		return( cf ) ;*/
        
	}

	/**
	 * デバイス状態取得
	 * @param dev_address :デバイスアドレス
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void getDeviceState( string dev_address, Hot2gEnumerations.EnumHot2gDevState type )
	{
        /*
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( getDeviceState( index, type ) ) ;
		}) ;

		return( cf ) ;*/
        
	}

	/**
	 * デバイス状態取得
	 * @param index :通信対象の通信IFリスト番号
	 * @param type :デバイス情報区分
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult getDeviceState( int index, Hot2gEnumerations.EnumHot2gDevState type )
	{
		Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;

		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.ErrorStatus )){
			result = mHot2gIfList[index].sendCommand20_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.BattGauge )){
			result = mHot2gIfList[index].sendCommand21_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.BattVolt )){
			result = mHot2gIfList[index].sendCommand22_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.DevTemp )){
			result = mHot2gIfList[index].sendCommand23_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.UsbState )){
			result = mHot2gIfList[index].sendCommand25_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.BtState )){
			result = mHot2gIfList[index].sendCommand26_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.SensModuleState )){
			result = mHot2gIfList[index].sendCommand27_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.PDState )){
			result = mHot2gIfList[index].sendCommand28_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.MotState )){
			result = mHot2gIfList[index].sendCommand29_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}
		if(( type == Hot2gEnumerations.EnumHot2gDevState.All ) || ( type == Hot2gEnumerations.EnumHot2gDevState.BattState )){
			result = mHot2gIfList[index].sendCommand2A_Read( true ) ;
			if( result != Hot2gEnumerations.EnumHot2gResult.Success )	return( result ) ;
		}

		return( Hot2gEnumerations.EnumHot2gResult.Success ) ;
	}

	/**
	 * デバイス機器名称更新
	 * @param device_name :機器名称(最大12文字)
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setDeviceName( string device_name )
	{
        /*
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setDeviceName( 0, device_name ) ) ;
		}) ;
        
		return( cf ) ;*/
    }

    /**
	 * デバイス機器名称更新
	 * @param dev_address :デバイスアドレス
	 * @param device_name :機器名称(最大12文字)
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
    public void setDeviceName( string dev_address, string device_name )
    {
		/*int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setDeviceName( index, device_name ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * デバイス機器名称更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param device_name :機器名称(最大12文字)
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setDeviceName( int index, string device_name )
	{
		return( mHot2gIfList[index].sendCommand02_Write( device_name, true ) ) ;
	}

	/**
	 * モーション加速度レンジ更新
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionAccRange( Hot2gEnumerations.EnumHot2gAccRange range )
	{
		setMotionAccRange(0, range);
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionAccRange( 0, range ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * モーション加速度レンジ更新
	 * @param dev_address :デバイスアドレス
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionAccRange( string dev_address, Hot2gEnumerations.EnumHot2gAccRange range )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionAccRange( index, range ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * モーション加速度レンジ更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMotionAccRange( int index, Hot2gEnumerations.EnumHot2gAccRange range )
	{
		Hot2gLocalInfo	info = mHot2gIfList[index].getHot2gLocalInfo() ;
		int	lpf = info.valAccLPF ;
		int	odr = info.valAccODR ;

		return( mHot2gIfList[index].sendCommand06_Write( range, lpf, odr, true ) ) ;
	}

	/**
	 * モーション角速度レンジ更新
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionGyroRange( Hot2gEnumerations.EnumHot2gGyroRange range )/* throws Hot2gException */
	{
		setMotionGyroRange(0, range);
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionGyroRange( 0, range ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * モーション角速度レンジ更新
	 * @param dev_address :デバイスアドレス
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionGyroRange( string dev_address, Hot2gEnumerations.EnumHot2gGyroRange range )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionGyroRange( index, range ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * モーション角速度レンジ更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param range :モーション加速度レンジ
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMotionGyroRange( int index, Hot2gEnumerations.EnumHot2gGyroRange range )
	{
		Hot2gLocalInfo	localinfo = mHot2gIfList[index].getHot2gLocalInfo() ;

		int	lpf = localinfo.valGyroLPF ;
		int	odr = localinfo.valGyroODR ;

		return( mHot2gIfList[index].sendCommand07_Write( range, lpf, odr, true ) ) ;
	}

	/**
	 * バッテリー収集情報更新
	 * @param trans_rate :バッテリー転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setBatterySamplingInfo( Hot2gEnumerations.EnumHot2gBattTransRate trans_rate )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setBatterySamplingInfo( 0, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
		setBatterySamplingInfo(0, trans_rate);
	}

	/**
	 * バッテリー収集情報更新
	 * @param dev_address :デバイスアドレス
	 * @param trans_rate :バッテリー転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setBatterySamplingInfo(
			string dev_address,
			Hot2gEnumerations.EnumHot2gBattTransRate trans_rate )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setBatterySamplingInfo( index, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * バッテリー収集情報更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param trans_rate :バッテリー転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setBatterySamplingInfo(
			int index,
			Hot2gEnumerations.EnumHot2gBattTransRate trans_rate )
	{
		Hot2gDevInfo	devinfo = mHot2gIfList[index].getHot2gDevInfo() ;
		Hot2gLocalInfo	localinfo = mHot2gIfList[index].getHot2gLocalInfo() ;

		return( mHot2gIfList[index].sendCommand0A_Write(
				localinfo.valTransRateMode,
				localinfo.valTransRatePD,
				devinfo.valTransRateMot,
				trans_rate,
				devinfo.valTransRateHR,
				true ) ) ;
	}

	public void ReadA10()
	{
		mHot2gIfList[0].sendCommand0A_Read(true);
	}

	/**
	 * モーション収集情報更新
	 * @param sampling :モーションサンプリング
	 * @param trans_rate :モーション転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionSamplingInfo_1(
			Hot2gEnumerations.EnumHot2gMotSmplRate sampling,
			Hot2gEnumerations.EnumHot2gMotTransRate trans_rate )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionSamplingInfo( 0, sampling, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
		setMotionSamplingInfo_1(0, sampling, trans_rate);
	}

	public void setMotionSamplingInfo_2(
			Hot2gEnumerations.EnumHot2gMotSmplRate sampling,
			Hot2gEnumerations.EnumHot2gMotTransRate trans_rate)/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionSamplingInfo( 0, sampling, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
		setMotionSamplingInfo_2(0, sampling, trans_rate);
	}

	/**
	 * モーション収集情報更新
	 * @param dev_address :デバイスアドレス
	 * @param sampling :モーションサンプリング
	 * @param trans_rate :モーション転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMotionSamplingInfo(
			string dev_address,
			Hot2gEnumerations.EnumHot2gMotSmplRate sampling,
			Hot2gEnumerations.EnumHot2gMotTransRate trans_rate )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMotionSamplingInfo( index, sampling, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * モーション収集情報更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param sampling :モーションサンプリング
	 * @param trans_rate :モーション転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMotionSamplingInfo_1(
			int index,
			Hot2gEnumerations.EnumHot2gMotSmplRate sampling,
			Hot2gEnumerations.EnumHot2gMotTransRate trans_rate )
	{
		Hot2gEnumerations.EnumHot2gResult	result = Hot2gEnumerations.EnumHot2gResult.Failed ;

		Hot2gDevInfo	devinfo = mHot2gIfList[index].getHot2gDevInfo() ;
		Hot2gLocalInfo	localinfo = mHot2gIfList[index].getHot2gLocalInfo() ;

		result = mHot2gIfList[index].sendCommand09_Write(
				sampling,
				devinfo.valSmplRateBatt,
				true ) ;


		return( result ) ;
	}

	private Hot2gEnumerations.EnumHot2gResult setMotionSamplingInfo_2(
			int index,
			Hot2gEnumerations.EnumHot2gMotSmplRate sampling,
			Hot2gEnumerations.EnumHot2gMotTransRate trans_rate)
	{
		Hot2gEnumerations.EnumHot2gResult result = Hot2gEnumerations.EnumHot2gResult.Failed;

		Hot2gDevInfo devinfo = mHot2gIfList[index].getHot2gDevInfo();
		Hot2gLocalInfo localinfo = mHot2gIfList[index].getHot2gLocalInfo();



		result = mHot2gIfList[index].sendCommand0A_Write(
				localinfo.valTransRateMode,
				localinfo.valTransRatePD,
				trans_rate,
				devinfo.valTransRateBatt,
				devinfo.valTransRateHR,
				true);

		return (result);
	}

	/**
	 * 心拍数収集情報更新
	 * @param trans_rate :心拍数転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setHRSamplingInfo( Hot2gEnumerations.EnumHot2gHRTransRate trans_rate )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setHRSamplingInfo( 0, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
		setHRSamplingInfo(0, trans_rate);
	}

	/**
	 * 心拍数収集情報更新
	 * @param dev_address :デバイスアドレス
	 * @param trans_rate :心拍数転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setHRSamplingInfo( string dev_address, Hot2gEnumerations.EnumHot2gHRTransRate trans_rate )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setHRSamplingInfo( index, trans_rate ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 心拍数収集情報更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param trans_rate :心拍数転送周期
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setHRSamplingInfo( int index, Hot2gEnumerations.EnumHot2gHRTransRate trans_rate )
	{
		Hot2gDevInfo	devinfo = mHot2gIfList[index].getHot2gDevInfo() ;
		Hot2gLocalInfo	localinfo = mHot2gIfList[index].getHot2gLocalInfo() ;

		return( mHot2gIfList[index].sendCommand0A_Write(
				localinfo.valTransRateMode,
				localinfo.valTransRatePD,
				devinfo.valTransRateMot,
				devinfo.valTransRateBatt,
				trans_rate,
				true ) ) ;
	}

	/**
	 * リアルタイムUSB出力機能更新
	 * @param flag :使用／未使用フラグ[T/F]
	 * @return :処理結果[EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setUsbRTOutputMode(bool flag)
	{
		// API Level=24以上, Language Level=8以降
		/*CompletableFuture<EnumHot2gResult> cf = CompletableFuture.supplyAsync(()->{
			if(mHot2gIfList.size() == 0 )	return(EnumHot2gResult.NotConnected ) ;
			return(setUsbRTOutputMode( 0, flag ) ) ;
		}) ;

		return(cf ) ;*/
		setUsbRTOutputMode(0, flag);

	}



	/**
	 * リアルタイムUSB出力機能更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param flag :使用／未使用フラグ[T/F]
	 * @return :処理結果[EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private void setUsbRTOutputMode(int index, bool flag)
	{
		int value = 0;
		if (flag) value = 1;

		mHot2gIfList[index].sendCommand0C_Write(value, true);
	}

	/**
	 * 計測開始
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public Hot2gEnumerations.EnumHot2gResult startMeasure()
	{
        return startMeasure(0);
	}

	/**
	 * 計測開始
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public Hot2gEnumerations.EnumHot2gResult startMeasure( string dev_address )
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

        return startMeasure(index);
	}

	/**
	 * 計測開始
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult startMeasure( int index )
	{
		return( mHot2gIfList[index].sendCommand40_Write( true ) ) ;
	}

	/**
	 * 計測停止
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public Hot2gEnumerations.EnumHot2gResult stopMeasure()/* throws Hot2gException */
	{
        return( stopMeasure( 0 ) ) ;
	}

	/**
	 * 計測停止
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public Hot2gEnumerations.EnumHot2gResult stopMeasure( string dev_address )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

        return (stopMeasure(index));
    }

	/**
	 * 計測停止
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult stopMeasure( int index )
	{
		return( mHot2gIfList[index].sendCommand41_Write( true ) ) ;
	}

#if false
    /**
	 * 計測開始
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
    public void startMeasureHot1g()/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( startMeasureHot1g( 0 ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 計測開始
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void startMeasureHot1g( string dev_address )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( startMeasureHot1g( index ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 計測開始
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult startMeasureHot1g( int index )
	{
		int	mode ;
		Hot2gDevInfo	devinfo = mHot2gIfList[index].getHot2gDevInfo() ;

		switch( devinfo.valDevType ){
			case Hot2gEnumerations.EnumHot2gDevType.HOT1000:	mode = 0 ;	break ;
			case Hot2gEnumerations.EnumHot2gDevType.HOT2000:	mode = 3 ;	break ;
			case Hot2gEnumerations.EnumHot2gDevType.XB01:		mode = 3 ;	break ;
			default:	mode = 0 ; break;
		}

		return( mHot2gIfList[index].sendCommandATR_Write( mode, false ) ) ;
	}

	/**
	 * 計測開始
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void stopMeasureHot1g()/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( stopMeasureHot1g( 0 ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 計測開始
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void stopMeasureHot1g( string dev_address )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( stopMeasureHot1g( index ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 計測開始
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult stopMeasureHot1g( int index )
	{
		return( mHot2gIfList[index].sendCommandATS_Write( false ) ) ;
	}
#endif

#if true
    /**
	 * 自動ゲイン調整開始
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
    public void startAutoGainControl()/* throws Hot2gException */
	{
		startAutoGainControl(0);
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( startAutoGainControl( 0 ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 自動ゲイン調整開始
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void startAutoGainControl( string dev_address )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( startAutoGainControl( index ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 自動ゲイン調整開始
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult startAutoGainControl( int index )
	{
		return( mHot2gIfList[index].sendCommand42_Write( true ) ) ;
	}

	/**
	 * 自動ゲイン調整停止
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void stopAutoGainControl()/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( stopAutoGainControl( 0 ) ) ;
		}) ;

		return( cf ) ;*/
		stopAutoGainControl(0);
	}

	/**
	 * 自動ゲイン調整停止
	 * @param dev_address :デバイスアドレス
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void stopAutoGainControl( string dev_address )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( stopAutoGainControl( index ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 自動ゲイン調整停止
	 * @param index :通信対象の通信IFリスト番号
	 * @return :処理結果[Hot2gEnumerations.EnumHot2gResult]
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult stopAutoGainControl( int index )
	{
		return( mHot2gIfList[index].sendCommand43_Write( true ) ) ;
	}
#endif

	/**
	 * 手動ゲイン設定用ゲイン値更新
	 * @param param_gain
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcGain(Hot2gGainData.ParamGain param_gain )/* throws Hot2gException */
	{
        setMgcGain(0, param_gain);

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcGain( 0, param_gain ) ) ;
		}) ;

		return( cf ) ;*/
	}



	/**
	 * 手動ゲイン設定用ゲイン値更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param param_gain
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMgcGain( int index, Hot2gGainData.ParamGain param_gain )
	{
        Hot2gGainData.ParamProbe paramProbe = new Hot2gGainData.ParamProbe();
		int led_L;
		int led_R;

		// 引数チェック
		if( (param_gain.ch0_L_AmpGain < 1) || (param_gain.ch0_L_AmpGain > Hot2gGainData.valMaxGain_0ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch1_L_AmpGain < 1) || (param_gain.ch1_L_AmpGain > Hot2gGainData.valMaxGain_1ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch0_R_AmpGain < 1) || (param_gain.ch0_R_AmpGain > Hot2gGainData.valMaxGain_0ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch1_R_AmpGain < 1) || (param_gain.ch1_R_AmpGain > Hot2gGainData.valMaxGain_1ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;

		// ゲイン設定の前回値を更新
		oldParamGain = param_gain;
		// 光量設定の前回値判定
		if( oldParamLed == null )
		{
			// 前回値がない場合は無効値を指定
			led_L = -1;
			led_R = -1;
		}else{
			led_L = oldParamLed.led_L;
			led_R = oldParamLed.led_R;
		}

		// ゲインパラメータをプローブパラメータへ変換
		Hot2gGainData.CnvGainToProbe( param_gain, paramProbe );

		// プローブセッティングに展開
		Hot2gGainData.ProbeSetting[] param = new Hot2gGainData.ProbeSetting[2];
		param[0] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_L_AmpGain,
				paramProbe.ch1_L_AmpGain,
				paramProbe.ch1_L_PreAmpGain,
				led_L);
		param[1] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_R_AmpGain,
				paramProbe.ch1_R_AmpGain,
				paramProbe.ch1_R_PreAmpGain,
				led_R);

		int invalidGain		= 0;
		int invalidLed		= 0;
		// ゲインパラメータチェック
		if( (param[0].ch0AmpGain < 0) || (param[0].ch0AmpGain > 15) )	invalidGain = 1;
		if( (param[0].ch1AmpGain < 0) || (param[0].ch1AmpGain > 15) )	invalidGain = 1;
		if( (param[1].ch0AmpGain < 0) || (param[1].ch0AmpGain > 15) )		invalidGain = 1;
		if( (param[1].ch1AmpGain < 0) || (param[1].ch1AmpGain > 15) )		invalidGain = 1;
		if( (param[0].ch1PreAmpGain < 0) || (param[0].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( (param[1].ch1PreAmpGain < 0) || (param[1].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( invalidGain == 1 )
		{
			// 範囲外の場合、ゲインパラメータの指定がすべて-1の時だけゲイン無効として扱う
			if( (param[0].ch0AmpGain    != -1) ||
					(param[0].ch1AmpGain    != -1) ||
					(param[1].ch0AmpGain    != -1) ||
					(param[1].ch1AmpGain    != -1) ||
					(param[0].ch1PreAmpGain != -1) ||
					(param[1].ch1PreAmpGain != -1) )
			{
				// ゲインパラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}
		// 光量パラメータチェック
		if( (param[0].ledGain < 0) || (param[0].ledGain > 255) )		invalidLed = 1;
		if( (param[1].ledGain < 0) || (param[1].ledGain > 255) )		invalidLed = 1;
		if( invalidLed == 1 )
		{
			// 範囲外の場合、光量パラメータの指定がすべて-1の時だけ光量無効として扱う
			if( (param[0].ledGain != -1) ||
					(param[1].ledGain != -1) )
			{
				// 光量パラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}

		// SelModeを算出
		Hot2gEnumerations.EnumHot2gMgcType mgctype = Hot2gEnumerations.EnumHot2gMgcType.Disable ;
		if( (invalidGain == 0) && (invalidLed == 0) )
		{
			mgctype = Hot2gEnumerations.EnumHot2gMgcType.All;				// 全てのパラメータが有効
		}else if( (invalidGain == 1) && (invalidLed == 0) )
		{
			mgctype = Hot2gEnumerations.EnumHot2gMgcType.Bright;			// ゲインパラメータが無効
		}else if( (invalidGain == 0) && (invalidLed == 1) )
		{
			mgctype = Hot2gEnumerations.EnumHot2gMgcType.Gain;			// 光量パラメータが無効
		}else{
			mgctype = Hot2gEnumerations.EnumHot2gMgcType.Disable;		// 全てのパラメータが無効
		}

		int[]	sw_gain = new int[]{ param[0].ch1PreAmpGain, param[1].ch1PreAmpGain } ;
		int[]	pm_gain = new int[]{ param[0].ch0AmpGain, param[0].ch1AmpGain,
				param[1].ch0AmpGain, param[1].ch1AmpGain } ;
		int[]	bright = new int[]{ param[0].ledGain, param[1].ledGain } ;

		// コマンド送信
		return( mHot2gIfList[index].sendCommand44_Write( mgctype, sw_gain, pm_gain, bright, true ) ) ;
	}

	/**
	 * 手動ゲイン設定用ゲイン値更新
	 * @param param_gain
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcGainHot1g(Hot2gGainData.ParamGain param_gain )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcGainHot1g( 0, param_gain ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン設定用ゲイン値更新
	 * @param dev_address :デバイスアドレス
	 * @param param_gain
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcGainHot1g( string dev_address, Hot2gGainData.ParamGain param_gain )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcGainHot1g( index, param_gain ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン設定用ゲイン値更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param param_gain
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMgcGainHot1g( int index, Hot2gGainData.ParamGain param_gain )
	{
        Hot2gGainData.ParamProbe paramProbe = new Hot2gGainData.ParamProbe();
		int led_L;
		int led_R;

		// 引数チェック
		if( (param_gain.ch0_L_AmpGain < 1) || (param_gain.ch0_L_AmpGain > Hot2gGainData.valMaxGain_0ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch1_L_AmpGain < 1) || (param_gain.ch1_L_AmpGain > Hot2gGainData.valMaxGain_1ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch0_R_AmpGain < 1) || (param_gain.ch0_R_AmpGain > Hot2gGainData.valMaxGain_0ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_gain.ch1_R_AmpGain < 1) || (param_gain.ch1_R_AmpGain > Hot2gGainData.valMaxGain_1ch) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;

		// ゲイン設定の前回値を更新
		oldParamGain = param_gain;
		// 光量設定の前回値判定
		if( oldParamLed == null )
		{
			// 前回値がない場合は無効値を指定
			led_L = -1;
			led_R = -1;
		}else{
			led_L = oldParamLed.led_L;
			led_R = oldParamLed.led_R;
		}

		// ゲインパラメータをプローブパラメータへ変換
		Hot2gGainData.CnvGainToProbe( param_gain, paramProbe );

		// プローブセッティングに展開
		Hot2gGainData.ProbeSetting[] param = new Hot2gGainData.ProbeSetting[2];
		param[0] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_L_AmpGain,
				paramProbe.ch1_L_AmpGain,
				paramProbe.ch1_L_PreAmpGain,
				led_L);
		param[1] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_R_AmpGain,
				paramProbe.ch1_R_AmpGain,
				paramProbe.ch1_R_PreAmpGain,
				led_R);

		int invalidGain		= 0;
		int invalidLed		= 0;
		// ゲインパラメータチェック
		if( (param[0].ch0AmpGain < 0) || (param[0].ch0AmpGain > 15) )	invalidGain = 1;
		if( (param[0].ch1AmpGain < 0) || (param[0].ch1AmpGain > 15) )	invalidGain = 1;
		if( (param[1].ch0AmpGain < 0) || (param[1].ch0AmpGain > 15) )		invalidGain = 1;
		if( (param[1].ch1AmpGain < 0) || (param[1].ch1AmpGain > 15) )		invalidGain = 1;
		if( (param[0].ch1PreAmpGain < 0) || (param[0].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( (param[1].ch1PreAmpGain < 0) || (param[1].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( invalidGain == 1 )
		{
			// 範囲外の場合、ゲインパラメータの指定がすべて-1の時だけゲイン無効として扱う
			if( (param[0].ch0AmpGain    != -1) ||
					(param[0].ch1AmpGain    != -1) ||
					(param[1].ch0AmpGain    != -1) ||
					(param[1].ch1AmpGain    != -1) ||
					(param[0].ch1PreAmpGain != -1) ||
					(param[1].ch1PreAmpGain != -1) )
			{
				// ゲインパラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}
		// 光量パラメータチェック
		if( (param[0].ledGain < 0) || (param[0].ledGain > 255) )		invalidLed = 1;
		if( (param[1].ledGain < 0) || (param[1].ledGain > 255) )		invalidLed = 1;
		if( invalidLed == 1 )
		{
			// 範囲外の場合、光量パラメータの指定がすべて-1の時だけ光量無効として扱う
			if( (param[0].ledGain != -1) ||
					(param[1].ledGain != -1) )
			{
				// 光量パラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}

		// SelModeを算出
		Hot2gEnumerations.EnumHot2gMgcType mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable ;
		if( (invalidGain == 0) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.All;				// 全てのパラメータが有効
		}else if( (invalidGain == 1) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Bright;			// ゲインパラメータが無効
		}else if( (invalidGain == 0) && (invalidLed == 1) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Gain;			// 光量パラメータが無効
		}else{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable;		// 全てのパラメータが無効
		}

		int[]	sw_gain = new int[]{ param[0].ch1PreAmpGain, param[1].ch1PreAmpGain } ;
		int[]	pm_gain = new int[]{ param[0].ch0AmpGain, param[0].ch1AmpGain,
				param[1].ch0AmpGain, param[1].ch1AmpGain } ;
		int[]	bright = new int[]{ param[0].ledGain, param[1].ledGain } ;

		// コマンド送信
		return( mHot2gIfList[index].sendCommandATC_Write( mgcType, sw_gain, pm_gain, bright, false ) ) ;
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcLed(Hot2gGainData.ParamLed param_led )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcLed( 0, param_led ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param dev_address :デバイスアドレス
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcLed( string dev_address, Hot2gGainData.ParamLed param_led )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcLed( index, param_led ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMgcLed( int index, Hot2gGainData.ParamLed param_led )
	{
        Hot2gGainData.ParamProbe paramProbe = new Hot2gGainData.ParamProbe();

		// 引数チェック
		if( (param_led.led_L < 0) || (param_led.led_L > 255) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_led.led_R < 0) || (param_led.led_R > 255) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;

		// 光量設定の前回値を更新
		oldParamLed = param_led ;
		// ゲイン設定の前回値判定
		if( oldParamGain == null )
		{
			// 前回値がない場合は無効値を指定
			paramProbe.ch0_L_AmpGain		= 0xFF;
			paramProbe.ch1_L_AmpGain		= 0xFF;
			paramProbe.ch1_L_PreAmpGain	= -1;
			paramProbe.ch0_R_AmpGain		= 0xFF;
			paramProbe.ch1_R_AmpGain		= 0xFF;
			paramProbe.ch1_R_PreAmpGain	= -1;
		}else{
			// ゲインパラメータをプローブパラメータへ変換
			Hot2gGainData.CnvGainToProbe( oldParamGain, paramProbe );
		}

		// プローブセッティングに展開
		Hot2gGainData.ProbeSetting[] param = new Hot2gGainData.ProbeSetting[2];
		param[0] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_L_AmpGain,
				paramProbe.ch1_L_AmpGain,
				paramProbe.ch1_L_PreAmpGain,
				param_led.led_L ) ;
		param[1] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_R_AmpGain,
				paramProbe.ch1_R_AmpGain,
				paramProbe.ch1_R_PreAmpGain,
				param_led.led_R ) ;

		int invalidGain		= 0;
		int invalidLed		= 0;
		// ゲインパラメータチェック
		if( (param[0].ch0AmpGain < 0) || (param[0].ch0AmpGain > 15) )	invalidGain = 1;
		if( (param[0].ch1AmpGain < 0) || (param[0].ch1AmpGain > 15) )	invalidGain = 1;
		if( (param[1].ch0AmpGain < 0) || (param[1].ch0AmpGain > 15) )		invalidGain = 1;
		if( (param[1].ch1AmpGain < 0) || (param[1].ch1AmpGain > 15) )		invalidGain = 1;
		if( (param[0].ch1PreAmpGain < 0) || (param[0].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( (param[1].ch1PreAmpGain < 0) || (param[1].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( invalidGain == 1 )
		{
			// 範囲外の場合、ゲインパラメータの指定がすべて-1の時だけゲイン無効として扱う
			if( (param[0].ch0AmpGain    != -1) ||
					(param[0].ch1AmpGain    != -1) ||
					(param[1].ch0AmpGain    != -1) ||
					(param[1].ch1AmpGain    != -1) ||
					(param[0].ch1PreAmpGain != -1) ||
					(param[1].ch1PreAmpGain != -1) )
			{
				// ゲインパラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}
		// 光量パラメータチェック
		if( (param[0].ledGain < 0) || (param[0].ledGain > 255) )		invalidLed = 1;
		if( (param[1].ledGain < 0) || (param[1].ledGain > 255) )		invalidLed = 1;
		if( invalidLed == 1 )
		{
			// 範囲外の場合、光量パラメータの指定がすべて-1の時だけ光量無効として扱う
			if( (param[0].ledGain != -1) ||
					(param[1].ledGain != -1) )
			{
				// 光量パラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}

		// SelModeを算出
		Hot2gEnumerations.EnumHot2gMgcType mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable ;
		if( (invalidGain == 0) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.All;				// 全てのパラメータが有効
		}else if( (invalidGain == 1) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Bright;			// ゲインパラメータが無効
		}else if( (invalidGain == 0) && (invalidLed == 1) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Gain;			// 光量パラメータが無効
		}else{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable;		// 全てのパラメータが無効
		}

		int[]	sw_gain = new int[]{ param[0].ch1PreAmpGain, param[1].ch1PreAmpGain } ;
		int[]	pm_gain = new int[]{ param[0].ch0AmpGain, param[0].ch1AmpGain,
				param[1].ch0AmpGain, param[1].ch1AmpGain } ;
		int[]	bright = new int[]{ param[0].ledGain, param[1].ledGain } ;

		// コマンド送信
		return( mHot2gIfList[index].sendCommand44_Write( mgcType, sw_gain, pm_gain, bright, true ) ) ;
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcLedHot1g(Hot2gGainData.ParamLed param_led )/* throws Hot2gException */
	{
		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( mHot2gIfList.size() == 0 )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcLedHot1g( 0, param_led ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param dev_address :デバイスアドレス
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	public void setMgcLedHot1g( string dev_address, Hot2gGainData.ParamLed param_led )/* throws Hot2gException */
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		/* // API Level=24以上, Language Level=8以降
		void cf = CompletableFuture.supplyAsync(()->{
			if( index < 0 )							return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			if( index >= mHot2gIfList.size() )	return( Hot2gEnumerations.EnumHot2gResult.NotConnected ) ;
			return( setMgcLedHot1g( index, param_led ) ) ;
		}) ;

		return( cf ) ;*/
	}

	/**
	 * 手動ゲイン調整用光量値更新
	 * @param index :通信対象の通信IFリスト番号
	 * @param param_led
	 * @return
	 * @throws Hot2gException	:ドライバ上の例外発生
	 */
	private Hot2gEnumerations.EnumHot2gResult setMgcLedHot1g( int index, Hot2gGainData.ParamLed param_led )
	{
        Hot2gGainData.ParamProbe paramProbe = new Hot2gGainData.ParamProbe();

		// 引数チェック
		if( (param_led.led_L < 0) || (param_led.led_L > 255) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
		if( (param_led.led_R < 0) || (param_led.led_R > 255) )		return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;

		// 光量設定の前回値を更新
		oldParamLed = param_led ;
		// ゲイン設定の前回値判定
		if( oldParamGain == null )
		{
			// 前回値がない場合は無効値を指定
			paramProbe.ch0_L_AmpGain		= 0xFF;
			paramProbe.ch1_L_AmpGain		= 0xFF;
			paramProbe.ch1_L_PreAmpGain	= -1;
			paramProbe.ch0_R_AmpGain		= 0xFF;
			paramProbe.ch1_R_AmpGain		= 0xFF;
			paramProbe.ch1_R_PreAmpGain	= -1;
		}else{
			// ゲインパラメータをプローブパラメータへ変換
			Hot2gGainData.CnvGainToProbe( oldParamGain, paramProbe );
		}

		// プローブセッティングに展開
		Hot2gGainData.ProbeSetting[] param = new Hot2gGainData.ProbeSetting[2];
		param[0] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_L_AmpGain,
				paramProbe.ch1_L_AmpGain,
				paramProbe.ch1_L_PreAmpGain,
				param_led.led_L ) ;
		param[1] = Hot2gGainData.getProbeSetting(
				paramProbe.ch0_R_AmpGain,
				paramProbe.ch1_R_AmpGain,
				paramProbe.ch1_R_PreAmpGain,
				param_led.led_R ) ;

		int invalidGain		= 0;
		int invalidLed		= 0;
		// ゲインパラメータチェック
		if( (param[0].ch0AmpGain < 0) || (param[0].ch0AmpGain > 15) )	invalidGain = 1;
		if( (param[0].ch1AmpGain < 0) || (param[0].ch1AmpGain > 15) )	invalidGain = 1;
		if( (param[1].ch0AmpGain < 0) || (param[1].ch0AmpGain > 15) )		invalidGain = 1;
		if( (param[1].ch1AmpGain < 0) || (param[1].ch1AmpGain > 15) )		invalidGain = 1;
		if( (param[0].ch1PreAmpGain < 0) || (param[0].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( (param[1].ch1PreAmpGain < 0) || (param[1].ch1PreAmpGain > 3) )	invalidGain = 1;
		if( invalidGain == 1 )
		{
			// 範囲外の場合、ゲインパラメータの指定がすべて-1の時だけゲイン無効として扱う
			if( (param[0].ch0AmpGain    != -1) ||
					(param[0].ch1AmpGain    != -1) ||
					(param[1].ch0AmpGain    != -1) ||
					(param[1].ch1AmpGain    != -1) ||
					(param[0].ch1PreAmpGain != -1) ||
					(param[1].ch1PreAmpGain != -1) )
			{
				// ゲインパラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}
		// 光量パラメータチェック
		if( (param[0].ledGain < 0) || (param[0].ledGain > 255) )		invalidLed = 1;
		if( (param[1].ledGain < 0) || (param[1].ledGain > 255) )		invalidLed = 1;
		if( invalidLed == 1 )
		{
			// 範囲外の場合、光量パラメータの指定がすべて-1の時だけ光量無効として扱う
			if( (param[0].ledGain != -1) ||
					(param[1].ledGain != -1) )
			{
				// 光量パラメータ不正
				return( Hot2gEnumerations.EnumHot2gResult.ParamWrong ) ;
			}
		}

		// SelModeを算出
		Hot2gEnumerations.EnumHot2gMgcType mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable ;
		if( (invalidGain == 0) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.All;				// 全てのパラメータが有効
		}else if( (invalidGain == 1) && (invalidLed == 0) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Bright;			// ゲインパラメータが無効
		}else if( (invalidGain == 0) && (invalidLed == 1) )
		{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Gain;			// 光量パラメータが無効
		}else{
			mgcType = Hot2gEnumerations.EnumHot2gMgcType.Disable;		// 全てのパラメータが無効
		}

		int[]	sw_gain = new int[]{ param[0].ch1PreAmpGain, param[1].ch1PreAmpGain } ;
		int[]	pm_gain = new int[]{ param[0].ch0AmpGain, param[0].ch1AmpGain,
				param[1].ch0AmpGain, param[1].ch1AmpGain } ;
		int[]	bright = new int[]{ param[0].ledGain, param[1].ledGain } ;

		// コマンド送信
		return( mHot2gIfList[index].sendCommandATC_Write( mgcType, sw_gain, pm_gain, bright, false ) ) ;
	}

	//---------------------------------------------------------------------------------------------

	/**
	 * インターフェースリストから指定アドレスでインデックスを検索
	 * @param dev_address :デバイスアドレス
	 * @return
	 */
	public int getHot2gIfListIndex( string dev_address )
	{
		int		index = -1 ;

		for( int ii=0; ii < mHot2gIfList.Count; ii++ ){
			if( mHot2gIfList[ii].getConnectedAddress().Equals( dev_address ) ){
				index = ii ;
				break ;
			}
		}

		return( index ) ;
	}

	/**
	 * デバイス情報データ
	 * @return
	 */
	public Hot2gDevInfo getHot2gDevInfo()
	{
		if( mHot2gIfList.Count == 0 )	return( null ) ;
		return( mHot2gIfList[0].getHot2gDevInfo() ) ;
	}

	/**
	 * デバイス情報データ
	 * @param dev_address :デバイスアドレス
	 * @return
	 */
	public Hot2gDevInfo getHot2gDevInfo( string dev_address )
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		if( index < 0 )							return( null ) ;
		if( index >= mHot2gIfList.Count )	return( null ) ;

		return( mHot2gIfList[index].getHot2gDevInfo() ) ;
	}

	/**
	 * デバイス状態データ
	 * @return
	 */
	public Hot2gDevState getHot2gDevState()
	{
		if( mHot2gIfList.Count == 0 )	return( null ) ;
		return( mHot2gIfList[0].getHot2gDevState() ) ;
	}

	/**
	 * デバイス状態データ
	 * @param dev_address :デバイスアドレス
	 * @return
	 */
	public Hot2gDevState getHot2gDevState(string dev_address )
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		if( index < 0 )							return( null ) ;
		if( index >= mHot2gIfList.Count )	return( null ) ;

		return( mHot2gIfList[index].getHot2gDevState() ) ;
	}

	/**
	 * デバイス内部データ
	 * @return
	 */
	public Hot2gLocalInfo getHot2gLocalInfo()
	{
		if( mHot2gIfList.Count == 0 )	return( null ) ;
		return( mHot2gIfList[0].getHot2gLocalInfo() ) ;
	}

	/**
	 * デバイス内部データ
	 * @param dev_address :デバイスアドレス
	 * @return
	 */
	public Hot2gLocalInfo getHot2gLocalInfo(string dev_address )
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		if( index < 0 )							return( null ) ;
		if( index >= mHot2gIfList.Count )	return( null ) ;

		return( mHot2gIfList[index].getHot2gLocalInfo() ) ;
	}

	/**
	 * 自動ゲイン調整結果データ
	 * @return
	 */
	public Hot2gAgcResult getHot2gAgcResult()
	{
		if( mHot2gIfList.Count== 0 )	return( null ) ;
		return( mHot2gIfList[0].getHot2gAgcResult() ) ;
	}

	/**
	 * 自動ゲイン調整結果データ
	 * @param dev_address :デバイスアドレス
	 * @return
	 */
	public Hot2gAgcResult getHot2gAgcResult(string dev_address )
	{
		int	index = getHot2gIfListIndex( dev_address ) ;

		if( index < 0 )							return( null ) ;
		if( index >= mHot2gIfList.Count )	return( null ) ;

		return( mHot2gIfList[index].getHot2gAgcResult() ) ;
	}

	/**
	 * 自動ゲイン調整結果データ
	 * @param index :通信対象の通信IFリスト番号
	 * @return
	 */
	public Hot2gAgcResult getHot2gAgcResult( int index )
	{
		return( mHot2gIfList[index].getHot2gAgcResult() ) ;
	}
}
