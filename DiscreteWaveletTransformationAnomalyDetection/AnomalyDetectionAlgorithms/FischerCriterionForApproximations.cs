using DiscreteWaveletTransformationAnomalyDetection.Distributions;
using System;
using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms
{
    class FischerCriterionForApproximations : IAnomalyDetectionAlgorithm
    {
        public string Name
        {
            get
            {
                return "Fischer criterion for approximations";
            }
        }

        private const double _anomalySignificanceLevel = 0.001;
        private const double _probabilityOfAnomalySignificanceLevel = 0.05;

        public AnomalyDetectionResult CheckOnAnomaly(List<double> firstApproximationCoefficients, List<double> firstDetailingCoefficients,
            List<double> secondApproximationCoefficients, List<double> secondDetailingCoefficients)
        {
            var firstApproximationApproximatedCoefficient = 0.0;
            foreach (var coefficient in firstApproximationCoefficients)
            {
                firstApproximationApproximatedCoefficient += coefficient;
            }
            firstApproximationApproximatedCoefficient /= firstApproximationCoefficients.Count;

            var secondApproximationApproximatedCoefficient = 0.0;
            foreach (var coefficient in secondApproximationCoefficients)
            {
                secondApproximationApproximatedCoefficient += coefficient;
            }
            secondApproximationApproximatedCoefficient /= secondApproximationCoefficients.Count;

            var statisticsResult = Math.Abs(firstApproximationApproximatedCoefficient / secondApproximationApproximatedCoefficient);

            var result = new AnomalyDetectionResult();

            if (statisticsResult > FischerDistribution.GetCriticalValue(2 * firstApproximationCoefficients.Count, 2 * secondApproximationCoefficients.Count, _anomalySignificanceLevel))
            {
                result.AnomalyProbabilityIsHigh = true;
                result.Message = "Обнаружена высокая вероятность наличия краткосрочной высокочастотной аномалии.";
            }

            if (statisticsResult > FischerDistribution.GetCriticalValue(2 * firstApproximationCoefficients.Count, 2 * secondApproximationCoefficients.Count, _probabilityOfAnomalySignificanceLevel))
            {
                result.AnomalyIsDetected = true;
                result.Message = "Обнаружена краткосрочная высокочастотная аномалия.";
            }

            return result;
        }
    }
}