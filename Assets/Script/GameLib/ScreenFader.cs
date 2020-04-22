using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : DefaultUIBase
{
    public enum eStep
    {
        Off,            // フェード用板は非表示(a=0)
        OffToOn,        // フェード用板を表示切り替え(a=0→1)
        On,             // フェード用板は表示中(a=1)
        OnToOff,        // フェード用板は非表示切り替え(a=1→0)
    };

	public override CanvasMgr.eDispSort DispZ
    {
        get
        {
			return CanvasMgr.eDispSort.Fader;
        }
    }


    static public ScreenFader Instance
    {
        get
        {
            return s_Instance;
        }
    }
    static ScreenFader s_Instance = null;

    static public bool isFadeEnd
    {
        get
        {
            if( s_Instance == null )
            {
                return false;
            }

            return (s_Instance.m_Step == eStep.Off || s_Instance.m_Step == eStep.On);
        }
    }

    eStep m_Step;
    Image m_Image;
    float m_FadeTime;

	// Use this for initialization
	public override void Init () 
    {
        if( s_Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
		m_Image = gameObject.AddComponent<Image>();//GetComponent<Image>();
		m_Image.sprite = null;
		m_Image.color = Color.black;
		m_Image.material = null;
		m_Image.raycastTarget = false;

		RectTransform rt = m_Image.rectTransform;
		rt.sizeDelta = new Vector2(4096, 4096);
	}

    void OnDestroy()
    {
        if( s_Instance == this )
        {
            s_Instance = null;
        }
    }

    // FadeTime = 0→1にかかる時間,isForceONで現在のスクリーン状態を無視してフェードを最初から行う
    public void SetFadeOut( Color Color, float FadeTime, bool isForce = false)
    {
        float a = m_Image.color.a;
        if (isForce)
        {
            a = 0;
        }
        Color.a = a;

        m_Image.color = Color;
        m_FadeTime = FadeTime;

        m_Step = eStep.OffToOn;
    }

    public void SetFadeIN(Color Color, float FadeTime, bool isForce = false)
    {
        float a = m_Image.color.a;
        if (isForce)
        {
            a = 1;
        }
        Color.a = a;

        m_Image.color = Color;
        m_FadeTime = FadeTime;

        m_Step = eStep.OnToOff;
    }

    public void SetFadeIN( float FadeTime )
    {
        m_FadeTime = FadeTime;
        m_Step = eStep.OnToOff;
    }

	
	// Update is called once per frame
	public override void UpdateExec () 
    {
        Color Color = m_Image.color;
        float a = Color.a;
        float Speed = 1.0f / m_FadeTime * Time.deltaTime;
        switch( m_Step )
        {
            case eStep.OffToOn:
                a += Speed;
                if( 1 <= a )
                {
                    a = 1;
                    m_Step = eStep.On;
                }
            break;
            case eStep.OnToOff:
            a -= Speed;
            if (a<=0)
            {
                a = 0;
                m_Step = eStep.Off;
            }
            break;
        }

        Color.a = a;
        m_Image.color = Color;
		m_Image.enabled = (0f<Color.a);
		
	}
}
