using System;
using UnityEngine;
/**
 * HOT-2G対応コマンド
 */
public class Hot2gCommands
{
	public Hot2gCommandRequest00		Request00 ;
	public Hot2gCommandRequest01		Request01 ;
	public Hot2gCommandRequest02		Request02 ;
	public Hot2gCommandRequest03		Request03 ;
	public Hot2gCommandRequest04		Request04 ;
	public Hot2gCommandRequest05		Request05 ;
	public Hot2gCommandRequest06		Request06 ;
	public Hot2gCommandRequest07		Request07 ;
	public Hot2gCommandRequest08		Request08 ;
	public Hot2gCommandRequest09		Request09 ;
	public Hot2gCommandRequest0A		Request0A ;
	public Hot2gCommandRequest0B		Request0B ;
	public Hot2gCommandRequest0C		Request0C ;
	public Hot2gCommandRequest0D		Request0D ;
	public Hot2gCommandRequest20		Request20 ;
	public Hot2gCommandRequest21		Request21 ;
	public Hot2gCommandRequest22		Request22 ;
	public Hot2gCommandRequest23		Request23 ;
	public Hot2gCommandRequest24		Request24 ;
	public Hot2gCommandRequest25		Request25 ;
	public Hot2gCommandRequest26		Request26 ;
	public Hot2gCommandRequest27		Request27 ;
	public Hot2gCommandRequest28		Request28 ;
	public Hot2gCommandRequest29		Request29 ;
	public Hot2gCommandRequest2A		Request2A ;
	public Hot2gCommandRequest40		Request40 ;
	public Hot2gCommandRequest41		Request41 ;
	public Hot2gCommandRequest42		Request42 ;
	public Hot2gCommandRequest43		Request43 ;
	public Hot2gCommandRequest44		Request44 ;
	public Hot2gCommandRequest45		Request45 ;
	public Hot2gCommandRequest46		Request46 ;
	public Hot2gCommandRequest47		Request47 ;
	public Hot2gCommandRequest48		Request48 ;
	public Hot2gCommandRequestATC		RequestATC ;
	public Hot2gCommandRequestATR		RequestATR ;
	public Hot2gCommandRequestATS		RequestATS ;

	public Hot2gCommandReport80			Report80 ;
	public Hot2gCommandReport81			Report81 ;
	public Hot2gCommandReport82			Report82 ;
	public Hot2gCommandReport83			Report83 ;
	public Hot2gCommandReport84			Report84 ;
	public Hot2gCommandReport85			Report85 ;
	public Hot2gCommandReport86			Report86 ;
	public Hot2gCommandReport87			Report87 ;
	public Hot2gCommandReport88			Report88 ;
	public Hot2gCommandReport89			Report89 ;
	public Hot2gCommandReport8A			Report8A ;
	public Hot2gCommandReport8B			Report8B ;
	public Hot2gCommandReport8C			Report8C ;
	public Hot2gCommandReport8D			Report8D ;
	public Hot2gCommandReportA0			ReportA0 ;
	public Hot2gCommandReportA1			ReportA1 ;
	public Hot2gCommandReportA2			ReportA2 ;
	public Hot2gCommandReportA3			ReportA3 ;
	public Hot2gCommandReportA4			ReportA4 ;
	public Hot2gCommandReportA5			ReportA5 ;
	public Hot2gCommandReportA6			ReportA6 ;
	public Hot2gCommandReportA7			ReportA7 ;
	public Hot2gCommandReportA8			ReportA8 ;
	public Hot2gCommandReportA9			ReportA9 ;
	public Hot2gCommandReportAA			ReportAA ;
	public Hot2gCommandReportC0			ReportC0 ;
	public Hot2gCommandReportC1			ReportC1 ;
	public Hot2gCommandReportC2			ReportC2 ;
	public Hot2gCommandReportC3			ReportC3 ;
	public Hot2gCommandReportC4			ReportC4 ;
	public Hot2gCommandReportC5			ReportC5 ;
	public Hot2gCommandReportC6			ReportC6 ;
	public Hot2gCommandReportC7			ReportC7 ;
	public Hot2gCommandReportC8			ReportC8 ;
	public Hot2gCommandReportD0			ReportD0 ;
	public Hot2gCommandReportD1			ReportD1 ;
	public Hot2gCommandReportD2			ReportD2 ;
	public Hot2gCommandReportD3			ReportD3 ;
	public Hot2gCommandReportD4			ReportD4 ;
	public Hot2gCommandReportD5			ReportD5 ;
	public Hot2gCommandReportD6			ReportD6 ;			// 18.10.11 ohara
	public Hot2gCommandReportD7			ReportD7 ;			// 18.10.11 ohara
	public Hot2gCommandReportATC		ReportATC ;
	public Hot2gCommandReportATR		ReportATR ;
	public Hot2gCommandReportATX		ReportATX ;
	public Hot2gCommandReportATY		ReportATY ;
	public Hot2gCommandReportATZ		ReportATZ ;
	public Hot2gCommandReportATS		ReportATS ;

	
	public Hot2gCommands( Hot2gDataManager dataManager )
	{
		Request00 = new Hot2gCommandRequest00() ;
		Request01 = new Hot2gCommandRequest01() ;
		Request02 = new Hot2gCommandRequest02() ;
		Request03 = new Hot2gCommandRequest03() ;
		Request04 = new Hot2gCommandRequest04() ;
		Request05 = new Hot2gCommandRequest05() ;
		Request06 = new Hot2gCommandRequest06() ;
		Request07 = new Hot2gCommandRequest07() ;
		Request08 = new Hot2gCommandRequest08() ;
		Request09 = new Hot2gCommandRequest09() ;
		Request0A = new Hot2gCommandRequest0A() ;
		Request0B = new Hot2gCommandRequest0B() ;
		Request0C = new Hot2gCommandRequest0C() ;
		Request0D = new Hot2gCommandRequest0D() ;
		Request20 = new Hot2gCommandRequest20() ;
		Request21 = new Hot2gCommandRequest21() ;
		Request22 = new Hot2gCommandRequest22() ;
		Request23 = new Hot2gCommandRequest23() ;
		Request24 = new Hot2gCommandRequest24() ;
		Request25 = new Hot2gCommandRequest25() ;
		Request26 = new Hot2gCommandRequest26() ;
		Request27 = new Hot2gCommandRequest27() ;
		Request28 = new Hot2gCommandRequest28() ;
		Request29 = new Hot2gCommandRequest29() ;
		Request2A = new Hot2gCommandRequest2A() ;
		Request40 = new Hot2gCommandRequest40() ;
		Request41 = new Hot2gCommandRequest41() ;
		Request42 = new Hot2gCommandRequest42() ;
		Request43 = new Hot2gCommandRequest43() ;
		Request44 = new Hot2gCommandRequest44() ;
		Request45 = new Hot2gCommandRequest45() ;
		Request46 = new Hot2gCommandRequest46() ;
		Request47 = new Hot2gCommandRequest47() ;
		Request48 = new Hot2gCommandRequest48() ;
		RequestATC = new Hot2gCommandRequestATC() ;
		RequestATR = new Hot2gCommandRequestATR() ;
		RequestATS = new Hot2gCommandRequestATS() ;

		Report80 = new Hot2gCommandReport80() ;
		Report81 = new Hot2gCommandReport81() ;
		Report82 = new Hot2gCommandReport82() ;
		Report83 = new Hot2gCommandReport83() ;
		Report84 = new Hot2gCommandReport84() ;
		Report85 = new Hot2gCommandReport85() ;
		Report86 = new Hot2gCommandReport86() ;
		Report87 = new Hot2gCommandReport87() ;
		Report88 = new Hot2gCommandReport88() ;
		Report89 = new Hot2gCommandReport89() ;
		Report8A = new Hot2gCommandReport8A() ;
		Report8B = new Hot2gCommandReport8B() ;
		Report8C = new Hot2gCommandReport8C() ;
		Report8D = new Hot2gCommandReport8D() ;
		ReportA0 = new Hot2gCommandReportA0() ;
		ReportA1 = new Hot2gCommandReportA1() ;
		ReportA2 = new Hot2gCommandReportA2() ;
		ReportA3 = new Hot2gCommandReportA3() ;
		ReportA4 = new Hot2gCommandReportA4() ;
		ReportA5 = new Hot2gCommandReportA5() ;
		ReportA6 = new Hot2gCommandReportA6() ;
		ReportA7 = new Hot2gCommandReportA7() ;
		ReportA8 = new Hot2gCommandReportA8() ;
		ReportA9 = new Hot2gCommandReportA9() ;
		ReportAA = new Hot2gCommandReportAA() ;
		ReportC0 = new Hot2gCommandReportC0() ;
		ReportC1 = new Hot2gCommandReportC1() ;
		ReportC2 = new Hot2gCommandReportC2() ;
		ReportC3 = new Hot2gCommandReportC3() ;
		ReportC4 = new Hot2gCommandReportC4() ;
		ReportC5 = new Hot2gCommandReportC5() ;
		ReportC6 = new Hot2gCommandReportC6() ;
		ReportC7 = new Hot2gCommandReportC7( dataManager ) ;
		ReportC8 = new Hot2gCommandReportC8() ;
		ReportD0 = new Hot2gCommandReportD0( dataManager ) ;
		ReportD1 = new Hot2gCommandReportD1( dataManager ) ;
		ReportD2 = new Hot2gCommandReportD2( dataManager ) ;
		ReportD3 = new Hot2gCommandReportD3() ;
		ReportD4 = new Hot2gCommandReportD4( dataManager ) ;
		ReportD5 = new Hot2gCommandReportD5( dataManager ) ;		// [181025.2]
		ReportD6 = new Hot2gCommandReportD6() ;			// 18.10.11 ohara
		ReportD7 = new Hot2gCommandReportD7() ;			// 18.10.11 ohara
		ReportATC = new Hot2gCommandReportATC() ;
		ReportATR = new Hot2gCommandReportATR( dataManager ) ;
		ReportATX = new Hot2gCommandReportATX( dataManager ) ;
		ReportATY = new Hot2gCommandReportATY( dataManager ) ;
		ReportATZ = new Hot2gCommandReportATZ( dataManager ) ;
		ReportATS = new Hot2gCommandReportATS() ;
	}

	/**
	 * Bluetooth通信用メッセージフォーマット
	 */
	public class Hot2gCommandBluetoothFormat
	{
		public static int				valMsgLen ;
		public static Hot2gEnumerations.EnumHot2gMsgID valCmdID ;
		public static int				valCmdLen ;
		public static byte[]			valCmdBytes ;
		public static int				valCheckSum ;

		/**
		 * コンストラクタ
		 */
		public Hot2gCommandBluetoothFormat()
		{
			valMsgLen = 0;
			valCmdLen = 0;
			valCmdID = Hot2gEnumerations.EnumHot2gMsgID.None;
			valCmdBytes = null;
			valCheckSum = 0;
		}

		/**
		 * 受信メッセージフォーマットチェック
		 * @param bytes	:受信データ
		 * @param size		:受信データサイズ
		 * @return			:T=OK, F=NG
		 */
		public static bool checkReceiveMessage(byte[] bytes, int size)
		{
			bool check = false;
			int temp = 0;
			int len = 0;

			valMsgLen = 0;
			valCmdID = Hot2gEnumerations.EnumHot2gMsgID.None;
            valCmdLen = 0;
			valCmdBytes = null;
			valCheckSum = 0;

			if( bytes == null )		return (false);
			if( size < 2 )			return (false);

			temp = (int)bytes[0];
			if (temp == 0 || temp > 20)
			{
				// temp=0の場合はBeaconの為、解析不要

				if( temp > 20 ) {
					// HOT-1000メッセージ
					len = size;
					valMsgLen = len;
					valCmdLen = len;
					switch( bytes[0] ){
						case 0x43:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_ProbeParam ;	    break ;
						case 0x52:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasBothAC ;	    break ;
						case 0x58:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasSideAC ;	    break ;
						case 0x59:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasDataMoton ;	break ;
						case 0x5A:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasDataMotBatt ;	break ;
						case 0x53:	valCmdID = Hot2gEnumerations.EnumHot2gMsgID.Report_1g_MeasStop ;	    break ;
						default:	valCmdID = (Hot2gEnumerations.EnumHot2gMsgID)(bytes[0]);	            break ;
					}

					valCmdBytes = new byte[valCmdLen];
				    System.Array.Copy(bytes, 0, valCmdBytes, 0, valCmdLen);
				}
				check = true;
			}
			else
			{
				// HOT-2Gメッセージ
				// LEN CMD(ID+PARAM) CHKSUM
				len = (int)bytes[0];
				if ((len + 1) >= size)
				{
					check = true;
				}

				if (check)
				{
					valMsgLen = len + 1;
					valCmdLen = len - 1;
					valCmdID = (Hot2gEnumerations.EnumHot2gMsgID)(bytes[1]);
					valCmdBytes = new byte[valCmdLen];
					System.Array.Copy(bytes, 1, valCmdBytes, 0, valCmdLen);
					valCheckSum = (int)bytes[valMsgLen - 2];
				}
			}
			return (check);
		}

		/**
		 * 送信メッセージ生成
		 * @param bytes	:送信コマンド
		 * @return			:送信メッセージ
		 */
		public static byte[] createSendMessage(byte[] bytes)
		{
			int cmd_len = bytes.Length;
			int pt = 0;
			byte[] send_bytes ;

			send_bytes = new byte[cmd_len + 2];

			send_bytes[pt++] = (byte)(cmd_len + 1);
			System.Array.Copy(bytes, 0, send_bytes, pt, cmd_len);
			pt += cmd_len;
			send_bytes[pt] = calcCheckSum(bytes);

			return(send_bytes) ;
		}

		/**
		 * チェックサム値計算
		 * @param bytes	:
		 * @return			:チェックサム値
		 */
		static byte calcCheckSum(byte[] bytes)
		{
			long sum = 0;

			for (int ii = 0; ii < (bytes.Length - 1); ii++)
			{
				sum += bytes[ii];
			}

			return ((byte)(sum & 0xFF));
		}
	}


    //---------------------------------------------------------------------------------------------

	/**
	 * [デバイス情報] - [機種情報]要求
	 */
	public class Hot2gCommandRequest00
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest00()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceType.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}
	}

	/**
	 * [デバイス情報] - [機種情報]報告
	 */
	public class Hot2gCommandReport80
    {
		private Hot2gEnumerations.EnumHot2gDevType valDevType ;				// 機種区分
		public Hot2gEnumerations.EnumHot2gDevType getDevType() {
			return valDevType;
		}

        public void setReportData(byte[] bytes)
        {
			valDevType = (Hot2gEnumerations.EnumHot2gDevType)(bytes[1]);
        }
    }

	/**
	 * [デバイス情報] - [機器ID情報]要求
	 */
	public class Hot2gCommandRequest01
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest01()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceID.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}
	}

	/**
	 * [デバイス情報] - [機器ID情報]報告
	 */
	public class Hot2gCommandReport81
	{
		private string	valDevId ;
		public string getDevId() {
			return valDevId;
		}

		public void setReportData(byte[] bytes)
		{
			// **:**:**:**:**:**
			//			string text = "";
			//			byte	data ;
			//			for (int ii = 0; ii < 6; ii++)
			//			{
			//				if (ii != 0) text += ":";
			//				data = bytes[1+ii] ;
			//				text += string.format( "%02X", bytes[1+ii] ) ;
			//			}
			valDevId = Converter.convertBytes2HexStr(bytes, 1, 6, ":");

			

			//Debug.Log("Report81:" + valDevId);
		}
	}

	/**
	 * [デバイス情報] - [機器名称]要求
	 */
	public class Hot2gCommandRequest02
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest02()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceName.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}

		public void createWriteCommand( string device_name )
		{
			valWriteReqCmd = new byte[10];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceName.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();			// RW

			byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(device_name);
			for (int ii = 0; ii < 8; ii++)
			{
				if (ii < nameBytes.Length)
				{
					valWriteReqCmd[2 + ii] = nameBytes[ii];
				}
				else
				{
					break;
				}
			}
		}
	}

	/**
	 * [デバイス情報] - [機器名称]報告
	 */
	public class Hot2gCommandReport82
	{
		private string	valDevName ;
		public string getDevName() {
			return valDevName;
		}

		public void setReportData(byte[] bytes)
		{
            valDevName = BitConverter.ToString(bytes); //new string( bytes, 1, 8 ) ;
		}
	}

	/**
	 * [デバイス情報] - [バージョン情報]要求
	 */
	public class Hot2gCommandRequest03
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest03()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceFirmVer.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}
	}

	/**
	 * [デバイス情報] - [バージョン情報]報告
	 */
	public class Hot2gCommandReport83
	{
		private string	valFwVersion ;
		public string getFwVersion() {
			return valFwVersion;
		}

		public void setReportData(byte[] bytes)
		{
//			string text = "";
//			for (int ii = 0; ii < 5; ii++)
//			{
//				text += string.format( "%02X", bytes[1+ii] ) ;
//			}
			valFwVersion = Converter.convertBytes2HexStr( bytes, 1, 5, "" ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest04
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest04()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceHwInfo.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}
	}

	public class Hot2gCommandReport84
	{
		private int valHwADEnableBits ;			    	// AD有効ビット数
		private int valHwADVoltMin ;						// AD最小電圧
		private int valHwADVoltMax ;						// AD最大電圧
		private int valHwAccEnableBits ;				// 加速度有効ビット数
		private int valHwGyroEnableBits ;			   	// 角速度有効ビット数
		private int valADDegitMax ;						// ADデジット最大値
		private double valResolutePd ;					// PD計測値分解能 [mV/bit]

		public int getHwADEnableBits() {
			return valHwADEnableBits;
		}
		public int getHwADVoltMin() {
			return valHwADVoltMin;
		}
		public int getHwADVoltMax() {
			return valHwADVoltMax;
		}
		public int getHwAccEnableBits() {
			return valHwAccEnableBits;
		}
		public int getHwGyroEnableBits() {
			return valHwGyroEnableBits;
		}
		public int getADDegitMax() {
			return valADDegitMax;
		}
		public double getResolutePd() { return valResolutePd; }

		public void setReportData( byte[] bytes )
		{
			valHwADEnableBits = bytes[1];
			valHwADVoltMin = Converter.convertBytes2IntLE( bytes, 2, 2 ) ;
			valHwADVoltMax = Converter.convertBytes2IntLE( bytes, 4, 2 ) ;
			valHwAccEnableBits = bytes[6];
			valHwGyroEnableBits = bytes[7];
			valADDegitMax = ( 1 << valHwADEnableBits ) ;
			valResolutePd = ( valHwADVoltMax - valHwADVoltMin ) / Math.Pow( 2, valHwADEnableBits ) ;
		}
	}

	/**
	 * [デバイス情報] - [待機時IR情報]要求
	 */
	public class Hot2gCommandRequest05
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest05()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_WaitIrInfo.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}

		public void createWriteCommand(int[] iSwGain, int[] iPmGain, int[] iOnOff, int[] iBright)
		{
			valWriteReqCmd = new byte[12];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_WaitIrInfo.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();				// RW

			valWriteReqCmd[2] = (byte)iSwGain[0];
			valWriteReqCmd[3] = (byte)iSwGain[1];
			valWriteReqCmd[4] = (byte)iPmGain[0];
			valWriteReqCmd[5] = (byte)iPmGain[1];
			valWriteReqCmd[6] = (byte)iPmGain[2];
			valWriteReqCmd[7] = (byte)iPmGain[3];
			valWriteReqCmd[8] = (byte)iOnOff[0];
			valWriteReqCmd[9] = (byte)iOnOff[1];
			valWriteReqCmd[10] = (byte)iBright[0];
			valWriteReqCmd[11] = (byte)iBright[1];
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport85
	{
		private int[] valWaitIrSwGains ;					// 待機IR スイッチゲイン(L3,R3)
		private int[] valWaitIrPmGains ;					// 待機IR ポテンショメータゲイン(L1,L3,R1,R3)
		private int[] valWaitIrIoOuts ;			   		// 待機IR IO出力(L/R)
		private int[] valWaitIrBrights ;					// 待機IR 光量(L/R)

		public int[] getWaitIrSwGains() {
			return valWaitIrSwGains;
		}
		public int[] getWaitIrPmGains() {
			return valWaitIrPmGains;
		}
		public int[] getWaitIrIoOuts() {
			return valWaitIrIoOuts;
		}
		public int[] getWaitIrBrights() {
			return valWaitIrBrights;
		}

		public Hot2gCommandReport85()
		{
			valWaitIrSwGains = new int[2] ;
			valWaitIrPmGains = new int[4] ;
			valWaitIrIoOuts = new int[2] ;
			valWaitIrBrights = new int[2] ;
		}

		public void setReportData( byte[] bytes )
		{
			int pt = 1;
			for (int ii = 0; ii < valWaitIrSwGains.Length; ii++)
			{
				valWaitIrSwGains[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valWaitIrPmGains.Length; ii++)
			{
				valWaitIrPmGains[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valWaitIrIoOuts.Length; ii++)
			{
				valWaitIrIoOuts[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valWaitIrBrights.Length; ii++)
			{
				valWaitIrBrights[ii] = bytes[pt++];
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest06
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest06()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionAcc.getByte();			// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand(Hot2gEnumerations.EnumHot2gAccRange AccRange, int AccLpf, int AccOdr)
		{
			valWriteReqCmd = new byte[5];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionAcc.getByte();			// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = AccRange.getByte();
			valWriteReqCmd[3] = (byte)AccLpf;
			valWriteReqCmd[4] = (byte)AccOdr;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport86
	{
		private Hot2gEnumerations.EnumHot2gAccRange valRangeAcc ;		    	// モーション加速度レンジ
		private int valAccLPF ;					    		// 加速度LPF
		private int valAccODR ;					    		// 加速度ODR
		private double valResoluteAcc ;						// モーション加速度分解能 ×10^-3 [G/bit]

		public Hot2gEnumerations.EnumHot2gAccRange getRangeAcc() {
			return valRangeAcc;
		}
		public int getAccLPF() {
			return valAccLPF;
		}
		public int getAccODR() {
			return valAccODR;
		}
		public double getResoluteAcc() {
			return valResoluteAcc;
		}

		public void setReportData(byte[] bytes, int enable_bits)
		{
			Debug.Log("setReportData 86");
			valRangeAcc = (Hot2gEnumerations.EnumHot2gAccRange)( bytes[1] ) ;
			if( valRangeAcc == 0 )		valRangeAcc = Hot2gEnumerations.EnumHot2gAccRange.e16G ;
			valAccLPF = bytes[2];
			valAccODR = bytes[3];

			int	range ;
			switch( valRangeAcc ){
				case Hot2gEnumerations.EnumHot2gAccRange.e2G:		range = 4 ;		break ;
				case Hot2gEnumerations.EnumHot2gAccRange.e4G:		range = 8 ;		break ;
				case Hot2gEnumerations.EnumHot2gAccRange.e8G:		range = 16 ;	break ;
				case Hot2gEnumerations.EnumHot2gAccRange.e16G:		range = 32 ;	break ;
				default:		range = 32 ;	break ;
			}
			valResoluteAcc = (double)range / Math.Pow( 2, enable_bits ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest07
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest07()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionGyro.getByte();			// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand(Hot2gEnumerations.EnumHot2gGyroRange AngRange, int AngLpf, int AngOdr)
		{
			valWriteReqCmd = new byte[5];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionGyro.getByte();			// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = AngRange.getByte();
			valWriteReqCmd[3] = (byte)AngLpf;
			valWriteReqCmd[4] = (byte)AngOdr;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport87
	{
		private Hot2gEnumerations.EnumHot2gGyroRange valRangeGyro ;		    	// モーション角速度レンジ
		private int valGyroLPF ;					    		// 角速度LPF
		private int valGyroODR ;					    		// 角速度ODR
		private double valResoluteGyro ;					// モーション角速度分解能 ×10^-3 [DPS/bit]

		public Hot2gEnumerations.EnumHot2gGyroRange getRangeGyro() {
			return valRangeGyro;
		}
		public int getGyroLPF() {
			return valGyroLPF;
		}
		public int getGyroODR() {
			return valGyroODR;
		}
		public double getResoluteGyro() {
			return valResoluteGyro;
		}

		public void setReportData(byte[] bytes, int enable_bits)
		{
			valRangeGyro = (Hot2gEnumerations.EnumHot2gGyroRange)( bytes[1] ) ;
			if( valRangeGyro == 0 )		valRangeGyro = Hot2gEnumerations.EnumHot2gGyroRange.e2000DPS ;
			valGyroLPF = bytes[2];
			valGyroODR = bytes[3];

			int	range ;
			switch( valRangeGyro ){
				case Hot2gEnumerations.EnumHot2gGyroRange.e125DPS:	range = 250 ;	break ;
				case Hot2gEnumerations.EnumHot2gGyroRange.e250DPS:	range = 500 ;	break ;
				case Hot2gEnumerations.EnumHot2gGyroRange.e500DPS:	range = 1000 ;	break ;
				case Hot2gEnumerations.EnumHot2gGyroRange.e1000DPS:	range = 2000 ;	break ;
				case Hot2gEnumerations.EnumHot2gGyroRange.e2000DPS:	range = 4000 ;	break ;
				default:	range = 4000 ;	break ;
			}
			valResoluteGyro = (double)range / Math.Pow( 2, enable_bits ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest08
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest08()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		public void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceBattery.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand(int Battery)
		{
			valWriteReqCmd = new byte[4];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DeviceBattery.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			byte[] cnvdata;
			cnvdata = Converter.convertInt2BytesLE( Battery, 2 ) ;
			valWriteReqCmd[2] = cnvdata[0];
			valWriteReqCmd[3] = cnvdata[1];
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport88
	{
		private int valBattCapacity ;			    		// バッテリー情報

		public int getBattCapacity() {
			return valBattCapacity;
		}

		public void setReportData(byte[] bytes)
		{
			valBattCapacity = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest09
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest09()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasSampling.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand( Hot2gEnumerations.EnumHot2gMotSmplRate motion_sampling, Hot2gEnumerations.EnumHot2gBattSmplRate battery_sampling )
		{
			valWriteReqCmd = new byte[4];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasSampling.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();				// RW

			valWriteReqCmd[2] = motion_sampling.getByte();
			valWriteReqCmd[3] = battery_sampling.getByte();
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport89
    {
		private Hot2gEnumerations.EnumHot2gBattSmplRate valSmplRateBatt ;	    // バッテリーサンプリング
		private Hot2gEnumerations.EnumHot2gMotSmplRate valSmplRateMot ;		// モーションサンプリング

		public Hot2gEnumerations.EnumHot2gBattSmplRate getSmplRateBatt() {
			return valSmplRateBatt;
		}
		public Hot2gEnumerations.EnumHot2gMotSmplRate getSmplRateMot() {
			return valSmplRateMot;
		}

        public void setReportData(byte[] bytes)
        {
			valSmplRateMot = (Hot2gEnumerations.EnumHot2gMotSmplRate)(bytes[1]) ;
			valSmplRateBatt = (Hot2gEnumerations.EnumHot2gBattSmplRate)(bytes[2]) ;
        }
    }

	/**
	 *
	 */
	public class Hot2gCommandRequest0A
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest0A()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasTransMode.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand(int TransMethod, int PDMeasure, Hot2gEnumerations.EnumHot2gMotTransRate MotionMeasure, Hot2gEnumerations.EnumHot2gBattTransRate BatteryMeasure, Hot2gEnumerations.EnumHot2gHRTransRate HRTransfer)
		{
			valWriteReqCmd = new byte[7];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasTransMode.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = (byte)TransMethod;
			valWriteReqCmd[3] = (byte)PDMeasure;
			valWriteReqCmd[4] = MotionMeasure.getByte();
			valWriteReqCmd[5] = BatteryMeasure.getByte();
			valWriteReqCmd[6] = HRTransfer.getByte();
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport8A
	{
		private int valTransRateMode ;			   			// 転送方式
		private int valTransRatePD ;							// PD計測転送周期
		private Hot2gEnumerations.EnumHot2gBattTransRate valTransRateBatt ;		// バッテリーデータ転送周期
		private Hot2gEnumerations.EnumHot2gMotTransRate valTransRateMot ;	    // モーションデータ転送周期
		private Hot2gEnumerations.EnumHot2gHRTransRate valTransRateHR ;			// 心拍数データ転送周期

		public int getTransRateMode() {
			return valTransRateMode;
		}
		public int getTransRatePD() {
			return valTransRatePD;
		}
		public Hot2gEnumerations.EnumHot2gBattTransRate getTransRateBatt() {
			return valTransRateBatt;
		}
		public Hot2gEnumerations.EnumHot2gMotTransRate getTransRateMot() {
			return valTransRateMot;
		}
		public Hot2gEnumerations.EnumHot2gHRTransRate getTransRateHR() {
			return valTransRateHR;
		}

		public void setReportData(byte[] bytes)
		{
			valTransRateMode = bytes[1];
			valTransRatePD = bytes[2];
			valTransRateMot = (Hot2gEnumerations.EnumHot2gMotTransRate)(bytes[3]) ;
			valTransRateBatt = (Hot2gEnumerations.EnumHot2gBattTransRate)(bytes[4]) ;
			valTransRateHR = (Hot2gEnumerations.EnumHot2gHRTransRate)(bytes[5]) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest0B
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest0B()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasAnalysisMode.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}

		public void createWriteCommand(int HRAnalysis, int AngleAnalysis)
		{
			valWriteReqCmd = new byte[4];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MeasAnalysisMode.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = (byte)HRAnalysis;
			valWriteReqCmd[3] = (byte)AngleAnalysis;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport8B
	{
		private int valAnalysisHR ;				    		// 心拍数解析有無
		private int valAnalysisAngle ;			    		// 姿勢解析有無

		public int getAnalysisHR() {
			return valAnalysisHR;
		}
		public int getAnalysisAngle() {
			return valAnalysisAngle;
		}

		public void setReportData(byte[] bytes)
		{
			valAnalysisHR = bytes[1];
			valAnalysisAngle = bytes[2];
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest0C
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest0C()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DebugFlag.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}

		public void createWriteCommand(int enabled_output_usb)
		{
			valWriteReqCmd = new byte[3];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_DebugFlag.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();			// RW

			valWriteReqCmd[2] = (byte)enabled_output_usb;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport8C
	{
		private bool valEnabledOutputUsb ;				// USBリアルタイム出力機能有無

		public bool getEnabledOutputUsb() {
			return valEnabledOutputUsb;
		}

		public void setReportData(byte[] bytes)
		{
			byte	data ;

			data = bytes[1] ;
			if( data == 1 )		valEnabledOutputUsb = true ;
			else				valEnabledOutputUsb = false ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest0D
	{
		private byte[] valReadReqCmd ;
		private byte[] valWriteReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest0D()
		{
			valReadReqCmd = null;
			valWriteReqCmd = null;

			createReadCommand();
		}

		private void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_LotID.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();			// RW
		}
		public void createWriteCommand( string lot_id )
		{
			valWriteReqCmd = new byte[18];

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_LotID.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();		// RW

			byte[] idBytes = System.Text.Encoding.ASCII.GetBytes( lot_id );
			for (int ii = 0; ii < 16; ii++)
			{
				if (ii < idBytes.Length)
				{
					valWriteReqCmd[2 + ii] = idBytes[ii];
				}
				else
				{
					break;
				}
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReport8D
	{
		private string valLotId ;							// ロットID

		public string getLotId() {
			return valLotId;
		}

		public void setReportData(byte[] bytes)
		{
            valLotId = BitConverter.ToString(bytes);//new string( bytes, 1, 16 ) ;
		}
	}

	/**
	 *
	 */
    public class Hot2gCommandRequest20
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest20()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

		private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_ErrorCode.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();			// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA0
    {
		private int valStsErrorCode ;			    		// 異常コード

		public int getStsErrorCode() {
			return valStsErrorCode;
		}

        public void setReportData(byte[] bytes)
        {
			valStsErrorCode = Converter.convertBytes2IntLE( bytes, 1, 4 ) ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest21
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest21()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

		private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_BatteryGauge.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public  class Hot2gCommandReportA1
    {
		private int           valBattGauge ;				// 電池残量Rawデータ
		private double        valConvBattGauge ;			// 電池残量換算値 [%]

		public int getBattGauge() {
			return valBattGauge;
		}
		public double getConvBattGauge() {
			return valConvBattGauge;
		}

        public void setReportData(byte[] bytes)
        {
			valBattGauge = bytes[1] ;
			valConvBattGauge = Converter.calcBatteryGauge( valBattGauge * 256 ) ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest22
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest22()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_BatteryVolt.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA2
    {
		private int			valBattVolt ;				// 電池電圧Rawデータ
		private double		valConvBattVolt ;			// 電池電圧換算値 [V]

		public int getBattVolt() {
			return valBattVolt;
		}
		public double getConvBattVolt() {
			return valConvBattVolt;
		}

        public void setReportData(byte[] bytes)
        {
			valBattVolt = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
			valConvBattVolt = Converter.calcBatteryVolt( valBattVolt ) ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest23
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest23()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_Temperature.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA3
    {
		private int[]		valDevTemps ;					// バッテリー,モーション,CPU 温度Rawデータ
		private double[]	valConvDevTemps ;		    	// バッテリー,モーション,CPU 温度換算値 [℃]

		public int[] getDevTemps() {
			return valDevTemps;
		}

		public double[] getConvDevTemps() {
			return valConvDevTemps;
		}

		public Hot2gCommandReportA3()
		{
			valDevTemps = new int[3] ;
			valConvDevTemps = new double[3] ;
		}

		public void setReportData(byte[] bytes)
        {
        	int	pt = 1 ;
			for( int ii=0; ii < valDevTemps.Length; ii++ ){
				valDevTemps[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				switch( ii ){
					case 0:	valConvDevTemps[ii] = Converter.calcBatteryTemp( valDevTemps[ii] ) ;		break ;
					case 1:	valConvDevTemps[ii] = Converter.calcMotionTemp( valDevTemps[ii] ) ;		break ;
					case 2:	valConvDevTemps[ii] = Converter.calcCpuTemp( valDevTemps[ii] ) ;			break ;
				}
				pt += 2 ;
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest24
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest24()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_IrLightOutput.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA4
    {
		private int[]	valStsIrIoOuts ;			    		// IR IO出力状態(L/R)
		private int[]	valStsIrBrights ;			    		// IR 光量状態(L/R)

		public int[] getStsIrIoOuts() {
			return valStsIrIoOuts;
		}

		public int[] getStsIrBrights() {
			return valStsIrBrights;
		}

		public Hot2gCommandReportA4()
		{
			valStsIrIoOuts = new int[2] ;
			valStsIrBrights = new int[2] ;
		}

		public void setReportData(byte[] bytes)
        {
			int pt = 1;
            for (int ii = 0; ii < valStsIrIoOuts.Length; ii++)
            {
				valStsIrIoOuts[ii] = bytes[pt++];
            }
            for (int ii = 0; ii < valStsIrBrights.Length; ii++)
            {
				valStsIrBrights[ii] = bytes[pt++];
            }
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest25
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest25()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_UsbState.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA5
    {
		private bool	valEnabledUsbVbus ;	    	// USB VBUS状態
		private bool	valEnabledUsbComm ;	    	// USB 通信接続状態

		public bool getEnabledUsbVbus() {
			return valEnabledUsbVbus;
		}

		public bool getEnabledUsbComm() {
			return valEnabledUsbComm;
		}

		public void setReportData(byte[] bytes)
        {
            byte	data ;

            data = bytes[1] ;
            if( data == 1 )		valEnabledUsbVbus = true ;
            else				valEnabledUsbVbus = false ;
            data = bytes[2] ;
            if( data == 1 )		valEnabledUsbComm = true ;
            else				valEnabledUsbComm = false ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest26
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest26()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_BluetoothState.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA6
    {
		private bool	valEnabledBtModule ;	    	// Bluetooth モジュール状態
		private bool	valEnabledBtComm ;	    	// Bluetooth 通信接続状態

		public bool getEnabledBtModule() {
			return valEnabledBtModule;
		}

		public bool getEnabledBtComm() {
			return valEnabledBtComm;
		}

		public void setReportData(byte[] bytes)
        {
            byte	data ;

            data = bytes[1] ;
            if( data == 1 )		valEnabledBtModule = true ;
            else				valEnabledBtModule = false ;
            data = bytes[2] ;
            if( data == 1 )		valEnabledBtComm = true ;
            else				valEnabledBtComm = false ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest27
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest27()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_SensorBoardEnabled.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA7
    {
		private bool	valEnabledSensModule ;	    	// センサモジュール検出状態

		public bool getEnabledSensModule() {
			return valEnabledSensModule;
		}

		public void setReportData(byte[] bytes)
        {
            byte	data ;

            data = bytes[1] ;
            if( data == 1 )		valEnabledSensModule = true ;
            else				valEnabledSensModule = false ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest28
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest28()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_PDEnabled.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA8
    {
		private bool[]	valEnabledPdLs ;			// PD-L1,L3値検出状態
		private bool[]	valEnabledPdRs ;			// PD-R1,R3値検出状態

		public bool[] getEnabledPdLs() {
			return valEnabledPdLs;
		}

		public bool[] getEnabledPdRs() {
			return valEnabledPdRs;
		}

		public Hot2gCommandReportA8()
		{
			valEnabledPdLs = new bool[2] ;
			valEnabledPdRs = new bool[2] ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
            byte	data ;

            for (int ii = 0; ii < valEnabledPdLs.Length; ii++)
            {
                data = bytes[pt++] ;
                if( data == 1 )		valEnabledPdLs[ii] = true ;
                else				valEnabledPdLs[ii] = false ;
            }
            for (int ii = 0; ii < valEnabledPdRs.Length; ii++)
            {
                data = bytes[pt++] ;
                if( data == 1 )		valEnabledPdRs[ii] = true ;
                else				valEnabledPdRs[ii] = false ;
            }
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest29
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest29()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionEnabled.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportA9
    {
		private bool	valEnabledMotion ;		// モーションセンサ検出状態

		public bool getEnabledMotion() {
			return valEnabledMotion;
		}

		public void setReportData(byte[] bytes)
        {
            byte	data ;

            data = bytes[1] ;
            if( data == 1 )		valEnabledMotion = true ;
            else				valEnabledMotion = false ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequest2A
    {
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

        public Hot2gCommandRequest2A()
        {
            valReadReqCmd = null;

            createReadCommand();
        }

        private void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_BatteryEnabled.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportAA
    {
		private bool	valEnabledGauge ;				// 残量ゲージセンサ検出状態

		public bool getEnabledGauge() {
			return valEnabledGauge;
		}

		public void setReportData(byte[] bytes)
        {
            byte	data ;

            data = bytes[1] ;
            if( data == 1 )		valEnabledGauge = true ;
            else				valEnabledGauge = false ;
        }
    }

	/**
	 *
	 */
	public class Hot2gCommandRequest40
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest40()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand()
		{
			valWriteReqCmd = new byte[2];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_CtrlAutoMeasStart.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC0
	{
		private int		valProcessResult ;			// 処理結果

		public int getProcessResult() {
			return valProcessResult;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
		}
	}

	public class Hot2gCommandRequest41
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest41()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand()
		{
			valWriteReqCmd = new byte[2];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_CtrlAutoMeasStop.getByte();		// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC1
	{
		private int		valProcessResult ;	    		// 処理結果

		public int getProcessResult() {
			return valProcessResult;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest42
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest42()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand()
		{
			valWriteReqCmd = new byte[2];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_CtrlAutoGainStart.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW
		}
	}

	public class Hot2gCommandReportC2
	{
		private int		valProcessResult ;			// 処理結果

		public int getProcessResult() {
			return valProcessResult;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest43
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest43()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand()
		{
			valWriteReqCmd = new byte[2];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_CtrlAutoGainStop.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC3
	{
		private int		valProcessResult ;			// 処理結果

		public int getProcessResult() {
			return valProcessResult;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest44
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest44()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand(Hot2gEnumerations.EnumHot2gMgcType UpdateClass, int[] iSwGain, int[] iPmGain, int[] iBright)
		{
			valWriteReqCmd = new byte[11];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_PDManuGainIr.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = UpdateClass.getByte();
			valWriteReqCmd[3] = (byte)iSwGain[0];
			valWriteReqCmd[4] = (byte)iSwGain[1];
			valWriteReqCmd[5] = (byte)iPmGain[0];
			valWriteReqCmd[6] = (byte)iPmGain[1];
			valWriteReqCmd[7] = (byte)iPmGain[2];
			valWriteReqCmd[8] = (byte)iPmGain[3];
			valWriteReqCmd[9] = (byte)iBright[0];
			valWriteReqCmd[10] = (byte)iBright[1];
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC4
	{
		private int		valProcessResult ;		  		  	// 処理結果
		private int[]	valCtrlManuGainIrSwGains ;			// 手動ゲイン設定 スイッチゲイン(L3,R3)
		private int[]	valCtrlManuGainIrPmGains ;			// 手動ゲイン設定 ポテンショメータゲイン(L1,L3,R1,R3)
		private int[]	valCtrlManuGainIrBrights ;			// 手動ゲイン設定 光量(L/R)

		public int getProcessResult() {
			return valProcessResult;
		}
		public int[] getCtrlManuGainIrSwGains() {
			return valCtrlManuGainIrSwGains;
		}
		public int[] getCtrlManuGainIrPmGains() {
			return valCtrlManuGainIrPmGains;
		}
		public int[] getCtrlManuGainIrBrights() {
			return valCtrlManuGainIrBrights;
		}

		public Hot2gCommandReportC4()
		{
			valCtrlManuGainIrSwGains = new int[2] ;
			valCtrlManuGainIrPmGains = new int[4] ;
			valCtrlManuGainIrBrights = new int[2] ;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;

			int pt = 3;
			for (int ii = 0; ii < valCtrlManuGainIrSwGains.Length; ii++)
			{
				valCtrlManuGainIrSwGains[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valCtrlManuGainIrPmGains.Length; ii++)
			{
				valCtrlManuGainIrPmGains[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valCtrlManuGainIrBrights.Length; ii++)
			{
				valCtrlManuGainIrBrights[ii] = bytes[pt++];
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest45
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest45()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		public void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_PDValueInput.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();				// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC5
	{
		private int		valProcessResult ;		    	// PD入力結果
		private int[]	valCtrlPDValues ;				    // PD入力値(L1,L3,R1,R3)

		public int getProcessResult() {
			return valProcessResult;
		}
		public int[] getCtrlPDValues() {
			return valCtrlPDValues;
		}

		public Hot2gCommandReportC5()
		{
			valCtrlPDValues = new int[4] ;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
			int pt = 3;
			for (int ii = 0; ii < valCtrlPDValues.Length; ii++)
			{
				valCtrlPDValues[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				pt += 2;
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest46
	{
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequest46()
		{
			valWriteReqCmd = null;
		}

		public void createWriteCommand(int[] iOnOff, int[] iBright)
		{
			valWriteReqCmd = new byte[6];        // CommandData: ID + PARAM

			valWriteReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_IrOutputEstablish.getByte();	// ID
			valWriteReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enWrite.getByte();					// RW

			valWriteReqCmd[2] = (byte)iOnOff[0];
			valWriteReqCmd[3] = (byte)iOnOff[1];
			valWriteReqCmd[4] = (byte)iBright[0];
			valWriteReqCmd[5] = (byte)iBright[1];
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC6
	{
		private int	valProcessResult ;					// 処理結果
		private int[]	valCtrlIrOutIos ;				    	// IO出力(L/R)
		private int[]	valCtrlIrOutBrights ;			    	// 光量出力(L/R)

		public int getProcessResult() {
			return valProcessResult;
		}
		public int[] getCtrlIrOutIos() {
			return valCtrlIrOutIos;
		}
		public int[] getCtrlIrOutBrights() {
			return valCtrlIrOutBrights;
		}

		public Hot2gCommandReportC6()
		{
			valCtrlIrOutIos = new int[2] ;
			valCtrlIrOutBrights = new int[2] ;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;

			int pt = 3;
			for (int ii = 0; ii < valCtrlIrOutIos.Length; ii++)
			{
				valCtrlIrOutIos[ii] = bytes[pt++];
			}
			for (int ii = 0; ii < valCtrlIrOutBrights.Length; ii++)
			{
				valCtrlIrOutBrights[ii] = bytes[pt++];
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest47
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest47()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		public void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_MotionValueInput.getByte();	// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC7
	{
		private int		valProcessResult ;				// 処理結果
		private int[]		valCtrlAccValues ;				// 加速度入力値(X,Y,Z)
		private int[]		valCtrlGyroValues ;			    // 角速度入力値(X,Y,Z)
		private double[]	valCtrlConvAccValues ;		    // 加速度入力値(X,Y,Z) [G]換算
		private double[]	valCtrlConvGyroValues ;		    // 角速度入力値(X,Y,Z) [DPS]換算
		private Hot2gDataManager	manager ;

		public int getProcessResult() {
			return valProcessResult;
		}
		public int[] getCtrlAccValues() {
			return valCtrlAccValues;
		}
		public int[] getCtrlGyroValues() {
			return valCtrlGyroValues;
		}
		public double[] getCtrlConvAccValues() {
			return valCtrlConvAccValues;
		}
		public double[] getCtrlConvGyroValues() {
			return valCtrlConvGyroValues;
		}

		public Hot2gCommandReportC7( Hot2gDataManager dataManager )
		{
			valCtrlAccValues = new int[3] ;
			valCtrlGyroValues = new int[3] ;
			valCtrlConvAccValues = new double[3] ;
			valCtrlConvGyroValues = new double[3] ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
			int pt = 3;
			for (int ii = 0; ii < valCtrlAccValues.Length; ii++)
			{
				valCtrlAccValues[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				valCtrlConvAccValues[ii] = Converter.calcMotionAcc( valCtrlAccValues[ii], manager.DevInfo.valResoluteAcc ) ;
				pt += 2;
			}
			for (int ii = 0; ii < valCtrlGyroValues.Length; ii++)
			{
				valCtrlGyroValues[ii] = Converter.convertBytes2IntLE(bytes, pt, 2);      // [181214.1] 引数間違い修正
				valCtrlConvGyroValues[ii] = Converter.calcMotionGyro( valCtrlGyroValues[ii], manager.DevInfo.valResoluteGyro ) ;
				pt += 2;
			}
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandRequest48
	{
		private byte[] valReadReqCmd ;

		public byte[] getReadReqCmd() {
			return valReadReqCmd;
		}

		public Hot2gCommandRequest48()
		{
			valReadReqCmd = null;

			createReadCommand();
		}

		public void createReadCommand()
		{
			valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

			valReadReqCmd[0] = Hot2gEnumerations.EnumHot2gMsgID.Request_BatteryInput.getByte();		// ID
			valReadReqCmd[1] = Hot2gEnumerations.EnumHot2gMsgRwType.enRead.getByte();					// RW
		}
	}

	/**
	 *
	 */
	public class Hot2gCommandReportC8
	{
		private int		valProcessResult ;		   	// 処理結果
		private int		valBattGauge ;					// 電池残量Rawデータ
		private double		valConvBattGauge ;			// 電池残量換算値 [%]
		private int		valBattVolt ;					// 電池電圧Rawデータ
		private double		valConvBattVolt ;				// 電池電圧換算値 [V]

		public int getProcessResult() {
			return valProcessResult;
		}
		public int getBattGauge() {
			return valBattGauge;
		}
		public int getBattVolt() {
			return valBattVolt;
		}
		public double getConvBattGauge() {
			return valConvBattGauge;
		}
		public double getConvBattVolt() {
			return valConvBattVolt;
		}

		public void setReportData(byte[] bytes)
		{
			valProcessResult = Converter.convertBytes2IntLE( bytes, 1, 2 ) ;
			valBattGauge = Converter.convertBytes2IntLE( bytes, 3, 2 ) ;
			valBattVolt = Converter.convertBytes2IntLE( bytes, 5, 2 ) ;
			valConvBattGauge = Converter.calcBatteryGauge( valBattGauge ) ;
			valConvBattVolt = Converter.calcBatteryGauge( valBattVolt ) ;
		}
	}

    /*
    public class Hot2gCommandRequest60
    {
        public byte[] valReadReqCmd ;
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest60()
        {
            valReadReqCmd = null;
            valWriteReqCmd = null;

            createReadCommand();
        }

        public void createReadCommand()
        {
            valReadReqCmd = new byte[2];        // CommandData: ID + PARAM

            valReadReqCmd[0] = 0x60;        // ID
            valReadReqCmd[1] = (byte)EnumHot2gMsgRwType.enRead;        // RW
        }

        public void createWriteCommand(int OutputPatt)
        {
            valWriteReqCmd = new byte[3];        // CommandData: ID + PARAM

            valWriteReqCmd[0] = 0x60;        // ID
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;        // RW

            valWriteReqCmd[2] = (byte)OutputPatt;
        }
    }

    public class Hot2gCommandReportE0
    {
        public void setReportData(byte[] bytes)
        {
            Hot2gLocalInfo.valOtherLedPattern = bytes[1];
        }
    }

    public class Hot2gCommandRequest61
    {
        public byte[] valReadReqCmd ;
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest61()
        {
            valWriteReqCmd = null;
        }

        public void createWriteCommand(int DiagCateg, int ManualDiagParam)
        {
            int iParamSiza = 0;
            valWriteReqCmd = new byte[2 + iParamSiza];        // CommandData: ID + PARAM

            valWriteReqCmd[0] = 0x61;        // ID
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;        // RW

            valWriteReqCmd[2] = (byte)DiagCateg;
            if (valWriteReqCmd[2] == 0 || valWriteReqCmd[2] == 1)
            {
                iParamSiza = 1;
                valWriteReqCmd[2 + iParamSiza] = (byte)ManualDiagParam;
            }
        }
    }

    public static class Hot2gCommandReportE1
    {
        public void setReportData(byte[] bytes)
        {
            int iParamDist;
            Hot2gLocalInfo.valManualDiagResult = BitConverter.ToUInt16(cmddata, 1);
            Hot2gLocalInfo.valOtherDiagnosisResult = cmddata[3];
            iParamDist = cmddata[3];
            if (iParamDist == 2)
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = null;
                Hot2gLocalInfo.valOtherSensManuUpdateParamText = "";
            }
            else
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = new byte[1];
                Array.Copy(cmddata, 4, Hot2gLocalInfo.valOtherSensManuUpdateParams, 0, 1);

                string sText = "";
                for (int ii = 0; ii < 1; ii++)
                {
                    sText += Hot2gLocalInfo.valOtherSensManuUpdateParams[ii].Tostring("X2");
                }
                Hot2gLocalInfo.valOtherSensManuUpdateParamText = sText;
            }
        }
    }

    public class Hot2gCommandRequest62
    {
        public byte[] valReadReqCmd ;
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest62()
        {
            valReadReqCmd = null;
            valWriteReqCmd = null;
        }

        public void createReadCommand(int Target, int Address, int ParamSize)
        {
            valReadReqCmd = new byte[5];

            valReadReqCmd[0] = 0x62;
            valReadReqCmd[1] = (byte)EnumHot2gMsgRwType.enRead;
            valReadReqCmd[2] = (byte)Target;		//
            valReadReqCmd[3] = (byte)Address;		//
            valReadReqCmd[4] = (byte)ParamSize;		//
        }

        public void createWriteCommand(int Target, int Address, int ParamSize, byte[] Params)
        {
            valWriteReqCmd = new byte[5 + ParamSize];

            valWriteReqCmd[0] = 0x62;
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;
            valWriteReqCmd[2] = (byte)Target;		//
            valWriteReqCmd[3] = (byte)Address;		//
            valWriteReqCmd[4] = (byte)ParamSize; //
            Array.Copy(Params, 0, valWriteReqCmd, 5, ParamSize);	//可変(Max8)
        }
    }

    public class Hot2gCommandReportE2
    {
        public void setReportData(byte[] bytes)
        {
            int iParamSize;

            Hot2gLocalInfo.valOtherSensManuUpdateResult = BitConverter.ToUInt16(cmddata, 1);
            Hot2gLocalInfo.valOtherSensManuUpdateTarget = cmddata[3];
            Hot2gLocalInfo.valOtherSensManuUpdateAddress = cmddata[4];
            iParamSize = cmddata[5];
            Hot2gLocalInfo.valOtherSensManuUpdateSize = iParamSize;
            if (iParamSize == 0)
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = null;
            }
            else
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = new byte[iParamSize];
                Array.Copy(cmddata, 6, Hot2gLocalInfo.valOtherSensManuUpdateParams, 0, iParamSize);

                string sText = "";
                for (int ii = 0; ii < iParamSize; ii++)
                {
                    sText += Hot2gLocalInfo.valOtherSensManuUpdateParams[ii].Tostring("X2");
                }
                Hot2gLocalInfo.valOtherSensManuUpdateParamText = sText;
            }
        }
    }

    public class Hot2gCommandRequest63
    {
        public byte[] valReadReqCmd ;
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest63()
        {
            valReadReqCmd = null;
            valWriteReqCmd = null;
        }

        public void createReadCommand(int iTopAddress, int iParamSize)
        {
            valReadReqCmd = new byte[7];

            valReadReqCmd[0] = 0x63;
            valReadReqCmd[1] = (byte)EnumHot2gMsgRwType.enRead;

            byte[] cnvdata;
            cnvdata = BitConverter.GetBytes(iTopAddress);
            valWriteReqCmd[2] = cnvdata[0];
            valWriteReqCmd[3] = cnvdata[1];
            valWriteReqCmd[4] = cnvdata[2];
            valWriteReqCmd[5] = cnvdata[3];

            valWriteReqCmd[6] = (byte)iParamSize;
        }

        public void createWriteCommand(int iTopAddress, int iParamSize, byte[] bytParams)
        {
            valWriteReqCmd = new byte[7 + iParamSize];

            valWriteReqCmd[0] = 0x63;
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;

            byte[] cnvdata;
            cnvdata = BitConverter.GetBytes(iTopAddress);
            valWriteReqCmd[2] = cnvdata[0];
            valWriteReqCmd[3] = cnvdata[1];
            valWriteReqCmd[4] = cnvdata[2];
            valWriteReqCmd[5] = cnvdata[3];

            valWriteReqCmd[6] = (byte)iParamSize;

            Array.Copy(bytParams, 0, valWriteReqCmd, 7, iParamSize);
        }
    }

    public class Hot2gCommandReportE3
    {
        public void setReportData(byte[] bytes)
        {
            int iParamSize;

            Hot2gLocalInfo.valOtherSensManuUpdateResult = BitConverter.ToUInt16(cmddata, 1);
            Hot2gLocalInfo.valTopAddress = BitConverter.ToInt32(cmddata, 3);
            iParamSize = cmddata[7];
            Hot2gLocalInfo.valDataBytes = iParamSize;
            if (iParamSize == 0)
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = null;
            }
            else
            {
                Hot2gLocalInfo.valOtherSensManuUpdateParams = new byte[iParamSize];
                Array.Copy(cmddata, 8, Hot2gLocalInfo.valOtherSensManuUpdateParams, 0, iParamSize);

                string sText = "";
                for (int ii = 0; ii < iParamSize; ii++)
                {
                    sText += Hot2gLocalInfo.valOtherSensManuUpdateParams[ii].Tostring("X2");
                }
                Hot2gLocalInfo.valOtherSensManuUpdateParamText = sText;
            }
        }
    }

    public class Hot2gCommandRequest70
    {
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest70()
        {
            valWriteReqCmd = null;
        }

        public void createWriteCommand(int DataTransRot)
        {
            valWriteReqCmd = new byte[4];        // CommandData: ID + PARAM

            valWriteReqCmd[0] = 0x70;        // ID
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;        // RW

            byte[] cnvdata;
            cnvdata = BitConverter.GetBytes((ushort)DataTransRot);
            valWriteReqCmd[2] = cnvdata[0];
            valWriteReqCmd[3] = cnvdata[1];
        }
    }

    public class Hot2gCommandReportF0
    {
        public void setReportData(byte[] bytes)
        {
            Hot2gLocalInfo.valFirmUpdateStartResult = BitConverter.ToUInt16(cmddata, 1);
        }
    }

    public class Hot2gCommandRequest71
    {
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest71()
        {
            valWriteReqCmd = null;
        }

        public void createWriteCommand(int count, byte[] program)
        {
            valWriteReqCmd = new byte[2 + 1 + program.Length];        // CommandData: ID + PARAM

            valWriteReqCmd[0] = 0x71;        // ID
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;        // RW

            valWriteReqCmd[2] = (byte)count;

            int pt = 3;
            for (int ii = 0; ii < program.Length; ii++)
            {
                valWriteReqCmd[ii] = program[pt++];
            }
        }
    }

    public class Hot2gCommandReportF1
    {
        public void setReportData(byte[] bytes)
        {
            Hot2gLocalInfo.valFirmUpdateProgramResult = BitConverter.ToUInt16(cmddata, 1);
        }
    }

    public class Hot2gCommandRequest72
    {
        public byte[] valReadReqCmd ;
        public byte[] valWriteReqCmd ;

        public Hot2gCommandRequest72()
        {
            valWriteReqCmd = null;
        }

        public void createWriteCommand()
        {
            valWriteReqCmd = new byte[2];        // CommandData: ID + PARAM

            valWriteReqCmd[0] = 0x72;        // ID
            valWriteReqCmd[1] = (byte)EnumHot2gMsgRwType.enWrite;        // RW
        }
    }
	*/

	/**
	 *
	 */
    public class Hot2gCommandRequestATC
    {
		private const string	command = "!AT C" ;
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequestATC()
        {
            byte[]	bytes ;

            valWriteReqCmd = new byte[19];

            // [0:4] "!AT C"固定
			bytes = System.Text.Encoding.ASCII.GetBytes(command);
			System.Array.Copy( bytes, 0, valWriteReqCmd, 0, bytes.Length ) ;
        }

        public void createWriteCommand(Hot2gEnumerations.EnumHot2gMgcType ModeSelect, int[] iGainAMPGain, int[] iSwicAPMGain, int[] iBright)
        {
            string	data_text ;
            byte[]	bytes ;

            // [0:4] "!AT C"固定

            valWriteReqCmd[5] = (byte)( ModeSelect.getByte() + 0x30 ) ;
            for( int ii=0; ii < iGainAMPGain.Length; ii++ ){
				data_text = String.Format( "%01X", iGainAMPGain[ii] ) ;
				bytes = System.Text.Encoding.ASCII.GetBytes(data_text);
                valWriteReqCmd[6+ii] = (byte)bytes[0] ;
            }
            for( int ii=0; ii < iSwicAPMGain.Length; ii++ ){
				data_text = String.Format("%02X", iSwicAPMGain[ii] ) ;
				bytes = System.Text.Encoding.ASCII.GetBytes(data_text) ;
                valWriteReqCmd[10+2*ii] = (byte)bytes[0] ;
                valWriteReqCmd[10+2*ii+1] = (byte)bytes[1] ;
            }
			data_text = String.Format("%02X", iBright[0] ) ;
			bytes = System.Text.Encoding.ASCII.GetBytes(data_text) ;
            valWriteReqCmd[14] = (byte)bytes[0];
            valWriteReqCmd[15] = (byte)bytes[1];
			data_text = String.Format("%02X", iBright[1] ) ;
			bytes = System.Text.Encoding.ASCII.GetBytes(data_text) ;
            valWriteReqCmd[16] = (byte)bytes[0];
            valWriteReqCmd[17] = (byte)bytes[1];
            valWriteReqCmd[18] = 0x0D;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportATC
    {
		private int		valProbeParameterReport ;		    	// プローブパラメータ報告

		public int getProbeParameterReport() {
			return valProbeParameterReport;
		}

        public void setReportData(byte[] bytes)
        {
			valProbeParameterReport = bytes[1];
        }
    }

	/**
	 *
	 */
    public  class Hot2gCommandRequestATR
    {
		private const string	command = "!AT R" ;
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequestATR()
		{
			byte[]	bytes ;

			valWriteReqCmd = new byte[8];

			// [0:4] "!AT R"固定
			bytes = System.Text.Encoding.ASCII.GetBytes(command) ;
			System.Array.Copy( bytes, 0, valWriteReqCmd, 0, bytes.Length ) ;
		}

		public void createWriteCommand(int ModeSelect)
        {
            // [0:4] "!AT R"固定

            valWriteReqCmd[5] = 0x00;
            valWriteReqCmd[6] = (byte)( ModeSelect + 0x30 ) ;
            valWriteReqCmd[7] = 0x0D;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportATR
    {
		private Hot2gMeasData2chAc		ResultData ;
		private Hot2gDataManager		manager ;

		public Hot2gMeasData2chAc geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportATR( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasData2chAc() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
            for( int ii=0; ii < ResultData.valAcValues.Length; ii++ ){
				ResultData.valAcValues[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				//ResultData.valBloodDensities[ii] = Converter.calcBloodDensity( ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd ) ;
                pt += 2 ;
            }
            for( int ii=0; ii < ResultData.valSaturations.Length; ii++ ){
				// [181211.1] 符号なしに修正 >>> ---------------------------------------------------
				//				ResultData.valSaturations[ii] = bytes[pt++] ;
				ResultData.valSaturations[ii] = Converter.convertByte2UInt(bytes, pt); pt++;
				// [181211.1] 符号なしに修正 <<< ---------------------------------------------------
			}
			for ( int ii=0; ii < ResultData.valOffDatas.Length; ii++ ){
				ResultData.valOffDatas[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
                pt += 2 ;
            }

			// [181211.1] 飽和カウンタを加味したヘモグロビン濃度変化換算を行う >>> -----------------
			for (int ii = 0; ii < ResultData.valAcValues.Length; ii++)
			{
				ResultData.valBloodDensities[ii] = Converter.calcBloodDensity(ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd, ResultData.valSaturations[ii]);
			}
			// [181211.1] <<< ----------------------------------------------------------------------



			if ( manager.LocalInfo.valFirstBloodFlag ){
				for( int ii=0; ii < manager.LocalInfo.valFirstBloodData.Length; ii++ ){
					manager.LocalInfo.valFirstBloodData[ii] = ResultData.valBloodDensities[ii] ;
				}
				manager.LocalInfo.valFirstBloodFlag = false ;
			}
			else{
				for( int ii=0; ii < ResultData.valBrainBloodDatas.Length/2; ii++ ) {
					int	index = ii * 2 ;
					// [181214.1] 引数間違い valAcValues->valBloodDensities
					ResultData.valBrainBloodDatas[ii] =
							Converter.calcBrainBloodDensityEx(
									ResultData.valBloodDensities[index],
									ResultData.valBloodDensities[index + 1],
									manager.LocalInfo.valFirstBloodData[index],
									manager.LocalInfo.valFirstBloodData[index + 1]);
				}
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportATX
    {
		private Hot2gMeasData1chAc		ResultData ;
		private Hot2gDataManager		manager ;

		public Hot2gMeasData1chAc geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportATX( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasData1chAc() ;
			manager = dataManager ;
		}

        public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAcValues.Length; ii++ ){
				ResultData.valAcValues[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				//ResultData.valBloodDensities[ii] = Converter.calcBloodDensity( ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valSaturations.Length; ii++ ){
				// [181211.1] 符号なしに修正 >>> ---------------------------------------------------
				//				ResultData.valSaturations[ii] = bytes[pt++] ;
				ResultData.valSaturations[ii] = Converter.convertByte2UInt(bytes, pt); pt++;
				// [181211.1] 符号なしに修正 <<< ---------------------------------------------------
			}
			ResultData.valOffData = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;

			// [181211.1] 飽和カウンタを加味したヘモグロビン濃度変化換算を行う >>> -----------------
			for (int ii = 0; ii < ResultData.valAcValues.Length; ii++)
			{
				ResultData.valBloodDensities[ii] = Converter.calcBloodDensity(ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd, ResultData.valSaturations[ii]);
			}
			// [181211.1] <<< ----------------------------------------------------------------------



			if ( manager.LocalInfo.valFirstBloodFlag ){
				for( int ii=0; ii < ResultData.valBloodDensities.Length; ii++ ){
					manager.LocalInfo.valFirstBloodData[ii] = ResultData.valBloodDensities[ii] ;
				}
				manager.LocalInfo.valFirstBloodFlag = false ;
			}
			else{
				// [181214.1] 引数間違い valAcValues->valBloodDensities
				ResultData.valBrainBloodData = Converter.calcBrainBloodDensityEx(ResultData.valBloodDensities[0], ResultData.valBloodDensities[1],
						manager.LocalInfo.valFirstBloodData[0], manager.LocalInfo.valFirstBloodData[1]);
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportATY
    {
		private Hot2gMeasDataMotion		ResultData ;
		private Hot2gDataManager	manager ;

		public Hot2gMeasDataMotion geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportATY( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasDataMotion() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAccels.Length; ii++ ){
				ResultData.valAccels[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				ResultData.valConvAccels[ii] = Converter.calcMotionAcc( ResultData.valAccels[ii], manager.DevInfo.valResoluteAcc ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valGyros.Length; ii++ ){
				ResultData.valGyros[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				ResultData.valConvGyros[ii] = Converter.calcMotionAcc( ResultData.valGyros[ii], manager.DevInfo.valResoluteGyro ) ;
				pt += 2 ;
			}
        }
    }

	/**
	 *
	 */
    public  class Hot2gCommandReportATZ
    {
		private Hot2gMeasDataMotBatt		ResultData ;
		private Hot2gDataManager	manager ;

		public Hot2gMeasDataMotBatt geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportATZ( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasDataMotBatt() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAccels.Length; ii++ ){
				ResultData.valAccels[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				ResultData.valConvAccels[ii] = Converter.calcMotionAcc( ResultData.valAccels[ii], manager.DevInfo.valResoluteAcc ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valGyros.Length; ii++ ){
				ResultData.valGyros[ii] = Converter.convertBytes2IntBE( bytes, pt, 2 ) ;
				ResultData.valConvGyros[ii] = Converter.calcMotionAcc(ResultData.valGyros[ii], manager.DevInfo.valResoluteGyro);
				pt += 2 ;
			}
			ResultData.valVolt = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valGauge = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valConvVolt = Converter.calcBatteryVolt( ResultData.valVolt * 256 ) ;
			ResultData.valConvGauge = Converter.calcBatteryGauge( ResultData.valGauge * 256 ) ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandRequestATS
    {
		private const string	command = "!AT C" ;
		private byte[] valWriteReqCmd ;

		public byte[] getWriteReqCmd() {
			return valWriteReqCmd;
		}

		public Hot2gCommandRequestATS()
        {
			byte[]	bytes ;

            valWriteReqCmd = new byte[6];

            // [0:4] "!AT S"固定
			bytes = System.Text.Encoding.ASCII.GetBytes(command) ;
			System.Array.Copy( bytes, 0, valWriteReqCmd, 0, bytes.Length ) ;
            valWriteReqCmd[5] = 0x0D;
        }

        public void createWriteCommand()
        {
            // 更新なし
        }
    }

	/**
	 *
	 */
    public  class Hot2gCommandReportATS
    {
        public void setReportData(byte[] bytes)
        {

        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportD0
    {
		private Hot2gMeasData2chAc		ResultData ;
		private Hot2gDataManager		manager ;

		public Hot2gMeasData2chAc geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD0( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasData2chAc() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
			int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAcValues.Length; ii++ ){
				ResultData.valAcValues[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				//ResultData.valBloodDensities[ii] = Converter.calcBloodDensity( ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valSaturations.Length; ii++ ){
				// [181211.1] 符号なしに修正 >>> ---------------------------------------------------
				//				ResultData.valSaturations[ii] = bytes[pt++] ;
				ResultData.valSaturations[ii] = Converter.convertByte2UInt(bytes, pt); pt++;
				// [181211.1] 符号なしに修正 <<< ---------------------------------------------------
			}
			for ( int ii=0; ii < ResultData.valOffDatas.Length; ii++ ){
				ResultData.valOffDatas[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				pt += 2 ;
			}

			// [181211.1] 飽和カウンタを加味したヘモグロビン濃度変化換算を行う >>> -----------------
			for (int ii = 0; ii < ResultData.valAcValues.Length; ii++)
			{
				ResultData.valBloodDensities[ii] = Converter.calcBloodDensity(ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd, ResultData.valSaturations[ii]);
			}
			// [181211.1] <<< ----------------------------------------------------------------------


			if ( manager.LocalInfo.valFirstBloodFlag ){
				for( int ii=0; ii < ResultData.valBloodDensities.Length; ii++ ){
					manager.LocalInfo.valFirstBloodData[ii] = ResultData.valBloodDensities[ii] ;
				}
				manager.LocalInfo.valFirstBloodFlag = false ;
			}
			else{
				for (int ii = 0; ii < ResultData.valBrainBloodDatas.Length; ii++)
				{           // [181105.1]
					int	index = ii * 2 ;
					// [181214.1] 引数間違い valAcValues->valBloodDensities
					ResultData.valBrainBloodDatas[ii] =
							Converter.calcBrainBloodDensityEx(
									ResultData.valBloodDensities[index],
									ResultData.valBloodDensities[index + 1],
									manager.LocalInfo.valFirstBloodData[index],
									manager.LocalInfo.valFirstBloodData[index + 1]);
				}
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportD1
    {
		private Hot2gMeasData1chAc		ResultData ;
		private Hot2gDataManager		manager ;

		public Hot2gMeasData1chAc geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD1( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasData1chAc() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
			UnityEngine.Debug.Log("Ac1ch:" + BitConverter.ToString(bytes));

			int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAcValues.Length; ii++ ){
				ResultData.valAcValues[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				//ResultData.valBloodDensities[ii] = Converter.calcBloodDensity( ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valSaturations.Length; ii++ ){
				// [181211.1] 符号なしに修正 >>> ---------------------------------------------------
				//				ResultData.valSaturations[ii] = bytes[pt++] ;
				ResultData.valSaturations[ii] = Converter.convertByte2UInt(bytes, pt); pt++;
				// [181211.1] 符号なしに修正 <<< ---------------------------------------------------
			}
			ResultData.valOffData = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;


			// [181211.1] 飽和カウンタを加味したヘモグロビン濃度変化換算を行う >>> -----------------
			for (int ii = 0; ii < ResultData.valAcValues.Length; ii++)
			{
				ResultData.valBloodDensities[ii] = Converter.calcBloodDensity(ResultData.valAcValues[ii], manager.DevInfo.valVmaxPd, ResultData.valSaturations[ii]);
			}
			// [181211.1] <<< ----------------------------------------------------------------------


			if ( manager.LocalInfo.valFirstBloodFlag ){
				for( int ii=0; ii < ResultData.valBloodDensities.Length; ii++ ){
					manager.LocalInfo.valFirstBloodData[ii] = ResultData.valBloodDensities[ii] ;
				}
				manager.LocalInfo.valFirstBloodFlag = false ;
			}
			else{
				// [181214.1] 引数間違い valAcValues->valBloodDensities
				ResultData.valBrainBloodData = Converter.calcBrainBloodDensityEx(ResultData.valBloodDensities[0], ResultData.valBloodDensities[1],
						manager.LocalInfo.valFirstBloodData[0], manager.LocalInfo.valFirstBloodData[1]);
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportD2
    {
		private Hot2gMeasDataMotion		ResultData ;
		private Hot2gDataManager	manager ;

		public Hot2gMeasDataMotion geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD2( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasDataMotion() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
			int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAccels.Length; ii++ ){
				ResultData.valAccels[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvAccels[ii] = Converter.calcMotionAcc( ResultData.valAccels[ii], manager.DevInfo.valResoluteAcc ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valGyros.Length; ii++ ){
				ResultData.valGyros[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvGyros[ii] = Converter.calcMotionGyro( ResultData.valGyros[ii], manager.DevInfo.valResoluteGyro ) ;
				pt += 2 ;
			}
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportD3
    {
		private Hot2gMeasDataBattery		ResultData ;

		public Hot2gMeasDataBattery geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD3()
		{
			ResultData = new Hot2gMeasDataBattery() ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valGauge = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valVolt = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valTemp = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valConvGauge = Converter.calcBatteryGauge( ResultData.valGauge * 256 ) ;
			ResultData.valConvVolt = Converter.calcBatteryVolt( ResultData.valVolt * 256 ) ;
			ResultData.valConvTemp = Converter.calcBatteryTemp( ResultData.valTemp * 256 ) ;
        }
    }

	/**
	 *
	 */
    public class Hot2gCommandReportD4
    {
		private Hot2gMeasDataMotBatt		ResultData ;
		private Hot2gDataManager	manager ;

		public Hot2gMeasDataMotBatt geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD4( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasDataMotBatt() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
            int pt = 1;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			for( int ii=0; ii < ResultData.valAccels.Length; ii++ ){
				ResultData.valAccels[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvAccels[ii] = Converter.calcMotionAcc( ResultData.valAccels[ii], manager.DevInfo.valResoluteAcc ) ;
				pt += 2 ;
			}
			for( int ii=0; ii < ResultData.valGyros.Length; ii++ ){
				ResultData.valGyros[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvGyros[ii] = Converter.calcMotionGyro( ResultData.valGyros[ii], manager.DevInfo.valResoluteGyro ) ;
				pt += 2 ;
			}
			ResultData.valGauge = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valVolt = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valTemp = Converter.convertByte2UInt( bytes, pt ) ;	pt ++ ;
			ResultData.valConvGauge = Converter.calcBatteryGauge( ResultData.valGauge * 256 ) ;
			ResultData.valConvVolt = Converter.calcBatteryVolt( ResultData.valVolt * 256 ) ;
			ResultData.valConvTemp = Converter.calcBatteryTemp( ResultData.valTemp * 256 ) ;
        }
    }

	/**
	 *
	 */
    public  class Hot2gCommandReportD5		// 18.10.11 ohara L/Rピーク検出カウンタ追加
    {
		private Hot2gMeasDataHR		ResultData ;
		private Hot2gDataManager	manager ;

		public Hot2gMeasDataHR geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD5( Hot2gDataManager dataManager )
		{
			ResultData = new Hot2gMeasDataHR() ;
			manager = dataManager ;
		}

		public void setReportData(byte[] bytes)
        {
			int		pt = 1 ;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt++ ) ;
//			ResultData.valHR = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;	pt += 2 ;
			for( int ii=0; ii < ResultData.valPeakCountsL.Length; ii++ ){
				ResultData.valPeakCountsL[ii] = Converter.convertByte2UInt( bytes, pt++ ) ;
				ResultData.valConvHR_L[ii] = Converter.calcHR( ResultData.valPeakCountsL[ii] ) ;		// 18.10.13 ohara
			}
			for( int ii=0; ii < ResultData.valPeakCountsR.Length; ii++ ){
				ResultData.valPeakCountsR[ii] = Converter.convertByte2UInt( bytes, pt++ ) ;
				ResultData.valConvHR_R[ii] = Converter.calcHR( ResultData.valPeakCountsR[ii] ) ;		// 18.10.13 ohara
			}
//			ResultData.valConvHR = Converter.calcHR( ResultData.valHR ) ;
//			ResultData.valConvHR = ResultData.valConvHR_L[4] ;			// 18.10.13 ohara L32
			if( manager.DevInfo.valDevType == Hot2gEnumerations.EnumHot2gDevType.HOT2000 ){
				ResultData.valConvHR = Converter.calcHR( ResultData.valPeakCountsL, ResultData.valPeakCountsR,
						ResultData.valPeakAveDatas, ResultData.valPeakAvePt, ResultData.valPeakAveSize ) ;
			}
			else if( manager.DevInfo.valDevType == Hot2gEnumerations.EnumHot2gDevType.XB01 ){
				ResultData.valConvHR = Converter.calcHR( ResultData.valPeakCountsL,
						ResultData.valPeakAveDatas, ResultData.valPeakAvePt, ResultData.valPeakAveSize ) ;
			}
        }
    }

	/**
	 *
	 */
	public class Hot2gCommandReportD6		// 18.10.11 ohara
	{
		private Hot2gMeasDataHRElement		ResultData ;

		public Hot2gMeasDataHRElement geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD6()
		{
			ResultData = new Hot2gMeasDataHRElement() ;
		}

		public void setReportData(byte[] bytes)
		{
			int		pt = 1 ;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt++ ) ;
			for( int ii=0; ii < ResultData.valElements.Length; ii++ ){
				ResultData.valElements[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvElements[ii] = Converter.calcHRElement( ResultData.valElements[ii] ) ;
				pt += 2 ;
			}
		}
	}

	/**
	 *
	 */
	public  class Hot2gCommandReportD7		// 18.10.11 ohara
	{
		private Hot2gMeasDataHRElement		ResultData ;

		public Hot2gMeasDataHRElement geResultData() {
			return ResultData;
		}

		public Hot2gCommandReportD7()
		{
			ResultData = new Hot2gMeasDataHRElement() ;
		}

		public void setReportData(byte[] bytes)
		{
			int		pt = 1 ;
			ResultData.valResultCount = Converter.convertByte2UInt( bytes, pt++ ) ;
			for( int ii=0; ii < ResultData.valElements.Length; ii++ ){
				ResultData.valElements[ii] = Converter.convertBytes2IntLE( bytes, pt, 2 ) ;
				ResultData.valConvElements[ii] = Converter.calcHRElement( ResultData.valElements[ii] ) ;
				pt += 2 ;
			}
		}
	}
}