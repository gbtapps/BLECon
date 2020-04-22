using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutFileMgr : SingletonMonoBehaviour<LayoutFileMgr>
{
	static public bool isBaseLoadDone
	{
		get
		{
			if( Instance == null )
			{
				return false;
			}

			if( Instance.m_LayoutFileLoader == null )
			{
				return false;
			}

			if( 0 < Instance.m_FontLoadingCount )
			{
				return false;
			}

			return Instance.m_LayoutFileLoader.assetBundle != null;
		}
	}

	private FileLoader m_LayoutFileLoader = null;


    protected override void Awake()
    {
        base.Awake();


		if (LayoutFileMgr.Instance != this)
        {
            return;
        }

		// ファイル名作成
		string assetBundleName = "layoutdata.asb";
		m_LayoutFileLoader = FileLoader.LoadAssetBundle( assetBundleName );
	}

	public LayoutData GetLayout( string FileName )
	{
		return JsonUtility.FromJson<LayoutData>( m_LayoutFileLoader.assetBundle.LoadAsset<TextAsset>( FileName ).text );
	}


	string m_DefaltFontName = null;
	Dictionary<string,Font> m_Fonts = null;
	int m_FontLoadingCount = 0;

	public void InitFont( string[] FontNameList, string[] FontFileNameList )
	{
		m_Fonts = new Dictionary<string, Font>();
		if( FontNameList == null )
		{
			m_DefaltFontName = "Arial";
			m_Fonts.Add( "Arial", Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font );
		}
		else
		{
			m_DefaltFontName = FontNameList[0];
			for( int i=0; i<FontNameList.Length; i++ )
			{
				m_FontLoadingCount++;
				StartCoroutine( FontLoadAsync(FontNameList[i], FontFileNameList[i] ) ); 
			}
		}
	}

	// ロードする
	private IEnumerator FontLoadAsync(string FontName, string FontFileName)
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<Font> (FontFileName);

		while(!resourceRequest.isDone)
		{
			yield return 0;
		}

		m_Fonts.Add( FontName, resourceRequest.asset as Font );

		m_FontLoadingCount--;
	}

	public Font GetFont( string FontName )
	{
		if( FontName == null || FontName == "" )
		{
			Debug.Log( "フォント名が空なのでデフォルトフォントを使います。" );
			return m_Fonts[m_DefaltFontName];
		}

		if( m_Fonts.ContainsKey(FontName) )
		{
			return m_Fonts[FontName];
		}

		Debug.Log( FontName + "といフォントは読み込んでないのでデフォルトフォントを使います。" );

		return m_Fonts[m_DefaltFontName];
	}

}
