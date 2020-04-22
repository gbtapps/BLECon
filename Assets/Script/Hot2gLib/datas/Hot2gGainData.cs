/**
 * デバイス自動ゲイン調整結果データ
 */
public class Hot2gGainData
{
	public const byte	valMaxGain_0ch	= 16;// 1cmゲイン設定値 最大値定数
	public const byte	valMaxGain_1ch	= 16*4;// 3cmゲイン設定値 最大値定数

	// MGC ã‚²ã‚¤ãƒ³åˆ�æœŸå€¤
	public const byte	val_ch0_L_AmpGain		= 4;		// Gain設定値(左1cm)
	public const byte	val_ch1_L_AmpGain		= 6;		// Gain設定値(左3cm)
	public const byte	val_ch0_R_AmpGain		= 4;		// Gain設定値(右1cm)
	public const byte	val_ch1_R_AmpGain		= 6;        // Gain設定値(右3cm)

    // singleton
    private static Hot2gGainData s_instance = null;
	public static Hot2gGainData instance
	{
        get
        {
            if( s_instance == null )
            {
                s_instance = new Hot2gGainData();
            }
            return s_instance;
        }
	}
	private Hot2gGainData()
	{
	}

    //-----------------------------------------------------------------
    // ゲインパラメータ 格納クラス定義	// 18.10.03 手動ゲイン設定用に追加
    public class ParamGain
    {
        public byte		ch0_L_AmpGain;				// Gain設定値(左1cm)
        public byte		ch1_L_AmpGain;				// Gain設定値(左3cm)
        public byte		ch0_R_AmpGain;				// Gain設定値(右1cm)
        public byte		ch1_R_AmpGain;				// Gain設定値(右3cm)

		public ParamGain()
		{
			ch0_L_AmpGain = val_ch0_L_AmpGain ;
			ch1_L_AmpGain = val_ch1_L_AmpGain ;
			ch0_R_AmpGain = val_ch0_R_AmpGain ;
			ch1_R_AmpGain = val_ch1_R_AmpGain ;
		}
    }

    //-----------------------------------------------------------------
    // プローブパラメータ 格納クラス定義
    public class ParamLed
    {
        public short	led_L;						// 近赤外線パワー設定(左)
        public short	led_R;						// 近赤外線パワー設定(右)

		public ParamLed()
		{
			led_L = -1 ;
			led_R = -1 ;
		}
    }

    //-----------------------------------------------------------------
    // プローブパラメータ 格納クラス定義
    public class ParamProbe	// 18.10.03 手動ゲイン設定用に追加
    {
        public byte		ch0_L_AmpGain;				// Gain設定値(左1cm)
        public byte		ch1_L_AmpGain;				// Gain設定値(左3cm)
        public byte		ch0_R_AmpGain;				// Gain設定値(右1cm)
        public byte		ch1_R_AmpGain;				// Gain設定値(右3cm)
        public short	ch1_L_PreAmpGain;			// AMP有効無効切り替え(左3cm)
        public short	ch1_R_PreAmpGain;			// AMP有効無効切り替え(右3cm)
    }

	public class ProbeSetting
	{
		public int ch0AmpGain;
		public int ch1AmpGain;
		public int ch1PreAmpGain;
		public int ledGain;

		public ProbeSetting(int ch0, int ch1, int ch1pre, int led)
		{
			ch0AmpGain = ch0;
			ch1AmpGain = ch1;
			ch1PreAmpGain = ch1pre;
			ledGain = led;
		}
	}

	static public ProbeSetting getProbeSetting(int ch0, int ch1, int ch1pre, int led)
	{
		return new ProbeSetting(ch0, ch1, ch1pre, led);
	}

	// ゲインパラメータをプローブパラメータへ変換
	public static void CnvGainToProbe( ParamGain paramGain, ParamProbe paramProbe )
	{
		byte calcGain;

		// 左1cmと右1cmを変換
		for( int i=0; i<2; i++ )
		{
			// paramGainから変換元のゲイン値を取得
			if( i == 0 )
			{
				calcGain = paramGain.ch0_L_AmpGain;
			}else{
				calcGain = paramGain.ch0_R_AmpGain;
			}

			// 最大値判定
			if( calcGain > valMaxGain_0ch )
			{
				calcGain = valMaxGain_0ch;
			}
			// 最小値判定
			if( calcGain < 1 )
			{
				calcGain = 1;
			}

			// paramProbe用の倍率指定に変換
			calcGain--;
			// paramProbeへ格納
			if( i == 0 )
			{
				paramProbe.ch0_L_AmpGain = calcGain;
			}else{
				paramProbe.ch0_R_AmpGain = calcGain;
			}
		}

		// 左3cmと右3cmを変換
		for( int i=0; i<2; i++ )
		{
			short probePre;
			byte  probeGain;

			// paramGainから変換元のゲイン値を取得
			if( i == 0 )
			{
				calcGain = paramGain.ch1_L_AmpGain;
			}else{
				calcGain = paramGain.ch1_R_AmpGain;
			}

			// 最大値＆最小値を有効範囲内に丸める
			if( calcGain > valMaxGain_1ch )
			{
				calcGain = 64;
			}else{
				if( calcGain == 0 )
				{
					calcGain = 1;
				}
			}
			// 計算
			// Pre4判定
			if( ((calcGain%4) == 0) ||
					((calcGain/3) > 16) )
			{
				// calcGainを4で割った余りが0の場合、もしくは
				// calcGainを3で割った値が16を超える場合
				probePre  = 4;
				probeGain = (byte)(calcGain/4);
			}else{
				// Pre3判定
				if( ((calcGain%3) == 0) ||
						((calcGain/2) > 16) )
				{
					// calcGainを3で割った余りが0の場合、もしくは
					// calcGainを2で割った値が16を超える場合
					probePre  = 3;
					probeGain = (byte)(calcGain/3);
				}else{
					if( ((calcGain%2) == 0) ||
							( calcGain    > 16) )
					{
						// calcGainを2で割った余りが0の場合、もしくは
						// calcGainが16を超える場合
						probePre  = 2;
						probeGain = (byte)(calcGain/2);
					}else{
						// Pre1設定
						probePre  = 1;
						probeGain = calcGain;
					}
				}
			}
			// paramProbeの倍率指定に変換
			probeGain--;					// AmpGainは1～16指定を0～15指定に変換
			switch( probePre )				// PreAmpGainは1～4指定を倍率に割り当てられた値へ変換
			{
				case 4:				// 倍率：4倍
					probePre = 3;
					break;
				case 3:				// 倍率：3倍
					probePre = 2;
					break;
				case 2:				// 倍率：2倍
					probePre = 1;
					break;
				case 1:				// 倍率：1倍
				default:			// ※それ以外の場合1倍扱いとする
					probePre = 0;
					break;
			}
			// paramProbeへ格納
			if( i == 0 )
			{
				paramProbe.ch1_L_PreAmpGain	= probePre;
				paramProbe.ch1_L_AmpGain	= probeGain;
			}else{
				paramProbe.ch1_R_PreAmpGain	= probePre;
				paramProbe.ch1_R_AmpGain	= probeGain;
			}
		}
/*
		if( IS_DEBUGGING )
		{
			String string;
			SdkLogger sdkLogger = new SdkLogger();
			string = String.format("paramGain : (L1)%d (L3)%d (R1)%d (R3)%d",
					paramGain.ch0_L_AmpGain,
					paramGain.ch1_L_AmpGain,
					paramGain.ch0_R_AmpGain,
					paramGain.ch1_R_AmpGain);
			sdkLogger.putInfo("CnvGainToProbe", string );

			string = String.format("paramProbe: (L1)%d (L3)%d (L3P)%d (R1)%d (R3)%d (R3P)%d",
					paramProbe.ch0_L_AmpGain,
					paramProbe.ch1_L_AmpGain,
					paramProbe.ch1_L_PreAmpGain,
					paramProbe.ch0_R_AmpGain,
					paramProbe.ch1_R_AmpGain,
					paramProbe.ch1_R_PreAmpGain);
			sdkLogger.putInfo("CnvGainToProbe", string );
		}
*/
	}

	// プローブパラメータをゲインパラメータへ変換
	public static void CnvProbeToGain( ParamProbe paramProbe, ParamGain paramGain )
	{
		byte	calcGain;
		// 左1cmと右1cmを変換
		for( int i=0; i<2; i++ )
		{
			// ParamProbeから変換元のゲイン値を取得
			if( i == 0 )
			{
				calcGain = paramProbe.ch0_L_AmpGain;
			}else{
				calcGain = paramProbe.ch0_R_AmpGain;
			}

			// 有効値に丸め込む
			if( calcGain > (valMaxGain_0ch-1) )
			{
				calcGain = (valMaxGain_0ch-1);
			}
			// 補正
			calcGain++;

			// paramGainへ格納
			if( i == 0 )
			{
				paramGain.ch0_L_AmpGain = calcGain;
			}else{
				paramGain.ch0_R_AmpGain = calcGain;
			}
		}

		// 左3cmと右3cmを変換
		for( int i=0; i<2; i++ )
		{
			short probePre;
			byte  probeGain;

			// ParamProbeから変換元のゲイン値を取得
			if( i == 0 )
			{
				probePre	= paramProbe.ch1_L_PreAmpGain;
				probeGain	= paramProbe.ch1_L_AmpGain;
			}else{
				probePre	= paramProbe.ch1_R_PreAmpGain;
				probeGain	= paramProbe.ch1_R_AmpGain;
			}
			// paramProbeの倍率指定を変換
			probeGain++;					// AmpGainは0～15指定を1～16指定に変換
			switch( probePre )				// PreAmpGainは倍率に割り当てられた値を1～4指定に変換
			{
				case 3:			// 倍率：4倍
					probePre = 4;
					break;
				case 2:			// 倍率：3倍
					probePre = 3;
					break;
				case 1:			// 倍率：2倍
					probePre = 2;
					break;
				case 0:			// 倍率：1倍
				default:		// ※それ以外の場合1倍扱いとする
					probePre = 1;
					break;
			}
			// paramGain用の倍率指定へ変換
			calcGain = (byte)(probePre * probeGain);

			// paramGainへ格納
			if( i == 0 )
			{
				paramGain.ch1_L_AmpGain = calcGain;
			}else{
				paramGain.ch1_R_AmpGain = calcGain;
			}
		}
/*
		if( IS_DEBUGGING )
		{
			String string;
			SdkLogger sdkLogger = new SdkLogger();
			string = String.format("paramProbe: (L1)%d (L3)%d (L3P)%d (R1)%d (R3)%d (R3P)%d",
					paramProbe.ch0_L_AmpGain,
					paramProbe.ch1_L_AmpGain,
					paramProbe.ch1_L_PreAmpGain,
					paramProbe.ch0_R_AmpGain,
					paramProbe.ch1_R_AmpGain,
					paramProbe.ch1_R_PreAmpGain);
			sdkLogger.putInfo("CnvProbeToGain", string );
			string = String.format("paramGain : (L1)%d (L3)%d (R1)%d (R3)%d",
					paramGain.ch0_L_AmpGain,
					paramGain.ch1_L_AmpGain,
					paramGain.ch0_R_AmpGain,
					paramGain.ch1_R_AmpGain);
			sdkLogger.putInfo("CnvProbeToGain", string );
		}
*/
	}

}
