using DiscreteWaveletTransformationAnomalyDetection.AnomalyDetectionAlgorithms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaveletAnomalyDetection;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    public class DiscreteWaveletTransformationAnomalyDetector : IWaveletAnomalyDetector
    {
        public int ComparisonWindowStart { get; set; }
        public int ComparisonWindowEnd { get; set; }

        public int DetectionWindowStart { get; set; }
        public int DetectionWindowEnd { get; set; }

        public double Sensitivity { get; set; }

        public IWavelet Wavelet { get; set; }

        private List<IAnomalyDetectionAlgorithm> _anomalyDetectionAlgorithms = new List<IAnomalyDetectionAlgorithm>() { new FischerCriterionForDispersions(), new FischerCriterionForApproximations(), new CochraneCoxCriterionForApproximations() };

        private List<double> _comparisonWindowApproximationCoefficients;
        private List<double> _comparisonWindowDetailingCoefficients;

        private List<double> _detectionWindowApproximationCoefficients;
        private List<double> _detectionWindowDetailingCoefficients;

        private Dictionary<string, AnomalyDetectionResult> _anomalyDetectionAlgorithmResults;

        public IEnumerable<AnomalyDetectionResult> CheckOnAnomaly(IEnumerable<double> data)
        {
            var comparisonWindow = new Window(data, ComparisonWindowStart, ComparisonWindowEnd);
            var detectionWindow = new Window(data, DetectionWindowStart, DetectionWindowEnd);

            _comparisonWindowApproximationCoefficients = comparisonWindow.Points;
            _detectionWindowApproximationCoefficients = detectionWindow.Points;

            _anomalyDetectionAlgorithmResults = new Dictionary<string, AnomalyDetectionResult>();
            foreach (var anomalyDetectionAlgorythm in _anomalyDetectionAlgorithms)
            {
                _anomalyDetectionAlgorithmResults.Add(anomalyDetectionAlgorythm.ToString(), null);
            }

            RunAnomalyDetectionAlgorithms();

            return _anomalyDetectionAlgorithmResults.Values;
        }

        private void RunAnomalyDetectionAlgorithms()
        {
            var decompositionIsSuccessful = Decompose();
            if (!decompositionIsSuccessful)
            {
                return;
            }

            var anomalyDetectionResults = new List<AnomalyDetectionResult>();

            Parallel.ForEach(_anomalyDetectionAlgorithms, (IAnomalyDetectionAlgorithm algorythm) =>
            {
                anomalyDetectionResults.Add(algorythm.CheckOnAnomaly(_comparisonWindowApproximationCoefficients, _comparisonWindowDetailingCoefficients, _detectionWindowApproximationCoefficients, _detectionWindowDetailingCoefficients, Sensitivity));
            });

            foreach (var result in anomalyDetectionResults)
            {
                if (_anomalyDetectionAlgorithmResults[result.Source] == null || result.Type > _anomalyDetectionAlgorithmResults[result.Source].Type)
                {
                    _anomalyDetectionAlgorithmResults[result.Source] = result;
                }
            }

            if (_anomalyDetectionAlgorithmResults.Values.Any(result => result.Type == AnomalyDetectionResultType.HighProbabilityOfAnomaly))
            {
                RunAnomalyDetectionAlgorithms();
            }
        }

        private bool Decompose()
        {
            var newComparisonWindowApproximationCoefficients = Filter(_comparisonWindowApproximationCoefficients, Wavelet.ScalingCoefficients);
            var newComparisonWindowDetailingCoefficients = Filter(_comparisonWindowApproximationCoefficients, Wavelet.Coefficients);            

            var newDetectionWindowApproximationCoefficients = Filter(_detectionWindowApproximationCoefficients, Wavelet.ScalingCoefficients);
            var newDetectionWindowDetailingCoefficients = Filter(_detectionWindowApproximationCoefficients, Wavelet.Coefficients);

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

        private List<double> Filter(List<double> coefficientsToFilter, double[] filterCoefficients)
        {
            var filteredCoefficients = new List<double>();

            for (var i = 0; i + filterCoefficients.Length - 1 < coefficientsToFilter.Count; i += 2)
            {
                var newCoefficient = 0.0;
                for (int j = 0; j < filterCoefficients.Length; j++)
                {
                    newCoefficient += filterCoefficients[j] * coefficientsToFilter[i + j];
                }
                filteredCoefficients.Add(newCoefficient);
            }

            return filteredCoefficients;
        }
    }
}