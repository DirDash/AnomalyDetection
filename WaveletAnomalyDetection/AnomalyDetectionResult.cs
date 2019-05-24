﻿namespace WaveletAnomalyDetection
{
    public class AnomalyDetectionResult
    {
        public AnomalyDetectionResultType Type { get; set; }

        public double StatisticsValue { get; set; }
        public double StatisticsLimit { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }
    }
}