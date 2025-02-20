// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Vortice.DXGI;

/// <summary>
/// Helper to use with <see cref="Format"/>.
/// </summary>
public static class FormatHelper
{
    private static readonly int[] sizeOfInBits = new int[256];
    private static readonly bool[] compressedFormats = new bool[256];
    private static readonly bool[] srgbFormats = new bool[256];
    private static readonly bool[] typelessFormats = new bool[256];

    /// <summary>
    /// Calculates the size of a <see cref="Format"/> in bytes. Can be 0 for compressed format (as they are less than 1 byte)
    /// </summary>
    /// <param name="format">The DXGI format.</param>
    /// <returns>size of in bytes</returns>
    public static int SizeOfInBytes(this Format format)
    {
        int sizeInBits = SizeOfInBits(format);
        return sizeInBits >> 3;
    }

    /// <summary>
    /// Calculates the size of a <see cref="Format"/> in bits.
    /// </summary>
    /// <param name="format">The DXGI format.</param>
    /// <returns>size of in bits</returns>
    public static int SizeOfInBits(this Format format) => sizeOfInBits[(int)format];

    /// <summary>
    /// Returns true if the <see cref="Format"/> is valid.
    /// </summary>
    /// <param name="format">A format to validate</param>
    /// <returns>True if the <see cref="Format"/> is valid.</returns>
    public static bool IsValid(this Format format)
    {
        return ((int)(format) >= 1 && (int)(format) <= 115);
    }

    /// <summary>
    /// Returns true if the <see cref="Format"/> is a compressed format.
    /// </summary>
    /// <param name="format">The format to check for compressed.</param>
    /// <returns>True if the <see cref="Format"/> is a compressed format</returns>
    public static bool IsCompressed(this Format format)
    {
        return compressedFormats[(int)format];
    }

    /// <summary>
    /// Determines whether the specified <see cref="Format"/> is packed.
    /// </summary>
    /// <param name="format">The DXGI Format.</param>
    /// <returns><c>true</c> if the specified <see cref="Format"/> is packed; otherwise, <c>false</c>.</returns>
    public static bool IsPacked(this Format format)
    {
        return ((format == Format.R8G8_B8G8_UNorm) || (format == Format.G8R8_G8B8_UNorm));
    }

    /// <summary>
    /// Determines whether the specified <see cref="Format"/> is video.
    /// </summary>
    /// <param name="format">The <see cref="Format"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Format"/> is video; otherwise, <c>false</c>.</returns>
    public static bool IsVideo(this Format format)
    {
        switch (format)
        {
            case Format.AYUV:
            case Format.Y410:
            case Format.Y416:
            case Format.NV12:
            case Format.P010:
            case Format.P016:
            case Format.YUY2:
            case Format.Y210:
            case Format.Y216:
            case Format.NV11:
                // These video formats can be used with the 3D pipeline through special view mappings
                return true;

            case Format.Opaque420:
            case Format.AI44:
            case Format.IA44:
            case Format.P8:
            case Format.A8P8:
                // These are limited use video formats not usable in any way by the 3D pipeline
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Determines whether the specified <see cref="Format"/> is a SRGB format.
    /// </summary>
    /// <param name="format">The <see cref="Format"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Format"/> is a SRGB format; otherwise, <c>false</c>.</returns>
    public static bool IsSRgb(this Format format)
    {
        return srgbFormats[(int)format];
    }

    /// <summary>
    /// Determines whether the specified <see cref="Format"/> is typeless.
    /// </summary>
    /// <param name="format">The <see cref="Format"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Format"/> is typeless; otherwise, <c>false</c>.</returns>
    public static bool IsTypeless(this Format format)
    {
        return typelessFormats[(int)format];
    }

    /// <summary>
    /// Computes the scanline count (number of scanlines).
    /// </summary>
    /// <param name="format">The <see cref="Format"/>.</param>
    /// <param name="height">The height.</param>
    /// <returns>The scanline count.</returns>
    public static int ComputeScanlineCount(this Format format, int height)
    {
        switch (format)
        {
            case Format.BC1_Typeless:
            case Format.BC1_UNorm:
            case Format.BC1_UNorm_SRgb:
            case Format.BC2_Typeless:
            case Format.BC2_UNorm:
            case Format.BC2_UNorm_SRgb:
            case Format.BC3_Typeless:
            case Format.BC3_UNorm:
            case Format.BC3_UNorm_SRgb:
            case Format.BC4_Typeless:
            case Format.BC4_UNorm:
            case Format.BC4_SNorm:
            case Format.BC5_Typeless:
            case Format.BC5_UNorm:
            case Format.BC5_SNorm:
            case Format.BC6H_Typeless:
            case Format.BC6H_Uf16:
            case Format.BC6H_Sf16:
            case Format.BC7_Typeless:
            case Format.BC7_UNorm:
            case Format.BC7_UNorm_SRgb:
                return Math.Max(1, (height + 3) / 4);

            default:
                return height;
        }
    }

    public static void GetSurfaceInfo(Format format, int width, int height, out int sizeInBytes, out int rowPitch, out int slicePitch)
    {
        bool bc = false;
        bool packed = false;
        bool planar = false;
        int bpe = 0;

        switch (format)
        {
            case Format.BC1_Typeless:
            case Format.BC1_UNorm:
            case Format.BC1_UNorm_SRgb:
            case Format.BC4_Typeless:
            case Format.BC4_UNorm:
            case Format.BC4_SNorm:
                bc = true;
                bpe = 8;
                break;

            case Format.BC2_Typeless:
            case Format.BC2_UNorm:
            case Format.BC2_UNorm_SRgb:
            case Format.BC3_Typeless:
            case Format.BC3_UNorm:
            case Format.BC3_UNorm_SRgb:
            case Format.BC5_Typeless:
            case Format.BC5_UNorm:
            case Format.BC5_SNorm:
            case Format.BC6H_Typeless:
            case Format.BC6H_Uf16:
            case Format.BC6H_Sf16:
            case Format.BC7_Typeless:
            case Format.BC7_UNorm:
            case Format.BC7_UNorm_SRgb:
                bc = true;
                bpe = 16;
                break;

            case Format.R8G8_B8G8_UNorm:
            case Format.G8R8_G8B8_UNorm:
            case Format.YUY2:
                packed = true;
                bpe = 4;
                break;

            case Format.Y210:
            case Format.Y216:
                packed = true;
                bpe = 8;
                break;

            case Format.NV12:
            case Format.Opaque420:
            case Format.P208:
                planar = true;
                bpe = 2;
                break;

            case Format.P010:
            case Format.P016:
                planar = true;
                bpe = 4;
                break;

            default:
                break;
        }

        if (bc)
        {
            int numBlocksWide = 0;
            if (width > 0)
            {
                numBlocksWide = Math.Max(1, (width + 3) / 4);
            }
            int numBlocksHigh = 0;
            if (height > 0)
            {
                numBlocksHigh = Math.Max(1, (height + 3) / 4);
            }
            rowPitch = numBlocksWide * bpe;
            slicePitch = numBlocksHigh;
            sizeInBytes = rowPitch * numBlocksHigh;
        }
        else if (packed)
        {
            rowPitch = ((width + 1) >> 1) * bpe;
            slicePitch = height;
            sizeInBytes = rowPitch * height;
        }
        else if (format == Format.NV11)
        {
            rowPitch = ((width + 3) >> 2) * 4;
            slicePitch = height * 2; // Direct3D makes this simplifying assumption, although it is larger than the 4:1:1 data
            sizeInBytes = rowPitch * slicePitch;
        }
        else if (planar)
        {
            rowPitch = ((width + 1) >> 1) * bpe;
            sizeInBytes = (rowPitch * height) + ((rowPitch * height + 1) >> 1);
            slicePitch = height + ((height + 1) >> 1);
        }
        else
        {
            int bpp = SizeOfInBits(format);
            rowPitch = (width * bpp + 7) / 8; // round up to nearest byte
            slicePitch = height;
            sizeInBytes = rowPitch * height;
        }
    }


    /// <summary>
    /// Static initializer to speed up size calculation (not sure the JIT is enough "smart" for this kind of thing).
    /// </summary>
    static FormatHelper()
    {
        InitFormat(new[] { Format.R1_UNorm }, 1);

        InitFormat(new[] { Format.A8_UNorm, Format.R8_SInt, Format.R8_SNorm, Format.R8_Typeless, Format.R8_UInt, Format.R8_UNorm }, 8);

        InitFormat(new[] {
                Format.B5G5R5A1_UNorm,
                Format.B5G6R5_UNorm,
                Format.D16_UNorm,
                Format.R16_Float,
                Format.R16_SInt,
                Format.R16_SNorm,
                Format.R16_Typeless,
                Format.R16_UInt,
                Format.R16_UNorm,
                Format.R8G8_SInt,
                Format.R8G8_SNorm,
                Format.R8G8_Typeless,
                Format.R8G8_UInt,
                Format.R8G8_UNorm,
                Format.B4G4R4A4_UNorm,
            }, 16);

        InitFormat(new[] {
                Format.B8G8R8X8_Typeless,
                Format.B8G8R8X8_UNorm,
                Format.B8G8R8X8_UNorm_SRgb,
                Format.D24_UNorm_S8_UInt,
                Format.D32_Float,
                Format.D32_Float_S8X24_UInt,
                Format.G8R8_G8B8_UNorm,
                Format.R10G10B10_Xr_Bias_A2_UNorm,
                Format.R10G10B10A2_Typeless,
                Format.R10G10B10A2_UInt,
                Format.R10G10B10A2_UNorm,
                Format.R11G11B10_Float,
                Format.R16G16_Float,
                Format.R16G16_SInt,
                Format.R16G16_SNorm,
                Format.R16G16_Typeless,
                Format.R16G16_UInt,
                Format.R16G16_UNorm,
                Format.R24_UNorm_X8_Typeless,
                Format.R24G8_Typeless,
                Format.R32_Float,
                Format.R32_Float_X8X24_Typeless,
                Format.R32_SInt,
                Format.R32_Typeless,
                Format.R32_UInt,
                Format.R8G8_B8G8_UNorm,
                Format.R8G8B8A8_SInt,
                Format.R8G8B8A8_SNorm,
                Format.R8G8B8A8_Typeless,
                Format.R8G8B8A8_UInt,
                Format.R8G8B8A8_UNorm,
                Format.R8G8B8A8_UNorm_SRgb,
                Format.B8G8R8A8_Typeless,
                Format.B8G8R8A8_UNorm,
                Format.B8G8R8A8_UNorm_SRgb,
                Format.R9G9B9E5_SharedExp,
                Format.X24_Typeless_G8_UInt,
                Format.X32_Typeless_G8X24_UInt,
            }, 32);

        InitFormat(new[] {
                Format.R16G16B16A16_Float,
                Format.R16G16B16A16_SInt,
                Format.R16G16B16A16_SNorm,
                Format.R16G16B16A16_Typeless,
                Format.R16G16B16A16_UInt,
                Format.R16G16B16A16_UNorm,
                Format.R32G32_Float,
                Format.R32G32_SInt,
                Format.R32G32_Typeless,
                Format.R32G32_UInt,
                Format.R32G8X24_Typeless,
            }, 64);

        InitFormat(new[] {
                Format.R32G32B32_Float,
                Format.R32G32B32_SInt,
                Format.R32G32B32_Typeless,
                Format.R32G32B32_UInt,
            }, 96);

        InitFormat(new[] {
                Format.R32G32B32A32_Float,
                Format.R32G32B32A32_SInt,
                Format.R32G32B32A32_Typeless,
                Format.R32G32B32A32_UInt,
            }, 128);

        InitFormat(new[] {
                Format.BC1_Typeless,
                Format.BC1_UNorm,
                Format.BC1_UNorm_SRgb,
                Format.BC4_SNorm,
                Format.BC4_Typeless,
                Format.BC4_UNorm,
            }, 4);

        InitFormat(new[] {
                Format.BC2_Typeless,
                Format.BC2_UNorm,
                Format.BC2_UNorm_SRgb,
                Format.BC3_Typeless,
                Format.BC3_UNorm,
                Format.BC3_UNorm_SRgb,
                Format.BC5_SNorm,
                Format.BC5_Typeless,
                Format.BC5_UNorm,
                Format.BC6H_Sf16,
                Format.BC6H_Typeless,
                Format.BC6H_Uf16,
                Format.BC7_Typeless,
                Format.BC7_UNorm,
                Format.BC7_UNorm_SRgb,
            }, 8);


        // Init compressed formats
        InitDefaults(new[]
                         {
                                 Format.BC1_Typeless,
                                 Format.BC1_UNorm,
                                 Format.BC1_UNorm_SRgb,
                                 Format.BC2_Typeless,
                                 Format.BC2_UNorm,
                                 Format.BC2_UNorm_SRgb,
                                 Format.BC3_Typeless,
                                 Format.BC3_UNorm,
                                 Format.BC3_UNorm_SRgb,
                                 Format.BC4_Typeless,
                                 Format.BC4_UNorm,
                                 Format.BC4_SNorm,
                                 Format.BC5_Typeless,
                                 Format.BC5_UNorm,
                                 Format.BC5_SNorm,
                                 Format.BC6H_Typeless,
                                 Format.BC6H_Uf16,
                                 Format.BC6H_Sf16,
                                 Format.BC7_Typeless,
                                 Format.BC7_UNorm,
                                 Format.BC7_UNorm_SRgb,
                             }, compressedFormats);

        // Init srgb formats
        InitDefaults(new[]
                         {
                                 Format.R8G8B8A8_UNorm_SRgb,
                                 Format.BC1_UNorm_SRgb,
                                 Format.BC2_UNorm_SRgb,
                                 Format.BC3_UNorm_SRgb,
                                 Format.B8G8R8A8_UNorm_SRgb,
                                 Format.B8G8R8X8_UNorm_SRgb,
                                 Format.BC7_UNorm_SRgb,
                             }, srgbFormats);

        // Init typeless formats
        InitDefaults(new[]
                         {
                                 Format.R32G32B32A32_Typeless,
                                 Format.R32G32B32_Typeless,
                                 Format.R16G16B16A16_Typeless,
                                 Format.R32G32_Typeless,
                                 Format.R32G8X24_Typeless,
                                 Format.R10G10B10A2_Typeless,
                                 Format.R8G8B8A8_Typeless,
                                 Format.R16G16_Typeless,
                                 Format.R32_Typeless,
                                 Format.R24G8_Typeless,
                                 Format.R8G8_Typeless,
                                 Format.R16_Typeless,
                                 Format.R8_Typeless,
                                 Format.BC1_Typeless,
                                 Format.BC2_Typeless,
                                 Format.BC3_Typeless,
                                 Format.BC4_Typeless,
                                 Format.BC5_Typeless,
                                 Format.B8G8R8A8_Typeless,
                                 Format.B8G8R8X8_Typeless,
                                 Format.BC6H_Typeless,
                                 Format.BC7_Typeless,
                             }, typelessFormats);


    }

    private static void InitFormat(IEnumerable<Format> formats, int bitCount)
    {
        foreach (Format format in formats)
        {
            sizeOfInBits[(int)format] = bitCount;
        }
    }

    private static void InitDefaults(IEnumerable<Format> formats, bool[] outputArray)
    {
        foreach (Format format in formats)
        {
            outputArray[(int)format] = true;
        }
    }
}
