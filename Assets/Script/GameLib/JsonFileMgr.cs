using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonFileMgr : SingletonMonoBehaviour<JsonFileMgr>
{
	static public bool isBaseLoadDone
	{
		get
		{
			if( Instance == null )
			{
				return false;
			}

			if( Instance.m_FileLoader == null )
			{
				return false;
			}
			return Instance.m_FileLoader.assetBundle != null;
		}
	}

	private FileLoader m_FileLoader = null;


    protected override void Awake()
    {
        base.Awake();


		if (JsonFileMgr.Instance != this)
        {
            return;
        }

		// ファイル名作成
		string assetBundleName = "jsondata.asb";
		m_FileLoader = FileLoader.LoadAssetBundle( assetBundleName );
	}

	public string GetJsonString( string FileName, bool isArray )
	{
		if( isArray )
		{
			return "{ \"DataArray\":" +  m_FileLoader.assetBundle.LoadAsset<TextAsset>( FileName ).text + "}";
		}
		return m_FileLoader.assetBundle.LoadAsset<TextAsset>( FileName ).text;
	}

}
