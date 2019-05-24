namespace WaveletAnomalyDetection.Wavelets
{
    public class D6Wavelet : IWavelet
    {
        public override string ToString()
        {
            return "Вейвлет Добеши 6";
        }

        double[] _coefficients = new double[] { 0.0498175, 0.12083221, -0.19093442, -0.650365, 1.14111692, -0.47046721 };
        double[] _scalingCoefficients = new double[] { 0.47046721, 1.14111692, 0.650365, -0.19093442, -0.12083221, 0.0498175 };

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