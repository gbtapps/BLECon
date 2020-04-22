/**
 * デバイス心拍数計測データ
 */
public class Hot2gMeasDataHR
{
    public int			valResultCount ;			// リザルト送信カウンタ
//    public int			valHR ;						// 心拍数Rawデータ
	public int[]		valPeakCountsL ;			// L側ピーク検出カウンタ		// 18.10.11 ohara
	public int[]		valPeakCountsR ;			// R側ピーク検出カウンタ		// 18.10.11 ohara
	public double		valConvHR ;				// 心拍数換算値					// 18.10.13 ohara int->double
	public double[]	valConvHR_L ;				// L側心拍数換算値 [bpm]		// 18.10.13 ohara
	public double[]	valConvHR_R ;				// R側心拍数換算値 [bpm]		// 18.10.13 ohara
	public int			valPeakAvePt ;				// ピーク検出カウンタポインタ			// [181025.2]
	public int			valPeakAveSize ;			// ピーク検出カウンタ平均値保持数		// [181025.2]
	public double[]	valPeakAveDatas ;			// ピーク検出カウンタ平均算出用保持値	// [181025.2]

    /**
     * コンストラクタ
     */
    public Hot2gMeasDataHR()
    {
        valResultCount = 0 ;
//		valHR = 0 ;
		valPeakCountsL = new int[6] ;			// 18.10.11 ohara
		valPeakCountsR = new int[6] ;			// 18.10.11 ohara
		valConvHR = 0.0f ;						// 18.10.13 ohara
		valConvHR_L = new double[valPeakCountsL.Length] ;		// 18.10.13 ohara L1*3+L3*3+R1*3+R3*3
		valConvHR_R = new double[valPeakCountsR.Length] ;		// 18.10.13 ohara L1*3+L3*3+R1*3+R3*3
		valPeakAvePt = 0 ;						// [181025.2]
		valPeakAveSize = 0 ;					// [181025.2]
		valPeakAveDatas = new double[10] ;	// [181025.2]
    }
}
