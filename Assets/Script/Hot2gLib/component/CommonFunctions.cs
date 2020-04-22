using System;

public class CommonFunctions
{
    private const string TAG = "CommonFunctions";

    /**
     * 現在日時をyyyy/MM/dd HH:mm:ss形式で取得する.<br>
     */
    public static string getNowDate( string format )
    {
        DateTime dt = DateTime.Now;
 		string result = dt.ToString(/*"yyyy/MM/dd HH:mm:ss"*/format);
 		
 		return result;
    }

    const string HEXES = "0123456789ABCDEF";
    public static string getHexDump( byte [] raw, string split ) 
    {
        if ( raw == null ) {
            return "";
        }
        
        string hex = "";
        
        for( int i=0; i<raw.Length; i++ )
        {
        	byte b = raw[i];
        	hex = hex + HEXES[((b & 0xF0) >> 4)];
        	hex = hex + HEXES[(b & 0x0F)];
        	hex = hex + split;
        }
        
        return hex;
    }

    public static string getHexDump( byte [] raw, int size, string split ) 
    {
        if ( raw == null ) {
            return "";
        }
        
        string hex = "";
        
        for( int i=0; i<raw.Length; i++ )
        {
        	byte b = raw[i];
        	hex = hex + HEXES[((b & 0xF0) >> 4)];
        	hex = hex + HEXES[(b & 0x0F)];
        	hex = hex + split;
        	
        	size--;
        	if( size <= 0 )
        	{
        		break;
        	}
        }
        
        return hex;
    }
}
