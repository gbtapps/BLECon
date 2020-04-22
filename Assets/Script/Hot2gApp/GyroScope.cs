using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScope
{
    public enum Stability
    {
        Stabling,
        Stabled,
        Moving,
        Moved
    };
    const double duration = 1 * 10;
    //const double threshold = 0.5;
    const double threshold = double.PositiveInfinity;

    /** 経過時間(サイズ) */
    private int pasted = 0;

    private int detected = 0;

    /** 加速度(acc) */
    private class Values
    {
        public double x;
        public double y;
        public double z;

        public Values( double xx, double yy, double zz )
        {
            x = xx;
            y = yy;
            z = zz;
        }

        string toString()
        {
            return "x: " + x + ", y: " + y + ", z: " + z;
        }
    }

    /** 前回値 */
    private Values baseValues = null;
        
    private Stability stability = Stability.Moved;

    public double debugM;

    public double debugX0
    {
        get
        {
            return baseValues.x;
        }
    }

    public double debugY0
    {
        get
        {
            return baseValues.y;
        }
    }

    public double debugZ0
    {
        get
        {
            return baseValues.z;
        }
    }
       

    /** 安定状態の算出 */
    public Stability calcStatability( double x, double y, double z)
    {//at 10Hz
        Values v = new Values(x, y, z);
        double m = calcMotion(v);
        debugM = m;

//        int detectTimeWindowSize = 10;//- @10Hz=1sec
        
        //println("    >>>  " + this.stability.ToString() + " @" + pasted + "\\" + m);

        if(m < threshold)
        {
            detected = 0;//- reset detection counter

            /* 安定 */
            if (this.stability == Stability.Moved || this.stability == Stability.Moving)
            {
                /* 不安定状態 > 安定化中 */
                pasted = 1;
                this.stability = Stability.Stabling;
                return Stability.Stabling;
            }
            if (pasted < duration)
            {
                /* 安定化中 ：状態継続１秒間*/
                pasted++;
                return this.stability; //Stability.Stabling
            }
            //            baseValues = v     //基準値として次回に利用
            this.stability = Stability.Stabled;
            return Stability.Stabled;
        }
        else
        {
            Debug.Log("Motion!!"+detected);
            /* 不安定 */
            if (this.stability == Stability.Stabled || this.stability == Stability.Stabling)
            {
                detected++;
                if (detected < duration) return Stability.Stabled;

                /* 安定状態 > 不安定化中 */
                pasted = 1;
                this.stability = Stability.Moving;
                return Stability.Moving;
            }
            if (pasted < duration)
            {
                /* 不安定化中 ：状態継続１秒間*/
                pasted++;
                return this.stability; //Stability.Moving
            }
            baseValues = v;     //基準値として次回に利用
            this.stability = Stability.Moved;
            return Stability.Moved;
        }
    }

    double calcMotion(Values currValues)
    {
        if (baseValues == null)
        {
            baseValues = currValues;
        }


        return System.Math.Pow(currValues.x - baseValues.x, 2.0) +
            System.Math.Pow(currValues.y - baseValues.y, 2.0) +
            System.Math.Pow(currValues.z - baseValues.z, 2.0);
    }
}
