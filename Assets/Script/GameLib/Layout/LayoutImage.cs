using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LayoutImage : MonoBehaviour
{
	public enum eNumArigen
	{
		Right,
		Left,
		Center,
	};

	public enum eTextArigen
	{
		Left = 0,
		Center = 1,
		Right = 2,
	};

	static TextAnchor[] TextArigenToTextAnchorTbl =
	{
		TextAnchor.MiddleLeft,
		TextAnchor.MiddleCenter,
		TextAnchor.MiddleRight
	};

	public LayoutImage[] child
	{
		get
		{
			return m_Child;
		}
	}

	static int DigitMax = 8;

	protected LayoutData m_Data = null;
	protected LayoutImage[] m_Child = null;

	Image	m_Image = null;
	LayoutButton m_Button = null;
	Toggle	m_Toggle = null;
	Text m_Text = null;

	int m_DispNum = -1;
	Sprite[] m_NumSprite = null;
	Image[] m_NumImage = null;
	bool m_LastHit = false;
	int  m_LastTouchID = -1;

	RectTransform m_RectTransform;
	public RectTransform rectTransform
	{
		get
		{
			if (m_RectTransform == null )
			{
				m_RectTransform = gameObject.GetComponent<RectTransform>();
			}
			return m_RectTransform;
		}
	}

	public string label
	{
		get
		{
			return this.m_Data.label;
		}
	}

	public Image image
	{
		get
		{
			return m_Image;
		}
	}

	public LayoutButton button
	{
		get
		{
			return m_Button;
		}
	}

	public LayoutData layoutData
	{
		get
		{
			return m_Data;
		}
	}

	public Vector2 Pos
	{
		get
		{

			return new Vector2(rectTransform.anchoredPosition.x, -rectTransform.anchoredPosition.y);
		}

		set
		{
			Vector2 pos = new Vector2(value.x, -value.y);
			rectTransform.anchoredPosition = pos;
		}
	}
	
	public Vector2 WorldPos
	{
		get
		{
			Vector2 pos = new  Vector2(rectTransform.position.x, rectTransform.position.y);

			pos.x /= CanvasMgr.CanvasScale.x;
			pos.y /= CanvasMgr.CanvasScale.y;

			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round((CanvasMgr.BaseResolution.y + (CanvasMgr.CanvasSize.y - CanvasMgr.BaseResolution.y) * 0.5f) - pos.y);

			return pos;
		}

		set
		{
			
			Vector3 pos = new Vector3(value.x, value.y, 0);

			pos.y = (CanvasMgr.BaseResolution.y + (CanvasMgr.CanvasSize.y - CanvasMgr.BaseResolution.y) * 0.5f) - pos.y;


			rectTransform.position = Vector2.Scale(pos, CanvasMgr.CanvasScale);
		}
	}

	public Vector2 Anchor
	{
		get
		{
			Vector2 Anc = rectTransform.pivot;
			Anc.y = 1 - Anc.y;

			return Anc;
		}

		set
		{
			value.y = 1 - value.y;

			rectTransform.pivot = value;
		}
	}

	public bool lastHit
	{
		get
		{
			return m_LastHit;
		}
	}

	public int lastTouchID
	{
		get
		{
			return m_LastTouchID;
		}
	}

	public LayoutImage GetObject( string Name )
	{
		LayoutImage l = GetObjectByName( Name );
		if( l != null )
		{
			return l;
		}

		return GetObjectByLabel( Name );
	}

	public LayoutImage GetObject( int ID )
	{
		if( this.m_Data.id == ID )
		{
			return this;
		}

		for( int i=0; i<this.m_Child.Length; i++ )
		{
			LayoutImage t = this.m_Child[i].GetObject(ID);
			if (t != null)
			{
				return t;
			}
		}

		return null;
	}


	public LayoutImage GetObjectByName(string Name)
	{
		if (this.m_Data.name == Name)
		{
			return this;
		}

		for (int i = 0; i < this.m_Child.Length; i++)
		{
			LayoutImage t = this.m_Child[i].GetObjectByName(Name);
			if (t != null)
			{
				return t;
			}
		}

		return null;
	}

	public LayoutImage GetTextObjectByName(string Name)
	{
		if (this.m_Data.name == Name)
		{
			if( this.m_Data.is_text )
			{
				return this;
			}
		}

		for (int i = 0; i < this.m_Child.Length; i++)
		{
			LayoutImage t = this.m_Child[i].GetTextObjectByName(Name);
			if (t != null)
			{
				return t;
			}
		}

		return null;
	}

	public LayoutImage GetObjectByLabel(string Label)
	{
		if (this.m_Data.label == Label)
		{
			return this;
		}

		for (int i = 0; i < this.m_Child.Length; i++)
		{
			LayoutImage t = this.m_Child[i].GetObjectByLabel(Label);
			if (t != null)
			{
				return t;
			}
		}

		return null;
	}

	public LayoutImage GetObjectByID(int PsdID)
	{
		if (this.m_Data.psd_id == PsdID)
		{
			return this;
		}

		for (int i = 0; i < this.m_Child.Length; i++)
		{
			LayoutImage t = this.m_Child[i].GetObjectByID(PsdID);
			if (t != null)
			{
				return t;
			}
		}

		return null;
	}

	public Rect GetRect()
	{
		Rect r = new Rect();

		float SclX = m_RectTransform.lossyScale.x / CanvasMgr.CanvasScale.x;
		float SclY = m_RectTransform.lossyScale.y / CanvasMgr.CanvasScale.y;

		

		r.x = WorldPos.x - Anchor.x * m_RectTransform.sizeDelta.x * SclX;
		r.y = WorldPos.y - Anchor.y * m_RectTransform.sizeDelta.y * SclY;
		r.width = m_RectTransform.sizeDelta.x * SclX;
		r.height = m_RectTransform.sizeDelta.y * SclY;
		
		return r;
	}

	public static LayoutImage Create( string Img, float x, float y, LayoutReproduction.eAnchor Anc, CanvasMgr.eDispSort DispZ, LayoutImage Parent = null )
	{
		return LayoutImage.CreateCore( Img, null, x, y, -1, -1, Anc, DispZ, Parent );
	}

	public static LayoutImage CreateNullNode( string NodeName, float x, float y, float w, float h, LayoutReproduction.eAnchor Anc, CanvasMgr.eDispSort DispZ, LayoutImage Parent = null )
	{
		LayoutImage l = LayoutImage.CreateCore( null, null, x, y, w, h, Anc, DispZ, Parent );

		l.gameObject.name = NodeName;

		return l;
	}

	public static LayoutImage CreateText( string NodeName, LayoutTextData Data, float x, float y, CanvasMgr.eDispSort DispZ, LayoutImage Parent = null )
	{
		LayoutImage l = LayoutImage.CreateCore( null, Data, x, y, 0, 0, LayoutReproduction.eAnchor.CC, DispZ, Parent );

		l.gameObject.name = NodeName;

		return l;
	}

	static LayoutImage CreateCore( string Img, LayoutTextData TextData, float x, float y, float w, float h, LayoutReproduction.eAnchor Anc, CanvasMgr.eDispSort DispZ, LayoutImage Parent )
	{
		GameObject Obj = new GameObject(Img);
		LayoutImage Image = Obj.AddComponent<LayoutImage>();


		string Origin = LayoutReproduction.eAnchor.LU.ToString();
		if( Parent != null )
		{
			Obj.transform.parent = Parent.transform;
			Origin = Parent.layoutData.anchor;
		}
		else
		{
			CanvasMgr.Instance.EntryDispObj(Obj, DispZ);
		}
		Obj.transform.localScale = Vector3.one;
		

		Sprite sp = SpriteBankMgr.Instance.GetSprite(Img);
		LayoutData Data = new LayoutData();

		Data.psd_id = -1;
		Data.id = -1;
		Data.name = Img;
		Data.label = null;
		Data.null_node = false;

		if( sp == null )
		{
			if( Img == null && TextData == null )
			{
				Data.null_node = true;
				Data.ow = w;
				Data.oh = h;
			}
			else
			{
				Data.ow = 0;
				Data.oh = 0;
			}
		}
		else
		{
			Data.ow = sp.rect.width;
			Data.oh = sp.rect.height;
		}
		Data.w = Data.ow;
		Data.h = Data.oh;

		Data.anchor = Anc.ToString();

		Data.alpha = 1;

		Data.is_text = (TextData != null);
		Data.is_button = false;
		Data.is_toggle = false;
		Data.is_nineslice = false;
		Data.is_num = false;

		Data.text_data = TextData;
		Data.drop_data = null;
		Data.outline_data = null;
		Data.child = null;
		
		Data.origin = Origin;



		Vector2 AncV = LayoutReproduction.GetAnchorLayout( Anc );

		Data.x = x - Data.ow * AncV.x;
		Data.y = y - Data.oh * AncV.y;

		Data.sx = Data.x;
		Data.sy = Data.y;

		Image.Create( Data );

		return Image;
	}
	
	public int TouchCheck( )
	{
		m_LastTouchID = TouchInputMgr.Instance.TouchCheck( GetRect() );


		return m_LastTouchID;
	}

	public bool TouchCheck( int TouchID )
	{
		return TouchInputMgr.Instance.TouchCheck( GetRect(), TouchID );
	}

	public void SetDefaultFont( /*SaveDataMgr.eLangage Langage*/)
	{
        m_Text.font = LayoutFileMgr.Instance.GetFont(m_Data.text_data.font_name);
        /*
		switch( Langage )
		{
			case SaveDataMgr.eLangage.CN:
				m_Text.font = LayoutFileMgr.Instance.GetFont( "FOTCARNewHeiB5-Heavy" );
			break;
			
			case SaveDataMgr.eLangage.KR:
				m_Text.font = LayoutFileMgr.Instance.GetFont( "FOTKYDGothic140" );
			break;

			default:
				m_Text.font = LayoutFileMgr.Instance.GetFont( m_Data.text_data.font_name ); //m_Fonts[m_DefaltFontName];
			break;
		}*/
    }

	public void Create(LayoutData Data)
	{
		m_Data = Data;


		if (m_Data.is_text)
		{
			m_Text = gameObject.AddComponent<Text>();
		}
		else
		{
			m_Image = gameObject.AddComponent<Image>();
		}

		// サイズの反映
		rectTransform.sizeDelta = new Vector2(Data.ow, Data.oh);

		// originの反映
		rectTransform.anchorMin = LayoutReproduction.GetAnchor(Data.origin);
		rectTransform.anchorMax = LayoutReproduction.GetAnchor(Data.origin);

		// anchorの反映
		rectTransform.pivot = LayoutReproduction.GetAnchor(LayoutReproduction.eAnchor.LU);

		// 位置の反映
		Pos = new Vector2(Data.sx, Data.sy);

		if (m_Data.is_text)
		{
			Vector3 pos = rectTransform.position;
			pos.x = Mathf.Round( pos.x );
			pos.y = Mathf.Round( pos.y );
			pos.z = Mathf.Round( pos.z );
			rectTransform.position = pos;
		}

		// テキスト反映
		if (m_Data.is_text)
		{
			SetDefaultFont();
			
			m_Text.text = m_Data.text_data.content;//"！" + m_Data.text_data.content + "！";
			m_Text.fontSize = m_Data.text_data.size;

			m_Text.fontStyle = FontStyle.Normal;
			if (m_Data.text_data.is_bold)
			{
				m_Text.fontStyle = FontStyle.Bold;
			}

			Color color = Color.white;
			if (!ColorUtility.TryParseHtmlString(m_Data.text_data.color, out color))
			{
				if (!ColorUtility.TryParseHtmlString("#" + m_Data.text_data.color, out color))
				{
					Debug.LogWarning("Unknown color code... " + m_Data.text_data.color);
				}
			}

			m_Text.color = color;

			m_Text.horizontalOverflow = HorizontalWrapMode.Overflow;
			m_Text.verticalOverflow = VerticalWrapMode.Overflow;
			m_Text.alignment = TextArigenToTextAnchorTbl[m_Data.text_data.aligan];
			m_Text.raycastTarget = false;
		}
		// Sprite系の反映
		else
		{
			if (!Data.null_node)
			{
				SetSprite(Data.name);

				if (m_Data.is_button)
				{
					m_Button = LayoutButton.Create( this );

				}
				else if (m_Data.is_toggle)
				{
					m_Toggle = gameObject.AddComponent<Toggle>();
					m_Toggle.graphic = m_Image;
				}
			}
			else
			{
				m_Image.enabled = false;
				m_Image.color = new Color(1, 1, 1, 0);
				m_Image.raycastTarget = false;
			}
		}

		//Canvas.ForceUpdateCanvases();
		m_Child = null;

		if (Data.child != null)
		{
			m_Child = new LayoutImage[Data.child.Length];
			for (int i = 0; i < Data.child.Length; i++)
			{
				GameObject Obj = new GameObject(Data.child[i].name);
				Obj.transform.parent = transform;
				Obj.transform.localScale = Vector3.one;

				LayoutImage Img = Obj.AddComponent<LayoutImage>();
				Img.Create(Data.child[i]);

				m_Child[i] = Img;
			}
		}

		// anchorの反映
		SetPivotCreate(LayoutReproduction.GetAnchor(Data.anchor));


		if (m_Data.is_num)
		{
			SetNum(12345678);
		}
	}

	public void LateUpdate()
	{
		if( m_Button != null )
		{
			m_Button.UpdateExec();
			m_LastHit = ( m_Button.state == LayoutButton.eState.Up );
		}
	}


	public void OnDestroy()
	{
		m_Data = null;
		m_Child = null;

		if (m_Image != null)
		{
			Destroy(m_Image);
			m_Image = null;
		}

		if (m_Button != null)
		{
			Destroy(m_Button);
			m_Button = null;
		}


		if (m_Toggle != null)
		{
			Destroy(m_Toggle);
			m_Toggle = null;
		}

		if (m_Text != null)
		{
			Destroy(m_Text);
			m_Text = null;
		}

		m_NumSprite = null;
		m_NumImage = null;
		m_RectTransform = null;
	}



	public void SetText( string text )
	{
		if (m_Text != null)
		{
			if (m_Text.text != text)
			{
				m_Text.text = text;
			}
		}
	}


	public void SetText( string Node, string text )
	{
		LayoutImage n = GetObject(Node);
		if (n != null)
		{
			n.SetText( text );
		}
	}

	public string GetText()
	{
		if (m_Text != null)
		{
			return m_Text.text;
		}
		return "";
	}

	public Text GetUITextObj()
	{
		return m_Text;
	}

	public void SetTextMultiLine( string[] NodeList, string Text )
	{
		string[] MultiText = Text.Split( new string[]{"\r\n", "\n\r", "\n", "\r" }, System.StringSplitOptions.None);

		for( int i=0; i<NodeList.Length; i++ )
		{
			LayoutImage t = GetTextObjectByName( NodeList[i] );
			if( t == null )
			{
				continue;
			}

			if( i < MultiText.Length )
			{
				t.SetText( MultiText[i] );
			}
			else
			{
				t.SetText( "" );
			}
		}
	}

	public void SetTextMultiLine( string[] NodeList3, string[] NodeList4, string Text )
	{
		string[] MultiText = Text.Split( new string[]{"\r\n", "\n\r", "\n", "\r" }, System.StringSplitOptions.None);

		LayoutImage t;
		// まっさらに
		for( int i=0; i<NodeList3.Length; i++ )
		{
			t = GetTextObjectByName( NodeList3[i] );
			if( t == null )
			{
				continue;
			}
			t.SetText( "" );
		}
		for( int i=0; i<NodeList4.Length; i++ )
		{
			t = GetTextObjectByName( NodeList4[i] );
			if( t == null )
			{
				continue;
			}
			t.SetText( "" );
		}
		
		switch( MultiText.Length )
		{
			case 0:
			break;

			case 1:
				if( NodeList3.Length < 3 )
				{
					SetTextMultiLine( NodeList3, Text );
				}
				else
				{
					int n = NodeList3.Length / 2;

					t = GetTextObjectByName( NodeList3[n] );
					if( t != null )
					{
						t.SetText( MultiText[0] );
					}
				}
			break;

			case 2:
				if( NodeList4.Length < 4 )
				{
					SetTextMultiLine( NodeList4, Text );
				}
				else
				{
					int n = (NodeList4.Length / 2) - 1;


					t = GetTextObjectByName( NodeList4[n] );
					if( t != null )
					{
						t.SetText( MultiText[0] );
					}
					t = GetTextObjectByName( NodeList4[n+1] );
					if( t != null )
					{
						t.SetText( MultiText[1] );
					}
				}
			break;

			case 3:
				if( NodeList3.Length <= 3 )
				{
					SetTextMultiLine( NodeList3, Text );
				}
				else
				{
					int n = (NodeList3.Length / 2) - 1;
					for( int i=0; i<3; i++ )
					{
						t = GetTextObjectByName( NodeList3[n+i] );
						if( t != null )
						{
							t.SetText( MultiText[i] );
						}
					}
				}
			break;

			case 4:
				if( NodeList4.Length == 4 )
				{
					SetTextMultiLine( NodeList4, Text );
				}
				else
				{
					int n = (NodeList4.Length / 2) - 2;
					for( int i=0; i<4; i++ )
					{
						t = GetTextObjectByName( NodeList4[n+i] );
						if( t != null )
						{
							t.SetText( MultiText[i] );
						}
					}
				}
			break;

			case 5:
				SetTextMultiLine( NodeList3, Text );
			break;

			default:
				SetTextMultiLine( NodeList4, Text );
			break;
		}
	}



	public void SetSprite( Sprite sprite )
	{
		if( m_Image != null )
		{
			if( m_Image.sprite == null && sprite != null && 0 <= m_Image.color.a )
			{
				m_Image.enabled = true;
			}
			m_Image.sprite = sprite;
		}
	}

	public void SetSprite( string ImgName )
	{
		Sprite sp = SpriteBankMgr.Instance.GetSprite(ImgName); //Resources.Load<Sprite>("Img/" + ImgName );
		SetSprite(sp);
	}

	public Color GetColor()
	{
		if( m_Image != null )
		{
			return m_Image.color;
		}

		return Color.white;
	}

	public void SetColor( Color color )
	{
		if( m_NumImage != null )
		{
			for( int i=0; i<m_NumImage.Length; i++ )
			{
				m_NumImage[i].color = color;
			}
			return;
		}

		if( m_Image != null )
		{
			if( m_Image.sprite == null )
			{
				m_Image.enabled = ( 0f <= color.a );
			}
			m_Image.color = color;
		}
	}

	public void SetPivotCreate( Vector2 Pivot )
	{
		Pivot.y = 1 - Pivot.y;
		SetPivot( Pivot );

	}

	public void SetPivot( Vector2 Pivot )
	{
		// Pivotを0,0へ
		var Offset = -Anchor;
		Offset.x *= rectTransform.sizeDelta.x;
		Offset.y *= rectTransform.sizeDelta.y;

		Vector2 pos = Pos;
		pos.x += Offset.x;
		pos.y += Offset.y;

		// 新しいPivotを適用
		Offset = Pivot;
		Offset.x *= rectTransform.sizeDelta.x;
		Offset.y *= rectTransform.sizeDelta.y;

		Anchor = Pivot;

		pos.x += Offset.x;
		pos.y += Offset.y;
		Pos = pos;
	}

	public RectMask2D CreateMask()
	{
		if (this.m_Image != null)
		{
			return gameObject.AddComponent<RectMask2D>();
		}

		return null;
	}

	public void DispDigit( int Digit )
	{
		for (int i = 0; i < Digit; i++)
		{
			if (m_NumImage[i] == null)
			{
				continue;
			}

			if( !m_NumImage[i].enabled )
			{
				m_NumImage[i].enabled = true;
				m_NumImage[i].sprite = m_NumSprite[0];
			}
		}
			
	}

	public void SetNum( int Num )
	{
		if( !m_Data.is_num )
		{
			return;
		}

		if( m_DispNum == Num )
		{
			return;
		}

		//m_Data.num_arigen = 1;

		m_Image.enabled = false;

		if (m_NumSprite == null)
		{
			string FileNameBase = Regex.Replace(m_Data.name, @"(.+)_0", "$1");

			m_NumSprite = new Sprite[10];
			for (int i = 0; i < 10; i++)
			{
				m_NumSprite[i] = SpriteBankMgr.Instance.GetSprite( FileNameBase + "_" + i.ToString() );
			}
		}

		if (m_NumImage == null)
		{
			m_NumImage = new Image[DigitMax];
		}

		float Dir = -1;
		if (m_Data.num_arigen == (int)eNumArigen.Left)
		{
			Dir = 1;
		}

		// 数値→文字列→桁毎の配列　左基準　例）1024→[1,0,2,4]
		string StrNum = Num.ToString();
		int[] DigitNum = new int[StrNum.Length];
		for (int i = 0; i < StrNum.Length; i++)
		{
			DigitNum[i] = int.Parse(StrNum[i].ToString());
		}

		// 全部非表示
		float OfsX = 0;

		for (int i = 0; i < DigitMax; i++)
		{
			if (m_NumImage[i] == null)
			{
				GameObject Obj = new GameObject("Num_"+i.ToString());
				Obj.transform.parent = transform;
				Obj.transform.localScale = Vector3.one;
				m_NumImage[i] = Obj.AddComponent<Image>();
				m_NumImage[i].raycastTarget = false;
			}
			m_NumImage[i].enabled = false;

			if( DigitNum.Length <= i )
			{
				continue;
			}

			if( 1 <= i )
			{
				OfsX += m_NumImage[i-1].rectTransform.sizeDelta.x * Dir;
			}

			int d = DigitNum[DigitNum.Length - 1 - i];
			if (m_Data.num_arigen == (int)eNumArigen.Left)
			{
				d = DigitNum[i];
			}

			m_NumImage[i].sprite = m_NumSprite[d];
			m_NumImage[i].SetNativeSize();
			m_NumImage[i].rectTransform.localPosition = new Vector3(OfsX, 0, 0);

			
			m_NumImage[i].enabled = true;
		}

		if( m_Data.num_arigen == (int)eNumArigen.Center )
		{
			for (int i = 0; i < DigitMax; i++)
			{		
				if( DigitNum.Length <= i )
				{
					continue;
				}

				Vector3 Pos = m_NumImage[i].rectTransform.localPosition;
				Pos.x -= OfsX * 0.5f;
				m_NumImage[i].rectTransform.localPosition = Pos;
			}
		}

	}

	public void SetUnityTextAnchor( TextAnchor Anc)
	{
		if( m_Text != null )
		{
			m_Text.alignment = Anc;
		}
	}

}


