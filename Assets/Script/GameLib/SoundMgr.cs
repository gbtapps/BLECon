using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMgr : SingletonMonoBehaviour<SoundMgr>
{
    public enum eType
    {
        BGM,
        SE,
    };

    public AudioMixer g_Mixer;
    public AudioMixerGroup g_GroupBGM;
    public AudioMixerGroup g_GroupSE;

    static public AudioMixerGroup GroupSE
    {
        get
        {
            if( Instance == null )
            {
                return null;
            }
            return Instance.g_GroupSE;
        }
    }

    static public AudioMixerGroup GroupBGM
    {
        get
        {
            if( Instance == null )
            {
                return null;
            }
            return Instance.g_GroupBGM;
        }
    }

	public bool isLoading
	{
		get
		{
			return 0 < m_LoadingBankCounter;
		}
	}

    const int BgmSourceMax = 2;
    int m_NextUseBGM = 0;
    SoundPlayer[] m_BgmPlayer;
    AudioSource[] m_SeSource;

	//Dictionary<string, FileLoader> m_Loader;
	Dictionary<string,AudioClip> m_BGM;
	Dictionary<string, SeBank> m_Bank;
	int m_LoadingBankCounter = 0;

    const int SeSourceMax = 2;


    float m_MasterVolume = 1f;
    float m_BgmVolume = 1f;
    float m_SeVolume = 1f;

    AudioSource m_lastBgmSorce = null;

    public int NowUseBGM
    {
        get
        {
            return (m_NextUseBGM + 1) % 2;
        }
    }

    public float MasterVolume
    {
        get
        {
            return m_MasterVolume;
        }

        set
        {
            m_MasterVolume = value;
            g_Mixer.SetFloat("MasterVolume",  CalcVolume(m_MasterVolume) );
        }
    }

    public float BgmVolume
    {
        get
        {
            return m_BgmVolume;
        }

        set
        {
            m_BgmVolume = value;
            g_Mixer.SetFloat("BGMVolume", CalcVolume(m_BgmVolume));
        }
    }

    public float SeVolume
    {
        get
        {
            return m_SeVolume;
        }

        set
        {
            m_SeVolume = value;
            g_Mixer.SetFloat("SEVolume", CalcVolume(m_SeVolume));
        }
    }


    float CalcVolume( float Vol )
    {
        if( 0.2f <= Vol )
        {
            return -20f + 20f * ((Vol - 0.2f) / 0.8f);
        }
        return -80f + 60f * (Vol / 0.2f); 
    }


	// Use this for initialization
	void Start () 
    {
        m_SeSource = new AudioSource[SeSourceMax];
        for(int i=0 ; i<SeSourceMax ; ++i )
        {
            m_SeSource[i] = gameObject.AddComponent<AudioSource>();
            m_SeSource[i].outputAudioMixerGroup = SoundMgr.GroupSE;
			m_SeSource[i].spatialBlend = 0;
            m_SeSource[i].loop = false;
        }

        m_BgmPlayer = new SoundPlayer[BgmSourceMax];

        for( int i=0; i<BgmSourceMax; i++)
        {
            m_BgmPlayer[i] = gameObject.AddComponent<SoundPlayer>();
            m_BgmPlayer[i].Init(eType.BGM);
        }

		m_Bank = new Dictionary<string, SeBank>();
		m_BGM = new Dictionary<string, AudioClip>();
	}

	public void LoadBank( string BankName )
	{
		if( m_Bank.ContainsKey(BankName) )
		{
			return;
		}

		m_LoadingBankCounter++;

		m_Bank.Add( BankName, null );

		StartCoroutine(LoadAssetBundleCoreAsync(BankName));
	}

	//非同期読み込み
	private IEnumerator LoadAssetBundleCoreAsync(string BankName)
	{
		// ファイル名作成
		string assetBundleName = BankName + ".asb";
		
		// ファイルロード開始
		FileLoader Loader = FileLoader.LoadAssetBundle( assetBundleName );

		yield return new WaitWhile(() => Loader.assetBundle == null);

		// バンクオブジェクトのロード
		Loader.LoadObject( BankName );
		
		yield return new WaitWhile(() => Loader.GetObject(BankName) == null);

		// バンク作成
		GameObject BankObj =Instantiate(Loader.GetObject<GameObject>(BankName), transform) as GameObject;

		m_Bank[BankName] = BankObj.GetComponent<SeBank>();
		m_Bank[BankName].Init();

		// アセットバンドルのアンロード
		Loader.UnloadAseetBundle();

		// 読み込みカウンタのデクリメント
		m_LoadingBankCounter--;
		if( m_LoadingBankCounter < 0 )
		{
			m_LoadingBankCounter = 0;
		}
	}


    public AudioSource GetNowSource()
    {
        return m_lastBgmSorce;
    }

    public void PauseBGM( )
    {
		m_BgmPlayer[NowUseBGM].Pause();
    }
    
    public void ForcePauseBGM(bool is_pause)
    {
        if(is_pause==true)
        {
            m_BgmPlayer[NowUseBGM].ForcePause();
        }
        else
        {
            m_BgmPlayer[NowUseBGM].ForceUnPause();
        }
    }

    public void StopBGM( float FadeTime = 1f, bool isAll = true )
    {
		if (isAll)
		{
			for (int i = 0; i < BgmSourceMax; i++)
			{
				if (FadeTime <= 0f)
				{
					m_BgmPlayer[i].Stop();
				}
				else
				{
					m_BgmPlayer[i].StopFadeout(FadeTime);
				}
			}
		}
		else
		{
			int i = NowUseBGM;
			if (FadeTime <= 0f)
			{
				m_BgmPlayer[i].Stop();
			}
			else
			{
				m_BgmPlayer[i].StopFadeout(FadeTime);
			}
		}
        m_lastBgmSorce = null;
    }

	public void StopLoopSE( bool force = true)
	{
		int i= 1;
            if(force==true)
            {//強制終了
                m_SeSource[i].Stop();
                m_SeSource[i].clip = null;
            }
            else
            {//ループフラグだけ倒して次で終わるように仕向ける
                if( m_SeSource[i].isPlaying==true)
                {
                    m_SeSource[i].loop = false;
                }
            }
	}

    public void StopSE(bool force=true)
    {
        for (int i = 0; i < SeSourceMax; i++)
        {
            if(force==true)
            {//強制終了
                m_SeSource[i].Stop();
                m_SeSource[i].clip = null;
            }
            else
            {//ループフラグだけ倒して次で終わるように仕向ける
                if( m_SeSource[i].isPlaying==true)
                {
                    m_SeSource[i].loop = false;
                }
            }
        }
    }

    public bool PlaySE( string bank, string Key, bool loop=false )
    {
		if( !m_Bank.ContainsKey(bank) )
		{
			Debug.Log( "SEBank:["+ bank + "]は存在しない");
			return false;
		}

		AudioClip c = m_Bank[bank].GetAudioClip( Key );
		if( c == null )
		{
			Debug.Log( "SEBank:["+ bank + "]にKey:["+Key+"]は存在しない");
			return false;
		}


        if(loop==true)
        {
            m_SeSource[1].loop = loop;
            m_SeSource[1].clip = c;
            m_SeSource[1].PlayDelayed(0);
        }
        else
        {
            m_SeSource[0].PlayOneShot(c);
        }
		
        return true;
    }

	public void LoadBGM( string Name )
	{
		if( m_BGM.ContainsKey(Name) )
		{
			return;
		}

		m_LoadingBankCounter++;

		m_BGM.Add( Name, null );

		StartCoroutine(LoadBGMAsync(Name));
	}

	//非同期読み込み
	private IEnumerator LoadBGMAsync(string Name)
	{
		// ファイル名作成
		string assetBundleName = Name + ".asb";
		
		// ファイルロード開始
		FileLoader Loader = FileLoader.LoadAssetBundle( assetBundleName );

		yield return new WaitWhile(() => Loader.assetBundle == null);

		// バンクオブジェクトのロード
		Loader.LoadObject( Name );
		
		yield return new WaitWhile(() => Loader.GetObject(Name) == null);

		// バンク作成
		GameObject BankObj =Instantiate(Loader.GetObject<GameObject>(Name), transform) as GameObject;

		m_BGM[Name] = BankObj.GetComponent<AudioSource>().clip;

		// アセットバンドルのアンロード
		Loader.UnloadAseetBundle();

		// 読み込みカウンタのデクリメント
		m_LoadingBankCounter--;
		if( m_LoadingBankCounter < 0 )
		{
			m_LoadingBankCounter = 0;
		}
	}

    public bool PlayBGM( string Key, float FadeTime = 1.0f)
    {
        m_lastBgmSorce = null;

        if( !m_BGM.ContainsKey(Key) )
        {
			return false;
        }

        if (m_BgmPlayer[NowUseBGM].AudioClip == m_BGM[Key])
        {
            m_BgmPlayer[NowUseBGM].PauseCheck();
            m_lastBgmSorce = m_BgmPlayer[NowUseBGM].source;
            return true;
        }

        m_BgmPlayer[m_NextUseBGM].Play(m_BGM[Key], FadeTime);
        m_lastBgmSorce = m_BgmPlayer[m_NextUseBGM].source;
		m_lastBgmSorce.loop = true;

        m_NextUseBGM = (m_NextUseBGM + 1) % 2;

        return true;
    }

}