using System.Collections.Generic;

namespace WaveletAnomalyDetection
{
    /// <summary>
    /// Детектор аномалий на основе вейвлет-преобразования. Данные для сбора статистики нормы и поиска аномалий указываются в окнах сравнения и обнаружения соответственно. 
    /// </summary>
    public interface IWaveletAnomalyDetector
    {
        int ComparisonWindowStart { get; set; }
        int ComparisonWindowEnd { get; set; }

        int DetectionWindowStart { get; set; }
        int DetectionWindowEnd { get; set; }

        double Sensitivity { get; set; }

        IWavelet Wavelet { get; set; }

        IEnumerable<AnomalyDetectionResult> CheckOnAnomaly(IEnumerable<double> data);
    }
}