using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutReproduction : LayoutImage
{
	public enum eAnchor // 横縦
	{ 
		LU,		// 左上
		LC,		// 左中
		LD,		// 左下
		
		CU,		// 中央上
		CC,		// 中央
		CD,		// 中央下

		RU,		// 右上
		RC,		// 右中
		RD,		// 右下

		Max
	}

	// Unity座標系でのアンカー
	static Vector2[] s_AnchorTbl =
	{
		new Vector2( 0, 1 ),
		new Vector2( 0, 0.5f ),
		new Vector2( 0, 0 ),

		new Vector2( 0.5f, 1 ),
		new Vector2( 0.5f, 0.5f ),
		new Vector2( 0.5f, 0 ),

		new Vector2( 1, 1 ),
		new Vector2( 1, 0.5f ),
		new Vector2( 1, 0 ),
	};

	public static Vector2 GetAnchor( string Anc )
	{
		eAnchor a = eAnchor.LU;

		for( eAnchor i=0; i<eAnchor.Max; i++ )
		{
			if( Anc.ToUpper() == i.ToString() )
			{
				a = i;
			}
		}

		return GetAnchor(a);
	}

	public static Vector2 GetAnchor( eAnchor Anc )
	{
		return s_AnchorTbl[(int)Anc];
	}

	public static Vector2 GetAnchorLayout( eAnchor Anc )
	{
		Vector2 AncV = GetAnchor( Anc );
		AncV.y = 1 - AncV.y;

		return AncV;
	}
	

	static public LayoutReproduction Create( Vector2 Ofs, string LayoutFileName, CanvasMgr.eDispSort DispZ, LayoutImage Parent = null)
	{

		LayoutData Data = LayoutFileMgr.Instance.GetLayout(LayoutFileName);//JsonUtility.FromJson<LayoutData>(JsonStr);

		return Create( Ofs, Data, DispZ, Parent);
	}

	static public LayoutReproduction Create( Vector2 Ofs, LayoutData Data, CanvasMgr.eDispSort DispZ, LayoutImage Parent = null)
	{
		GameObject Obj = new GameObject("root");
		LayoutReproduction Layout = Obj.AddComponent<LayoutReproduction>();

		if( Parent != null )
		{
			Obj.transform.parent = Parent.transform;
		}
		else
		{
			CanvasMgr.Instance.EntryDispObj(Obj, DispZ);
		}
		Obj.transform.localScale = Vector3.one;
		
		Data.ow = CanvasMgr.CanvasSize.x;
		Data.oh = CanvasMgr.CanvasSize.y;

		Layout.Create(Data, Ofs);

		return Layout;
	}


	public void Create(LayoutData Data, Vector2 Ofs)
	{
		Create(Data);
		Pos += Ofs;
	}

	private IEnumerator LoadingWait()
	{
		yield return 0;
	}



}
