using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
	public enum eScene
    {
        BMTop,
        BMMain,
		BMResult,

		TTNTitle,
		TTNMain,

        Max,

        None = -1,
    };

    public static string[] SceneNameTbl =
    {
        "BMTop",
        "BMMain",
		"BMResult",
		"TTNTitle",
		"TTNMain",

	};

	public enum eBank
	{
        BrainMeter,
        TouchNumber,
		Common,
	}

	public static string[] BankNameTbl =
	{
        "bm",
        "ttn",
		"common",
	};

	public const float CANVAS_BASE_SIZE_X = 1080.0f;
	public const float CANVAS_BASE_SIZE_Y = 1920.0f;

	public const string SOUND_BANK_SE = "se_all";
	public const string SOUND_BANK_JINGLE = "jingle_all";

}


