using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonGUI
{

    const int BtnFontSize = 28;
    const int LabelFontSize = 22;
    const int ToggleFontSize = 22;
	const int InputFontSize = 20;

    static GUIStyle s_BtnStyle = null;
    static GUIStyle s_LabelStyle = null;
	static GUIStyleState s_LabelStyleState = new GUIStyleState();
    static GUIStyle s_ToggleStyle = null;
	static GUIStyle s_InputFieldStyle = null;

    public static bool Button( float x, float y, float w, float h, string Text )
    {
        if (s_BtnStyle == null)
        {
            s_BtnStyle = new GUIStyle(GUI.skin.button);
            s_BtnStyle.fontSize = BtnFontSize;
        }

        return GUI.Button(CommonFunc.GetRateRect(x, y, w, h), Text, s_BtnStyle);
    }

    public static bool Toggle( float x, float y, float w, float h, string Text, bool value )
    {
        if( s_ToggleStyle == null )
        {
            s_ToggleStyle = new GUIStyle(GUI.skin.toggle);
            s_ToggleStyle.fontSize = ToggleFontSize;
        }

        return GUI.Toggle(CommonFunc.GetRateRect(x, y, w, h), value, Text, s_ToggleStyle);
    }

    public static void Label( float x, float y, string Text, Color? color = null )
    {
        if( s_LabelStyle == null )
        {
            s_LabelStyle = new GUIStyle(GUI.skin.label);
            s_LabelStyle.fontSize = LabelFontSize;

			s_LabelStyleState = new GUIStyleState();

			
        }

		if (color != null && color.HasValue)
		{
			s_LabelStyleState.textColor = color.Value;
		}
		else
		{
			s_LabelStyleState.textColor = Color.white;
		}

		s_LabelStyle.normal = s_LabelStyleState;

        GUI.Label(CommonFunc.GetRateRect(x, y, 1f-x, 1f-y), Text, s_LabelStyle);
    }

	public static string InputField( float x, float y, float w, float h, string Text )
	{
		if (s_InputFieldStyle == null)
		{
			s_InputFieldStyle = new GUIStyle(GUI.skin.textField);
			s_InputFieldStyle.fontSize = InputFontSize;
		}


		return GUI.TextField(CommonFunc.GetRateRect(x, y, w, h), Text, s_InputFieldStyle);
	}
}
