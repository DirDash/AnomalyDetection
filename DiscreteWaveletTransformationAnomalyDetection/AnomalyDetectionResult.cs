namespace DiscreteWaveletTransformationAnomalyDetection
{
    public struct AnomalyDetectionResult
    {
        public bool AnomalyIsDetected { get; set; }
        public bool AnomalyProbabilityIsHigh { get; set; }
        public string Message { get; set; }
    }
}