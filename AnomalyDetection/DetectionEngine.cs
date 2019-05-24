using DiscreteWaveletTransformationAnomalyDetection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WaveletAnomalyDetection;
using WaveletAnomalyDetection.Wavelets;

namespace AnomalyDetectionApplication
{
    class DetectionEngine
    {
        private List<double> _data = new List<double>();
        public List<double> Data
        {
            get => _data;

            set
            {
                _data = value;

                if (_data.Count > 0)
                {
                    DataMinValue = _data[0];
                    DataMaxValue = _data[0];

                    for (int i = 1; i < _data.Count; i++)
                    {
                        if (_data[i] < DataMinValue)
                        {
                            DataMinValue = _data[i];
                        }
                        if (_data[i] > DataMaxValue)
                        {
                            DataMaxValue = _data[i];
                        }
                    }
                }
            }
        }

        public double DataMinValue { get; private set; }
        public double DataMaxValue { get; private set; }

        public int ComparisonWindowStart
        {
            get => _anomalyDetector.ComparisonWindowStart;

            set
            {
                var newValue = value;

                if (newValue < 0)
                {
                    newValue = 0;
                }

                if (newValue > Data.Count - _windowGasp - 1)
                {
                    newValue = Data.Count - _windowGasp - 1;
                }

                _anomalyDetector.ComparisonWindowStart = newValue;

                if (_anomalyDetector.ComparisonWindowStart >= _anomalyDetector.ComparisonWindowEnd)
                {
                    _anomalyDetector.ComparisonWindowEnd = _anomalyDetector.ComparisonWindowStart + _windowGasp;
                }
            }
        }

        public int ComparisonWindowEnd
        {
            get => _anomalyDetector.ComparisonWindowEnd;

            set
            {
                var newValue = value;

                if (newValue < _windowGasp)
                {
                    newValue = _windowGasp;
                }

                if (newValue > Data.Count)
                {
                    newValue = Data.Count;
                }

                _anomalyDetector.ComparisonWindowEnd = newValue;

                if (_anomalyDetector.ComparisonWindowEnd <= _anomalyDetector.ComparisonWindowStart)
                {
                    _anomalyDetector.ComparisonWindowStart = _anomalyDetector.ComparisonWindowEnd - _windowGasp;
                }
            }
        }

        public int DetectionWindowStart
        {
            get => _anomalyDetector.DetectionWindowStart;

            set
            {
                var newValue = value;

                if (newValue < 0)
                {
                    newValue = 0;
                }

                if (newValue > Data.Count - _windowGasp - 1)
                {
                    newValue = Data.Count - _windowGasp - 1;
                }

                _anomalyDetector.DetectionWindowStart = newValue;

                if (_anomalyDetector.DetectionWindowStart >= _anomalyDetector.DetectionWindowEnd)
                {
                    _anomalyDetector.DetectionWindowEnd = _anomalyDetector.DetectionWindowStart + _windowGasp;
                }
            }
        }

        public int DetectionWindowEnd
        {
            get => _anomalyDetector.DetectionWindowEnd;

            set
            {
                var newValue = value;

                if (newValue < _windowGasp)
                {
                    newValue = _windowGasp;
                }

                if (newValue > Data.Count)
                {
                    newValue = Data.Count;
                }

                _anomalyDetector.DetectionWindowEnd = newValue;

                if (_anomalyDetector.DetectionWindowEnd <= _anomalyDetector.DetectionWindowStart)
                {
                    _anomalyDetector.DetectionWindowStart = _anomalyDetector.DetectionWindowEnd - _windowGasp;
                }
            }
        }

        public IWavelet ChosenWavelet
        {
            get => _anomalyDetector.Wavelet;

            set
            {
                _anomalyDetector.Wavelet = value;
            }
        }

        public double Sensivity
        {
            get => _anomalyDetector.Sensitivity;

            set
            {                
                _anomalyDetector.Sensitivity = value;
            }
        }

        private List<IWavelet> _allWaveLets = new List<IWavelet>() { new HaarWavelet(), new D4Wavelet(), new D6Wavelet(), new D8Wavelet() };
        public List<IWavelet> AllWavelets
        {
            get => _allWaveLets;
        }

        public List<AnomalyDetectionResult> AnomalyDetectionResults { get; private set; }

        private IWaveletAnomalyDetector _anomalyDetector = new DiscreteWaveletTransformationAnomalyDetector();

        private const int _windowGasp = 1;

        public double MinSensivity
        {
            get => 0.01;
        }

        public void LoadDataFromFile(string filePath)
        {
            var data = new List<double>();
            using (var streamReader = new StreamReader(filePath))
            {
                var line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    data.Add(double.Parse(line));
                }
            }
            Data = data;
        }

        public void CheckOnAnomaly()
        {
            AnomalyDetectionResults = _anomalyDetector.CheckOnAnomaly(Data).ToList();
        }

        public void SaveDataToFile(string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteLine("Results:");
                foreach (var detectionResult in AnomalyDetectionResults)
                {
                    streamWriter.WriteLine($"Source:{detectionResult.Source}");
                    streamWriter.WriteLine($"Result:{detectionResult.Type}");
                    streamWriter.WriteLine($"Value:{detectionResult.StatisticsValue}");
                    streamWriter.WriteLine($"Limit:{detectionResult.StatisticsLimit}");
                }

                streamWriter.WriteLine($"Comparison window:[{ComparisonWindowStart},{ComparisonWindowEnd})");
                streamWriter.WriteLine($"Detection window:[{DetectionWindowStart},{DetectionWindowEnd})");
                streamWriter.WriteLine($"Sensivity:{Sensivity}");

                streamWriter.WriteLine("Data:");
                foreach (var value in Data)
                {
                    streamWriter.WriteLine(value);
                }
            }
        }
    }
}