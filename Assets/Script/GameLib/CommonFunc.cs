using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Linq.Expressions;

public class CommonFunc 
{
    // ObjのRectTransformからColliderを更新
    public static void ImageToBoxCollider2D( GameObject Obj )
    {
        RectTransform rt = Obj.GetComponent<RectTransform>();


        BoxCollider2D bc = Obj.GetComponent<BoxCollider2D>();
        if (bc == null)
        {
            return;
        }

        bc.size = rt.sizeDelta;
    }

    // 度数を0-359にFix
    public static float FixDeg( float Deg )
    {
        while( Deg < 0.0f )
        {
            Deg += 360.0f;
        }

        while( 360.0f <= Deg )
        {
            Deg -= 360.0f;
        }

        return Deg;
    }

    // 度数を-180から180にFix
    public static float FixDeg180(float Deg)
    {
        while (Deg < -180.0f)
        {
            Deg += 360.0f;
        }

        while (180.0f <= Deg)
        {
            Deg -= 360.0f;
        }

        return Deg;
    }
    
    public static void SetNum( int Num, int Digit, ref Image[] Target, ref Sprite[] NumSprite )
    {
        if( Num < 0 )
        {
            Num = 0;
        }

        int Max = 1;
        for (int i = 0; i < Digit; i++ )
        {
            Max *= 10;
        }

        if( Max <= Num )
        {
            Num = Max - 1;
        }

        for( int i=0; i<Digit; i++ )
        {
            int DNum = Num % 10;

            Target[i].sprite = NumSprite[DNum];

            Num = Num / 10;
        }
    }

    public static void SetNumL(long Num, long Digit, ref Image[] Target, ref Sprite[] NumSprite)
    {
        SetNumL( Num, Digit, ref Target, ref NumSprite, true);
    }

    public static void SetNumL(long Num, long Digit, ref Image[] Target, ref Sprite[] NumSprite, bool isZeroPadding )
    {
        //long BaseNum = Num;
        //long 

        if (Num < 0)
        {
            Num = 0;
        }

        long Max = 1;
        for (int i = 0; i < Digit; i++)
        {
            Max *= 10;
        }

        if (Max <= Num)
        {
            Num = Max - 1;
        }

        for (int i = 0; i < Digit; i++)
        {
            if( i == 0 || isZeroPadding || Num != 0 )
            {
               Target[i].gameObject.SetActive(true);
            }
            else if( Num == 0 )
            {
                Target[i].gameObject.SetActive(false);
            }


            long DNum = Num % 10;

            Target[i].sprite = NumSprite[DNum];

            Num = Num / 10;

        }
    }

    public static bool HitRect2Point( float left, float right, float top, float bottom, Vector3 Point )
    {
        return HitRect2Point(left, right, top, bottom, Point.x, Point.y);
    }

    public static bool HitRect2Point( float left, float right, float top, float bottom, float pointx, float pointy )
    {
        return (left <= pointx && pointx <= right && bottom <= pointy && pointy <= top);
    }

    public static bool HitRect( float cx1, float cy1, float w1, float h1, float cx2, float cy2, float w2, float h2 )
    {
        float l1 = cx1 - w1 * 0.5f;
        float r1 = cx1 + w1 * 0.5f;
        float t1 = cy1 + h1 * 0.5f;
        float b1 = cy1 - h1 * 0.5f;

        float l2 = cx2 - w2 * 0.5f;
        float r2 = cx2 + w2 * 0.5f;
        float t2 = cy2 + h2 * 0.5f;
        float b2 = cy2 - h2 * 0.5f;

        return (l1 <= r2 && l2 <= r1 && t1 >= b2 && t2 >= b1 );
    }

    public static float GetLockTargetDegY( Vector3 Pos, Vector3 Target )
    {
        return Mathf.Atan2(Target.x - Pos.x, Target.z - Pos.z) * Mathf.Rad2Deg;
    }

    public static bool CheckArcArea( Vector3 Pos, float PosRotY, Vector3 Target, float Arc)
    {
        // 2点の角度
        float R = Mathf.Atan2(Target.x - Pos.x, Target.z - Pos.z) * Mathf.Rad2Deg;

        // Posの向きによる補正
        R -= PosRotY;
        
        // -180 - 180 に補正
        while( 180f < R )
        {
            R -= 360f;
        }
        while( R < -180f )
        {
            R += 360f;
        }

        // -Arc*0.5 - Arc*0.5 に Rが収まっていれば弧の中にいる

        if( -Arc*0.5f <= R && R <= Arc*0.5f )
        {
            return true;
        }

        return false;
    }

    public static void DrawGizmoCube( Vector3 position, Quaternion rotation, Vector3 scale, Color color, bool isWire = true)
    {
        Gizmos.color = color;

        Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= cubeTransform;
        if (isWire)
        {
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        else
        {
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }

        Gizmos.matrix = oldGizmosMatrix;
    }


    // 壁ずりベクトル
    public static Vector3 CalcWallVec(Vector3 Front, Vector3 Normal) 
    {
        return Vector3.Normalize(Front.normalized - Vector3.Dot(Front.normalized, Normal) * Normal);
    }





    public static String SecToHHMMSS(int Sec)
    {
        int Rest = Sec;
        int h = Rest / (60 * 60);
        Rest -= h * (60 * 60);

        int m = Rest / 60;
        Rest -= m * 60;

        int s = Rest;

        return String.Format("{0:00}", h) + ":" + String.Format("{0:00}", m) + ":" + String.Format("{0:00}", s);
    }

    public static String SecToMMSS(int Sec)
    {
        int Rest = Sec;

        int m = Rest / 60;
        Rest -= m * 60;

        int s = Rest;

        return String.Format("{0:00}", m) + ":" + String.Format("{0:00}", s);
    }

    public static int GetDigit( int Num )
    {
        int n = Math.Abs(Num).ToString().Length;

        if( Num < 0 )
        {
            n++;
        }

        return n;
    }

    public static float RotDist( float a, float b )
    {
        a = FixDeg(a);
        b = FixDeg(b);

        float Dist = Mathf.Abs(a - b);
        if( Dist < 180f )
        {
            return Dist;
        }

        return 360f - Dist; 
    }

    public static bool CheckClockwise(float Current, float Target)
    {
        if( Target > Current )
        {
            return !(Target - Current > 180f);
        }
        
        return Current - Target > 180f;
    }

    public static string ConvHHMM( int HHMM )
    {
        int HH = HHMM / 100;
        int MM = HHMM % 100;


        return String.Format("{0:00}", HH) + ":" + String.Format("{0:00}", MM);
    }

    public static Rect GetRateRect( float x, float y, float w, float h )
    {
        float sw = Screen.width;
        float sh = Screen.height;

        return new Rect(x * sw, y * sh, w * sw, h * sh);
    }

    public static string GetFileMD5( string FilePath )
    {
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
        
        
        return md5str.ToString();
    }

    // 子孫から指定の名前をしたオブジェクトを探す
    public static GameObject FindDescendant( GameObject Obj, string Name )
    {
        Transform t = Obj.transform.Find(Name);
        if( t != null )
        {
            return t.gameObject;
        }


        foreach (Transform child in Obj.transform)
        {
            GameObject o = FindDescendant(child.gameObject, Name);
            if( o != null )
            {
                return o;
            }
        }

        return null;
    }

	public static void SetImageColor( GameObject Obj, Color Col )
	{
		Image[] Img = Obj.GetComponentsInChildren<Image>();

		for( int i=0; i<Img.Length; i++ )
		{
			Img[i].color = Col;
		}
	}


	/// <summary>
	/// 変数名をstringとして受け取る(例) int hoge; NameOf( ()=>hoge); で "hoge"が変える
	/// </summary>
	public static string NameOf<T>(Expression<Func<T>> e)
	{
		return ((MemberExpression)e.Body).Member.Name;
	}


    // 
    public static Vector3 GetOffset( float DegY, Vector3 Ofs )
    {
        return Quaternion.AngleAxis(DegY, Vector3.up) * Ofs;
    }


	// Rate0-1を入れると0-PeakRate-1時に0-PeakNum-1の山がある数値を返す
	public static float ValueMount( float Rate, float PeakRate, float PeakValue)
	{
		if( 1f <= Rate )
		{
			return 1f;
		}

		if( Rate <= PeakRate )
		{
			return Mathf.Sin( Mathf.Clamp( Rate / PeakRate, 0f, 1f) * 90f * Mathf.Deg2Rad ) * PeakValue;
		}

		return PeakValue - Mathf.Sin( Mathf.Clamp( ( Rate - PeakRate ) / ( 1f - PeakRate ), 0f, 1f ) * 90f * Mathf.Deg2Rad ) * ( PeakValue - 1f ) ; 
	}
}
