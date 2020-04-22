/**
 * デバイス情報データ
 */
public class Hot2gDevInfo
{
	public Hot2gEnumerations.EnumHot2gDevType valDevType ;				// 機種区分
	public string						valDevId ;					// 機器ID
	public string						valDevName ;				// 機器名称
	public string						valDevFirmVersion ;		// ファームウェアバージョン
	public string						valLotId ;					// ロットID
	public int							valVmaxPd ;				// PD計測値デジット最大値
	public double						valResolutePd ;			// PD計測値分解能 [mV/bit]
	public Hot2gEnumerations.EnumHot2gAccRange			valRangeAcc ;		    	// モーション加速度レンジ
	public Hot2gEnumerations.EnumHot2gGyroRange			valRangeGyro ;				// モーション角速度レンジ
	public double						valResoluteAcc ;			// モーション加速度分解能 ×10^-3 [G/bit]
	public double						valResoluteGyro ;			// モーション角速度分解能 [DPS/bit]
	public Hot2gEnumerations.EnumHot2gBattSmplRate		valSmplRateBatt ;			// バッテリーサンプリング
	public Hot2gEnumerations.EnumHot2gMotSmplRate			valSmplRateMot ;			// モーションサンプリング
	public Hot2gEnumerations.EnumHot2gBattTransRate		valTransRateBatt ;		// バッテリーデータ転送周期
	public Hot2gEnumerations.EnumHot2gMotTransRate		valTransRateMot ;			// モーションデータ転送周期
	public Hot2gEnumerations.EnumHot2gHRTransRate			valTransRateHR ;			// 心拍数データ転送周期
	public bool						valEnabledOutputUsb ;		// USBリアルタイム出力機能有無

	/**
	 * コンストラクタ
	 */
	public Hot2gDevInfo()
	{
		valDevType = Hot2gEnumerations.EnumHot2gDevType.HOT2000 ;
		valDevId = "" ;
		valDevName = "" ;
		valDevFirmVersion = "" ;
		valLotId = "" ;
		valVmaxPd = 4096 ;
		valRangeAcc = Hot2gEnumerations.EnumHot2gAccRange.e16G ;
		valRangeGyro = Hot2gEnumerations.EnumHot2gGyroRange.e2000DPS ;
		valSmplRateBatt = Hot2gEnumerations.EnumHot2gBattSmplRate.e1HZ ;
		valSmplRateMot = Hot2gEnumerations.EnumHot2gMotSmplRate.e10HZ ;
		valTransRateBatt = Hot2gEnumerations.EnumHot2gBattTransRate.e1HZ ;
		valTransRateMot = Hot2gEnumerations.EnumHot2gMotTransRate.e10HZ ;
		valTransRateHR = Hot2gEnumerations.EnumHot2gHRTransRate.e1HZ ;
		valResolutePd = 0.805664f ;             // 12bit, 0～3.3V
		valResoluteAcc = 0.000488289f ;         // 16bit, ±16G
		valResoluteGyro = 0.061036088f ;        // 16bit, ±2000DPS
		valEnabledOutputUsb = false ;
	}
}
