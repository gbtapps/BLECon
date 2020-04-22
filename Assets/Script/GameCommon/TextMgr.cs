using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMgr : SingletonMonoBehaviour<TextMgr> 
{
	[System.Serializable]
	public class TextData
	{
		public string Label;
		public string JP;
/*		public string EN;
		public string CH;
		public string KR;
*/
	}

	JsonArray<TextData> m_Data;

	public void Init()
	{
		string JsonStr = JsonFileMgr.Instance.GetJsonString("TextData", true);

		m_Data =  JsonUtility.FromJson<JsonArray<TextData>>( JsonStr );

		if( m_Data == null )
		{
			Debug.Log( "Data null");
		}


	}

	TextData FindData( string Label )
	{
		for( int i=0; i<m_Data.DataArray.Length; i++)
		{
			if( m_Data.DataArray[i].Label == Label )
			{
				return m_Data.DataArray[i];
			}
		}

		return null;
	}

	string Find( string Label )
	{
		TextData Data = FindData( Label );
		if( Data == null )
		{
			return "指定したLabel[" + Label + "]のテキストは無い";
		}
		return Data.JP;
	}

	public string Langage
	{
		get
		{
			return "jp";
		}
	}



	public string Get( string Label, Dictionary<string,string> Arg = null )
	{
		string Text = Find( Label );

		// 引数無しならおしまい
		if( Arg == null )
		{
			return Text;
		}

		// 引数が有る場合は置換
		foreach(KeyValuePair<string, string> Item in Arg) 
		{
			Text = Text.Replace( "|"+ Item.Key + "|", Item.Value );
		}

		return Text;
	}


}
