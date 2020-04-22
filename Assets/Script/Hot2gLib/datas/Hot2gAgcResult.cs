/**
 * デバイス自動ゲイン調整結果データ
 */
public class Hot2gAgcResult
{
    public int[] valSwGains ;			// L1,L3,R1,R3 SWゲイン値
    public int[] valPmGains ;			// L1,L3,R1,R3 PMゲイン値
    public int[] valBrights ;			// L,R 光量値

    /**
     * コンストラクタ
     */
    public Hot2gAgcResult()
    {
        valSwGains = new int[4] ;
        valPmGains = new int[4] ;
        valBrights = new int[2] ;
    }
}
