using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonValue : SingletonMonoBehaviour<CommonValue> 
{
	[System.Serializable]
	public class CommonData
	{
		public string Label;
		public string Value;
	}

	JsonArray<CommonData> m_Data;

	public void Init()
	{
		m_Data =  JsonUtility.FromJson<JsonArray<CommonData>>( JsonFileMgr.Instance.GetJsonString("CommonValue", true) );
	}

	public string GetString( string Label )
	{
		for( int i=0; i<m_Data.DataArray.Length; i++)
		{
			if( m_Data.DataArray[i].Label == Label )
			{
				return m_Data.DataArray[i].Value;
			}
		}

		Debug.Log( "CommonValueにKey:" + Label + "はない" );

		return null;
	}

	public int GetInt( string Label )
	{
		return int.Parse( GetString( Label ) );
	}

	public float GetFloat( string Label )
	{
		return float.Parse( GetString( Label ) );
	}


}
