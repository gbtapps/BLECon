using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSoundObj : MonoBehaviour 
{

	AudioSource m_Source = null;


	public void Init( AudioClip Clip, Vector3 Pos )
	{
		transform.position = Pos;

		m_Source = gameObject.GetComponent<AudioSource>();
		m_Source.clip = Clip;
		m_Source.Play();
	}

	// Update is called once per frame
	void Update () 
	{
		if( m_Source == null )
		{
			Destroy(gameObject);
			return;
		}

		if( !m_Source.isPlaying )
		{
			Destroy(gameObject);
			return;
		}
	}
}
