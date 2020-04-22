using UnityEngine;
using System.Collections;

public class ShakeObj : MonoBehaviour
{
    float m_Timer = 0;
    float m_ShakeTimeMax = 0;
    float m_ShakePower = 0;
    
    void Update()
    {
        if( m_ShakeTimeMax <= m_Timer )
        {
            transform.localPosition = Vector3.zero;
            return;
        }

        transform.localPosition = new Vector3(Random.Range(-m_ShakePower, m_ShakePower), Random.Range(-m_ShakePower, m_ShakePower), Random.Range(-m_ShakePower, m_ShakePower));

        m_Timer += Time.deltaTime;
    }

    public void SetShake( float Power, float Time )
    {
        m_ShakePower = Power;
        m_Timer = 0;
        m_ShakeTimeMax = Time;
    }
}