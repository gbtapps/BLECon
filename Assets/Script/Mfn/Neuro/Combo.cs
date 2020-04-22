using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    float alpha;
    //Image ring;
    //Image mul;
    //SpriteNumber comboNum;
    Image perfect;
    Text txtComboNum;

    // Start is called before the first frame update
    public void Init()
    {
        //ring = gameObject.FindDescendant("RingIcon").GetComponent<Image>();
        //mul = gameObject.FindDescendant("mul").GetComponent<Image>();
        //comboNum = gameObject.FindDescendant("ComboNum").GetComponent<SpriteNumber>();
        //comboNum.gameObject.SetActive(false);
        txtComboNum = gameObject.FindDescendant("txtComboNum").GetComponent<Text>();
        perfect = gameObject.FindDescendant("Perfect").GetComponent<Image>();
        perfect.gameObject.SetActive(false);
        alpha = 1;
    }

    public void StartAnime(Vector3 _pos)
    {
        gameObject.transform.localPosition = _pos;
        SetColor(new Color(1,1,1,1));
        alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime*2;
            if (alpha < 0) { alpha = 0; }
            SetColor(new Color(1, 1, 1, alpha));

            gameObject.transform.localPosition = new Vector3(
                gameObject.transform.localPosition.x,
                gameObject.transform.localPosition.y + Time.deltaTime*300,
                gameObject.transform.localPosition.z
                );
        }
    }

    public void SetColor(Color _color)
    {
        //ring.color = _color;
        //mul.color = _color;
        //comboNum.SetColor(_color);
        perfect.color = _color;

        Color col = txtComboNum.color;
        col.a = _color.a;
        txtComboNum.color = col;
    }

    public void SetComboNum(int _value,bool _perfect =false)
    {
        //comboNum.SetViewNum(_value);
        perfect.gameObject.SetActive(_perfect);
        txtComboNum.text = "x" + _value.ToString();
    }

}
