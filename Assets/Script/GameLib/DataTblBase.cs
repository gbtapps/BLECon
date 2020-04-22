using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class DataTblBase_Struct<S> where S : struct
{
	static S[] s_Data = null;
	static FieldInfo FieldId;//メンバのid　これは絶対にある

	static public void Init(string JsonString, string IndexName="id")
	{
/*
#if DEBUG_MODE
		if (isLocalDebug)
		{
			TextAsset text = Resources.Load("Battle/Debug/" + JsonFile) as TextAsset;
			s_Data = JsonMapperEX.ToObject<S[]>(text.text);
		}
		else
#endif
*/
		{
			s_Data = JsonUtility.FromJson<S[]>(JsonString);//JsonMapperEX.ToObject<S[]>(ParamMgr.GetJson(JsonFile));
		}
		FieldId = typeof(S).GetField(IndexName);
	}

	/// <summary>
	/// １つのデータを得る
	/// </summary>
	static public S? GetData(int ID)
	{
		for (int i = 0; i < s_Data.Length; i++)
		{

			int SrcID = (int)FieldId.GetValue(s_Data[i]);
			if (SrcID == ID)
			{
				return s_Data[i];
			}
		}

		return null;
	}


	/// <summary>
	/// １つの確定データを得る(範囲外を要求したら０番のを返す)
	/// </summary>
	static public S GetDataRaw(int ID)
	{
		for (int i = 0; i < s_Data.Length; i++)
		{

			int SrcID = (int)FieldId.GetValue(s_Data[i]);
			if (SrcID == ID)
			{
				return s_Data[i];
			}
		}

		return s_Data[0];
	}

	static public S? GetDataIdx(int Index)
	{
		if (Index < 0 || s_Data.Length <= Index)
		{
			return null;
		}

		return s_Data[Index];
	}

	static public S GetDataIdxRaw(int Index)
	{
		if (Index < 0 || s_Data.Length <= Index)
		{
			return s_Data[0];
		}

		return s_Data[Index];
	}

	// IDからデータのIndexを取得
	static public int IDToIndex(int ID)
	{
		for (int i = 0; i < s_Data.Length; i++)
		{
			int SrcID = (int)FieldId.GetValue(s_Data[i]);
			if (SrcID == ID)
			{
				return i;
			}
		}
		return -1;
	}

	static public void AscSort(string ColumnName )
	{
		FieldInfo Column = typeof(S).GetField(ColumnName);

		for(int i=0; i<s_Data.Length-1; i++ )
		{
			for( int j=i+1; j<s_Data.Length; j++ )
			{
				int NumI = (int)Column.GetValue(s_Data[i]);
				int NumJ = (int)Column.GetValue(s_Data[j]);

				if( NumJ < NumI )
				{
					S Buf = s_Data[i];
					s_Data[i] = s_Data[j];
					s_Data[j] = Buf;
				}
			}
		}
	}

	static public int DataNum
	{
		get
		{
			if( s_Data == null )
			{
				return 0;
			}

			return s_Data.Length;
		}
	}
}
