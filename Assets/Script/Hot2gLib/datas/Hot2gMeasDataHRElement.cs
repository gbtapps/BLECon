/**
 * デバイス心拍成分データ
 */
public class Hot2gMeasDataHRElement		// 18.10.11 ohara 追加
{
	public int			valResultCount ;			// リザルト送信カウンタ
	public int[]		valElements ;				// 心拍成分値
	public double[]	valConvElements ;			// 心拍成分換算値

    /**
     * コンストラクタ
     */
    public Hot2gMeasDataHRElement()
    {
        valResultCount = 0 ;
		valElements = new int[6] ;
		valConvElements = new double[6] ;
    }
}
