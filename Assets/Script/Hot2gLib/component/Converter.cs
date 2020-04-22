using System;

public class Converter
{
	private const string TAG = "Converter";

	private const string HEXES = "0123456789ABCDEF";

	/**
	 * バイトデータから正数のint値を生成
	 * @param bytes
	 * @param offset
	 * @return
	 */
	public static int convertByte2UInt( byte[] bytes, int offset )
	{
		int	result = 0 ;

		result = bytes[offset] ;
		if( result < 0 )	result += 256 ;

		return( result ) ;
	}

	/**
	 * バイト配列からビッグエンディアンでint値を生成
	 * @param bytes
	 * @param offset
	 * @param size :2 or 4
	 * @return
	 */
	public static int convertBytes2IntBE( byte[] bytes, int offset, int size )
	{
		int	result = 0 ;

		if( size == 2 ) {
			result = (int)EndianConverter.Reverse(BitConverter.ToInt16(bytes, offset));
		}
		else if( size == 4 ) {
			result = EndianConverter.Reverse(BitConverter.ToInt32(bytes, offset));
		}

		return( result ) ;
	}

	/**
	 * バイト配列からリトルエンディアンでint値を生成
	 * @param bytes
	 * @param offset
	 * @param size :2 or 4
	 * @return
	 */
	public static int convertBytes2IntLE( byte[] bytes, int offset, int size )
	{
		int	result = 0 ;

		if( size == 2 ) {
			result = BitConverter.ToInt16( bytes, offset );
		}
		else if( size == 4 ) {
			result = BitConverter.ToInt32( bytes, offset );
		}

		return( result ) ;
	}

	/**
	 * 数値からビッグエンディアンのバイト配列を生成
	 * @param data
	 * @param size :2 or 4
	 * @return
	 */
	public static byte[] convertInt2BytesBE( int data, int size )
	{
		byte[]	bytes = null ;

		if( size == 2 )
		{
			short sdata = EndianConverter.Reverse( (short)data );
			bytes = BitConverter.GetBytes(sdata);
		}
		else if( size == 4 )
		{
			bytes = BitConverter.GetBytes( EndianConverter.Reverse(data) );
		}

		return( bytes ) ;
	}

	/**
	 * 数値からリトルエンディアンのバイト配列を生成
	 * @param data
	 * @param size :2 or 4
	 * @return
	 */
	public static byte[] convertInt2BytesLE( int data, int size )
	{
		byte[]	bytes = null ;

		if( size == 2 )
		{
			short sdata = (short)data;
			bytes = BitConverter.GetBytes(sdata);
		}
		else if( size == 4 )
		{
			bytes = BitConverter.GetBytes(data);
		}

		return( bytes ) ;
	}

	/**
	 * @param bytes
	 * @param offset
	 * @param size
	 * @param splitter
	 * @return
	 */
	public static string convertBytes2HexStr( byte[] bytes, int offset, int size, string splitter )
	{
		if( size <= 0 )						return( "" ) ;
		if( offset >= bytes.Length )		return( "" ) ;

		byte[]	array = new byte[size] ;
		System.Array.Copy( bytes, offset, array, 0, size ) ;

        string hex = "";
        
        for( int i=0; i<array.Length; i++ )
        {
        	byte b = array[i];
        	hex = hex + HEXES[((b & 0xF0) >> 4)];
        	hex = hex + HEXES[(b & 0x0F)];
        	hex = hex + splitter;
        }
        
        return hex;
	}

	/**
	 * 電池残量換算
	 * 対象：MAX17055
	 * @param raw_data	:16bit
	 * @return				:[%]
	 */
	public static double calcBatteryGauge( int raw_data )
	{
		double	calc = 0.0f ;

		calc = (double)raw_data / 256 ;

		return( calc ) ;
	}

	/**
	 * 電池電圧換算
	 * 対象：MAX17055
	 * @param raw_data	:16bit
	 * @return				:[mV]
	 */
	public static double calcBatteryVolt( int raw_data )
	{
		double	calc = 0.0f ;

		calc = (double)raw_data * 78.125f / 1000.0f ;

		return( calc ) ;
	}

	/**
	 * 電池温度換算
	 * 対象：MAX17055
	 * @param raw_data	:16bit
	 * @return				:[℃]
	 */
	public static double calcBatteryTemp( int raw_data )
	{
		double	calc = 0.0f ;
		double	Tref = 0.0f ;

		// -40～+85℃, 256 LSB/℃, 上位バイトのみ使用
		calc = ( (double)raw_data / 256.0f ) + Tref ;

		return( calc ) ;
	}

	/**
	 * モーション温度換算
	 * 対象：LSM6DSL
	 * @param raw_data		:16bit
	 * @return				:[℃]
	 */
	public static double calcMotionTemp( int raw_data )
	{
		double	calc = 0.0f ;
		double	Tref = 25.0f ;

		// -40～+85℃, 256 LSB/℃
		calc = ( (double)raw_data / 256.0f ) + Tref ;

		return( calc ) ;
	}

	/**
	 * CPU温度換算
	 * 対象：STM32L4x
	 * @param raw_data	:16bit
	 * @return				:[℃]
	 */
	public static double calcCpuTemp( int raw_data )
	{
		double	calc = 0.0f ;
		//short	Calib1 = 0x040A ;		// TS_CAL1 A8:0A, A9:04
		//short	Calib2 = 0x0518 ;		// TS_CAL2 CA:18, CB:05

		//Result = ( 110.0f - 30.0f ) / ( Calib2 - Calib1 ) * ( rawdata - Calib1 ) + 30.0f ;

		calc = (double)raw_data * 0.1f ;

		return( calc ) ;
	}

	/**
	 * モーション加速度換算
	 * 対象：LSM6DSL
	 * @param raw_data	:16bit
	 * @param rate	:加速度分解能
	 * @return				:[G]
	 */
	public static double calcMotionAcc( int raw_data, double rate )
	{
		double	calc = 0.0f ;

		calc = (double)raw_data * rate ;

		return( calc ) ;
	}

	/**
	 * モーション加速度換算
	 * 対象：LSM6DSL
	 * @param raw_data	:16bit
	 * @param range		:[EnumHot2gAccRange]
	 * @return				:[G]
	 */
	public static double calcMotionAcc( int raw_data, Hot2gEnumerations.EnumHot2gAccRange range )
	{
		double	calc = 0.0f ;
		double	rate ;

		switch( range ){
			case Hot2gEnumerations.EnumHot2gAccRange.e2G:		rate = 0.000061036f ;	break ;
			case Hot2gEnumerations.EnumHot2gAccRange.e4G:		rate = 0.000122072f ;	break ;
			case Hot2gEnumerations.EnumHot2gAccRange.e8G:		rate = 0.000244144f ;	break ;
			case Hot2gEnumerations.EnumHot2gAccRange.e16G:		rate = 0.000488289f ;	break ;
			default:		rate = 1.0f ;	break ;
		}

		calc = (double)raw_data * rate ;

		return( calc ) ;
	}

	/**
	 * モーション角速度換算
	 * 対象：LSM6DSL
	 * @param raw_data	:16bit
	 * @param rate	:角速度分解能
	 * @return				:[DPS]
	 */
	public static double calcMotionGyro( int raw_data, double rate )
	{
		double	Result = 0.0f ;

		Result = (double)raw_data * rate ;

		return( Result ) ;
	}

	/**
	 * モーション角速度換算
	 * 対象：LSM6DSL
	 * @param raw_data	:16bit
	 * @param range		:[EnumHot2gGyroRange]
	 * @return				:[DPS]
	 */
	public static double calcMotionGyro( int raw_data, Hot2gEnumerations.EnumHot2gGyroRange range )
	{
		double	calc = 0.0f ;
		double	rate ;

		switch( range ){
			case Hot2gEnumerations.EnumHot2gGyroRange.e125DPS:	rate = 0.003814755f ;	break ;
			case Hot2gEnumerations.EnumHot2gGyroRange.e250DPS:	rate = 0.007629511f ;	break ;
			case Hot2gEnumerations.EnumHot2gGyroRange.e500DPS:	rate = 0.015290219f ;	break ;
			case Hot2gEnumerations.EnumHot2gGyroRange.e1000DPS:	rate = 0.030518044f ;	break ;
			case Hot2gEnumerations.EnumHot2gGyroRange.e2000DPS:	rate = 0.061036088f ;	break ;
			default:	rate = 1.0f ;	break ;
		}

		calc = (double)raw_data * rate ;

		return( calc ) ;
	}

	/**
	 * 心拍数換算
	 * @param raw_data	:16bit
	 * @return				:[bpm]
	 */
	public static double calcHR( int raw_data )
	{
//		double	calc = 0.0f ;
//
//		// 0.01bpm
//		calc = (double)raw_data / 100.0f ;
//
//		return( calc ) ;
		double	calc = 0.0f ;

		// 0.01bpm
		if( raw_data != 0 ){
			calc = (double)( 2400.0f / (double)raw_data ) ;
		}

		return( calc ) ;
	}

	/**
	 * 心拍数換算
	 * @param peak
	 * @param average
	 * @param avept
	 * @param avesize
	 * @return
	 */
	public static double calcHR( int[] peak, double[] average, int avept, int avesize )			// [181025.2]
	{
		double	Result = 0.0f ;
		double	peakV = 0.0f ;
		double	ave_peak = 0.0f ;

        // 平均化
        /*
		peakV += peak[1] ;
		peakV += peak[2] ;
		peakV += peak[4] ;
		peakV += peak[5] ;
		peakV /= 4 ;
        */

        peakV += peak[1];
        peakV += peak[4];
        peakV /= 2;

        average[avept++] = peakV ;
		if( avept >= average.Length )	avept = 0 ;
		if( avesize < average.Length )	avesize ++ ;

		double	total = 0.0f ;
		for( int ii=0; ii < avesize; ii++ ){
			total += average[ii] ;
		}
		ave_peak = total / avesize ;

		// 0.01bpm
		if( ave_peak != 0 ){
			Result = 2400.0f / ave_peak ;
		}

		return( Result ) ;
	}

	/**
	 * 心拍数換算
	 * @param peakL
	 * @param peakR
	 * @param average
	 * @param avept
	 * @param avesize
	 * @return
	 */
	public static double calcHR( int[] peakL, int[] peakR, double[] average, int avept, int avesize )		// [181025.2]
	{
		double	Result = 0.0f ;
		double	peakV = 0.0f ;
		double	ave_peak = 0.0f ;

		// 平均化
		peakV += peakL[1] ;
		peakV += peakL[2] ;
		peakV += peakL[4] ;
		peakV += peakL[5] ;
		peakV += peakR[1] ;
		peakV += peakR[2] ;
		peakV += peakR[4] ;
		peakV += peakR[5] ;
		peakV /= 8 ;

		average[avept++] = peakV ;
		if( avept >= average.Length )	avept = 0 ;
		if( avesize < average.Length )	avesize ++ ;

		double	total = 0.0f ;
		for( int ii=0; ii < avesize; ii++ ){
			total += average[ii] ;
		}
		ave_peak = total / avesize ;

		// 0.01bpm
		if( ave_peak != 0 ){
			Result = 2400.0f / ave_peak ;
		}

		return( Result ) ;
	}

	// absorption coefficient of hemoglobin (@810nm)[1/mMol/mm]
	// via https://www.hitachi-medical.co.jp/products/nirs/contents01.html
	public const double ABSORPTION_COEFFICIENT = 0.2;

	// [181211.1] 飽和カウンタ閾値
	public const int SATURATION_THRESHOLD = 100;


	/**
	 * calculate Hemoglobin density in blood
	 *
	 * @param data output of A/D converter
	 * @param vmaxpd AD電圧最大値
	 * @return Hemoglobin density in arbitrary unit when ENABLE_UNIT_mMol_mm is false
	 *         or mMol*mm when ENABLE_UNIT_mMol_mm is true
	 *
	 */
	public static double calcBloodDensity( int data, int vmaxpd )
	{
		if( data == 0 )		return 0;

		return( -Math.Log((double) data / vmaxpd) / ABSORPTION_COEFFICIENT ) ;
	}

	/**
	 * calculate Hemoglobin density in blood
	 *
	 * @param data output of A/D converter
	 * @param vmaxpd AD電圧最大値
	 * @return Hemoglobin density in arbitrary unit when ENABLE_UNIT_mMol_mm is false
	 *         or mMol*mm when ENABLE_UNIT_mMol_mm is true
	 *
	 */
	public static double calcBloodDensity(int data, int vmaxpd, int saturation)     // [181211.1] 引数に飽和カウンタ追加
	{
		// 飽和カウンタが閾値を超えた場合、NaNとする
		if (saturation > SATURATION_THRESHOLD)
		{
			return (Double.NaN);
		}
		else
		{
			if (data == 0) return 0;
			return (-Math.Log((double)data / vmaxpd) / ABSORPTION_COEFFICIENT);
		}
	}

	/**
	 * extract Hemoglobin density in brain only
	 * @param data0	:Hemoglobin density in skin (sensed at 1cm apart from light source)
	 * @param data1	:Hemoglobin density in skin and brain (sensed at 3cm apart from light source)
	 * @param first0	:t=0 BloodDestiny
	 * @param first1	:t=0 BloodDestiny
	 * @return
	 */
	public static double calcBrainBloodDensityEx(double data0, double data1, double first0, double first1)
	{
		double	param_a		= 1.0;
		//double	param_a		= 0.4;
		double	param_b		= 0.0;
		double	calcS3		= first1 - data1;		// 引数データはいずれもlog計算後の値なのでそのまま計算する
		double	calcS1 		= first0 - data0;
		double	calcS		= -(calcS3 - (param_a * calcS1)) + param_b;
		return calcS;
	}

	/**
	 * 心拍成分換算
	 * @param raw_data	:16bit
	 * @return				:[bpm]
	 */
	public static double calcHRElement( int raw_data )		// 18.10.11 ohara
	{
		double	calc = 0.0f ;
		double	resolution = 10.0f ;

		// 0.01bpm
		calc = (double)raw_data / resolution ;

		return( calc ) ;
	}

}
