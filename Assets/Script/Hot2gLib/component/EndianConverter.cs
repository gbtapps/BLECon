using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EndianConverter
{
    // .NETはintは32bitという風にサイズが固定で変化しない

    // 共通化できるものは処理を移譲する
    public static char Reverse(char value)
    {
        return (char)Reverse((ushort)value);
    }

    public static short Reverse(short value)
    {
        return (short)Reverse((ushort)value);
    }

    public static int Reverse(int value)
    {
        return (int)Reverse((uint)value);
    }
    public static long Reverse(long value)
    {
        return (long)Reverse((ulong)value);
    }

    // 伝統的な16ビット入れ替え処理16bit
    public static ushort Reverse(ushort value)
    {
        return (ushort)((value & 0xFF) << 8 | (value >> 8) & 0xFF);
    }

    // 伝統的な16ビット入れ替え処理32bit
    public static uint Reverse(uint value)
    {
        return (value & 0xFF) << 24 |
                ((value >> 8) & 0xFF) << 16 |
                ((value >> 16) & 0xFF) << 8 |
                ((value >> 24) & 0xFF);
    }

    // 伝統的な16ビット入れ替え処理64bit
    public static ulong Reverse(ulong value)
    {
        return (value & 0xFF) << 56 |
                ((value >> 8) & 0xFF) << 48 |
                ((value >> 16) & 0xFF) << 40 |
                ((value >> 24) & 0xFF) << 32 |
                ((value >> 32) & 0xFF) << 24 |
                ((value >> 40) & 0xFF) << 16 |
                ((value >> 48) & 0xFF) << 8 |
                ((value >> 56) & 0xFF);
    }

    // 浮動小数点はちょっと効率悪いけどライブラリでできる操作でカバーする
    public static float Reverse(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // これ以上いい処理が思いつかない
        Array.Reverse(bytes);
        return BitConverter.ToSingle(bytes, 0);
    }

    public static double Reverse(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
    }
}