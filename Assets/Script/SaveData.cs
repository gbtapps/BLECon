using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SaveData : SingletonMonoBehaviour<SaveData>
{
    //  保存するディレクトリ
    static string path;
    static readonly string prefix = "InBrainBloodFlow_";
    static readonly string ext = ".dat";

    private void Start()
    {
        path = Application.persistentDataPath + "/History";
        if (Directory.Exists(path))
        {
            return;
        }
        Directory.CreateDirectory(path);
#if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
    }
    static public void Save(string fileName,string data,string dir="")
    {
        if(dir != "")
        {
            string _path = Application.persistentDataPath + "/History/" +dir;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        using (StreamWriter streamWriter = new StreamWriter(path+"/"+ dir+ prefix + fileName + ext, false))
        {
            streamWriter.Write(data);
        }
    }

    static public string Load(string fileName,string dir ="")
    {
        Char[] buffer;
        string data;
        using (StreamReader streamReader = new StreamReader(path + "/"+dir + fileName))
        {
            buffer = new Char[streamReader.BaseStream.Length];
            streamReader.Read(buffer,0, (int)streamReader.BaseStream.Length);
            data = new String(buffer);
        }
        return data;
    }

    static public string[] LoadDirectory(string _dir)
    {
        if (!Directory.Exists(path + "/" + _dir))
        {
            return null;
        }

        DirectoryInfo dir = new DirectoryInfo(path+"/"+_dir);
        FileInfo[] info = dir.GetFiles("*"+ ext);
        List<string> _ret = new List<string>();
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
            string _data = Load(f.Name, _dir);
            if (_data != null)
            {
                _ret.Add(_data);
            }
        }
        return _ret.ToArray();
    }
}
