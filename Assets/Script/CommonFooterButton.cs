using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommonFooterButton : MonoBehaviour
{
    Toggle m_Toggle;

    // Start is called before the first frame update
    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        for (int i = 0; i < ConstData.ButtonList.Length; i++)
        {
            if(ConstData.ButtonScene[i] == CommonData.NowScene) {
                transform.parent.Find(ConstData.ButtonList[i]).GetComponent<Toggle>().isOn = true;
                break;
            }
        }

        m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_Toggle); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void ToggleValueChanged(Toggle change)
    {
        if( m_Toggle.isOn)
        {
            for(int i=0;i < ConstData.ButtonList.Length;i++)
            {
                if (m_Toggle.name.Contains(ConstData.ButtonList[i]))
                {
                    SceneFunc.ChangeScene(ConstData.ButtonScene[i]);
                    break;
                }
            }
        }
    }
}
