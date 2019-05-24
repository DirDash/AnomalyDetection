using System.Collections.Generic;
using WaveletAnomalyDetection;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    /// <summary>
    /// Алгоритм поиска аномалий на основе статистических критериев.
    /// </summary>
    interface IAnomalyDetectionAlgorithm
    {
        string Name { get; }

        AnomalyDetectionResult CheckOnAnomaly(List<double> firstApproximationCoefficients, List<double> firstDetailingCoefficients, List<double> secondApproximationCoefficients, List<double> secondDetailingCoefficients, double sensitivity);
    }
}