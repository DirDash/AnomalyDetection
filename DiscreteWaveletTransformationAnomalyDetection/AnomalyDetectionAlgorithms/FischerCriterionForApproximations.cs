﻿using DiscreteWaveletTransformationAnomalyDetection.Distributions;
using System;
using System.Collections.Generic;
using WaveletAnomalyDetection;

namespace DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms
{
    class FischerCriterionForApproximations : IAnomalyDetectionAlgorithm
    {
        public string Name
        {
            get => "Критерий Фишера (среднее значение)";
        }

        private const double _anomalySignificanceLevel = 0.001;
        private const double _probabilityOfAnomalySignificanceLevel = 0.05;

        public AnomalyDetectionResult CheckOnAnomaly(List<double> firstApproximationCoefficients, List<double> firstDetailingCoefficients,
            List<double> secondApproximationCoefficients, List<double> secondDetailingCoefficients, double sensitivity)
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

            statisticsResult *= sensitivity;

            var probabilityOfAnomalyLimit = FischerDistributionTable.GetCriticalValue(2 * firstApproximationCoefficients.Count, 2 * secondApproximationCoefficients.Count, _probabilityOfAnomalySignificanceLevel);
            var anomalyLimit = FischerDistributionTable.GetCriticalValue(2 * firstApproximationCoefficients.Count, 2 * secondApproximationCoefficients.Count, _anomalySignificanceLevel);

            var result = new AnomalyDetectionResult() { Source = Name, Type = AnomalyDetectionResultType.Normal, StatisticsValue = statisticsResult, StatisticsLimit = probabilityOfAnomalyLimit };

            if (statisticsResult > probabilityOfAnomalyLimit)
            {
                result.Type = AnomalyDetectionResultType.HighProbabilityOfAnomaly;
                result.Message = "Обнаружена высокая вероятность наличия долговременной низкочастотной аномалии.";
            }

            if (statisticsResult > anomalyLimit)
            {
                result.Type = AnomalyDetectionResultType.Anomaly;
                result.StatisticsLimit = anomalyLimit;
                result.Message = "Обнаружена долговременная низкочастотная аномалия.";
            }

            return result;
        }

        public override string ToString() => Name;
    }
}