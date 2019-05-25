using DiscreteWaveletTransformationAnomalyDetection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WaveletAnomalyDetection;
using WaveletAnomalyDetection.Wavelets;

namespace AnomalyDetectionApplication
{
    /// <summary>
    /// Класс-обёртка над детектором аномалий на основе дискретного вейвлет-преобразования с функцией чтения исходных данных и сохранением результатов поиска.
    /// </summary>
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

                if (newValue > Data.Count - _windowGap - 1)
                {
                    newValue = Data.Count - _windowGap - 1;
                }

                _anomalyDetector.ComparisonWindowStart = newValue;

                if (ComparisonWindowStart >= ComparisonWindowEnd - _windowGap)
                {
                    var newEnd = ComparisonWindowStart + _windowGap;
                    if (newEnd > Data.Count)
                    {
                        newEnd = Data.Count;
                    }
                    _anomalyDetector.ComparisonWindowEnd = newEnd;
                }
            }
        }

        public int ComparisonWindowEnd
        {
            get => _anomalyDetector.ComparisonWindowEnd;

            set
            {
                var newValue = value;

                if (newValue < _windowGap)
                {
                    newValue = _windowGap;
                }

                if (newValue > Data.Count)
                {
                    newValue = Data.Count;
                }

                _anomalyDetector.ComparisonWindowEnd = newValue;

                if (ComparisonWindowStart >= ComparisonWindowEnd - _windowGap)
                {
                    var newStart = ComparisonWindowEnd - _windowGap;
                    if (newStart < 0)
                    {
                        newStart = 0;
                    }
                    _anomalyDetector.ComparisonWindowStart = newStart;
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

                if (newValue > Data.Count - _windowGap - 1)
                {
                    newValue = Data.Count - _windowGap - 1;
                }

                _anomalyDetector.DetectionWindowStart = newValue;

                if (DetectionWindowStart >= DetectionWindowEnd - _windowGap)
                {
                    var newEnd = DetectionWindowStart + _windowGap;
                    if (newEnd > Data.Count)
                    {
                        newEnd = Data.Count;
                    }
                    _anomalyDetector.DetectionWindowEnd = newEnd;
                }
            }
        }

        public int DetectionWindowEnd
        {
            get => _anomalyDetector.DetectionWindowEnd;

            set
            {
                var newValue = value;

                if (newValue < _windowGap)
                {
                    newValue = _windowGap;
                }

                if (newValue > Data.Count)
                {
                    newValue = Data.Count;
                }

                _anomalyDetector.DetectionWindowEnd = newValue;

                if (DetectionWindowStart >= DetectionWindowEnd - _windowGap)
                {
                    var newStart = DetectionWindowEnd - _windowGap;
                    if (newStart < 0)
                    {
                        newStart = 0;
                    }
                    _anomalyDetector.DetectionWindowStart = newStart;
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

        public List<string> Warnings { get; private set; }

        public double MinSensivity
        {
            get => 0.01;
        }

        private IWaveletAnomalyDetector _anomalyDetector = new DiscreteWaveletTransformationAnomalyDetector();

        private const int _windowGap = 50;

        private const int _recomendedMinSize = 100;

        private const int _recomendedWindowDifference = 4;

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
            CheckParameters();

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

        public void ClearResults()
        {
            if (AnomalyDetectionResults != null)
            {
                AnomalyDetectionResults.Clear();
            }
            if (Warnings != null)
            {
                Warnings.Clear();
            }
        }

        private void CheckParameters()
        {
            Warnings = new List<string>();

            var comparisonWindowSize = ComparisonWindowEnd - ComparisonWindowStart;
            var detectionWindowSize = DetectionWindowEnd - DetectionWindowStart;

            if (comparisonWindowSize < _recomendedMinSize)
            {
                Warnings.Add($"ПРЕДУПРЕЖДЕНИЕ: Размер окна сравнения меньше рекомендуемого ({_recomendedMinSize}): результат поиска может не соответствовать действительности.");
            }

            if (detectionWindowSize < _recomendedMinSize)
            {
                Warnings.Add($"ПРЕДУПРЕЖДЕНИЕ: Размер окна обнаружения меньше рекомендуемого ({_recomendedMinSize}): результат поиска может не соответствовать действительности.");
            }

            if (detectionWindowSize * _recomendedWindowDifference > comparisonWindowSize)
            {
                Warnings.Add($"ПРЕДУПРЕЖДЕНИЕ: Размер окна обнаружения слишком велик по отношению к размеру окна сравнения: результат поиска может не соответствовать действительности.");
            }
        }
    }
}