using DiscreteWaveletTransformationAnomalyDetection.Distributions;
using System;
using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms
{
    class FischerCriterionForDispersions : IAnomalyDetectionAlgorithm
    {
        public string Name
        {
            get
            {
                return "Fischer criterion for dispersions";
            }
        }

        private const double _anomalySignificanceLevel = 0.001;
        private const double _probabilityOfAnomalySignificanceLevel = 0.05;

        public AnomalyDetectionResult CheckOnAnomaly(List<double> firstApproximationCoefficients, List<double> firstDetailingCoefficients,
            List<double> secondApproximationCoefficients, List<double> secondDetailingCoefficients)
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

            var result = new AnomalyDetectionResult();

            if (statisticsResult > FischerDistribution.GetCriticalValue(firstDetailingCoefficients.Count - 1, secondDetailingCoefficients.Count - 1, _anomalySignificanceLevel))
            {
                result.AnomalyProbabilityIsHigh = true;
                result.Message = "Обнаружена высокая вероятность наличия краткосрочной высокочастотной аномалии.";
            }

            if (statisticsResult > FischerDistribution.GetCriticalValue(firstDetailingCoefficients.Count - 1, secondDetailingCoefficients.Count - 1, _probabilityOfAnomalySignificanceLevel))
            {
                result.AnomalyIsDetected = true;
                result.Message = "Обнаружена краткосрочная высокочастотная аномалия.";
            }

            return result;
        }
    }
}