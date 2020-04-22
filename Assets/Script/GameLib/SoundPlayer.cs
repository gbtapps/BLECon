using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour 
{
    public bool isPlaying
    {
        get
        {
            return m_Source.isPlaying;
        }
    }

    public AudioSource source{
        get {
            return m_Source;
        }
    }

    AudioSource m_Source = null;
    SoundMgr.eType m_Type;
    bool m_FadeIn = false;
    bool m_FadeOut = false;
    public float m_FadeMax = 1.0f;
    public float m_FadeCount = 0.0f;
    bool m_Pause = false;

    public AudioClip AudioClip
    {
        get
        {
            return m_Source.clip;
        }
    }

    public void Init(SoundMgr.eType Type)
    {
        m_Type = Type;
        m_Source = gameObject.AddComponent<AudioSource>();
		m_Source.spatialBlend = 0f;
        if (Type == SoundMgr.eType.SE)
        {
            m_Source.outputAudioMixerGroup = SoundMgr.GroupSE;
            m_Source.loop = false;
        }
        else
        {
            m_Source.outputAudioMixerGroup = SoundMgr.GroupBGM;
            m_Source.loop = true;
        }

        m_Source.clip = null;
	}
	
	// Update is called once per frame
    void Update()
    {
        if( m_Source == null )
        {
            return;
        }

        // フェードイン
        if (m_FadeIn)
        {
            m_FadeCount += Time.deltaTime;
            if (m_FadeMax < m_FadeCount)
            {
                m_Source.volume = 1;
                m_FadeIn = false;
            }
            else
            {
                m_Source.volume = m_FadeCount / m_FadeMax;
            }
        }

        if (m_FadeOut)
        {
            m_FadeCount += Time.deltaTime;
            if (m_FadeMax < m_FadeCount)
            {
                Stop();
            }
            else
            {
                m_Source.volume = 1.0f - (m_FadeCount / m_FadeMax);
            }
        }
    }

    public void Stop()
    {
        m_Source.Stop();
        m_Source.clip = null;
        m_FadeOut = false;
        m_FadeIn = false;
        m_FadeCount = 0;
    }

    public void StopFadeout( float FadeTime )
    {
        if( FadeTime <= 0f )
        {
            Stop();
            return;
        }

        if( m_FadeOut )
        {
            float rate = (m_FadeCount / m_FadeMax);
            m_FadeMax = FadeTime;
            m_FadeCount = m_FadeMax * rate;
            return;
        }
        else if( m_FadeIn )
        {
            m_FadeIn = false;
            m_FadeOut = true;
            float rate = 1f - (m_FadeCount / m_FadeMax);
            m_FadeMax = FadeTime;
            m_FadeCount = m_FadeMax * rate;
            return;
        }

        m_FadeIn = false;
        m_FadeOut = true;
        m_FadeCount = 0;
		m_FadeMax = FadeTime;
    }

    public void Pause()
    {
        if (m_Pause)
        {
            m_Source.UnPause();
        }
        else
        {
            m_Source.Pause();
        }
        m_Pause = !m_Pause;
    }

    public void PauseCheck()
    {
        if( m_Pause )
        {
            Pause();
        }
    }

    public void Play( AudioClip Clip, float FadeTime = 1f )
    {
        m_Source.clip = Clip;

        if (m_Type == SoundMgr.eType.SE)
        {
            m_Source.PlayOneShot(m_Source.clip);
            return;
        }

        m_Source.UnPause();
        if( m_Source.isPlaying )
        {
            if( m_FadeOut )
            {
                m_Source.Stop();
            }
            else
            {
                return;
            }
        }

        SoundMgr.Instance.StopBGM(FadeTime, false);
        m_FadeMax = FadeTime;
        m_FadeOut = false;

        m_Source.time = 0;//引数にシークタイムが必要かもしれないし必要じゃないかもしれない
        if (FadeTime <= 0f)
        {
            m_FadeIn = false;
			m_Source.volume = 0;
            m_Source.Play();
			m_Source.volume = 1;
        }
        else
        {
            // BGM再生
            m_FadeIn = true;
            m_FadeCount = 0f;
            m_Source.volume = 0;
            m_Source.Play();
        }

    }

	
	//強制ポーズとポーズ解除
    public void ForcePause()
    {
        m_Source.Pause();
        m_Pause = true;
    }
    public void ForceUnPause()
    {
        m_Source.UnPause();
        m_Pause = false;
    }
 
}
