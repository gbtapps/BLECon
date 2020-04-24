//using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//  ドロップダウンのLabelにInputFieldを乗せて使う(名称変更不可)
//  このスクリプトはドロップダウンにアタッチする
abstract class RecentDropdown:MonoBehaviour
{
    public abstract string PrefsKey { get; }
    public abstract int Limit { get; }

    List<string> m_recent = new List<string>();

    Dropdown m_dropdown;
    InputField m_inputField;
    bool m_nc = false;  //  changeしたことにしないフラグ
    public string text
    {
        get
        {
            return m_inputField.text;
        }
    }

    private void Start()
    {
        string rec = PlayerPrefs.GetString(PrefsKey, "");
        m_dropdown = GetComponent<Dropdown>();
        m_inputField = m_dropdown.transform.Find("Label").Find("InputField").GetComponent<InputField>();

        //  履歴データの準備
        m_recent.Clear();
        if (rec != "" && rec != "{}")
        {
  //          m_recent = JsonConvert.DeserializeObject<List<string>>(rec);
            int i = 0;
            foreach (string s in m_recent)
            {
                if (s == "")
                {
                    m_recent.RemoveAt(i);
                    break;
                }
                i++;
            }
        }
        //  新規入力用を追加
        m_recent.Insert(0,"");
        m_dropdown.ClearOptions();
        m_dropdown.AddOptions(m_recent);

        m_inputField.onValueChanged.AddListener((str) => {
            if (m_nc) return;
            m_nc = true;
            if (m_dropdown.value != 0)
            {
                m_dropdown.value = 0;
            }
            m_dropdown.options[0].text = m_inputField.text;
            m_nc = false;
        });

        m_dropdown.onValueChanged.AddListener((value) => {
            if (m_nc) return;
            m_nc = true;
            m_inputField.text = m_dropdown.options[value].text;
            Debug.Log(value + ":" + m_dropdown.options[value].text);
            m_nc = false;
        });
    }

    private void OnDestroy()
    {
        if (m_dropdown.options.Count > 0 && m_dropdown.value != 0 &&
            m_dropdown.options[m_dropdown.value].text == m_inputField.text)
        {
            //  使ったものを一番上に持ってくる
            var tmp = m_recent[m_dropdown.value];
            m_recent.RemoveAt(m_dropdown.value);
            //  追加ではないので入力用を削除
            m_recent.RemoveAt(0);
            m_recent.Insert(0, tmp);
        }
        else
        {
            //  新しいのを追加
            m_recent[0] = m_inputField.text;
        }
        //  あふれた分を消す
        while(m_recent.Count > Limit)
        {
            m_recent.RemoveAt(m_recent.Count-1);
        }

    //    var json = JsonConvert.SerializeObject(m_recent);
//        PlayerPrefs.SetString(PrefsKey, json);
        PlayerPrefs.Save();
    }
}
