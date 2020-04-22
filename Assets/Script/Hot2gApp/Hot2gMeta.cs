using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hot2gMeta
{
	/** デバイス種別 */
	public string type;

	/** mac address */
	public string mac;

	/** 測定日 */
	public string measured;

	/** 実験デザイン */
	public string designId;


	/** 性別 */
	public string gender;

	/** 年齢 */
	public byte age;

	public string ToCSV()
	{
		string CSV = type + "," + mac + "," + measured + "," + designId + "," + gender + "," + age;

		return CSV;
	}

}