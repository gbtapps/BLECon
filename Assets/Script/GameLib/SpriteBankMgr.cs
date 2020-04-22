using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBankMgr : SingletonMonoBehaviour<SpriteBankMgr>
{
	// ロード中？
	public bool isLoading
	{
		get
		{
			return 0 < m_ExecCounter;
		}
	}

	private int m_ExecCounter = 0;
	//private SpriteBank[] m_Bank;
	private Dictionary<GameDefine.eBank,SpriteBank> m_Bank = null;

	public SpriteBankMgr()
	{
		m_Bank = new Dictionary<GameDefine.eBank, SpriteBank>();
	}

	// バンク読み込み開始
	public void LoadBank( GameDefine.eBank Bank )
	{
		if( m_Bank.ContainsKey(Bank) )
		{
			//if( m_Bank[Bank] != null )
			{
				return;
			}
		}

		m_Bank.Add( Bank, null );

		// 読み込みカウントインクリメント
		m_ExecCounter++;

		// 読み込み
		StartCoroutine(LoadAssetBundleCoreAsync(Bank));
	}

	public void UnloadBank( GameDefine.eBank Bank )
	{
		if( !m_Bank.ContainsKey(Bank) )
		{
			return;
		}

		Destroy( m_Bank[Bank].gameObject );
		m_Bank.Remove( Bank );
	}

	public void UnloadBankSceneEnd()
	{
#if false
        for( GameDefine.eBank i=GameDefine.eBank.Start; i<GameDefine.eBank.Common; i++ )
		{
            /*
			switch( i )
			{
				case GameDefine.eBank.EN:
					if( SaveDataMgr.Instance.Langage == SaveDataMgr.eLangage.EN )
					{
						continue;
					}
				break;
				case GameDefine.eBank.CN:
					if( SaveDataMgr.Instance.Langage == SaveDataMgr.eLangage.CN )
					{
						continue;
					}
				break;
				case GameDefine.eBank.KR:
					if( SaveDataMgr.Instance.Langage == SaveDataMgr.eLangage.KR )
					{
						continue;
					}
				break;
			}*/
			UnloadBank(i);
		}
#endif
	}

	//非同期読み込み
	private IEnumerator LoadAssetBundleCoreAsync(GameDefine.eBank Bank)
	{
		// ファイル名作成
		string assetBundleName = "sb_" + GameDefine.BankNameTbl[(int)Bank] + ".asb";
		
		// ファイルロード開始
		FileLoader Loader = FileLoader.LoadAssetBundle( assetBundleName );

		yield return new WaitWhile(() => Loader.assetBundle == null);

		// バンクオブジェクトのロード
		Loader.LoadObject( GameDefine.BankNameTbl[(int)Bank] );
		
		yield return new WaitWhile(() => Loader.GetObject(GameDefine.BankNameTbl[(int)Bank]) == null);

		// バンク作成
		GameObject BankObj =Instantiate(Loader.GetObject<GameObject>(GameDefine.BankNameTbl[(int)Bank]), transform) as GameObject;

		m_Bank[Bank] = BankObj.GetComponent<SpriteBank>();

		// アセットバンドルのアンロード
		Loader.UnloadAseetBundle();

		// 読み込みカウンタのデクリメント
		m_ExecCounter--;
		if( m_ExecCounter < 0 )
		{
			m_ExecCounter = 0;
		}
	}

	// 全バンクからスプライトを取得
	public Sprite GetSprite( string name )
	{
		if( name == null )
		{
			return null;
		}
        /*
		for( GameDefine.eBank i =GameDefine.eBank.EN; i<=GameDefine.eBank.KR; i++ )
		{
			switch( i )
			{
				case GameDefine.eBank.EN:
					if( SaveDataMgr.Instance.Langage != SaveDataMgr.eLangage.EN )
					{
						continue;
					}
				break;
				case GameDefine.eBank.CN:
					if( SaveDataMgr.Instance.Langage != SaveDataMgr.eLangage.CN )
					{
						continue;
					}
				break;
				case GameDefine.eBank.KR:
					if( SaveDataMgr.Instance.Langage != SaveDataMgr.eLangage.KR )
					{
						continue;
					}
				break;
			}
			if( m_Bank[i] != null )
			{
				Sprite sp = GetSprite( i, name );
				if( sp != null )
				{
					return sp;
				}
			}
		}
        */
		foreach (GameDefine.eBank key in m_Bank.Keys) 
		{
            /*
			switch( key )
			{
				case GameDefine.eBank.EN:
				case GameDefine.eBank.CN:
				case GameDefine.eBank.KR:
					continue;
			}
            */

			if( m_Bank[key] != null )
			{
				Sprite sp = GetSprite( key, name );
				if( sp != null )
				{
					return sp;
				}
			}
		}

		return null;
	}

	public Sprite GetSprite( GameDefine.eBank Bank, string name )
	{
		for( int j=0; j<m_Bank[Bank].FileNameList.Length; j++ )
		{
			if( m_Bank[Bank].FileNameList[j] == name )
			{
				return m_Bank[Bank].SpriteList[j];
			}
		
		}
		return null;
	}
}
