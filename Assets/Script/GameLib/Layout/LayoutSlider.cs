using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSlider : MonoBehaviour 
{
	static public LayoutSlider Create( eType Type, LayoutImage Root, string SlideNode, string MinNode, string MaxNode )
	{
		LayoutImage SlideObj = Root.GetObject( SlideNode );
		LayoutImage MinObj =  Root.GetObject( MinNode );
		LayoutImage MaxObj =  Root.GetObject( MaxNode );

		if( SlideObj == null || MinObj == null || MaxObj == null )
		{
			return null;
		}

		LayoutSlider s = SlideObj.gameObject.AddComponent<LayoutSlider>();
		s.Init( Type, MinObj, MaxObj );

		return s;
	}

	public enum eType
	{
		H,
	}

	public enum eMode
	{
		Idle,
		Move,
	}

	public float rate
	{
		get
		{
			return m_Rate;
		}

		set
		{
			if( m_Mode == eMode.Idle )
			{
				SetRate( value );
			}
		}
	}

	public bool isExec
	{
		get
		{
			return m_Mode != eMode.Idle;
		}
	}

	LayoutImage m_Slide;
	LayoutImage m_Min;
	LayoutImage m_Max;
	float m_Rate;

	int m_TouchID = -1;
	Vector2 m_TouchBasePos = Vector2.zero;
	Vector2 m_SlideNodeBasePos;
	eMode m_Mode = eMode.Idle;

	void Init( eType Type, LayoutImage MinNode, LayoutImage MaxNode )
	{
		m_Slide = gameObject.GetComponent<LayoutImage>();
		m_Min = MinNode;
		m_Max = MaxNode;

		SetRate( 0 );
	}

	void SetRate( float Rate )
	{
		m_Rate = Mathf.Clamp( Rate, 0f, 1f );

		float min = m_Min.WorldPos.x;
		float max = m_Max.WorldPos.x;

		Vector3 Pos = m_Slide.WorldPos;
		Pos.x = min + (max-min) * m_Rate;
		m_Slide.WorldPos = Pos;
	}

	void SetPx( float Px )
	{
		float min = m_Min.WorldPos.x;
		float max = m_Max.WorldPos.x;

		SetRate( (Px - min) / (max-min) );
	}


	// Update is called once per frame
	void Update () 
	{
		switch( m_Mode )
		{
			case eMode.Idle:	UpdateIdle();	break;
			case eMode.Move:	UpdateMove();	break;
		}
	}

	void UpdateIdle()
	{
		int ID = m_Slide.TouchCheck();

		if( 0 <= ID )
		{
			TouchInputMgr.TouchData Data = TouchInputMgr.Instance.FindTouchData( ID );
			if( Data.State == TouchInputMgr.eTouchState.Down )
			{
				m_TouchID = ID;
				m_TouchBasePos.x = Data.x;
				m_TouchBasePos.y = Data.y;
				m_SlideNodeBasePos = m_Slide.WorldPos;

				m_Mode = eMode.Move;
			}
		}
	}

	void UpdateMove()
	{
		TouchInputMgr.TouchData Data = TouchInputMgr.Instance.FindTouchData( m_TouchID );

		if( Data == null )
		{
			m_Mode = eMode.Idle;
		}

		if( Data.State == TouchInputMgr.eTouchState.Up )
		{
			m_Mode = eMode.Idle;
		}

		float move = Data.x - m_TouchBasePos.x;

		SetPx( m_SlideNodeBasePos.x + move );

	}
}
