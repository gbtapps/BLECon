using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLEMgr : SingletonMonoBehaviour<BLEMgr>
{
    public delegate void StatusChangedCallback(Hot2gEnumerations.EnumHot2gDriverCommStsEvent e, string Address);
    public delegate void ReceiveCallback(Hot2gEnumerations.EnumHot2gCommBtMsgEvent e, byte[] bytes);
    public delegate void Callback();

	public const float cScanTime = 2;
    public const float cTimeOut = 10;

    public UnityEngine.UI.Text g_DebugText;

    public const string DeviceName = "XB-01";
    public const string ServiceUUID = "00001800-dab3-4f5e-836c-d0c0377c3536";

    public const string GENERAL_PURPOSE_CONTROL         = "00002a00-dab3-4f5e-836c-d0c0377c3536";       // 送信用
    public const string GENERAL_PURPOSE_INDICATION      = "00002a01-dab3-4f5e-836c-d0c0377c3536";       // よーわからん
    public const string GENERAL_PURPOSE_NOTIFICATION    = "00002a02-dab3-4f5e-836c-d0c0377c3536";       // 受信用
    //public const string GENERAL_PURPOSE_CONFIG_NOTIFY   = "00002902-0000-1000-8000-00805f9b34fb";     // よーわからん



    public readonly string[] CharacteristicUUID =
    {
        GENERAL_PURPOSE_CONTROL,
        GENERAL_PURPOSE_INDICATION,
        GENERAL_PURPOSE_NOTIFICATION,
        //GENERAL_PURPOSE_CONFIG_NOTIFY,
    };

    public enum eMode
    {
        None,
        ScanDevice,
		ScanOK,
        Connect,
        ConnectOK,
        Error,
		DisConnect,
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
    }


    public bool isConnected
    {
        get
        {
            return m_Mode == eMode.ConnectOK;
        }
    }


    float m_ModeTimer = 0;
    ReceiveCallback m_ReceiveCallback = null;
    StatusChangedCallback m_StatusChangedCallback = null;

    string m_DeviceAddress= null;
    eMode m_Mode = eMode.None;
    Dictionary<string, bool> m_ConnectCharacteristicUUID;
	List<string> m_ScanDeviceList;

	public List<string> GetScanDeviceList()
	{
		return m_ScanDeviceList;
	}

    Callback m_ScanAndConnectSuccess;
    Callback m_ScanAndConnectError;

    public void Init(StatusChangedCallback statusChangedCallback, ReceiveCallback receiveCallback)
    {
        m_StatusChangedCallback = statusChangedCallback;
        m_ReceiveCallback = receiveCallback;

    }


    public void StartScan(Callback Success, Callback Error)
    {

        m_ScanAndConnectSuccess = Success;
        m_ScanAndConnectError = Error;

        m_ConnectCharacteristicUUID = new Dictionary<string, bool>();
        for (int i = 0; i < CharacteristicUUID.Length; i++)
        {
            m_ConnectCharacteristicUUID[CharacteristicUUID[i]] = false;
        }

        //Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () => {

            SetMode(eMode.ScanDevice);

        }, (error) => 
		{
			BluetoothLEHardwareInterface.Log("Error during initialize: " + error);
			if (error == "Bluetooth LE Not Enabled")
			{
				BluetoothLEHardwareInterface.DeInitialize(() =>
				{
					BluetoothLEHardwareInterface.BluetoothEnable(true);
					if (m_ScanAndConnectError != null)
					{
						m_ScanAndConnectError();
					}
				});
			}
		});
    }

	public void StartConnect( string Address, Callback Success, Callback Error, bool isForce = false )
	{
		m_ScanAndConnectSuccess = Success;
		m_ScanAndConnectError = Error;

		if ( !isForce || m_Mode != eMode.ScanOK )
		{
			if( m_ScanDeviceList.IndexOf(Address) < 0 )
			{
				Error();
				return;
			}
		}

		m_DeviceAddress = Address;
		SetMode(eMode.Connect);
	}

	void SetMode( eMode Mode )
    {
        DebugLog("BLEMgr:" + Mode.ToString());
        m_Mode = Mode;
        m_ModeTimer = 0;

		switch (m_Mode)
		{
			case eMode.ScanDevice: ExecScanDevice(); break;
			case eMode.Connect: ExecConnect(); break;
			case eMode.ConnectOK:
				if (m_ScanAndConnectSuccess != null)
				{
					m_ScanAndConnectSuccess();
				}
				m_ScanAndConnectSuccess = null;
				m_ScanAndConnectError = null;
				m_StatusChangedCallback(Hot2gEnumerations.EnumHot2gDriverCommStsEvent.Connected, m_DeviceAddress);
			break;

			case eMode.Error:
				BluetoothLEHardwareInterface.StopScan();

				BluetoothLEHardwareInterface.DeInitialize(() => 
				{
					if (m_ScanAndConnectError != null)
					{
						m_ScanAndConnectError();
					}
				});
				break;

			case eMode.DisConnect:
				BluetoothLEHardwareInterface.DeInitialize(() =>
				{
					m_StatusChangedCallback(Hot2gEnumerations.EnumHot2gDriverCommStsEvent.Disconnected, m_DeviceAddress);
				});
			break;
		}
    }

    void ExecScanDevice()
    {
		/*if (m_ScanDeviceList != null)
		{

		}
		else*/
		{
			m_ScanDeviceList = new List<string>();



			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,
				(address, name) =>
				{
					// if your device does not advertise the rssi and manufacturer specific data
					// then you must use this callback because the next callback only gets called
					// if you have manufacturer specific data

					//DebugLog("ScanDevice:" + name);

					if (name.Contains(DeviceName))
					{
						if (m_ScanDeviceList.IndexOf(address) < 0)
						{
							DebugLog("AddDevice:" + name + "," + address);
							m_ScanDeviceList.Add(address);
						}


						// BluetoothLEHardwareInterface.StopScan();

						// found a device with the name we want
						// this example does not deal with finding more than one
						//m_DeviceAddress = address;
						//SetMode(eMode.Connect);
					}

				}, (address, name, rssi, advertisingInfo) => {

					if (advertisingInfo != null)
						BluetoothLEHardwareInterface.Log(string.Format("Device: {0} RSSI: {1} Data Length: {2} Bytes: {3}", name, rssi, advertisingInfo.Length, BytesToString(advertisingInfo)));
				}, false);
		}
    }


    void ExecConnect()
    {
        DebugLog("ConnectAddress:" + m_DeviceAddress);

        BluetoothLEHardwareInterface.ConnectToPeripheral(m_DeviceAddress, null, null,
			// 接続に成功した時の挙動
            (address, serviceUUID, characteristicUUID) =>
            {
                DebugLog("Address:"+address+", serviceUUID:" + serviceUUID + ", characteristicUUID:" + characteristicUUID);

                m_ConnectCharacteristicUUID[characteristicUUID.ToLower()] = true;

                for (int i = 0; i < CharacteristicUUID.Length; i++)
                {
                    if (m_ConnectCharacteristicUUID[CharacteristicUUID[i].ToLower()] == false)
                    {
                        return;
                    }
                }

                ReceiveNotification();

                SetMode(eMode.ConnectOK);
            }, 
			// 切断コールバック
			(disconnectedAddress) =>
			{
				if( disconnectedAddress == m_DeviceAddress )
				{
					SetMode(eMode.DisConnect);
				}
			});
    }

    public bool SendCommand( byte[] data, Callback cc = null)
    {
        if( !isConnected )
        {
            return false;
        }

        //byte[] data = createSendMessage(commandArray);

        BluetoothLEHardwareInterface.WriteCharacteristic(m_DeviceAddress, ServiceUUID, GENERAL_PURPOSE_CONTROL, data, data.Length, true, 
            (characteristicUUID) => 
            {
                m_ReceiveCallback(Hot2gEnumerations.EnumHot2gCommBtMsgEvent.Notified, data);
				if (cc != null)
				{
					Debug.Log(BytesToString(data));
					cc();
				}
            }
        );

        /*


        void ExecStartSensing()
        {
            byte[] data = createSendMessage(new byte[] { 0x40, 0x01 });
            BluetoothLEHardwareInterface.WriteCharacteristic(m_DeviceAddress, ServiceUUID, GENERAL_PURPOSE_CONTROL, data, data.Length, true, (characteristicUUID) => {

                BluetoothLEHardwareInterface.Log("Write Succeeded");
                SetMode(eMode.WaitData);
            });
        }
        */

        return true;
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
        byte[] send_bytes;

        send_bytes = new byte[cmd_len + 2];

        send_bytes[pt++] = (byte)(cmd_len + 1);
        System.Array.Copy(bytes, 0, send_bytes, pt, cmd_len);
        pt += cmd_len;
        send_bytes[pt] = calcCheckSum(bytes);

        return (send_bytes);
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



    void ReceiveNotification()
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(m_DeviceAddress, ServiceUUID, GENERAL_PURPOSE_NOTIFICATION, null, (address, characteristicUUID, bytes) => {

            if (bytes != null)
            {
                /*string data = "";
                foreach (var b in bytes)
                    data += b.ToString("X") + " ";

                DebugLog(data);*/
                m_ReceiveCallback(Hot2gEnumerations.EnumHot2gCommBtMsgEvent.Notified, bytes);


            }

            //SetMode(eMode.WaitData);
        });
    }
   
    void DebugLog(string Text)
    {
        Debug.Log(Text);

        if (g_DebugText != null)
        {
            g_DebugText.text = Text + "\n" + g_DebugText.text;
        }
    }

    private void Update()
    {
        switch (m_Mode)
        {
            case eMode.ScanDevice:
				m_ModeTimer += Time.deltaTime;
				if( cScanTime <= m_ModeTimer )
				{
					

					if ( m_ScanDeviceList.Count <= 0 )
					{
						SetMode(eMode.Error);
						if (m_ScanAndConnectError != null)
						{
							m_ScanAndConnectError();
						}
					}
					else
					{
						BluetoothLEHardwareInterface.StopScan();
						SetMode(eMode.ScanOK);
						if (m_ScanAndConnectSuccess != null)
						{
							m_ScanAndConnectSuccess();
						}
					}
					
				}
			break;

            case eMode.Connect:
                m_ModeTimer += Time.deltaTime;

                if (cTimeOut <= m_ModeTimer)
                {
                    SetMode(eMode.Error);
                }
            break;
        }


    }



	protected string BytesToString(byte[] bytes)
	{
		string result = "";

		foreach (var b in bytes)
			result += b.ToString("X2");

		return result;
	}


	public void ExecStartSensing()
	{
		byte[] data = createSendMessage(new byte[] { /*0x40, 0x01*/0x44, 0x01, 0x02, 0x03, 0x03, 0x07, 0x02, 0x07, 0x02, 0xFF, 0xFF });
		BluetoothLEHardwareInterface.WriteCharacteristic(m_DeviceAddress, ServiceUUID, GENERAL_PURPOSE_CONTROL, data, data.Length, true, (characteristicUUID) => {

			BluetoothLEHardwareInterface.Log("Write Succeeded");

		});
	}

}
