/**
 * デバイス片側AC計測データ
 */
public class Hot2gMeasData1chAc
{
    public int			valResultCount ;		// リザルト送信カウンタ
    public int[]		valAcValues ;			// L1,L3 AC値
    public int[]		valSaturations ;		// L1,L3 AC内飽和データ個数
    public int			valOffData ;			// L,R LED-Off時のAC平均値
	public double[]	valBloodDensities ;	// ヘモグロビン濃度変化
	public double		valBrainBloodData ;	// 脳活動指標

    /**
     * コンストラクタ
     */
    public Hot2gMeasData1chAc()
    {
        valResultCount = 0 ;
        valAcValues = new int[2] ;
        valSaturations = new int[2] ;
		valOffData = 0 ;
        valBloodDensities = new double[2] ;
		valBrainBloodData = 0.0f ;
    }
}
