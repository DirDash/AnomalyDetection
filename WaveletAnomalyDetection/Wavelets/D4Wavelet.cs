namespace WaveletAnomalyDetection.Wavelets
{
    public class D4Wavelet : IWavelet
    {
        public override string ToString()
        {
            return "Вейвлет Добеши 4";
        }

        double[] _coefficients = new double[] { -0.1830127, -0.3169873, 1.1830127, -0.6830127 };
        double[] _scalingCoefficients = new double[] { 0.6830127, 1.1830127, 0.3169873, -0.1830127 };

        public double[] Coefficients
        {
            get
            {
                return _coefficients;
            }
        }

        public double[] ScalingCoefficients
        {
            get
            {
                return _scalingCoefficients;
            }
        }
    }
}