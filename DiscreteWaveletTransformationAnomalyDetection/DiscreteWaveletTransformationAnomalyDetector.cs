using DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms;
using DiscreteWaveletTransformationAnomalyDetection.Wavelets;

using System.Collections.Generic;
using System.Linq;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    public class DiscreteWaveletTransformationAnomalyDetector
    {
        public int ComparisonWindowLeftLimit { get; set; }
        public int ComparisonWindowRightLimit { get; set; }

        public int DetectionWindowLeftLimit { get; set; }
        public int DetectionWindowRightLimit { get; set; }

        private IWavelet _wavelet = new DaubechiesWavelet();

        private List<IAnomalyDetectionAlgorithm> _anomalyDetectionAlgorithms = new List<IAnomalyDetectionAlgorithm>() { new FischerCriterionForDispersions(), new FischerCriterionForApproximations(), new CochraneCoxCriterionForApproximations() };

        private List<double> _comparisonWindowApproximationCoefficients;
        private List<double> _comparisonWindowDetailingCoefficients;

        private List<double> _detectionWindowApproximationCoefficients;
        private List<double> _detectionWindowDetailingCoefficients;

        public IEnumerable<AnomalyDetectionResult> CheckOnAnomaly(IEnumerable<double> points)
        {
            var comparisonWindow = new Window(points, ComparisonWindowLeftLimit, ComparisonWindowRightLimit);
            var detectionWindow = new Window(points, DetectionWindowLeftLimit, DetectionWindowRightLimit);

            _comparisonWindowApproximationCoefficients = comparisonWindow.Points;
            _detectionWindowApproximationCoefficients = detectionWindow.Points;
            
            Decompose();

            var anomalyDetectionAlgorithmResults = new Dictionary<string, AnomalyDetectionResult>();
            foreach (var anomalyDetectionAlgorythm in _anomalyDetectionAlgorithms)
            {
                anomalyDetectionAlgorithmResults.Add(anomalyDetectionAlgorythm.Name, new AnomalyDetectionResult());
            }

            while (true)
            {
                foreach (var anomalyDetectionAlgorythm in _anomalyDetectionAlgorithms)
                {
                    var anomalyDetectionResult = anomalyDetectionAlgorythm.CheckOnAnomaly(_comparisonWindowApproximationCoefficients, _comparisonWindowDetailingCoefficients, _detectionWindowApproximationCoefficients, _detectionWindowDetailingCoefficients);
                    if (anomalyDetectionResult.AnomalyIsDetected || (anomalyDetectionResult.AnomalyProbabilityIsHigh && anomalyDetectionAlgorithmResults[anomalyDetectionAlgorythm.Name].AnomalyIsDetected))
                    {
                        anomalyDetectionAlgorithmResults[anomalyDetectionAlgorythm.Name] = anomalyDetectionResult;
                    }
                }
                                
                if (anomalyDetectionAlgorithmResults.Values.Any(result => result.AnomalyProbabilityIsHigh && !result.AnomalyIsDetected))
                {
                    var decompositionIsSuccessful = Decompose();
                    if (!decompositionIsSuccessful)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            var anomalytDetectionResult = new List<AnomalyDetectionResult>();

            foreach (var anomalyDetectionAlgorithmResult in anomalyDetectionAlgorithmResults.Values)
            {
                if (anomalyDetectionAlgorithmResult.AnomalyIsDetected || anomalyDetectionAlgorithmResult.AnomalyProbabilityIsHigh)
                {
                    anomalytDetectionResult.Add(anomalyDetectionAlgorithmResult);
                }
            }

            return anomalytDetectionResult;
        }

        private bool Decompose()
        {
            var newComparisonWindowApproximationCoefficients = Filter(_comparisonWindowApproximationCoefficients, _wavelet.ScalingCoefficients);
            var newComparisonWindowDetailingCoefficients = Filter(_comparisonWindowApproximationCoefficients, _wavelet.Coefficients);            

            var newDetectionWindowApproximationCoefficients = Filter(_detectionWindowApproximationCoefficients, _wavelet.ScalingCoefficients);
            var newDetectionWindowDetailingCoefficients = Filter(_detectionWindowApproximationCoefficients, _wavelet.Coefficients);

            if (newComparisonWindowApproximationCoefficients.Count == 0 || newDetectionWindowApproximationCoefficients.Count == 0)
            {
                return false;
            }

            _comparisonWindowApproximationCoefficients = newComparisonWindowApproximationCoefficients;
            _comparisonWindowDetailingCoefficients = newComparisonWindowDetailingCoefficients;

            _detectionWindowApproximationCoefficients = newDetectionWindowApproximationCoefficients;
            _detectionWindowDetailingCoefficients = newDetectionWindowDetailingCoefficients;

            return true;
        }

        private List<double> Filter(List<double> coefficientsToFilter, List<double> filterCoefficients)
        {
            var filteredCoefficients = new List<double>();

            for (var i = 0; i + filterCoefficients.Count - 1 < coefficientsToFilter.Count; i += 2)
            {
                var newCoefficient = 0.0;
                for (int j = 0; j < filterCoefficients.Count; j++)
                {
                    newCoefficient += filterCoefficients[j] * coefficientsToFilter[i + j];
                }
                filteredCoefficients.Add(newCoefficient);
            }

            return filteredCoefficients;
        }
    }
}