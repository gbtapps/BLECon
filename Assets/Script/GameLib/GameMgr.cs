using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour 
{
    public enum eMode
    {
        None,
        ReadyScene,
        LoadingWait,
        InitScene,
        Main,
        ExitScene,
        ChangeScene,
    };



    public enum eChangeSceneStep 
    {
        CallVoidScene,
        UnloadAsset,
        GC,
        End,
    };

    const float DefaultFadeTime = 0.5f;

    // 実体
    static GameMgr s_Instance = null;

    // 外部アクセス用
    public static GameMgr Instance { get { return s_Instance; } }

    public static bool isInitDone { get { return eMode.None < s_Instance.m_Mode; } }

	public static bool isExitScene{ get{ return eMode.ExitScene == s_Instance.m_Mode; }}

    public static GUIStyle DebugGUIStyle { get { return s_Instance.m_GUIStyle; } }

	// 汎用入力待ち
	public static bool InputWait()
	{
		TouchInputMgr.TouchData Data = TouchInputMgr.Instance.FindTouchData(0);
		if( Data != null )
		{
			if( Data.State == TouchInputMgr.eTouchState.Down )
			{
				return true;
			}
		}

        //return PadMgr.Instance.GetPad(0).GetKeyDown( PadData.eBtn.A );
        return false;
	}

    // デバッグ用GuiStyle
    public GUIStyle m_GUIStyle;

    // ゲーム内共通プレハブ
    public GameObject[] g_CommonPrefab;

    // 初期化用カウンタ
    int m_InitCounter = 0;

	bool m_JsonUseInitDone = false;
    bool m_FooterInitDone = false;

    // モード
    eMode m_Mode = eMode.None;
    
    // シーンチェンジステップ
    eChangeSceneStep m_Step = eChangeSceneStep.CallVoidScene;

    // 現在のシーン
	GameDefine.eScene m_Scene = GameDefine.eScene.None;

	// 次のシーン
	GameDefine.eScene m_NextScene = GameDefine.eScene.None;

	public eMode Mode
    {
        get
        {
            return m_Mode;
        }
    }

	public eChangeSceneStep ChangeSceneStep
	{
		get
		{
			return m_Step;
		}
	}

	public GameDefine.eScene Scene
	{
		get
		{
			return m_Scene;
		}
	}

	// Use this for initialization
	void Awake () 
    {
        if( s_Instance != null )
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;

		QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // シーン変更で壊れない   今回は他でやってるのでいらない
//        DontDestroyOnLoad(this.gameObject);
        /*
        // 今のシーンを設定しておく
		for (int i = 0; i < (int)GameDefine.eScene.Max; i++)
        {
			if (GameDefine.SceneNameTbl[i] == SceneManager.GetActiveScene().name)
            {
				m_Scene = (GameDefine.eScene)i;
            }
        }

		if (m_Scene == GameDefine.eScene.None)
        {
            Debug.Log(SceneManager.GetActiveScene().name + "はGameMgrに登録されていないシーンです。");
        }
        */

        BLEMgr.Create("BLEMgr", transform);
        Hot2gApplication.Create("Hot2gApplication", transform);
//		CanvasMgr.Create( "CanvasMgr", transform );
//		SpriteBankMgr.Create( "SpriteBankMgr", transform );
//		LayoutFileMgr.Create( "LayoutFileMgr", transform);
//		JsonFileMgr.Create( "JsonFileMgr", transform);
//		TouchInputMgr.Create( "TouchInputMgr", transform );
		//PadMgr.Create( "PadMgr", transform );
		//AccountMgr.Create( "AccountMgr", transform );
		//SaveDataAccess.Create( "SaveDataAccess", transform );
		//SaveDataMgr.Create( "SaveDataMgr", transform );
//		LayoutFileMgr.Instance.InitFont( null, null );
//		TTNSaveData.Create("TTNSaveData", transform);
//		TTNSaveData.Instance.Init();

		// スリープ無効
		Screen.sleepTimeout = SleepTimeout.NeverSleep;


		// プレハブの実体化
		if (g_CommonPrefab != null)
        {
            for (int i = 0; i < g_CommonPrefab.Length; i++)
            {
                GameObject Obj = Instantiate(g_CommonPrefab[i]);
                Obj.transform.SetParent(gameObject.transform, false);
            }
        }
	}

    void OnDestroy()
    {
        if (s_Instance == this)
        {
            s_Instance = null;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        return;
        switch( m_Mode )
        {
			case eMode.None:

#if ASSET_BUNDLE_DOWNLOAD
                if( 3 == m_InitCounter )
                {
                    // データダウンロード以降のシーン
                    if( eScene.DataDownload <= m_Scene )
                    {
                        AssetBundleMgr.Instance.StartDownload();
                    }
                    m_InitCounter++;
                }
                else if ( 4 <= m_InitCounter)
                {
                    if (AssetBundleMgr.Instance.Step == AssetBundleMgr.eStep.Idle || AssetBundleMgr.Instance.Step == AssetBundleMgr.eStep.OK)
                    {
                        // シーンの初期化
                        SceneBase NowScene = FindObjectOfType<SceneBase>();
                        if (NowScene != null)
                        {
                            NowScene.Init();
                            NowScene.SetInitDone();
                        }
                        ScreenFader.Instance.SetFadeIN(DefaultFadeTime);
                        m_Mode = eMode.Main;
                    }
                }
                else
                {
                    m_InitCounter++;
                }
#else
				if( 3 == m_InitCounter )
                {
					//SaveDataMgr.Instance.Load();
					SpriteBankMgr.Instance.LoadBank( GameDefine.eBank.Common );
//					SoundMgr.Instance.LoadBank( GameDefine.SOUND_BANK_SE );
					//SoundMgr.Instance.LoadBank( GameDefine.SOUND_BANK_JINGLE );


					//DebugMessageView.Create();
		

					m_InitCounter++;
				}
				else if (4 <= m_InitCounter)
				{
					// シーンの初期化
					SceneBase NowScene = FindObjectOfType<SceneBase>();
					if (NowScene != null)
					{
                        NowScene.LoadSpriteBank();
						NowScene.LoadBGM();                    
                    }
					
					m_Mode = eMode.LoadingWait;
				}
				else
				{
					m_InitCounter++;
				}

#endif

			break;          

            case eMode.ReadyScene:

				if (SceneManager.GetActiveScene().name == GameDefine.SceneNameTbl[(int)m_NextScene])
                {
                    m_Scene = m_NextScene;
					m_NextScene = GameDefine.eScene.None;

                    // シーンの初期化
                    SceneBase NowScene = FindObjectOfType<SceneBase>();
					if (NowScene != null)
					{
                        NowScene.LoadSpriteBank();
						NowScene.LoadBGM();
					}
					
					m_Mode = eMode.LoadingWait;
                }
            break;

            case eMode.LoadingWait:
			{
                /* moritomi
				if( !LayoutFileMgr.isBaseLoadDone )
				{
					return;
				}
                */

                if( SpriteBankMgr.Instance.isLoading )
                {
					return;
				}
                else
                {
                    if(!m_FooterInitDone)
                    {
                        m_FooterInitDone = true;
//                        BMFooter.Create();
                        return;
                    }


                    /*
                    if (BMFooter.Instance.isLoading)
                    {
                        return;
                    }
                    */
                }
                /*
				if( SoundMgr.Instance.isLoading )
				{
					return;
				}
                */



				if( !JsonFileMgr.isBaseLoadDone )
				{
					return;
				}
				else
				{
					// Json使う系の初期化
					if( !m_JsonUseInitDone )
					{
						//TextMgr.Create("TextMgr", transform);
						//TextMgr.Instance.Init();

						//CommonValue.Create("CommonValue", transform);
						//CommonValue.Instance.Init();

						m_JsonUseInitDone = true;
					}
				}
                

                SceneBase NowScene = FindObjectOfType<SceneBase>();
				NowScene.Init();


                m_Mode = eMode.InitScene;

            }
            break;

            case eMode.InitScene:
            {
                SceneBase NowScene = FindObjectOfType<SceneBase>();
                if( NowScene.isInitDone)
                {
                    NowScene.SetExecFlag();
				    ScreenFader.Instance.SetFadeIN(DefaultFadeTime);
				    m_Mode = eMode.Main;
                }
			}
            break;

            case eMode.Main:

                // 通常時は30フレーム毎にGC
                if (Time.frameCount % 30 == 0)
                {
                    System.GC.Collect();
                }

            break;

            case eMode.ExitScene:

                if (ScreenFader.isFadeEnd)
                {
                    m_Mode = eMode.ChangeScene;
                }

            break;

            case eMode.ChangeScene: 
                Update_ChangeScene();
            break;

        }
	}

    void Update_ChangeScene()
    {


        switch( m_Step )
        {
            case eChangeSceneStep.CallVoidScene:
				//SaveDataMgr.Instance.Save();
                SceneManager.LoadScene("SceneChange", LoadSceneMode.Single);
                m_Step = eChangeSceneStep.UnloadAsset;
            break;

            case eChangeSceneStep.UnloadAsset:
                if( SceneManager.GetActiveScene().name != "SceneChange" )
                {
                    return;
                }
                CanvasMgr.Instance.SceneChange();
				SpriteBankMgr.Instance.UnloadBankSceneEnd();
                Resources.UnloadUnusedAssets();
                //AssetBundleMgr.Instance.UnloadUnuseAll(false);
				//TextureMgr.Instance.UnloadUnuseAll(false);
                m_Step = eChangeSceneStep.GC;
            break;

            case eChangeSceneStep.GC:
                System.GC.Collect();
                m_Step = eChangeSceneStep.End;
            break;

            case eChangeSceneStep.End:
				SceneManager.LoadScene(GameDefine.SceneNameTbl[(int)m_NextScene], LoadSceneMode.Single);
                m_Mode = eMode.ReadyScene;
            break;
        }

    }

	public bool ChangeScene(GameDefine.eScene NextScene)
    {
        return ChangeScene(NextScene, true, Color.black, DefaultFadeTime);
    }

	public bool ChangeScene(GameDefine.eScene NextScene, bool isFade, Color FadeColor, float FadeTime)
    {
        if( m_Mode != eMode.Main )
        {
            Debug.Log("ChangeSceneError: シーンの変異はすでに始まっているため上書きできません。");
            return false;
        }

        m_Step = eChangeSceneStep.CallVoidScene;
        m_NextScene = NextScene;

        if (isFade)
        {
            m_Mode = eMode.ExitScene;
            ScreenFader.Instance.SetFadeOut(FadeColor, FadeTime);
        }
        else
        {
            m_Mode = eMode.ChangeScene;
        }
        return true;
    }

}
