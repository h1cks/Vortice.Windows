// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.DXGI
{
    /// <summary>
    /// Controls the gamma capabilities of an adapter.
    /// </summary>
    public partial struct GammaControlCapabilities
    {
        /// <summary>
        /// True if scaling and offset operations are supported during gamma correction; otherwise, false.
        /// </summary>
        public SharpGen.Runtime.RawBool ScaleAndOffsetSupported;
        /// <summary>
        /// A value describing the maximum range of the control-point positions.
        /// </summary>
        public float MaxConvertedValue;
        /// <summary>
        /// A value describing the minimum range of the control-point positions.
        /// </summary>
        public float MinConvertedValue;
        /// <summary>
        /// A value describing the number of control points in the array.
        /// </summary>
        public int NumGammaControlPoints;
        /// <summary>
        /// An array of values describing control points; the maximum length of control points is 1025.
        /// </summary>
        public unsafe fixed float ControlPointPositions[1025];
    }
}
