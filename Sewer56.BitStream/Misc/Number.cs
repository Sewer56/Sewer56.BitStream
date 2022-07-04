#if DEBUG
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sewer56.BitStream.Misc;

/// <summary>
/// Provides zero overhead compile time zero overhead casting by abusing compile time typeof checks.
/// Idea shamelessly stolen from from Amicitia.IO. https://github.com/TGEnigma/Amicitia.IO/blob/95a874f5d094f0487002a8b66684e67a2c461c51/src/Amicitia.IO/Generics/Number.cs
/// </summary>
public static class Number
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(byte value)
    {
        if (typeof(TTo) == typeof(byte)) return Unsafe.As<byte, TTo>(ref value);
        else if (typeof(TTo) == typeof(sbyte)) return Unsafe.As<byte, TTo>(ref value);
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(sbyte value)
    {
        if (typeof(TTo) == typeof(byte)) return Unsafe.As<sbyte, TTo>(ref value);
        else if (typeof(TTo) == typeof(sbyte)) return Unsafe.As<sbyte, TTo>(ref value);
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(short value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) return Unsafe.As<short, TTo>(ref value);
        else if (typeof(TTo) == typeof(ushort)) return Unsafe.As<short, TTo>(ref value);
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(ushort value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) return Unsafe.As<ushort, TTo>(ref value);
        else if (typeof(TTo) == typeof(ushort)) return Unsafe.As<ushort, TTo>(ref value);
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(int value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) return Unsafe.As<int, TTo>(ref value);
        else if (typeof(TTo) == typeof(uint)) return Unsafe.As<int, TTo>(ref value);
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) return Unsafe.As<int, TTo>(ref value);
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(uint value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) return Unsafe.As<uint, TTo>(ref value);
        else if (typeof(TTo) == typeof(uint)) return Unsafe.As<uint, TTo>(ref value);
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) return Unsafe.As<uint, TTo>(ref value);
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(long value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) return Unsafe.As<long, TTo>(ref value);
        else if (typeof(TTo) == typeof(ulong)) return Unsafe.As<long, TTo>(ref value);
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) return Unsafe.As<long, TTo>(ref value);
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(ulong value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) return Unsafe.As<ulong, TTo>(ref value);
        else if (typeof(TTo) == typeof(ulong)) return Unsafe.As<ulong, TTo>(ref value);
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) return Unsafe.As<ulong, TTo>(ref value);
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(float value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) return Unsafe.As<float, TTo>(ref value);
        else if (typeof(TTo) == typeof(uint)) return Unsafe.As<float, TTo>(ref value);
        else if (typeof(TTo) == typeof(long)) { var tmp = (long)value; return Unsafe.As<long, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ulong)) { var tmp = (ulong)value; return Unsafe.As<ulong, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(float)) return Unsafe.As<float, TTo>(ref value);
        else if (typeof(TTo) == typeof(double)) { var tmp = (double)value; return Unsafe.As<double, TTo>(ref tmp); }
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TTo>(double value)
    {
        if (typeof(TTo) == typeof(byte)) { var tmp = (byte)value; return Unsafe.As<byte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(sbyte)) { var tmp = (sbyte)value; return Unsafe.As<sbyte, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(short)) { var tmp = (short)value; return Unsafe.As<short, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(ushort)) { var tmp = (ushort)value; return Unsafe.As<ushort, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(int)) { var tmp = (int)value; return Unsafe.As<int, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(uint)) { var tmp = (uint)value; return Unsafe.As<uint, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(long)) return Unsafe.As<double, TTo>(ref value);
        else if (typeof(TTo) == typeof(ulong)) return Unsafe.As<double, TTo>(ref value);
        else if (typeof(TTo) == typeof(float)) { var tmp = (float)value; return Unsafe.As<float, TTo>(ref tmp); }
        else if (typeof(TTo) == typeof(double)) return Unsafe.As<double, TTo>(ref value);
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo Cast<TFrom, TTo>(TFrom value)
    {
        if (typeof(TTo) == typeof(byte)) return Cast<TTo>(Unsafe.As<TFrom, byte>(ref value));
        else if (typeof(TTo) == typeof(sbyte)) return Cast<TTo>(Unsafe.As<TFrom, sbyte>(ref value));
        else if (typeof(TTo) == typeof(short)) return Cast<TTo>(Unsafe.As<TFrom, short>(ref value));
        else if (typeof(TTo) == typeof(ushort)) return Cast<TTo>(Unsafe.As<TFrom, ushort>(ref value));
        else if (typeof(TTo) == typeof(int)) return Cast<TTo>(Unsafe.As<TFrom, int>(ref value));
        else if (typeof(TTo) == typeof(uint)) return Cast<TTo>(Unsafe.As<TFrom, uint>(ref value));
        else if (typeof(TTo) == typeof(long)) return Cast<TTo>(Unsafe.As<TFrom, long>(ref value));
        else if (typeof(TTo) == typeof(ulong)) return Cast<TTo>(Unsafe.As<TFrom, ulong>(ref value));
        else if (typeof(TTo) == typeof(float)) return Cast<TTo>(Unsafe.As<TFrom, float>(ref value));
        else if (typeof(TTo) == typeof(double)) return Cast<TTo>(Unsafe.As<TFrom, double>(ref value));
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }
}