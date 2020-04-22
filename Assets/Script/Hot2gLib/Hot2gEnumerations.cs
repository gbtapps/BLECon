public static class Hot2gEnumerations
{
	/**
	 * HOT2Gデバイス対応通信メッセージID
	 */
    public enum EnumHot2gMsgID
    {
        Request_DeviceType= 0x00,
        Report_DeviceType= 0x80,
        Request_DeviceID= 0x01,
        Report_DeviceID= 0x81,
        Request_DeviceName= 0x02,
        Report_DeviceName= 0x82,
        Request_DeviceFirmVer= 0x03,
        Report_DeviceFirmVer= 0x83,
        Request_DeviceHwInfo= 0x04,
        Report_DeviceHwInfo= 0x84,
        Request_WaitIrInfo= 0x05,
        Report_WaitIrInfo= 0x85,
        Request_MotionAcc= 0x06,
        Report_MotionAcc= 0x86,
        Request_MotionGyro= 0x07,
        Report_MotionGyro= 0x87,
        Request_DeviceBattery= 0x08,
        Report_DeviceBattery= 0x88,
        Request_MeasSampling= 0x09,
        Report_MeasSampling= 0x89,
        Request_MeasTransMode= 0x0A,
        Report_MeasTransMode= 0x8A,
        Request_MeasAnalysisMode= 0x0B,
        Report_MeasAnalysisMode= 0x8B,
        Request_DebugFlag= 0x0C,
        Report_DebugFlag= 0x8C,
        Request_LotID= 0x0D,
        Report_LotID= 0x8D,

        Request_ErrorCode= 0x20,
        Report_ErrorCode= 0xA0,
        Request_BatteryGauge= 0x21,
        Report_BatteryGauge= 0xA1,
        Request_BatteryVolt= 0x22,
        Report_BatteryVolt= 0xA2,
		Request_Temperature= 0x23,
        Report_Temperature= 0xA3,
        Request_IrLightOutput= 0x24,
        Report_IrLightOutput= 0xA4,
        Request_UsbState= 0x25,
        Report_UsbState= 0xA5,
        Request_BluetoothState= 0x26,
        Report_BluetoothState= 0xA6,
        Request_SensorBoardEnabled= 0x27,
        Report_SensorBoardEnabled= 0xA7,
        Request_PDEnabled= 0x28,
        Report_PDEnabled= 0xA8,
        Request_MotionEnabled= 0x29,
        Report_MotionEnabled= 0xA9,
        Request_BatteryEnabled= 0x2A,
        Report_BatteryEnabled= 0xAA,

        Request_CtrlAutoMeasStart= 0x40,
        Report_CtrlAutoMeasStart= 0xC0,
        Request_CtrlAutoMeasStop= 0x41,
        Report_CtrlAutoMeasStop= 0xC1,
        Request_CtrlAutoGainStart= 0x42,
        Report_CtrlAutoGainStart= 0xC2,
        Request_CtrlAutoGainStop= 0x43,
        Report_CtrlAutoGainStop= 0xC3,
        Request_PDManuGainIr= 0x44,
        Report_PDManuGainIr= 0xC4,
        Request_PDValueInput= 0x45,
        Report_PDValueInput= 0xC5,
        Request_IrOutputEstablish= 0x46,
        Report_IrOutputEstablish= 0xC6,
        Request_MotionValueInput= 0x47,
        Report_MotionValueInput= 0xC7,
        Request_BatteryInput= 0x48,
        Report_BatteryInput= 0xC8,

        Report_MeasBothAC= 0xD0,
        Report_MeasSideAC= 0xD1,
        Report_MeasDataMotion= 0xD2,
        Report_MeasDataBattery= 0xD3,
        Report_MeasDataMotBatt= 0xD4,
		Report_MeasDataHR= 0xD5,
		Report_MeasDataHRElement1= 0xD6,			// 18.10.11 ohara
		Report_MeasDataHRElement2= 0xD7,			// 18.10.11 ohara

        Request_OtherLedOutput= 0x60,
        Report_OtherLedOutput= 0xE0,
        Request_CtrlAutoDiagStart= 0x61,
        Report_CtrlAutoDiagStart= 0xE1,
        Request_OtherSensManuUpdate= 0x62,
        Report_OtherSensManuUpdate= 0xE2,
        Request_MpuInFlashInformation= 0x63,
        Report_MpuInFlashInformation= 0xE3,

        Request_FirmUpdateStart= 0x70,
        Report_FirmUpdateStart= 0xF0,
        Request_FirmDateTransfer= 0x71,
        Report_FirmDateTransfer= 0xF1,
        Request_FirmSwitchRequest= 0x72,
        Report_FirmSwitchRequest= 0xF2,

		Report_Answer= 0xFF,

        Report_1g_ProbeParam= 0x43,				// C
        Report_1g_MeasBothAC= 0x52,		    	// R
        Report_1g_MeasSideAC= 0x58,			    // X
        Report_1g_MeasDataMoton= 0x59,			// Y
        Report_1g_MeasDataMotBatt= 0x5A,			// Z
        Report_1g_MeasStop = 0x53,					// S


        None = 0xFFFF,

    }
    
    public static byte getByte(this EnumHot2gMsgID msgID) 
    {
        return (byte)msgID;
    }
    
    
    

	/**
	 * HOT2Gデバイス対応通信メッセージ用RW区分
	 */
	public enum EnumHot2gMsgRwType
    {
        enRead = 0,
        enWrite = 1,
    }


    public static byte getByte(this EnumHot2gMsgRwType t)
    {
        return (byte)t;
    }


    /**
	 * HOT-2G API処理結果
	 */
    public enum EnumHot2gResult
	{
		Success,
		Failed,
		NotConnected,
		ReadTimeout,
		WriteTimeout,
		ParamWrong,
		SystemError
	}

	/**
	 * HOT-2G デバイス情報区分定義
	 */
    public enum EnumHot2gDevInfo
    {
        All,
        DevType,
        DevId,
        DevName,
        LotId,
        PDResolution,
        MotAccRange,
        MotGyroRange,
        SamplingRate,
        TransRate,
        AnalysisMode,
        UsbRTOutput,
        Version
    }

	/**
	 * HOT-2G デバイス状態区分定義
	 */
    public enum EnumHot2gDevState
    {
        All,
		ErrorStatus,
        BattGauge,
        BattVolt,
        DevTemp,
        UsbState,
        BtState,
        SensModuleState,
        PDState,
        MotState,
        BattState
    }

	/**
	 * 通信ドライバ区分定義
	 */
    public enum EnumHot2gCommType
    {
        NotSelected,
        Bluetooth,
        Usb
    } ;

	/**
	 * 機種定義
	 */
    public enum EnumHot2gDevType
    {
        HOT1000 = 0,
        HOT2000 = 1,
        XB01 = 2,

    }

	/**
	 * モーション加速度レンジ定義
	 */
    public enum EnumHot2gAccRange
    {
        Default = 0,
        e2G =1,
        e4G =2,
        e8G =3,
        e16G =4,
    }
    
    
    public static byte getByte(this EnumHot2gAccRange range) 
    {
        return (byte)range;
    }

	/**
	 * モーション角速度レンジ定義
	 */
    public enum EnumHot2gGyroRange
    {
        Default = 0,
        e125DPS = 1,
        e250DPS = 2,
        e500DPS = 3,
        e1000DPS = 4,
        e2000DPS = 5,
    }
    
    public static byte getByte(this EnumHot2gGyroRange range) 
    {
        return (byte)range;
    }

	/**
	 * バッテリーサンプリング定義
	 */
    public enum EnumHot2gBattSmplRate
    {
        Disable = 0,
        e0_1HZ =1,
        e0_2HZ =2,
        e0_5HZ =3,
        e1HZ =4,
    }
    
    
    public static byte getByte(this EnumHot2gBattSmplRate rate) 
    {
        return (byte)rate;
    }

	/**
	 * モーションサンプリング定義
	 */
    public enum EnumHot2gMotSmplRate
    {
        Disable =0,
        e1HZ =1,
        e10HZ =2,
        e20HZ =3,
        e50HZ =4,
    }
    
    public static byte getByte(this EnumHot2gMotSmplRate rate) 
    {
        return (byte)rate;
    }

	/**
	 * バッテリー転送周期定義
	 */
    public enum EnumHot2gBattTransRate
    {
        Disable = 0,
        e0_1HZ =1,
        e0_2HZ =2,
        e0_5HZ =3,
    	e1HZ =4,
    }
    public static byte getByte(this EnumHot2gBattTransRate rate) 
    {
        return (byte)rate;
    }

	/**
	 * モーション転送周期定義
	 */
    public enum EnumHot2gMotTransRate
    {
        Disable =0,
        e1HZ =1,
        e10HZ =2,
        e20HZ =3,
        e50HZ =4,
    }
    
    public static byte getByte(this EnumHot2gMotTransRate rate) 
    {
        return (byte)rate;
    }

	/**
	 * 心拍数転送周期定義
	 */
    public enum EnumHot2gHRTransRate
    {
        Disable = 0,
        e1HZ = 1,
        e2HZ = 2,
        e5HZ = 3,
        e10HZ = 4,
    }
    
    public static byte getByte(this EnumHot2gHRTransRate rate) 
    {
        return (byte)rate;
    }

	/**
	 * ゲイン更新区分定義
	 */
    public enum EnumHot2gMgcType
    {
        Disable = 3,
        All = 0,
        Gain = 2,
        Bright = 1,
    }
    
    public static byte getByte(this EnumHot2gMgcType t) 
    {
        return (byte)t;
    }

	/**
	 * 計測結果区分定義
	 */
    public enum EnumHot2gMeasResult
    {
        Ac2ch,
        Ac1ch,
        Motion,
        MotionBatt,
        Battery,
		HeartRate,
		HRElement1,		// 18.10.11 ohara 追加
		HRElement2,			// 18.10.11 ohara 追加



        None = -1,
    }

	/**
	 * 機器検索結果区分定義
	 */
    public enum EnumHot2gWatcherEvent
    {
        Added,
        Updated,
        Removed,
        EnumerationCompleted,
        Stopped
    }

	/**
	 *
	 */
    public enum EnumMsgNotifyType
    {
        NormalMessage,
        StatusMessage,
        ErrorMessage
    };

	/**
	 * ドライバ通信状態イベント
	 */
    public enum EnumHot2gDriverCommStsEvent
    {
        Connected,
        Disconnected,
        ServiceDiscovered,
		UsbAttached,			// [181025.3] 接続検知
		UsbDetached			// [181025.3] 切断検知
    }

	/**
	 * 通信状態変化イベント
	 */
	public enum EnumHot2gCommStsChangedEvent
	{
		Disconnected,
		Connected,
		Attached,			// [181025.3] 接続検知
		Detached			// [181025.3] 切断検知
	}

	/**
	 * BT受信メッセージ区分
	 */
	public enum EnumHot2gCommBtMsgEvent
	{
		Read,
		Written,
		Notified
	}

	/**
	 * USB受信メッセージ区分
	 */
	public enum EnumHot2gCommUsbMsgEvent
	{
		Read,
		Written,
		Notified
	}
}
