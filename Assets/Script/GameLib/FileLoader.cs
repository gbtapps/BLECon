using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// こいつ自身はAssetBundleから派生で作成された物までメモリの面倒をみない
public class FileLoader  : MonoBehaviour
{
	AssetBundle m_AseetBundle = null;
	Hashtable	m_ObjectTbl = null;

	private FileLoader()
	{
		m_AseetBundle = null;
		m_ObjectTbl = new Hashtable();
	}

	~FileLoader()
	{
		if( m_AseetBundle != null )
		{
			m_AseetBundle.Unload(false);
		}
	}

	public AssetBundle assetBundle
	{
		get
		{
			return m_AseetBundle;
		}
	}

	public object GetObject(string ObjectName)
	{
		if( m_ObjectTbl.ContainsKey(ObjectName) )
		{
			return m_ObjectTbl[ObjectName];
		}
		return null;
	}

	public T GetObject<T>( string ObjectName ) where T : class
	{
		if( m_ObjectTbl.ContainsKey(ObjectName) )
		{
			return m_ObjectTbl[ObjectName] as T;
		}
		return null;
	}

	
	static byte[] ReadAllBytes(string filePath)
	{
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_IOS
		return System.IO.File.ReadAllBytes(filePath);
#elif UNITY_SWITCH
        nn.fs.FileHandle fileHandle = new nn.fs.FileHandle();
        nn.Result result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Read);
        result.abortUnlessSuccess();

        long fileSize = 0;
        result = nn.fs.File.GetSize(ref fileSize, fileHandle);
        result.abortUnlessSuccess();

        byte[] data = new byte[fileSize];
        result = nn.fs.File.Read(fileHandle, 0, data, fileSize);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        return data;
#else
        return null;
#endif
    }

	public void UnloadAseetBundle()
	{

		m_ObjectTbl.Clear();
		m_ObjectTbl = null;

		m_AseetBundle.Unload(false);
		m_AseetBundle = null;

		Destroy( gameObject );
	}

	static public FileLoader LoadAssetBundle(string FileName)
	{
		GameObject Obj = new GameObject( "File_" + FileName );
		DontDestroyOnLoad( Obj );

		FileLoader Loader = Obj.AddComponent<FileLoader>();

		Loader.StartCoroutine( Loader.LoadAssetBundleCore(FileName) );
		return Loader;
	}


	//非同期読み込み(アセットバンドル）
	private IEnumerator LoadAssetBundleCore(string FileName)
	{
		if( m_AseetBundle != null )
		{
			Debug.LogError( "すでにアセットバンドルは読まれてる！" );
		}
		else
		{
			string assetBundleName = (FileName).ToLower();

			string assetBundlePath = "";
#if UNITY_STANDALONE || UNITY_EDITOR
			assetBundlePath = Application.streamingAssetsPath + "/Win/" + assetBundleName;
#elif UNITY_IOS
			assetBundlePath = Application.streamingAssetsPath + "/iOS/" + assetBundleName;
#elif UNITY_ANDROID
			assetBundlePath = Application.streamingAssetsPath + "/Android/" + assetBundleName;
#elif UNITY_SWITCH
			assetBundlePath = Application.streamingAssetsPath + "/Switch/" + assetBundleName;


#endif

#if !UNITY_EDITOR && (UNITY_ANDROID)
            WWW www = new WWW(assetBundlePath);
            yield return www;
            byte[] data = www.bytes;
#else
            //バイナリをメモリに読み込み
            byte[] data = ReadAllBytes(assetBundlePath);
#endif
            //読み込み
            AssetBundleCreateRequest resultAssetBundle = AssetBundle.LoadFromMemoryAsync(data);
			yield return new WaitWhile(() => resultAssetBundle.isDone == false);

			m_AseetBundle = resultAssetBundle.assetBundle;
		}
	}

	

	//非同期読み込み
	public void LoadObject( string ObjectName )
	{
		StartCoroutine( LoadObjectCore( ObjectName ) );
	}

	private IEnumerator LoadObjectCore( string ObjectName )
	{
		if( m_AseetBundle == null )
		{
			Debug.LogError( "アセットバンドルは読み込まれていません！" );
		}
		else	
		{
			if( !m_ObjectTbl.ContainsKey(ObjectName) )
			{
				m_ObjectTbl.Add( ObjectName, null );

				AssetBundleRequest prefabRequest = m_AseetBundle.LoadAssetAsync(ObjectName);

				yield return new WaitWhile(() => prefabRequest.isDone == false);

				m_ObjectTbl[ObjectName] = prefabRequest.asset;
			}
		}
	}
}
