using DiscreteWaveletTransformationAnomalyDetection.Distributions;
using System;
using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms
{
    class CochraneCoxCriterionForApproximations : IAnomalyDetectionAlgorithm
    {
        public string Name
        {
            get
            {
                return "Cochrane-Cox criterion for approximations";
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

            var firstDispersion = 0.0;
            foreach (var coefficient in firstApproximationCoefficients)
            {
                firstDispersion += Math.Pow(coefficient - firstApproximationApproximatedCoefficient, 2);
            }
            firstDispersion /= firstApproximationCoefficients.Count - 1;

            var secondDispersion = 0.0;
            foreach (var coefficient in secondApproximationCoefficients)
            {
                secondDispersion += Math.Pow(coefficient - secondApproximationApproximatedCoefficient, 2);
            }
            secondDispersion /= secondApproximationCoefficients.Count - 1;

            var firstWeightedDispersion = firstDispersion / firstApproximationCoefficients.Count;
            var secondWeightedDispersion = secondDispersion / secondApproximationCoefficients.Count;
            var summaryDispersion = firstWeightedDispersion + secondWeightedDispersion;

            var statisticsResult = (Math.Abs(secondApproximationApproximatedCoefficient - firstApproximationApproximatedCoefficient)) / summaryDispersion;

            var anomalyLimit = (firstWeightedDispersion * StudentDistribution.GetCriticalValue(firstApproximationCoefficients.Count - 1, _anomalySignificanceLevel)
                + secondWeightedDispersion * StudentDistribution.GetCriticalValue(secondApproximationCoefficients.Count - 1, _anomalySignificanceLevel))
                / (firstWeightedDispersion + secondWeightedDispersion);

            var probabilityOfAnomalyLimit = (firstWeightedDispersion * StudentDistribution.GetCriticalValue(firstApproximationCoefficients.Count - 1, _probabilityOfAnomalySignificanceLevel)
                + secondWeightedDispersion * StudentDistribution.GetCriticalValue(secondApproximationCoefficients.Count - 1, _probabilityOfAnomalySignificanceLevel))
                / (firstWeightedDispersion + secondWeightedDispersion);

            var result = new AnomalyDetectionResult();

            if (statisticsResult > probabilityOfAnomalyLimit)
            {
                result.AnomalyProbabilityIsHigh = true;
                result.Message = "Обнаружена высокая вероятность наличия долговременной низкочастотной аномалии.";
            }

            if (statisticsResult > anomalyLimit)
            {
                result.AnomalyIsDetected = true;
                result.Message = "Обнаружена долговременная низкочастотная аномалия.";
            }

            return result;
        }
    }
}