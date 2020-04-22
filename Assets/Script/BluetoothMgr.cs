using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class BluetoothMgr : SingletonMonoBehaviour<BluetoothMgr>
{
    string permission = "android.permission.ACCESS_COARSE_LOCATION";
    public const string cLastConnectSaveKey = "BM_LastConnect";
    Sprite[] m_BlueToothButtonSprite = new Sprite[3];
    Button m_BlueToothButton;

    //  自動再接続のタイマー
    float m_timer = 0;
    bool m_on = false;

    private void Start()
    {
        //m_BlueToothButtonSprite[0] = Resources.Load("Texture/btn_connect01", typeof(Sprite)) as Sprite;
        //m_BlueToothButtonSprite[1] = Resources.Load("Texture/btn_connect02", typeof(Sprite)) as Sprite;
        //m_BlueToothButtonSprite[2] = Resources.Load("Texture/btn_connect03", typeof(Sprite)) as Sprite;
        m_BlueToothButtonSprite[0] = Resources.Load("Texture/btOFF", typeof(Sprite)) as Sprite;
        m_BlueToothButtonSprite[1] = Resources.Load("Texture/btConnecting", typeof(Sprite)) as Sprite;
        m_BlueToothButtonSprite[2] = Resources.Load("Texture/btOK", typeof(Sprite)) as Sprite;
    }

    public void SetButton(Button button)
    {
        m_BlueToothButton = button;
    }

    private void Update()
    {
        switch (Hot2gApplication.Instance.mode)
        {
            case Hot2gApplication.eMode.None:
            case Hot2gApplication.eMode.ConnectError:
            case Hot2gApplication.eMode.ScanError:
                if(m_BlueToothButton != null) {
                    m_BlueToothButton.GetComponent<Image>().sprite = m_BlueToothButtonSprite[0];
                    m_BlueToothButton.interactable = true;
                }
                if (m_on)
                {
                    m_timer += Time.deltaTime;
                    if(m_timer > 7.0f)
                    {
                        Hot2gApplication.Instance.Scan(() => { }, () => { });
                        m_timer = 0;
                    }
                }
                break;


            case Hot2gApplication.eMode.ScanOK:
                string LastConncetAddress = "None";

                m_timer = 0;

                if (PlayerPrefs.HasKey(cLastConnectSaveKey))
                {
                    LastConncetAddress = PlayerPrefs.GetString(cLastConnectSaveKey);
                }

                List<string> DeviceList = Hot2gApplication.Instance.GetScanDeviceList();

#if false
			LastConncetAddress = "None";
			DeviceList.Add("Dummy");
			DeviceList.Add("Dummy2");
			DeviceList.Add("Dummy3");
			DeviceList.Add("Dummy4");
			DeviceList.Add("Dummy5");
			DeviceList.Add("Dummy6");
			DeviceList.Add("Dummy7");
			DeviceList.Add("Dummy8");
			DeviceList.Add("Dummy9");
#endif
                bool flg = false;
                for (int i = 0; i < DeviceList.Count; i++)
                {
                    if (DeviceList[i] == LastConncetAddress)
                    {
                        Hot2gApplication.Instance.Connecting(LastConncetAddress, () => { }, () => { }, () => { });
                        flg = true;
                    }
                }

                if (!flg)
                {
                    BtDialog.OpenDialog(LastConncetAddress, DeviceList);
                }

                /*
                if (DeviceList.Count == 1)
                {
                    // 見つかったひとつは前回のデバイスと同じ
                    if (DeviceList[0] == LastConncetAddress)
                    {
                        Hot2gApplication.Instance.Connecting(LastConncetAddress, () => { }, () => { }, () => { });
                    }
                    // 見つかったひとつは前回と違う
                    else
                    {
                        BtDialog.OpenDialog(LastConncetAddress, DeviceList);
                    }
                }
                // 複数のデバイスが見つかったぞ
                else
                {
                    BtDialog.OpenDialog(LastConncetAddress, DeviceList);
                }
                */
                break;

            case Hot2gApplication.eMode.Scan:
            case Hot2gApplication.eMode.Connecting:
            case Hot2gApplication.eMode.InitSetting:
                if(m_BlueToothButton != null)
                {
                    m_BlueToothButton.GetComponent<Image>().sprite = m_BlueToothButtonSprite[1];
                    m_BlueToothButton.interactable = false;
                }
                break;

            default:
                if (m_BlueToothButton != null)
                {
                    m_BlueToothButton.GetComponent<Image>().sprite = m_BlueToothButtonSprite[2];
                    m_BlueToothButton.interactable = true;
                }
                m_timer = 0;
                break;
        }
    }

    public void Bluetooth()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        LocationService l = new LocationService();
        if (!l.isEnabledByUser)
        {
            if (RuntimePermissionHelper.HasPermission(permission))
            {
                //Debug.Log("permission 位置情報サービスはオフでパーミッションがある");

                Debug.Log("CallIntent");
                // 位置情報サービスをオンにするのを促す
                RuntimePermissionHelper.CallIntent();
            }
            else
            {
                //Debug.Log("permission 位置情報サービスは謎だがとりあえずパーミッションがない");

                Debug.Log("OpenPermission");
                // パーミッションをONにするのを促す
                RuntimePermissionHelper.RequestPermission(new string[] { permission });
            }
        }
        else
        {
            //Debug.Log("permission 位置情報サービスがオンでパーミッションも許諾済み");
            //            SetMode(eMode.AutoConnect);
            BLEConnect();
        }
#else
        BLEConnect();
#endif
    }

    public void BLEConnect()
    {
        if (m_on && Hot2gApplication.Instance.mode >= Hot2gApplication.eMode.Connecting)
        {
            Hot2gApplication.Instance.DisConnect();
            m_on = false;
        }
        else
        {
            // 接続されていないなら接続する
            if (Hot2gApplication.Instance.mode <= Hot2gApplication.eMode.None)
            {
                Hot2gApplication.Instance.Scan(() => { }, () => { });
                //            Hot2gApplication.Instance.Connecting(IconUpdate, IconUpdate);
            }
            m_on = true;
            m_timer = 0;
        }

#if false    //  ダイアログの動作テスト
        List<string> DeviceList = new List<string>();
        //        string LastConncetAddress = "None";
        string LastConncetAddress = "Dummy5";
        DeviceList.Add("Dummy");
        DeviceList.Add("Dummy2");
        DeviceList.Add("Dummy3");
        DeviceList.Add("Dummy4");
        DeviceList.Add("Dummy5");
        DeviceList.Add("Dummy6");
        DeviceList.Add("Dummy7");
        DeviceList.Add("Dummy8");
        DeviceList.Add("Dummy9");
        BtDialog.OpenDialog(LastConncetAddress, DeviceList);
#endif
    }
}
