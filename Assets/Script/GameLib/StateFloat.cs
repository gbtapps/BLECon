using UnityEngine;
using System.Collections;

public struct StateFloat
{
    public float Now
    {
        get
        {
            return m_Now;
        }
        set
        {
            m_Now = value;
            if( m_Max <= m_Now )
            {
                m_Now = m_Max;
            }
            if( m_Now <= m_Min )
            {
                m_Now = m_Min;
            }
        }
    }

    public float Max
    {
        get
        {
            return m_Max;
        }
        set
        {
            m_Max = value;
            if( m_Max < m_Min )
            {
                float n = m_Min;
                m_Min = m_Max;
                m_Max = n;
            }

            if (m_Max <= m_Now)
            {
                m_Now = m_Max;
            }

        }
    }

    public float Min
    {
        get
        {
            return m_Min;
        }
        set
        {
            m_Min = value;
            if (m_Max < m_Min)
            {
                float n = m_Min;
                m_Min = m_Max;
                m_Max = n;
            }

            if ( m_Now <= m_Min )
            {
                m_Now = m_Min;
            }

        }
    }

    public float Rate
    {
        get
        {
            if( m_Max == m_Min )
            {
                return 1f;
            }

            return (m_Now - m_Min) / (m_Max - m_Min);
        }
        set
        {
            float r = value;
            if( 1f < r )
            {
                r = 1f;
            }
            if( r < 0f )
            {
                r = 0f;
            }

            m_Now = m_Min + (m_Max - m_Min) * r;

        }
    }

    public void Set(float now, float min, float max)
    {
        Min = min;
        Max = max;
        Now = now;

    }

    public void SetNow( float now )
    {
        Now = now;
    }


    float m_Now;
    float m_Min;
    float m_Max;
}
