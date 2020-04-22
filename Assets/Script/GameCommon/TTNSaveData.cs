using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTNSaveData : SingletonMonoBehaviour<TTNSaveData> 
{
	const string SaveKey = "TTNSaveData3";
	public const int ScoreNum = 10;

	
	public class SaveData
	{
		public int Version;
		public double[] Score;
	}

	SaveData m_SaveData = null;

	// Use this for initialization
	public void Init () 
	{
		bool SaveInit = true;

		if (PlayerPrefs.HasKey(SaveKey))
		{
			try
			{
				string Json = PlayerPrefs.GetString(SaveKey);
				m_SaveData = LitJson.JsonMapper.ToObject<SaveData>(Json);

				if (m_SaveData.Version == 1 && m_SaveData.Score.Length == ScoreNum)
				{
					SaveInit = false;
				}
			}
			catch (LitJson.JsonException e)
			{

			}
		}


		if( SaveInit )
		{
			m_SaveData = new SaveData();
			m_SaveData.Version = 1;
			m_SaveData.Score = new double[ScoreNum];
			for( int i=0; i<ScoreNum; i++ )
			{
				m_SaveData.Score[i] = -1;
			}
			Save();
		}
	}

	void Save()
	{
		string Json = LitJson.JsonMapper.ToJson(m_SaveData);
		PlayerPrefs.SetString(SaveKey, Json);
		PlayerPrefs.Save();
	}

	public int EntryScore( double Score )
	{
		for( int i=0; i<ScoreNum; i++ )
		{
			if( m_SaveData.Score[i] >= Score || m_SaveData.Score[i] < 0)
			{
				for( int j=ScoreNum-1; i<j; j--)
				{
					m_SaveData.Score[j] = m_SaveData.Score[j - 1];
				}
				m_SaveData.Score[i] = Score;
				Save();

				return i;
			}
		}


		return -1;
	}
	
	public double GetScore( int RankIndex )
	{
		return m_SaveData.Score[RankIndex];
	}

}
