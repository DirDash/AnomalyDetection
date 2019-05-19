using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection.Wavelets
{
    class DaubechiesWavelet : IWavelet
    {
        public List<double> Coefficients
        {
            get
            {
                return new List<double>() { -0.1830127, -0.3169873, 1.1830127, -0.6830127 };
            }
        }

        public List<double> ScalingCoefficients
        {
            get
            {
                return new List<double>() { 0.6830127, 1.1830127, 0.3169873, -0.1830127 };
            }
        }        
    }
}