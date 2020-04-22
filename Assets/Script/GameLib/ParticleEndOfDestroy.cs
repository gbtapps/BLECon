using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEndOfDestroy : MonoBehaviour 
{
    ParticleSystem[] m_Particle;

	// Use this for initialization
	void Start ()
    {
        m_Particle = gameObject.GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if( m_Particle == null )
        {
            Destroy(gameObject);
            return;
        }

        bool isDestroy = true;

        for( int i=0 ; i<m_Particle.Length; i++ )
        {
            if( m_Particle[i].IsAlive() )
            {
                isDestroy = false;
            }
        }

        if( isDestroy )
        {
            Destroy(gameObject);
        }
	}
}
