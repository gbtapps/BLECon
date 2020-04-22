using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnalyseHelper
{
    public static float Average(float[] data, int start, int end)
    {
        float Ret = data[start];
        int c = 0;
        for (int i = start; i < end && i < data.Length ; i++)
        {
            if (c == 0)
            {
                Ret = 0;
            }

            Ret += data[i];
            c++;
        }

        if (0 < c)
        {
            Ret = Ret / c;
        }

        return Ret;
    }


    public static double Average( double[] data, int start, int end )
    {
        if(data.Length <=start)
        {
            return 0;
        }

        double Ret = data[start];
        int c = 0;
        for( int i=start; i<end && i<data.Length; i++ )
        {
            if (c == 0)
            {
                Ret = 0;
            }

            Ret += data[i];
            c++;
        }

        if( 0 < c )
        {
            Ret = Ret / c;
        }

        return Ret;
    }

	public static double Average(List<double> data, int start, int end)
	{
		if( start < 0 )
		{
			start = 0;
		}

		if (data.Count <= start)
		{
			return 0;
		}

		double Ret = data[start];
		int c = 0;
		for (int i = start; i < end && i < data.Count; i++)
		{
			if (c == 0)
			{
				Ret = 0;
			}

			Ret += data[i];
			c++;
		}

		if (0 < c)
		{
			Ret = Ret / c;
		}

		return Ret;
	}

	/**
     * 移動平均
     * @param data
     * @param width サイズ(秒x10)
     * @return 処理済みデータ
     */
	public static double[] movingAverage( double[] data, int width)
    {
        double[] proceed = new double[data.Length];
        System.Array.Copy(data, proceed, data.Length);


        for (int i = width / 2; i <= data.Length - width / 2 - 1; i++ )
        {
            proceed[i] = Average(data, i - width / 2, i + width / 2);
        }

        return proceed;
    }
    public static float[] movingAverage(float[] data, int width)
    {
        float[] proceed = new float[data.Length];
        System.Array.Copy(data, proceed, data.Length);


        for (int i = width / 2; i <= data.Length - width / 2 - 1; i++)
        {
            proceed[i] = Average(data, i - width / 2, i + width / 2);
        }

        return proceed;
    }
    /**
     * ベースライン補正
     * @param data データ配列
     * @param startRange
     * @param endRange
     * @return 処理済みデータ
     */
    public static double[] baselineFitting( double[] data, int ss, int se, int es, int ee )
    {
        if (ee > data.Length)
            return data;

        double startAve = Average(data, ss, se);
        double endAve = Average(data, es, ee);

        double b = (startAve + endAve) / 2;

        for( var i=0; i<data.Length; i++ )
        {
            data[i] = data[i] - b;
        }

        return data;
    }

    /**
     * 中央値
     * 長さが偶数の場合は中央２値の平均値
     * @param data データ配列
     * @return 中央値
     */
    public static double median(double[] sdata)
    {
        double[] data = (double[])sdata.Clone();
        System.Array.Sort(data);        

        if (data.Length % 2 == 1)
        {
            return data[data.Length / 2];
        }

        return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
    }
    public static float median(float[] sdata)
    {
        float[] data = (float[])sdata.Clone();
        System.Array.Sort(data);

        if (data.Length % 2 == 1)
        {
            return data[data.Length / 2];
        }
        
        return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
    }

    //- Average using 80% of medial data afte sorting
    public static double Average80(double[] inputData)
    {
        double[] d = (double[])inputData.Clone();
        System.Array.Sort(d);
        if (d.Length >= 3)
        {            
            int _ds = (int)System.Math.Round(d.Length * 0.2);
            int _sz = d.Length - _ds * 2;
            double[] d1 = new double[_sz];
            System.Array.Copy(d, _ds, d1, 0, _sz);
            return d1.Average();
        }
        else
        {
            return inputData.Average();
        }
        



        

    }

    //--------------------------------------------------------------------------
    // This function returns the data filtered. Converted to C# 2 July 2014.
    // Original source written in VBA for Microsoft Excel, 2000 by Sam Van
    // Wassenbergh (University of Antwerp), 6 june 2007.
    //
    //https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp#_comments
    //License: https://www.codeproject.com/info/cpol10.aspx
    //--------------------------------------------------------------------------
    public static double[] ButterworthLPF(double[] indata, double deltaTimeinsec, double CutOffHz)
    {
        if (indata == null) return null;
        if (CutOffHz == 0) return indata;

        double Samplingrate = 1 / deltaTimeinsec;
        long dF2 = indata.Length - 1;        // The data range is set with dF2
        double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
        double[] data = indata; // Ptr., changes passed data

        // Copy indata to Dat2
        for (long r = 0; r < dF2; r++)
        {
            Dat2[2 + r] = indata[r];
        }
        Dat2[1] = Dat2[0] = indata[0];
        Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];

        const double pi = 3.14159265358979;
        double wc = System.Math.Tan(CutOffHz * pi / Samplingrate);
        double k1 = 1.414213562 * wc; // Sqrt(2) * wc
        double k2 = wc * wc;
        double a = k2 / (1 + k1 + k2);
        double b = 2 * a;
        double c = a;
        double k3 = b / k2;
        double d = -2 * a + k3;
        double e = 1 - (2 * a) - k3;

        // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
        double[] DatYt = new double[dF2 + 4];
        DatYt[1] = DatYt[0] = indata[0];
        for (long s = 2; s < dF2 + 2; s++)
        {
            DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                       + d * DatYt[s - 1] + e * DatYt[s - 2];
        }
        DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];

        // FORWARD filter
        double[] DatZt = new double[dF2 + 2];
        DatZt[dF2] = DatYt[dF2 + 2];
        DatZt[dF2 + 1] = DatYt[dF2 + 3];
        for (long t = -dF2 + 1; t <= 0; t++)
        {
            DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                        + d * DatZt[-t + 1] + e * DatZt[-t + 2];
        }

        // Calculated points copied for return
        for (long p = 0; p < dF2; p++)
        {
            data[p] = DatZt[p];
        }

        return data;
    }
    public static double[] ButterworthHPF(double[] indata, double deltaTimeinsec, double CutOffHz)
    {
        double[] d1 = ButterworthLPF(indata, deltaTimeinsec, CutOffHz);
        double[] d2 = new double[d1.Length];
        for (int i = 0; i < d2.Length; i++)
        {
            d2[i] = indata[i] - d1[i];
        }
        return d2;
    }
    public static float[] ButterworthLPF(float[] indata, double deltaTimeinsec, double CutOffHz)
    {
        return ArrayCastDoubleToFloat(ButterworthLPF(ArrayCastFloatToDouble(indata), deltaTimeinsec, CutOffHz));
    }
    public static float[] ButterworthHPF(float[] indata, double deltaTimeinsec, double CutOffHz)
    {
        return ArrayCastDoubleToFloat(ButterworthHPF(ArrayCastFloatToDouble(indata), deltaTimeinsec, CutOffHz));
    }

    private static float[] ArrayCastDoubleToFloat(double[] d)
    {
        float[] d1 = new float[d.Length];
        for (int i = 0; i < d1.Length; i++) d1[i] = (float)d[i];
        return d1;
    }
    private static double[] ArrayCastFloatToDouble(float[] d)
    {
        double[] d1 = new double[d.Length];
        for (int i = 0; i < d1.Length; i++) d1[i] = (double)d[i];
        return d1;
    }

    public static byte ModeFromArray(List<byte> data_in)
    {
        List<byte> count = new List<byte>();
        List<byte> item = new List<byte>();

        List<byte> data = new List<byte>(data_in);

        data.Sort();

        //- Count items
        item.Add(data[0]);
        count.Add(1);
        for (int i = 1; i < data.Count; i++)
        {
            if (item[item.Count - 1] != data[i])
            {
                item.Add(data[i]);
                count.Add(1);
            }
            else
            {
                count[count.Count - 1]++;
            }
        }

        //- Search max count
        List<byte> count_s = new List<byte>(count);
        count_s.Sort();
        int index = count.IndexOf(count_s[count_s.Count - 1]);

        return item[index];
    }

}
