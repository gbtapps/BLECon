using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_SWITCH
using nn.hid;
#endif

public class TouchInputMgr : SingletonMonoBehaviour<TouchInputMgr> 
{
	public enum eTouchState
	{
		Down,
		Press,
		Up,
	}

	public class TouchData
	{
		public int ID;
		public eTouchState State;
		public float x;
		public float y;
		public float RawX;
		public float RawY;

		public bool isDelete
		{
			get
			{
				return State == eTouchState.Up;
			}
		}

		public TouchData( int iD, float rawX, float rawY)
		{
			ID = iD;
			Update( rawX, rawY );
			State = eTouchState.Down;
		}

		public void BeginUpdate()
		{
			State = eTouchState.Up;
		}

		public void Update( float rawX, float rawY )
		{
			RawX = rawX;
			RawY = rawY;
#if UNITY_SWITCH && !UNITY_EDITOR
			x = RawX / GameDefine.SWITCH_MOBILE_SIZE_X * GameDefine.CANVAS_BASE_SIZE_X;
			y = RawY / GameDefine.SWITCH_MOBILE_SIZE_Y * GameDefine.CANVAS_BASE_SIZE_Y;
#else
			x = RawX / (float)(Screen.width) * GameDefine.CANVAS_BASE_SIZE_X;
			y = RawY / (float)(Screen.height) * GameDefine.CANVAS_BASE_SIZE_Y;

			//x = GameDefine.CANVAS_BASE_SIZE_X - x;
			y = GameDefine.CANVAS_BASE_SIZE_Y - y;

#endif
			State = eTouchState.Press;
		}
	}

	private List<TouchData> m_TouchList;
	private System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

#if UNITY_SWITCH
    private TouchScreenState5 touchScreenState = new TouchScreenState5();
    private Vector3 screen = new Vector3();
    private Vector3 world = new Vector3();
#endif

	public void Start()
	{
		m_TouchList = new List<TouchData>();

#if UNITY_SWITCH
		TouchScreen.Initialize();
        touchScreenState.SetDefault();
#endif
	}

	void UpdateTouchData( int ID, float x, float y )
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( m_TouchList[i].ID == ID )
			{
				m_TouchList[i].Update( x, y );
				return;
			}
		}

		m_TouchList.Add( new TouchData( ID, x, y ) );
	}

	public TouchData FindTouchData( int ID )
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( m_TouchList[i].ID == ID )
			{
				return m_TouchList[i];
			}
		}

		return null;
	}

	public int TouchCheck( Rect rect )
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( rect.x <= m_TouchList[i].x && m_TouchList[i].x <= rect.x+rect.width &&
				rect.y <= m_TouchList[i].y && m_TouchList[i].y <= rect.y+rect.height )
			{
				return m_TouchList[i].ID;
			}
		}


		//Lay
		return -1;
	}

	public bool TouchCheck( Rect rect, int TouchID )
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( rect.x <= m_TouchList[i].x && m_TouchList[i].x <= rect.x+rect.width &&
				rect.y <= m_TouchList[i].y && m_TouchList[i].y <= rect.y+rect.height )
			{
				if( m_TouchList[i].ID == TouchID )
				{
					return true;
				}
			}
		}


		//Lay
		return false;
	}

	public bool isPress()
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( m_TouchList[i].State == eTouchState.Press )
			{
				return true;
			}
		}
		return false;
	}

	public bool isDown()
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( m_TouchList[i].State == eTouchState.Down )
			{
				return true;
			}
		}
		return false;
	}

	public bool isUp()
	{
		for( int i=0; i<m_TouchList.Count; i++ )
		{
			if( m_TouchList[i].State == eTouchState.Up )
			{
				return true;
			}
		}
		return false;
	}

    void Update()
    {
		// 準備
		for( int i=m_TouchList.Count-1; 0<=i; i-- )
		{
			if( m_TouchList[i].isDelete )
			{
				m_TouchList.RemoveAt( i );
			}
			else
			{
				m_TouchList[i].BeginUpdate();
			}
		}

		// タッチ更新
        //stringBuilder.Length = 0;

#if UNITY_SWITCH && !UNITY_EDITOR
        TouchScreen.GetState(ref touchScreenState);
        //stringBuilder.AppendFormat("TouchCount: {0} / {1}\n", touchScreenState.count, touchScreenState.touches.Length);
        for (int n = 0; n < touchScreenState.count; n++)
        {
            //stringBuilder.AppendFormat("{0}\n", touchScreenState.touches[n].ToString());
			UpdateTouchData( touchScreenState.touches[n].fingerId, touchScreenState.touches[n].x, touchScreenState.touches[n].y );
        }
#else
		if( Input.GetButton("Fire1") )
		{
			UpdateTouchData( 0, Input.mousePosition.x, Input.mousePosition.y);
			//string s = JsonUtility.ToJson( m_TouchList[0] ) ;
			//Debug.Log( s );
		}
#endif

        // Marker
		/*
        Vector3 temp;
        for (int n = 0; n < touchMarkers.Length; n++)
        {
            if (n < touchScreenState.count)
            {
                touchMarkers[n].SetActive(true);
                if (Camera.main != null)
                {
                    screen.Set(touchScreenState.touches[n].x, touchScreenState.touches[n].y, 0);
                    world = Camera.main.ScreenToWorldPoint(screen);
                    //stringBuilder.AppendFormat("ScreenToWorldPoint: ({0} {1} {2}) -> ({3} {4} {5})\n", screen.x, screen.y, screen.z, world.x, world.y, world.z);
                    temp = touchMarkers[n].transform.position;
                    temp.Set(world.x, world.y * -1, 0);
                    touchMarkers[n].transform.position = temp;
                }
            }
            else
            {
                touchMarkers[n].SetActive(false);
            }
        }
		*/

    }


	public string GetTouchInfo()
	{
		string str = "";
		for( int i=0; i<m_TouchList.Count; i++ )
		{

			str += "ID:" + m_TouchList[i].ID.ToString() + " State:" + m_TouchList[i].State.ToString() + "\n";
			str += " x:" + m_TouchList[i].x.ToString() + " y:" + m_TouchList[i].y.ToString()+ "\n";
			str += " RawX:" + m_TouchList[i].RawX.ToString() + " RawY:" + m_TouchList[i].RawY.ToString()+ "\n\n";
		}

		return str;
	}
}
