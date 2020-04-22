/**
 * 内部ローカルデータ
 */
public class Hot2gLocalInfo
{
    public int valHwADEnableBits ;			    	// AD有効ビット数
    public int valHwADVoltMin ;						// AD最小電圧
    public int valHwADVoltMax ;						// AD最大電圧
    public int valHwAccEnableBits ;					// 加速度有効ビット数
    public int valHwGyroEnableBits ;			    // 角速度有効ビット数
    public int[] valWaitIrSwGains ;					// 待機IR スイッチゲイン(L3,R3)
    public int[] valWaitIrPmGains ;					// 待機IR ポテンショメータゲイン(L1,L3,R1,R3)
    public int[] valWaitIrIoOuts ;			    	// 待機IR IO出力(L/R)
    public int[] valWaitIrBrights ;					// 待機IR 光量(L/R)
    public int valAccLPF ;					    	// 加速度LPF
    public int valAccODR ;					    	// 加速度ODR
    public int valGyroLPF ;							// 角速度LPF
    public int valGyroODR ;							// 角速度ODR
    public int valBattCapacity ;			    		// バッテリー情報
    public int valTransRateMode ;			    	// 転送方式
    public int valTransRatePD ;						// PD計測転送周期
    public int valAnalysisHR ;				    	// 心拍数解析有無
    public int valAnalysisAngle ;			    	// 姿勢解析有無
    public int valStsErrorCode ;			    		// 異常コード
    public int[] valStsIrIoOuts ;			    	// IR IO出力状態(L/R)
    public int[] valStsIrBrights ;			    	// IR 光量状態(L/R)
    public int valCtrlAutoMeasStartResult ;		// 自動計測開始結果
    public int valCtrlAutoMeasStopResult ;	    	// 自動計測停止結果
    public int valCtrlAutoGainStartResult ;		// 自動ゲイン調整開始結果
    public int valCtrlAutoGainStopResult ;		    // 自動ゲイン調整停止結果
    public int valCtrlManuGainResult ;			    // 手動ゲイン設定結果
    public int[] valCtrlManuGainIrSwGains ;			// 手動ゲイン設定 スイッチゲイン(L3,R3)
    public int[] valCtrlManuGainIrPmGains ;			// 手動ゲイン設定 ポテンショメータゲイン(L1,L3,R1,R3)
    public int[] valCtrlManuGainIrBrights ;			// 手動ゲイン設定 光量(L/R)
    public int valCtrlPDResult ;				    	// PD入力結果
    public int[] valCtrlPDValues ;				    // PD入力値(L1,L3,R1,R3)
    public int valCtrlIrOutResult ;					// IR出力結果
    public int[] valCtrlIrOutIos ;				    // IO出力(L/R)
    public int[] valCtrlIrOutBrights ;			    // 光量出力(L/R)
    public int valCtrlMotionResult ;			    // モーション値入力結果
    public int[] valCtrlAccValues ;					// 加速度入力値(X,Y,Z)
    public int[] valCtrlGyroValues ;			    	// 角速度入力値(X,Y,Z)
    public double[] valCtrlConvAccValues ;		    // 加速度入力値(X,Y,Z) [G]換算
    public double[] valCtrlConvGyroValues ;		   	// 角速度入力値(X,Y,Z) [DPS]換算
    public int valCtrlBatteryResult ;			    // 電池情報入力結果
    public int valPulseRateData ;				    // 心拍数解析データ
    public int valOtherLedPattern ;					// ユーザー表示灯表示パターン
    public int valOtherDiagnosisResult ;		    // 診断結果
    public int valOtherSensManuUpdateResult ;	    	// センサ設定手動更新結果
    public int valOtherSensManuUpdateTarget ;	    	// センサ設定手動更新 対象
    public int valOtherSensManuUpdateAddress ;	    	// センサ設定手動更新 アドレス
    public int valOtherSensManuUpdateSize ;			// センサ設定手動更新 データ数
    public byte[] valOtherSensManuUpdateParams ;   	// センサ設定手動更新 パラメータ
    public string valOtherSensManuUpdateParamText ;	// センサ設定手動更新 パラメータ
    public int valFirmUpdateStartResult ;		    	// ファーム更新開始結果
    public int valFirmUpdateProgramResult ;			// ファーム更新プログラム書込み結果
    public int valProbeParameterReport ;		    	// プローブパラメータ報告
    public int valTopAddress ;

    public int valDataBytes ;
    public int valManualDiagResult ;

    public bool valFirstBloodFlag;
    public double[] valFirstBloodData;

    /**
     * コンストラクタ
     */
    public Hot2gLocalInfo()
    {
        //00~03
        //04
        valHwADEnableBits = 12;
        valHwADVoltMin = 0;
        valHwADVoltMax = 3300;
        valHwAccEnableBits = 16;
        valHwGyroEnableBits = 16;
        //05
        valWaitIrSwGains = new int[2] ;
        valWaitIrPmGains = new int[4] ;
        valWaitIrIoOuts = new int[2] ;
        valWaitIrBrights = new int[2] ;
        //06
        valAccLPF = 0;
        valAccODR = 0;
        //07
        valGyroLPF = 0;
        valGyroODR = 0;
        //08
        valBattCapacity = 1100;
        //09
        //0A
        valTransRateMode = 0;
        valTransRatePD = 0;
        //0B
        valAnalysisHR = 0;
        valAnalysisAngle = 0;
        //0C
        //20
        valStsErrorCode = 0;
        //21
        //22
        //23
        //24
        valStsIrIoOuts = new int[2] ;
        valStsIrBrights = new int[2] ;
        //25
        //26
        //27
        //28
        //29
        //2A
        //40~43
        valCtrlAutoMeasStartResult = 0;
        valCtrlAutoMeasStopResult = 0;
        valCtrlAutoGainStartResult = 0;
        valCtrlAutoGainStopResult = 0;
        //44
        valCtrlManuGainResult = 0;
        valCtrlManuGainIrSwGains = new int[2] ;
        valCtrlManuGainIrPmGains = new int[4] ;
        valCtrlManuGainIrBrights = new int[2] ;
        //45
        valCtrlPDValues = new int[4] ;
        valCtrlIrOutResult = 0;
        //46
        valCtrlIrOutIos = new int[2] ;
        valCtrlIrOutBrights = new int[2] ;
        //47
        valCtrlAccValues = new int[3] ;
        valCtrlGyroValues = new int[3] ;
        valCtrlConvAccValues = new double[3] ;
        valCtrlConvGyroValues = new double[3] ;

        //60~71
        valOtherLedPattern = 0;
        valOtherDiagnosisResult = 0;
        valFirmUpdateStartResult = 0;
        valFirmUpdateProgramResult = 0;

        valOtherSensManuUpdateResult = 0 ;
        valOtherSensManuUpdateTarget = 0 ;
        valOtherSensManuUpdateAddress = 0 ;
        valOtherSensManuUpdateSize = 0 ;
        valOtherSensManuUpdateParamText = "" ;
        valOtherSensManuUpdateParams = new byte[16] ;
        valTopAddress = 0 ;
        valDataBytes = 0 ;
        valManualDiagResult = 0 ;

        valProbeParameterReport = 0;

        valCtrlPDResult = 0;
        valCtrlMotionResult = 0;
        valPulseRateData = 0;

        valFirstBloodFlag = false ;
        valFirstBloodData = new double[4] ;
    }
}
