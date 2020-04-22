/**
 * デバイスモーション計測データ
 */
public class Hot2gMeasDataMotion
{
    public int			valResultCount ;	// リザルト送信カウンタ
    public int[]		valAccels ;			// X,Y,Z 加速度Rawデータ
    public int[]		valGyros ;			// X,Y,Z 角速度Rawデータ
    public double[]	valConvAccels ;		// X,Y,Z 加速度換算値 [G]
    public double[]	valConvGyros ;		// X,Y,Z 角速度換算値 [DPS]

	/**
	 * コンストラクタ
	 */
	public Hot2gMeasDataMotion()
	{
		valResultCount = 0 ;
		valAccels = new int[3] ;
		valGyros = new int[3] ;
		valConvAccels = new double[3] ;
		valConvGyros = new double[3] ;
	}
}
