using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMHexagon : MonoBehaviour
{
    public static BMHexagon Create( Transform Parent )
    {
        GameObject Obj = new GameObject("BMHexagon");

        RectTransform rt = Obj.AddComponent<RectTransform>();
        rt.SetParent(Parent, false);
        rt.sizeDelta = new Vector2(GameDefine.CANVAS_BASE_SIZE_X, GameDefine.CANVAS_BASE_SIZE_Y);

        BMHexagon UI = Obj.AddComponent<BMHexagon>();
//        UI.Init();

        return UI;
    }

    public const int HexMax = 16;
    const float ImgBaseSize = 1024f;

    string m_toColorCode = "#000000";
    Color m_FromColor = Color.black;
    Color m_ToColor = Color.black;
    Image[] m_Hexagon;

    public string toColorCode
    {
        get
        {
            return m_toColorCode;
        }
    }


    public void Init (Sprite sprite)
    {
        float BaseSize = 270 * 2;
        float SizeStep = 120 * 2;

        m_Hexagon = new Image[HexMax];
        for( int i= HexMax-1; 0<=i; i-- )
        {
            float size = BaseSize + SizeStep * i;
            /*if( i == HexMax -1 )
            {
                size *= 2;
            }*/
            float scl = size / ImgBaseSize;

            GameObject Obj = new GameObject("Hexagon_" + i.ToString());
            Obj.transform.SetParent(transform, false);

            m_Hexagon[i] = Obj.AddComponent<Image>();
            //            m_Hexagon[i].sprite = SpriteBankMgr.Instance.GetSprite("bm_hexagon");//g_HexagonSprite;
            m_Hexagon[i].sprite = sprite;
            m_Hexagon[i].SetNativeSize();
            m_Hexagon[i].rectTransform.localScale = new Vector3(scl, scl, scl);
            m_Hexagon[i].color = Color.black;
        }
	}
	
    public void DrawExec( int i, int ordinal, Color preColor)
    {
        if( ordinal < 0 )
        {
            m_Hexagon[i].color = preColor;
        }
        else
        {
            m_Hexagon[i].color = CalcColor(ordinal);
        }
    }

    public Color CalcColor( int ordinal )
    {
        float r = ((float)ordinal) / 20f;
        return Color.Lerp(m_FromColor, m_ToColor, r);
    }

    public void updateEndColor( string fromCC, string toCC )
    {
        m_toColorCode = toCC;

        m_FromColor = fromCC.ToColor();
        m_ToColor = toCC.ToColor();
    }

}
