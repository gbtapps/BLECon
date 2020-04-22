using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsbUI : MonoBehaviour
{
	public delegate void Callback();

	public static AsbUI CreateAsbUI(Vector2 Ofs, string FileName, CanvasMgr.eDispSort DispZ)
	{
		return CreateAsbUI(Ofs, FileName, DispZ, null);
	}

	public static AsbUI CreateAsbUI(Vector2 Ofs, string FileName, GameObject Parent)
	{
		return CreateAsbUI(Ofs, FileName, CanvasMgr.eDispSort.None, Parent);
	}


	static AsbUI CreateAsbUI(Vector2 Ofs, string FileName, CanvasMgr.eDispSort DispZ, GameObject Parent)
	{
		GameObject Obj = new GameObject("AsbUI_" + FileName);

		if (Parent != null)
		{
			Obj.transform.SetParent(Parent.transform, false);
		}
		else
		{
			CanvasMgr.Instance.EntryDispObj(Obj, DispZ);
		}

		RectTransform rt = Obj.GetComponent<RectTransform>();
		if (rt == null)
		{
			rt = Obj.AddComponent<RectTransform>();
		}
		RectTransform prt = rt.parent.gameObject.GetComponent<RectTransform>();

		/*rt.anchorMax = prt.anchorMax;
        rt.anchorMin = prt.anchorMin;
        rt.pivot = prt.pivot;*/
		rt.sizeDelta = prt.sizeDelta;
		rt.localPosition = Vector3.zero;
		rt.localScale = Vector3.one;
		rt.localEulerAngles = Vector3.zero;




		AsbUI asbUI = Obj.AddComponent<AsbUI>();
		asbUI.Init(Ofs, FileName);

		return asbUI;
	}

	public bool isLoadDone
	{
		get
		{
			return m_isLoadDone;
		}
	}

	bool m_isLoadDone = false;
	Vector2 m_Ofs;
	Callback m_Callback = null;
	RectTransform m_RectTransform = null;

	public RectTransform rectTransform
	{
		get
		{
			return m_RectTransform;
		}
	}

	void Init(Vector2 Ofs, string FileName)
	{
		m_Ofs = Ofs;
		// 読み込み
		StartCoroutine(LoadAssetBundleCoreAsync(FileName));
	}

	public void SetCallback(Callback cb)
	{
		m_Callback = cb;
	}

	public GameObject FindDescendant(string Name)
	{
		return gameObject.FindDescendant(Name);
	}

	public T FindDescendant<T>(string Name)
	{
		return gameObject.FindDescendant<T>(Name);
	}


    private IEnumerator LoadAssetBundleCoreAsync(string FileName)
    {
        // ファイル名作成
        string assetBundleName = "ly_" + FileName + ".asb";

        // ファイルロード開始
        FileLoader Loader = FileLoader.LoadAssetBundle(assetBundleName);

        yield return new WaitWhile(() => Loader.assetBundle == null);

        // バンクオブジェクトのロード
        Loader.LoadObject(FileName);

        yield return new WaitWhile(() => Loader.GetObject(FileName) == null);

        // バンク作成
        GameObject Obj = Instantiate(Loader.GetObject<GameObject>(FileName), transform) as GameObject;
        Obj.transform.SetParent(transform, false);

        RectTransform crt = Obj.GetComponent<RectTransform>();
        RectTransform rt = transform.GetComponent<RectTransform>();
        rt.pivot = crt.pivot;
        rt.anchorMin = crt.pivot;
        rt.anchorMax = crt.pivot;




        
        crt.localPosition = m_Ofs;
        m_RectTransform = crt;

        // プレハブ内のイメージ情報からスプライトの画像を復元
        LPIInfo[] ImgInfo = Obj.GetComponentsInChildren<LPIInfo>();
        for( int i=0; i<ImgInfo.Length; i++ )
        {
            Image Img = ImgInfo[i].gameObject.GetComponent<Image>();

            if( Img == null )
            {
                Debug.Log(ImgInfo[i].gameObject.name + "に画像情報があるのにImageオブジェクトがない");
                continue;
            }

            Img.sprite = SpriteBankMgr.Instance.GetSprite(ImgInfo[i].image_name);
        }

        // ボタンに便利クラス張り付けておく
        Button[] Btn = Obj.GetComponentsInChildren<Button>();
        for( int i=0; i<Btn.Length; i++ )
        {
            Btn[i].gameObject.AddComponent<ExButton>();
        }

        // アセットバンドルのアンロード
        Loader.UnloadAseetBundle();
        m_isLoadDone = true;

        if(m_Callback != null)
        {
            m_Callback();
        }
        
    }







}
