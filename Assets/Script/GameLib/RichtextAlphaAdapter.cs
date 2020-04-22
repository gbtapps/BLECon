using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class RichtextAlphaAdapter : MonoBehaviour 
{
	private Text text;
	private Regex regex;

	void Start () 
	{
		text = GetComponent<Text> ();

		// 大文字小文字の区別、二重引用符の有無など、リッチテキストの仕様に準拠
		string pattern = "(<color=\"?#[a-f0-9]{6})([a-f0-9]{2})?(\"?>)";
		regex = new Regex(pattern, RegexOptions.IgnoreCase);

		prevAlpha = text.color.a;
		prevText = text.text;
	}

	private float prevAlpha;
	private string prevText;
	void Update()
	{
		if (!Mathf.Approximately(text.color.a, prevAlpha)
			|| !text.text.Equals(prevText)) {
			UpdateAlpha ();
			prevAlpha = text.color.a;
			prevText = text.text;
		}
	}

	private void UpdateAlpha()
	{		
		float alpha = text.color.a;
		UpdateAlpha (alpha);
	}
	private void UpdateAlpha(float alpha)
	{
		string alpha16 = ConvertTo16 (alpha);

		string replacement = "${1}" + alpha16 + "${3}";
		text.text = regex.Replace (text.text, replacement);
	}

	private string ConvertTo16(float alpha)
	{
		// Text.colorの持つアルファ値は0f-1f
		// 16進数に変換するために0-255の整数型に変換
		int alpha10 = (int)(alpha * 255f);

		string alpha16 = Convert.ToString (alpha10, 16);

		// 16進数で一桁の場合、二桁目を0詰する。
		if (alpha10  < 16) alpha16 = "0" + alpha16;

		//Debug.Log ("aaa");
		return alpha16;
	}
}
