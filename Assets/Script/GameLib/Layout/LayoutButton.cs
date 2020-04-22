using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LayoutButton : MonoBehaviour 
{
	public static LayoutButton Create( LayoutImage Owner )
	{
		LayoutButton b = Owner.gameObject.AddComponent<LayoutButton>();
		b.Init();

		return b;
	}

	public enum eState
	{
		Idle,
		Press,
		Up,
	}

	static Color PressdColor = new Color( 200f / 255f, 200f / 255f, 200f / 255f );

	Sprite m_IdleSprite;
	Sprite m_PressSprite;
	LayoutImage m_Obj;
	eState m_State;
	int m_TouchID;

	public eState state
	{
		get
		{
			return m_State;
		}
	}

	public Sprite idleSprite
	{
		get
		{
			return m_IdleSprite;
		}
	}

	public Sprite pressdSprite
	{
		get
		{
			return m_PressSprite;
		}
	}

	// Use this for initialization
	void Init () 
	{
		m_Obj = gameObject.GetComponent<LayoutImage>();

		m_IdleSprite = m_Obj.image.sprite;
		m_PressSprite = SpriteBankMgr.Instance.GetSprite( m_Obj.layoutData.name + "_1" );


		m_State = eState.Idle;
	}
	
	public void UpdateExec () 
	{
		switch( m_State )
		{
			case eState.Idle:	UpdateIdle(); break;
			case eState.Press:	UpdatePress(); break;
			
			case eState.Up:
				DispChange( false );
				m_TouchID = -1;
				m_State = eState.Idle;
			break;
		}
	}

	void UpdateIdle()
	{
		m_TouchID = m_Obj.TouchCheck();
		if( 0 <= m_TouchID )
		{
			TouchInputMgr.TouchData Data = TouchInputMgr.Instance.FindTouchData( m_TouchID );

			if( Data.State == TouchInputMgr.eTouchState.Down )
			{
				// raycastチェックをする

				PointerEventData pointer = new PointerEventData (EventSystem.current);

				float rx = Data.RawX;
				float ry = Data.RawY;

#if UNITY_SWITCH && !UNITY_EDITOR
				//rx = Data.RawX / GameDefine.SWITCH_MOBILE_SIZE_X * GameDefine.CANVAS_BASE_SIZE_X;
				//ry = Data.RawY / GameDefine.SWITCH_MOBILE_SIZE_Y * GameDefine.CANVAS_BASE_SIZE_Y;

				ry =  GameDefine.SWITCH_MOBILE_SIZE_Y - Data.RawY;


#endif

				pointer.position = new Vector3( rx, ry, 0 );

				List<RaycastResult> result = new List<RaycastResult> ();
				EventSystem.current.RaycastAll (pointer, result);

				//for( int i=0; i<result.Count; i++ )
				{
					int i=0;
					if( result[i].gameObject == gameObject )
					{
						Debug.Log( result[i].depth );
						DispChange( true );
						m_State = eState.Press;
						return;
					}
				}
			}
		}
	}

	void UpdatePress()
	{
		TouchInputMgr.TouchData Data = TouchInputMgr.Instance.FindTouchData( m_TouchID );
		if( Data == null )
		{
			m_State = eState.Up;
			return;
		}

		if( Data.State == TouchInputMgr.eTouchState.Up )
		{
			m_State = eState.Up;
			return;
		}

		if( !m_Obj.TouchCheck( m_TouchID ) )
		{
			DispChange(false);
			m_State = eState.Idle;
			m_TouchID = -1;
			return;
		}
	}

	void DispChange( bool isPress )
	{
		if( isPress )
		{
			if( m_PressSprite == null )
			{
				m_Obj.SetColor( PressdColor );
			}
			else
			{
				m_Obj.SetSprite( m_PressSprite );
			}

		}
		else
		{
			m_Obj.SetSprite( m_IdleSprite );
			m_Obj.SetColor( Color.white );
		}
	}
}
