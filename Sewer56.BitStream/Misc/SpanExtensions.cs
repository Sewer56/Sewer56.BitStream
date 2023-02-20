using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sewer56.BitStream.Misc;

/// <summary>
/// Utilities for spans which have zero overhead.
/// </summary>
internal static class SpanExtensions
{
    /// <summary>
    /// Slices a span without any bounds checks.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> SliceFast(this Span<byte> data, int start, int length)
    {
#if NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start), length);
#else
        return data.Slice(start, length);
#endif
    }
    
    /// <summary>
    /// Converts an array into a span fast; omitting the needs for bounds checks.
    /// </summary>
    /// <param name="data">The data to convert into spans.</param>
    /// <param name="offset">The offset of the data.</param>
    /// <param name="length">The length of the data.</param>
    public static Span<byte> AsSpanFast(this byte[] data, int offset, int length)
    {
#if NET5_0_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(data), offset), length);
#else
        return data.AsSpan(offset, length);
#endif
    }
    
    /// <summary>
    /// Casts a span to another type without bounds checks.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<TTo> CastFast<TFrom, TTo>(this Span<TFrom> data) where TFrom : struct where TTo : struct
    {
#if NETCOREAPP3_1_OR_GREATER
        // Taken from the runtime.
        // Use unsigned integers - unsigned division by constant (especially by power of 2)
        // and checked casts are faster and smaller.
        uint fromSize = (uint)Unsafe.SizeOf<TFrom>();
        uint toSize = (uint)Unsafe.SizeOf<TTo>();
        uint fromLength = (uint)data.Length;
        int toLength;
        if (fromSize == toSize)
        {
            // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // should be optimized to just `length` but the JIT doesn't do that today.
            toLength = (int)fromLength;
        }
        else if (fromSize == 1)
        {
            // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
            // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
            // the JIT can't eliminate long multiply by 1.
            toLength = (int)(fromLength / toSize);
        }
        else
        {
            // Ensure that casts are done in such a way that the JIT is able to "see"
            // the uint->ulong casts and the multiply together so that on 32 bit targets
            // 32x32to64 multiplication is used.
            ulong toLengthUInt64 = fromLength * (ulong)fromSize / toSize;
            toLength = (int)toLengthUInt64;
        }
        return MemoryMarshal.CreateSpan(
                    ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(data)),
                    toLength);
#else
        return MemoryMarshal.Cast<TFrom, TTo>(data);
#endif
    }
    
    /// <summary>
    /// Returns a reference to an element at a specified index within a given <typeparamref name="T"/> array, with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
    /// <param name="span">The input <typeparamref name="T"/> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span"/>.</param>
    /// <returns>A reference to the element within <paramref name="span"/> at the index specified by <paramref name="i"/>.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this Span<T> span, int i)
    {
        return ref Unsafe.Add(ref MemoryMarshal.GetReference(span), (nint)(uint)i);
    }
    
    /// <summary>
    /// Returns a reference to an element at a specified index within a given <typeparamref name="T"/> array, with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="array"/>.</param>
    /// <returns>A reference to the element within <paramref name="array"/> at the index specified by <paramref name="i"/>.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this T[] array, int i)
    {
#if NET5_0_OR_GREATER
        return ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), (nint)(uint)i);
#else
        return ref DangerousGetReferenceAt(array.AsSpan(), i);
#endif
    }

    /// <summary>
    /// Returns a reference to an element at a specified index within a given <typeparamref name="T"/> array, with no bounds checks,
    /// as a specified type.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
    /// <typeparam name="TType">Type to return the reference as.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="array"/>.</param>
    /// <returns>A reference to the element within <paramref name="array"/> at the index specified by <paramref name="i"/>.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TType DangerousGetReferenceAtAs<T, TType>(this T[] array, int i)
    {
        return ref Unsafe.As<T, TType>(ref array.DangerousGetReferenceAt(i));
    }
    
    /// <summary>
    /// Returns a reference to an element at a specified index within a given <typeparamref name="T"/> array, with no bounds checks,
    /// as a specified type.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
    /// <typeparam name="TType">Type to return the reference as.</typeparam>
    /// <param name="span">The input <typeparamref name="T"/> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span"/>.</param>
    /// <returns>A reference to the element within <paramref name="span"/> at the index specified by <paramref name="i"/>.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TType DangerousGetReferenceAtAs<T, TType>(this Span<T> span, int i)
    {
        return ref Unsafe.As<T, TType>(ref span.DangerousGetReferenceAt(i));
    }
}