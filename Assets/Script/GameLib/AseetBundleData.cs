using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public struct AssetBundleData
{
    public string FileName;
    public string FileMD5;
    public int FileSize;//単位はキロバイト
    public int PathNo;

    public void SetData(string FilePath)
    {
        FileName = System.IO.Path.GetFileName(FilePath);

        // .NET FrameworkのMD5計算クラスを作成する
        MD5 md5 = MD5.Create();

        // 計算結果を保存するバイナリ配列を宣言する
        byte[] bytes;
        // 対象ファイルを開い、ComputeHashメソッドを呼び出してMD5計算を行う
        using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read))
        {
            bytes = md5.ComputeHash(fs);
        }

        // 計算結果を16進数の文字列に変換する
        StringBuilder md5str = new StringBuilder();
        foreach (byte b in bytes) md5str.Append(b.ToString("x2"));


        FileMD5 = md5str.ToString();
    }
}