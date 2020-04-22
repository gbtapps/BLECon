using UnityEngine;
using System.Collections;

public struct StateInt
{
    public int Now
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

    public int Max
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
                int n = m_Min;
                m_Min = m_Max;
                m_Max = n;
            }

            if (m_Max <= m_Now)
            {
                m_Now = m_Max;
            }

        }
    }

    public int Min
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
                int n = m_Min;
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
            return ((float)m_Now - (float)m_Min) / ((float)m_Max - (float)m_Min);
        }
        set
        {
            float r = value;

            if( 1f <= r )
            {
                m_Now = m_Max;
                return;
            }

            if( r <= 0f )
            {
                m_Now = m_Min;
                return;
            }

            m_Now = m_Min + Mathf.RoundToInt(((float)m_Max - (float)m_Min) * r);

        }
    }
    public void Set( int now, int min, int max )
    {
        Min = min;
        Max = max;
        Now = now;
    }


    int m_Now;
    int m_Min;
    int m_Max;
}
