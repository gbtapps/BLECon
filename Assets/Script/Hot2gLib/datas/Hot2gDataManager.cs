/**
 * デバイス情報データ
 */
public class Hot2gDataManager
{
	public Hot2gDevInfo			DevInfo ;
	public Hot2gDevState		DevState ;
	public Hot2gLocalInfo		LocalInfo ;
	public Hot2gAgcResult		AgcResult ;

	/**
	 * コンストラクタ
	 */
	public Hot2gDataManager()
	{
		DevInfo = new Hot2gDevInfo() ;
		DevState = new Hot2gDevState() ;
		LocalInfo = new Hot2gLocalInfo() ;
		AgcResult = new Hot2gAgcResult() ;
	}
}
