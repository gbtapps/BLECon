/**
 * デバイス両側AC計測データ
 */
public class Hot2gMeasData2chAc
{
	public int			valResultCount ;		// リザルト送信カウンタ
	public int[]		valAcValues ;			// L1,L3,R1,R3 AC値
	public int[]		valSaturations ;		// L1,L3,R1,R3 AC内飽和データ個数
	public int[]		valOffDatas ;			// L,R LED-Off時のAC平均値
	public double[]	valBloodDensities ;	// ヘモグロビン濃度変化
	public double[]	valBrainBloodDatas ;	// L,R 脳活動指標

	/**
	 * コンストラクタ
	 */
	public Hot2gMeasData2chAc()
	{
		valResultCount = 0 ;
		valAcValues = new int[4] ;
		valSaturations = new int[4] ;
		valOffDatas = new int[2] ;
		valBloodDensities = new double[4] ;
		valBrainBloodDatas = new double[2] ;
	}
}
