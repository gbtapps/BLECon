/**
 * デバイスバッテリー計測データ
 */
public class Hot2gMeasDataBattery
{
	public int		valResultCount ;		// リザルト送信カウンタ
    public int		valGauge ;				// 電池残量Rawデータ
    public int		valVolt ;				// 電池電圧Rawデータ
    public int		valTemp ;				// 電池温度Rawデータ
    public double	valConvGauge ;			// 電池残量換算値 [%]
    public double	valConvVolt ;			// 電池電圧換算値 [mV]
    public double	valConvTemp ;			// 電池温度換算値 [℃]

    /**
     * コンストラクタ
     */
    public Hot2gMeasDataBattery()
    {
		valResultCount = 0 ;
		valGauge = 0 ;
		valVolt = 0 ;
		valTemp = 0 ;
		valConvGauge = 0 ;
		valConvVolt = 0 ;
		valConvTemp = 0 ;
    }
}
