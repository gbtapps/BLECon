/**
 * デバイスモーション拡張計測データ
 */
public class Hot2gMeasDataMotBatt
{
	public int			valResultCount ;		// リザルト送信カウンタ
    public int[]		valAccels ;				// X,Y,Z 加速度Rawデータ
    public int[]		valGyros ;				// X,Y,Z 角速度Rawデータ
    public int			valGauge ;				// 電池残量Rawデータ
    public int			valVolt ;				// 電池電圧Rawデータ
    public int			valTemp ;				// 電池温度Rawデータ
    public double[]	valConvAccels ;			// X,Y,Z 加速度換算値 [G]
    public double[]	valConvGyros ;			// X,Y,Z 角速度換算値 [DPS]
    public double		valConvGauge ;			// 電池残量換算値 [%]
    public double		valConvVolt ;			// 電池電圧換算値 [mV]
    public double		valConvTemp ;			// 電池温度換算値 [℃]

    /**
     * コンストラクタ
     */
    public Hot2gMeasDataMotBatt()
    {
        valResultCount = 0 ;
		valAccels = new int[3] ;
		valGyros = new int[3] ;
		valGauge = 0 ;
		valVolt = 0 ;
		valTemp = 0 ;
		valConvAccels = new double[3] ;
		valConvGyros = new double[3] ;
		valConvGauge = 0 ;
		valConvVolt = 0 ;
		valConvTemp = 0 ;
    }
}
