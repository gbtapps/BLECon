using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExButton : MonoBehaviour
{
    Button m_Button;
    Image m_Image;
    string m_LastPath = "";

    public Button button
    {
        get
        {
            return m_Button;
        }
    }

    public bool lastHit
    {
        get
        {
            return m_LastHit;
        }
    }

    public bool lastHit2
    {
        get
        {
            bool r = m_LastHit2;
            m_LastHit2 = false;
            return r;
        }
    }

    public bool isPress { get; private set; }

    bool m_LastHit = false;
	bool m_LastHit2 = false;

    // Use this for initialization
    void Awake ()
    {
        m_Image = gameObject.GetComponent<Image>();
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(this.OnClickButton);

        EventTrigger _eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry _entry = new EventTrigger.Entry();
        _entry.eventID = EventTriggerType.PointerUp;
        _entry.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });

        _eventTrigger.triggers.Add(_entry);


        EventTrigger.Entry _entry2 = new EventTrigger.Entry();
        _entry2.eventID = EventTriggerType.PointerDown;
        _entry2.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });

        _eventTrigger.triggers.Add(_entry2);

        isPress = false;
    }

    public void LateUpdate()
    {
        m_LastHit = false;
    }

    public void OnClickButton()
    {
        m_LastHit = true;
		m_LastHit2 = true;
    }

    public void OnPointerUpDelegate(PointerEventData data)
    {
        isPress = false;
    }
    public void OnPointerDownDelegate(PointerEventData data)
    {
        isPress = true;
    }

    public void ChangeImgFromResources( string ImagePath )
    {
        if (m_LastPath != ImagePath)
        {
            Sprite sp = Resources.Load<Sprite>(ImagePath);
            m_Image.sprite = sp;
        }
        m_LastPath = ImagePath;
    }

    public void ChangeImgFromSpriteBank( string ImgName )
    {
        if( m_LastPath != ImgName )
        {
            m_Image.sprite = SpriteBankMgr.Instance.GetSprite(ImgName);
        }
        m_LastPath = ImgName;
    }

    public void SetColor(Color _color)
    {
        m_Image.color = _color;
    }

}
