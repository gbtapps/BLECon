using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シーンオブジェクトのベース
// 各シーンに1個
// GameMgrの初期化を待ってから動き出す

public class SceneBase : MonoBehaviour
{
    protected bool m_ExecFlag = false; // 実行許可
    protected GameDefine.eBank[] m_UseBankList = null;
    protected string[] m_UseBgmList = null;

    // Awake Startは使用しない
    private void Awake() { }
    private void Start() { }

    public virtual bool isInitDone
    {
        get
        {
            return true;
        }
    }


    // GameMgrがInitDone読んだ後に呼ぶから気にしないで
    public void SetExecFlag()
    {
        m_ExecFlag = true;
    }

    // スプライトのロード
    public void LoadSpriteBank()
    {
        /*switch( SaveDataMgr.Instance.Langage )
		{
			case SaveDataMgr.eLangage.EN: SpriteBankMgr.Instance.LoadBank(GameDefine.eBank.EN); break;
			case SaveDataMgr.eLangage.CN: SpriteBankMgr.Instance.LoadBank(GameDefine.eBank.CN); break;
			case SaveDataMgr.eLangage.KR: SpriteBankMgr.Instance.LoadBank(GameDefine.eBank.KR); break;
		}*/

        if (m_UseBankList == null)
        {
            return;
        }

        for (int i = 0; i < m_UseBankList.Length; i++)
        {
            SpriteBankMgr.Instance.LoadBank(m_UseBankList[i]);
        }


    }

    public void LoadBGM()
    {
        if (m_UseBgmList == null)
        {
            return;
        }

        for (int i = 0; i < m_UseBgmList.Length; i++)
        {
            SoundMgr.Instance.LoadBGM(m_UseBgmList[i]);
        }
    }



    // Updateは使用しない
    private void Update()
    {
        if( !m_ExecFlag )
        {
            return;
        }

        UpdateExec();
    }




    // Awake Startの代わりにInit
    public virtual void Init()
    {

    }

    // Updateの代わりにUpdateExec
    public virtual void UpdateExec()
    {

    }

    //
    //public virtual void Exit(){}

}
