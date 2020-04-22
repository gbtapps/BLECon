/**
 * デバイス状態データ
 */
public class Hot2gDevState
{
	public int           valErrorCode ;			    	// エラー状態
    public int           valBattGauge ;			    	// 電池残量Rawデータ
    public int           valBattVolt ;					// 電池電圧Rawデータ
    public int[]         valDevTemps ;					// バッテリー,モーション,CPU 温度Rawデータ
    public double        valConvBattGauge ;		    	// 電池残量換算値 [%]
    public double        valConvBattVolt ;		    	// 電池電圧換算値 [V]
    public double[]      valConvDevTemps ;		    	// バッテリー,モーション,CPU 温度換算値 [℃]
    public bool       valEnabledUsbVbus ;	    		// USB VBUS状態
    public bool       valEnabledUsbComm ;	    		// USB 通信接続状態
    public bool       valEnabledBtModule ;	    		// Bluetooth モジュール使用状態
    public bool       valEnabledBtComm ;			    // Bluetooth 通信接続状態
    public bool       valEnabledSensModule ;		    // センサモジュール検出状態
    public bool[]     valEnabledPdLs ;		   		 	// PD-L1,L3値検出状態
    public bool[]     valEnabledPdRs ;		   		 	// PD-R1,R3値検出状態
    public bool       valEnabledMotion ;			    // モーションセンサ検出状態
    public bool       valEnabledGauge ;				    // 残量ゲージセンサ検出状態

    /**
     * コンストラクタ
     */
    public Hot2gDevState()
    {
        valErrorCode = 0 ;
        valBattGauge = 0 ;
        valBattVolt = 0 ;
        valDevTemps = new int[3] ;
        valConvBattGauge = 0.0f ;
        valConvBattVolt = 0.0f ;
        valConvDevTemps = new double[3] ;
        valEnabledUsbVbus = false ;
        valEnabledUsbComm = false ;
        valEnabledBtModule = false ;
        valEnabledBtComm = false ;
        valEnabledSensModule = false ;
        valEnabledPdLs = new bool[2] ;
        valEnabledPdRs = new bool[2] ;
        valEnabledMotion = false ;
        valEnabledGauge = false ;
    }
}