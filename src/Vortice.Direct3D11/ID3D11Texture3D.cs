﻿// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.Direct3D11;

public partial class ID3D11Texture3D
{
    /// <inheritdoc/>
    public override int CalculateSubResourceIndex(int mipSlice, int arraySlice, out int mipSize)
    {
        var desc = GetDescription();
        mipSize = CalculateMipSize(mipSlice, desc.Depth);
        return CalculateSubResourceIndex(mipSlice, arraySlice, desc.MipLevels);
    }
}
