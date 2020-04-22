using UnityEngine;
using System.Collections;

public class DefaultUIBase : MonoBehaviour
{
	public virtual CanvasMgr.eDispSort DispZ
    {
        get
        {
			return CanvasMgr.eDispSort.Debug;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public virtual void Init(){}

    public virtual void Update()
    {
        if( GameMgr.isInitDone )
        {
            UpdateExec();
        }
    }

    public virtual void UpdateExec()
    {

    }
}
