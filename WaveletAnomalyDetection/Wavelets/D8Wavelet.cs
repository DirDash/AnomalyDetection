namespace WaveletAnomalyDetection.Wavelets
{
    public class D8Wavelet : IWavelet
    {
        public override string ToString()
        {
            return "Вейвлет Добеши 8";
        }

        double[] _coefficients = new double[] { -0.01498699, -0.0465036, 0.0436163, 0.26450717, -0.03957503, -0.89220014, 1.01094572, -0.32580343 };
        double[] _scalingCoefficients = new double[] { 0.32580343, 1.01094572, 0.89220014, -0.03957503, -0.26450717, 0.0436163, 0.0465036, -0.01498699 };

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