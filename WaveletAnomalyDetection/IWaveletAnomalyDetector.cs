using System.Collections.Generic;

namespace WaveletAnomalyDetection
{
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