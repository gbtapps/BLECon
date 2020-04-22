using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CanvasMgr : SingletonMonoBehaviour<CanvasMgr>
{
	public static Vector2 BaseResolution = new Vector2(GameDefine.CANVAS_BASE_SIZE_X, GameDefine.CANVAS_BASE_SIZE_Y);
	static RectTransform s_CanvasBaseRectTransform;

	public static Vector2 CanvasSize
	{
		get
		{
			return s_CanvasBaseRectTransform.sizeDelta;
		}
	}

	public static Vector2 CanvasScale
	{
		get
		{
			return s_CanvasBaseRectTransform.lossyScale;
		}
	}


    // 標準描画順 SortOrder=1000 - (eDisp+1) * 50
    // SortOrderは大きいほど手前
    // 各自シーン内のCanvasは100以下のSortOrderを使うこと
    public enum eDispSort
    {
        Debug,          // デバッグCanvas
        System,         // システムCanvas
        Fader,          // フェード用Canvas
		Dialog,
        Footer,
		
		Front,			// 通常表示最前面
		UI,				// UI等
		Board,			// 盤面
		BG,				// 背景

        Max,

		CommonMax = Front,

		None = -1,
    };

	// enum を toStringすると基本名より別名が優先されてしまうので
	string[] CanvasName =
	{
		"Debug",
		"System",
		"Fader",
		"Dialog",
        "Footer",


        "Front",
		"UI",
		"Board",
		"BG",

	};

    Canvas[] m_Canvas = null;
	EventSystem m_EventSystem = null;

    protected override void Awake()
    {
        base.Awake();


		if (CanvasMgr.Instance != this)
        {
            return;
        }

		m_Canvas = new Canvas[(int)CanvasMgr.eDispSort.Max];
		for (eDispSort i = 0; i < CanvasMgr.eDispSort.Max; i++)
        {
			CreateCanvas(i); 
        }

		s_CanvasBaseRectTransform = m_Canvas[0].GetComponent<RectTransform>();


		//Debug.Log( m_Canvas[0]. );
		//Debug.Log( );

		CreateScreenFader();

    }

	void Start()
	{
		//Debug.Log(m_Canvas[0].GetComponent<Transform>().lossyScale);
	}

	void CreateCanvas(eDispSort Idx)
	{
		if( m_EventSystem == null )
		{
			CreateEventSystem();
		}

		GameObject Obj = new GameObject("Canvas" + CanvasName[(int)Idx]);
		Obj.transform.parent = transform;
		Canvas c = Obj.AddComponent<Canvas>();

		c.renderMode = RenderMode.ScreenSpaceOverlay;
		c.sortingOrder = 1000 - ((int)Idx * 100);
	
		// Canvasのスケール調整に必要
		CanvasScaler Scaler = Obj.AddComponent<CanvasScaler>();
		Scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		Scaler.referenceResolution = new Vector2(BaseResolution.x, BaseResolution.y);
		Scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		Scaler.matchWidthOrHeight = 0;
		Scaler.referencePixelsPerUnit = 100;

		// Canvas以下のオブジェクトのタッチに必要
		Obj.AddComponent<GraphicRaycaster>();

		m_Canvas[(int)Idx] = c;
	}

	void CreateEventSystem()
	{
		GameObject Obj = new GameObject("EventSystem");
		Obj.transform.parent = transform;

		EventSystem ev = Obj.AddComponent<EventSystem>();
		ev.sendNavigationEvents = true;
		ev.pixelDragThreshold = 5;


		StandaloneInputModule sim = Obj.AddComponent<StandaloneInputModule>();
		sim.inputActionsPerSecond = 10;
		sim.repeatDelay = 0.5f;
		sim.forceModuleActive = false;

		sim.horizontalAxis = "None";
		sim.verticalAxis = "None";
		sim.submitButton = "None";
		sim.cancelButton = "None";


		m_EventSystem = ev;
	}

	void CreateScreenFader()
	{
		GameObject Obj = new GameObject("ScreenFader");
		ScreenFader s = Obj.AddComponent<ScreenFader>();

		EntryUI(s);
		s.Init();
	}

    public DefaultUIBase CreateUI(DefaultUIBase Prefab)
    {
        DefaultUIBase UI = null;

        UI = Instantiate<DefaultUIBase>(Prefab);
        UI.transform.SetParent(m_Canvas[(int)UI.DispZ].transform, false);
        UI.Init();

        return UI;
    }

    public GameObject CreateDispObj(GameObject Prefab, eDispSort DispZ)
    {
        GameObject Obj = null;

        Obj = Instantiate(Prefab);
        Obj.transform.SetParent(m_Canvas[(int)DispZ].transform, false);

        return Obj;
    }

	public void EntryUI(DefaultUIBase UI)
	{
		EntryDispObj(UI.gameObject, UI.DispZ);
	}

    public void EntryDispObj(GameObject Obj, eDispSort DispZ)
    {
        Obj.transform.SetParent(m_Canvas[(int)DispZ].transform, false);
    }

    public void SceneChange( )
    {
		for (eDispSort i = eDispSort.CommonMax; i < CanvasMgr.eDispSort.Max; i++)
		{
			Transform DispSortTrans = m_Canvas[(int)i].transform;

			foreach (Transform child in DispSortTrans)
			{
				Destroy(child.gameObject);
			}
		}
    }

	public void Update()
	{
		/*
#if UNITY_EDITOR
		if( Input.GetButtonDown("Fire1") )
		{
			PlayEffect(Input.mousePosition);
		}
#else
		Touch[] t = Input.touches;
		for( int i=0; i<t.Length; i++ )
		{
			if( t[i].phase == TouchPhase.Began )
			{
				PlayEffect(t[i].position);
			}
		}
#endif
		 * */
	}



}
