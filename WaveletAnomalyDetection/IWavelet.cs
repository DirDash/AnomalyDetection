namespace WaveletAnomalyDetection
{
    /// <summary>
    /// Конкретный вейвлет, заданный своими коэффициентами и коэффициентами своей масштабирующей функции.
    /// </summary>
    public interface IWavelet
    {
        double[] Coefficients { get; }
        double[] ScalingCoefficients { get; }
    }
}