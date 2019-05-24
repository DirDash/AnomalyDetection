using DiscreteWaveletTransformationAnomalyDetection.Distributions;
using System;
using System.Collections.Generic;
using WaveletAnomalyDetection;

namespace DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms
{
    class FischerCriterionForDispersions : IAnomalyDetectionAlgorithm
    {
        public string Name => "Критерий Фишера (дисперсии)";

        private const double _anomalySignificanceLevel = 0.001;
        private const double _probabilityOfAnomalySignificanceLevel = 0.05;

        public AnomalyDetectionResult CheckOnAnomaly(List<double> firstApproximationCoefficients, List<double> firstDetailingCoefficients,
            List<double> secondApproximationCoefficients, List<double> secondDetailingCoefficients, double sensitivity)
        {
            var firstDetailingApproximatedCoefficient = 0.0;
            foreach (var coefficient in firstDetailingCoefficients)
            {
                firstDetailingApproximatedCoefficient += coefficient;
            }
            firstDetailingApproximatedCoefficient /= firstDetailingCoefficients.Count;

            var secondDetailingApproximatedCoefficient = 0.0;
            foreach (var coefficient in secondDetailingCoefficients)
            {
                secondDetailingApproximatedCoefficient += coefficient;
            }
            secondDetailingApproximatedCoefficient /= secondDetailingCoefficients.Count;

            var firstDispersion = 0.0;
            foreach (var coefficient in firstDetailingCoefficients)
            {
                firstDispersion += Math.Pow(coefficient - firstDetailingApproximatedCoefficient, 2);
            }
            firstDispersion /= firstDetailingCoefficients.Count - 1;

            var secondDispersion = 0.0;
            foreach (var coefficient in secondDetailingCoefficients)
            {
                secondDispersion += Math.Pow(coefficient - secondDetailingApproximatedCoefficient, 2);
            }
            secondDispersion /= secondDetailingCoefficients.Count - 1;

            var statisticsResult = secondDispersion / firstDispersion;

            statisticsResult *= sensitivity;

            var probabilityOfAnomalyLimit = FischerDistribution.GetCriticalValue(firstDetailingCoefficients.Count - 1, secondDetailingCoefficients.Count - 1, _probabilityOfAnomalySignificanceLevel);
            var anomalyLimit = FischerDistribution.GetCriticalValue(firstDetailingCoefficients.Count - 1, secondDetailingCoefficients.Count - 1, _anomalySignificanceLevel);

            var result = new AnomalyDetectionResult() { Source = Name, Type = AnomalyDetectionResultType.Normal, StatisticsValue = statisticsResult, StatisticsLimit = probabilityOfAnomalyLimit };

            if (statisticsResult > probabilityOfAnomalyLimit)
            {
                result.Type = AnomalyDetectionResultType.HighProbabilityOfAnomaly;
                result.Message = "Обнаружена высокая вероятность наличия кратковременной высокочастотной аномалии.";
            }

            if (statisticsResult > anomalyLimit)
            {
                result.Type = AnomalyDetectionResultType.Anomaly;
                result.StatisticsLimit = anomalyLimit;
                result.Message = "Обнаружена кратковременная высокочастотная аномалия.";
            }

            return result;
        }

        public override string ToString() => "Критерий Фишера (дисперсии)";
    }
}