using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataLogger
{
	string FilePath = null;
	StreamWriter streamWriter = null;
	int Counter = 0;

	public DataLogger()
	{
		FilePath = Application.persistentDataPath + "/xblog" + DateTime.Now.ToString("yyyy-MM-dd.HH.mm.ss") + ".csv";
		Debug.Log("Logger Init SavePath:"+ FilePath);
		streamWriter = File.AppendText(FilePath);
	}

	/**
     * データ1件分＝1行分を書き込みます。ヘッダ書き込みもこれを利用してください。
     * @param data データ
     */
	public void write(string DataStr)
	{
		if(FilePath == null )
		{
			Debug.Log("Logger is not init");
			return;
		}
		//Debug.Log("Logger "+DataStr);

		//File.AppendAllText(FilePath, DataStr + Environment.NewLine);
		streamWriter.WriteLine(DataStr);
		Counter++;

		if(5 < Counter)
		{
			streamWriter.Flush();
			Counter = 0;
		}
	}

	public void close()
	{
		if(streamWriter ==null)
		{
			return;
		}
		streamWriter.Flush();
		streamWriter.Close();

		FilePath = null;
		streamWriter = null;
		Counter = 0;
	}

}
