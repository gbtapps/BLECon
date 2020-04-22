using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveObj : MonoBehaviour 
{
	public Vector3 g_WaveSize;
	public float g_WaveCycle;

	RectTransform m_RTransform = null;
	float m_Timer = 0;

	void Awake()
	{
		m_RTransform = gameObject.GetComponent<RectTransform>();
	}

	void Update()
	{
		float Deg = 360f * (m_Timer / g_WaveCycle);

		Vector3 Pos = new Vector3();
		Pos.x = g_WaveSize.x * Mathf.Sin(Deg * Mathf.Deg2Rad);
		Pos.y = g_WaveSize.y * Mathf.Sin(Deg * Mathf.Deg2Rad);
		Pos.z = g_WaveSize.z * Mathf.Sin(Deg * Mathf.Deg2Rad);

		m_Timer += Time.deltaTime;
		while( g_WaveCycle <= m_Timer )
		{
			m_Timer -= g_WaveCycle;
		}

		if( m_RTransform != null )
		{
			m_RTransform.localPosition = Pos;
		}
		else
		{
			transform.localPosition = Pos;
		}

	}

}
