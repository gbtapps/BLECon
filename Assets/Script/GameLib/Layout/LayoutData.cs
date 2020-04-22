using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LayoutData 
{


	public int psd_id;
	public int id;
	public string name;
	public string label;
	public bool null_node;

	public float x;
	public float y;
	public float w;
	public float h;

	public string origin;

	public float ox;
	public float oy;
	public float ow;
	public float oh;

	public float sx;
	public float sy;

	public string anchor;

	public float alpha;

	public bool is_button;
	public bool is_toggle;
	public bool is_nineslice;

	public bool is_num;
	public int num_arigen;

	public bool is_text;
	public LayoutTextData text_data = null;
	public LayoutDropData drop_data = null;
	public LayoutOutlineData outline_data = null;

	public LayoutData[] child = null;
}

[System.Serializable]
public class LayoutTextData
{
	public int size;
	public int aligan;
	public string content;
	public string color;
	public bool is_bold;
	public string font_name;

	public LayoutTextData()
	{

	}

	public LayoutTextData( string Text, int Size, Color col, bool isBold, LayoutImage.eTextArigen Arigen, string FontName )
	{
		size = Size;
		aligan = (int)Arigen;
		content = Text;
		color = ColorUtility.ToHtmlStringRGB( col );
		is_bold = isBold;
		font_name = FontName;
	}
}

[System.Serializable]
public class LayoutDropData
{
	public string color;
	public int alpha;
	public float angle;
	public int blur;
	public int distance;
}

[System.Serializable]
public class LayoutOutlineData
{
	public string color;
	public int thickness;
	public string line_join;
}
