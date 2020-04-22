using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeBank : MonoBehaviour 
{
	Dictionary<string, AudioClip> m_AudioClip;

	public void Init()
	{
		m_AudioClip = new Dictionary<string, AudioClip>();
	}

	public AudioClip GetAudioClip( string Name )
	{
		if( m_AudioClip.ContainsKey( Name ) )
		{
			return m_AudioClip[Name];
		}

		m_AudioClip.Add( Name, null);

		GameObject obj = gameObject.FindDescendant(Name);
		if( obj != null )
		{
			AudioSource s = obj.GetComponent<AudioSource>();
			if( s != null )
			{
				m_AudioClip[Name] = s.clip;
			}
			//m_AudioClip[Name] = obj.GetComponent<AudioClip>();
		}
		
		return m_AudioClip[Name];
	}
}
