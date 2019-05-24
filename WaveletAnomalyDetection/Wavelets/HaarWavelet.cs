namespace WaveletAnomalyDetection.Wavelets
{
    public class HaarWavelet : IWavelet
    {
        public override string ToString()
        {
            return "Вейвлет Хаара";
        }

        double[] _coefficients = new double[] { 1, -1 };
        double[] _scalingCoefficients = new double[] { 1, 1 };

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