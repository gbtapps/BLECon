using UnityEngine;
using System.Collections;

public class RotateObj : MonoBehaviour 
{
    public Vector3 g_RotateDegPerSec;
    public bool g_isRotateLocal = true;

	
	// Update is called once per frame
	void Update () 
    {
        if (g_isRotateLocal)
        {
            Vector3 R = transform.localEulerAngles;
            R += g_RotateDegPerSec * Time.deltaTime;
            transform.localEulerAngles = R;
        }
        else
        {
            Vector3 R = transform.eulerAngles;
            R += g_RotateDegPerSec * Time.deltaTime;
            transform.eulerAngles = R;
        }
	}
}
