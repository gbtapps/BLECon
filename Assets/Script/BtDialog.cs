using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BtDialog : MonoBehaviour
{
    List<Toggle> m_toggleList = new List<Toggle>();

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            m_instance = this;
        }
    }

    //  生成用
    static BtDialog instance;
    //  表示用
    static BtDialog m_instance;

    static public void OpenDialog(string last, List<string> list)
    {
        if (m_instance != null)
        {
            return;
        }

        m_instance = Instantiate(instance, instance.transform.parent, false);

        list = list.Distinct().ToList<string>();
        m_instance.transform.Find("ConnectBG").gameObject.SetActive(true);
        var baseTransform = m_instance.transform.Find("ConnectBG").Find("ConnectDialog");
        baseTransform.Find("Text").GetComponent<Text>().text = "Last connect:\n" + last;
        var content = baseTransform.Find("Scroll View").Find("Viewport").Find("Content");
        var toggle = content.Find("Toggle").GetComponent<Toggle>();
        content.GetComponent<ToggleGroup>().allowSwitchOff = true;
        bool isOn = false;
        bool first = true;
        foreach (var dev in list)
        {
            Toggle newToggle = toggle;
            if (first)
            {
                //  一個目はある奴を使う
                first = false;
            }
            else
            {
                newToggle = Instantiate(toggle, content, false);
                m_instance.m_toggleList.Add(newToggle);
            }
            newToggle.transform.Find("Label").GetComponent<Text>().text = dev;

            //  最後につないでたのをOnに
            if (dev == last)
            {
                newToggle.isOn = true;
                isOn = true;
            }
            else
            {
                newToggle.isOn = false;
            }
        }
        if (!isOn)
        {
            toggle.isOn = true;
        }

        content.GetComponent<ToggleGroup>().allowSwitchOff = false;

        var button = baseTransform.Find("Ok").GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            string dev = null;
            foreach (var t in content.GetComponent<ToggleGroup>().ActiveToggles())
            {
                if (t.isOn)
                {
                    dev = t.transform.Find("Label").GetComponent<Text>().text;
                }
            }
            if (dev != null)
            {
                Hot2gApplication.Instance.Connecting(dev, () => { }, () => { }, () => { });
                PlayerPrefs.SetString(BluetoothMgr.cLastConnectSaveKey, dev);
            }
            Destroy(m_instance.gameObject);
            m_instance = null;
        });

        var buttonClose = baseTransform.Find("Close").GetComponent<Button>();
        buttonClose.onClick.AddListener(() =>
        {
            Hot2gApplication.Instance.DisConnect();
            Destroy(m_instance.gameObject);
        });

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
