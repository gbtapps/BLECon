using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xb01data
{
    /** オフセット(S.s) */
    public float tick;

    /** L1用AC値 */
    public double l1;
    /** L3用AC値 */
    public double l3;
    /** L1用AC内飽和データ個数*/
    public int l1sa;
    /** L3用AC内飽和データ個数*/
    public int l3sa;
    /** 心拍数 */
    public double heartRate;
    /** マーカー */
    public string mark;
    /** 反応 */
    public string reaction;
    /** 背景色 */
    public double color;

    public double accelX;
    public double accelY;
    public double accelZ;
    public double gyroX;
    public double gyroY;
    public double gyroZ;

    public GyroScope.Stability stability;

    public double x0;
    public double y0;
    public double z0;

	public double pulseCoeff1L1;
    public double pulseCoeff2L1;
    public double pulseCoeff3L1;
    public double pulseCoeff1L3;
    public double pulseCoeff2L3;
	public double pulseCoeff3L3;
	public int ac1;
	public int ac3;


	public static string csvHeader
	{
		get
		{
			return "tick,heartRate rate,HbT change(SD1cm),HbT change(SD3cm),saturation(SD1cm),saturation(SD3cm),Mark,Reaction,color,accelX,accelY,accelZ,gyroX,gyroY,gyroZ,stability,x0,y0,z0," +
				   "Pulse Coeff1 L1,Pulse Coeff2 L1,Pulse Coeff3 L1,Pulse Coeff1 L3,Pulse Coeff2 L3,Pulse Coeff3 L3,AC SD1cm,AC SD3cm";
		}
	}

	public string ConvCSV()
	{
		return tick + "," + heartRate + "," + l1 + "," + l3 + "," + l1sa + "," + l3sa + "," +
			   mark + "," + reaction + "," + color + "," + accelX + "," + accelY + "," + accelZ + "," +
			   gyroX + "," + gyroY + "," + gyroZ + "," + stability + "," + x0 + "," + y0 + "," + z0 + "," +
			   pulseCoeff1L1 + "," + pulseCoeff2L1 + "," + pulseCoeff3L1 + "," + pulseCoeff1L3 + "," + pulseCoeff2L3 + "," + pulseCoeff3L3 + "," +
			   ac1 + "," + ac3;
	}


}
