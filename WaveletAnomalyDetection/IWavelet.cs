namespace WaveletAnomalyDetection
{
    public interface IWavelet
    {
        double[] Coefficients { get; }
        double[] ScalingCoefficients { get; }
    }
}