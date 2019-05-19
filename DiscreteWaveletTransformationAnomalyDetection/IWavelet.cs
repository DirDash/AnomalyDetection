using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    interface IWavelet
    {
        List<double> Coefficients { get; }
        List<double> ScalingCoefficients { get; }
    }
}