// Copyright © Aaron Sun, Amer Koleci, and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.DirectML;

public partial struct RoiAlign1OperatorDescription : IOperatorDescription, IOperatorDescriptionMarshal
{
    /// <summary>
    /// Gets the type of operator description.
    /// </summary>
    public OperatorType OperatorType => OperatorType.RoiAlign1;

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::InputTensor']/*" />
    public TensorDescription InputTensor { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::ROITensor']/*" />
    public TensorDescription RoiTensor { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::BatchIndicesTensor']/*" />
    public TensorDescription BatchIndicesTensor { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::OutputTensor']/*" />
    public TensorDescription OutputTensor { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::ReductionFunction']/*" />
    public ReduceFunction ReductionFunction { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::InterpolationMode']/*" />
    public InterpolationMode InterpolationMode { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::SpatialScaleX']/*" />
    public float SpatialScaleX { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::SpatialScaleY']/*" />
    public float SpatialScaleY { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::InputPixelOffset']/*" />
    public float InputPixelOffset { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::OutputPixelOffset']/*" />
    public float OutputPixelOffset { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::OutOfBoundsInputValue']/*" />
    public float OutOfBoundsInputValue { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::MinimumSamplesPerOutput']/*" />
    public int MinimumSamplesPerOutput { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::MaximumSamplesPerOutput']/*" />
    public int MaximumSamplesPerOutput { get; set; }

    /// <include file="Documentation.xml" path="/comments/comment[@id='DML_ROI_ALIGN1_OPERATOR_DESC::AlignRegionsToCorners']/*" />
    public bool AlignRegionsToCorners { get; set; }

    #region Marshal
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct __Native
    {
        public IntPtr InputTensor;
        public IntPtr RoiTensor;
        public IntPtr BatchIndicesTensor;
        public IntPtr OutputTensor;
        public ReduceFunction ReductionFunction;
        public InterpolationMode InterpolationMode;
        public float SpatialScaleX;
        public float SpatialScaleY;
        public float InputPixelOffset;
        public float OutputPixelOffset;
        public float OutOfBoundsInputValue;
        public int MinimumSamplesPerOutput;
        public int MaximumSamplesPerOutput;
        public bool AlignRegionsToCorners;
    }

    unsafe IntPtr IOperatorDescriptionMarshal.__MarshalAlloc()
    {
        __Native* @ref = UnsafeUtilities.Alloc<__Native>();

        @ref->InputTensor = InputTensor.__MarshalAlloc();
        @ref->RoiTensor = RoiTensor.__MarshalAlloc();
        @ref->BatchIndicesTensor = BatchIndicesTensor.__MarshalAlloc();
        @ref->OutputTensor = OutputTensor.__MarshalAlloc();
        @ref->ReductionFunction = ReductionFunction;
        @ref->InterpolationMode = InterpolationMode;
        @ref->SpatialScaleX = SpatialScaleX;
        @ref->SpatialScaleY = SpatialScaleY;
        @ref->InputPixelOffset = InputPixelOffset;
        @ref->OutputPixelOffset = OutputPixelOffset;
        @ref->OutOfBoundsInputValue = OutOfBoundsInputValue;
        @ref->MinimumSamplesPerOutput = MinimumSamplesPerOutput;
        @ref->MaximumSamplesPerOutput = MaximumSamplesPerOutput;
        @ref->AlignRegionsToCorners = AlignRegionsToCorners;

        return new(@ref);
    }

    unsafe void IOperatorDescriptionMarshal.__MarshalFree(ref IntPtr pDesc)
    {
        var @ref = (__Native*)pDesc;

        InputTensor.__MarshalFree(ref @ref->InputTensor);
        RoiTensor.__MarshalFree(ref @ref->RoiTensor);
        BatchIndicesTensor.__MarshalFree(ref @ref->BatchIndicesTensor);
        OutputTensor.__MarshalFree(ref @ref->OutputTensor);

        UnsafeUtilities.Free(@ref);
    }
    #endregion

    public static implicit operator OperatorDescription(RoiAlign1OperatorDescription description)
    {
        return new(description);
    }
}
