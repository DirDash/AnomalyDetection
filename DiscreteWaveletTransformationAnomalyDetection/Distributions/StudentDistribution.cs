using System;
using System.Collections.Generic;

namespace DiscreteWaveletTransformationAnomalyDetection.Distributions
{
    static class StudentDistribution
    {
        private static Dictionary<double, List<DistributionPair>> _distributionTables = new Dictionary<double, List<DistributionPair>> {
            { 0.05, new List<DistributionPair>() {
                new DistributionPair() {FreedomDegree = 1, Value = 12.7},
                new DistributionPair() {FreedomDegree = 2, Value = 4.3},
                new DistributionPair() {FreedomDegree = 3, Value = 3.18},
                new DistributionPair() {FreedomDegree = 4, Value = 2.78},
                new DistributionPair() {FreedomDegree = 5, Value = 2.57},
                new DistributionPair() {FreedomDegree = 6, Value = 2.45},
                new DistributionPair() {FreedomDegree = 7, Value = 2.36},
                new DistributionPair() {FreedomDegree = 8, Value = 2.31},
                new DistributionPair() {FreedomDegree = 9, Value = 2.26},
                new DistributionPair() {FreedomDegree = 10, Value = 2.23},
                new DistributionPair() {FreedomDegree = 11, Value = 2.2},
                new DistributionPair() {FreedomDegree = 12, Value = 2.18},
                new DistributionPair() {FreedomDegree = 13, Value = 2.16},
                new DistributionPair() {FreedomDegree = 14, Value = 2.14},
                new DistributionPair() {FreedomDegree = 15, Value = 2.13},
                new DistributionPair() {FreedomDegree = 16, Value = 2.12},
                new DistributionPair() {FreedomDegree = 17, Value = 2.11},
                new DistributionPair() {FreedomDegree = 18, Value = 2.10},
                new DistributionPair() {FreedomDegree = 19, Value = 2.09},
                new DistributionPair() {FreedomDegree = 20, Value = 2.09},
                new DistributionPair() {FreedomDegree = 21, Value = 2.08},
                new DistributionPair() {FreedomDegree = 22, Value = 2.07},
                new DistributionPair() {FreedomDegree = 23, Value = 2.07},
                new DistributionPair() {FreedomDegree = 24, Value = 2.06},
                new DistributionPair() {FreedomDegree = 25, Value = 2.06},
                new DistributionPair() {FreedomDegree = 26, Value = 2.06},
                new DistributionPair() {FreedomDegree = 27, Value = 2.05},
                new DistributionPair() {FreedomDegree = 28, Value = 2.05},
                new DistributionPair() {FreedomDegree = 29, Value = 2.05},
                new DistributionPair() {FreedomDegree = 30, Value = 2.04},
                new DistributionPair() {FreedomDegree = 40, Value = 2.02},
                new DistributionPair() {FreedomDegree = 60, Value = 2.0},
                new DistributionPair() {FreedomDegree = 120, Value = 1.98},
                new DistributionPair() {FreedomDegree = 180, Value = 1.96},
                new DistributionPair() {FreedomDegree = 240, Value = 1.92},
                new DistributionPair() {FreedomDegree = 480, Value = 1.86},
                new DistributionPair() {FreedomDegree = 960, Value = 1.78},
                new DistributionPair() {FreedomDegree = 1920, Value = 1.65},
                new DistributionPair() {FreedomDegree = 3840, Value = 1.35},
                new DistributionPair() {FreedomDegree = 7680, Value = 1.05},
                new DistributionPair() {FreedomDegree = 15360, Value = 0.4},
                new DistributionPair() {FreedomDegree = 30720, Value = 0.05},
                new DistributionPair() {FreedomDegree = int.MaxValue, Value = 0.025},
            }},

            { 0.001, new List<DistributionPair>() {
                new DistributionPair() {FreedomDegree = 1, Value = 637.0},
                new DistributionPair() {FreedomDegree = 2, Value = 31.6},
                new DistributionPair() {FreedomDegree = 3, Value = 12.9},
                new DistributionPair() {FreedomDegree = 4, Value = 8.61},
                new DistributionPair() {FreedomDegree = 5, Value = 6.86},
                new DistributionPair() {FreedomDegree = 6, Value = 5.96},
                new DistributionPair() {FreedomDegree = 7, Value = 5.4},
                new DistributionPair() {FreedomDegree = 8, Value = 5.04},
                new DistributionPair() {FreedomDegree = 9, Value = 4.78},
                new DistributionPair() {FreedomDegree = 10, Value = 4.59},
                new DistributionPair() {FreedomDegree = 11, Value = 4.44},
                new DistributionPair() {FreedomDegree = 12, Value = 4.32},
                new DistributionPair() {FreedomDegree = 13, Value = 4.22},
                new DistributionPair() {FreedomDegree = 14, Value = 4.14},
                new DistributionPair() {FreedomDegree = 15, Value = 4.07},
                new DistributionPair() {FreedomDegree = 16, Value = 4.01},
                new DistributionPair() {FreedomDegree = 17, Value = 3.95},
                new DistributionPair() {FreedomDegree = 18, Value = 3.92},
                new DistributionPair() {FreedomDegree = 19, Value = 3.88},
                new DistributionPair() {FreedomDegree = 20, Value = 3.85},
                new DistributionPair() {FreedomDegree = 21, Value = 3.82},
                new DistributionPair() {FreedomDegree = 22, Value = 3.79},
                new DistributionPair() {FreedomDegree = 23, Value = 3.77},
                new DistributionPair() {FreedomDegree = 24, Value = 3.74},
                new DistributionPair() {FreedomDegree = 25, Value = 3.72},
                new DistributionPair() {FreedomDegree = 26, Value = 3.71},
                new DistributionPair() {FreedomDegree = 27, Value = 3.69},
                new DistributionPair() {FreedomDegree = 28, Value = 3.66},
                new DistributionPair() {FreedomDegree = 29, Value = 3.66},
                new DistributionPair() {FreedomDegree = 30, Value = 3.65},
                new DistributionPair() {FreedomDegree = 40, Value = 3.55},
                new DistributionPair() {FreedomDegree = 60, Value = 3.46},
                new DistributionPair() {FreedomDegree = 120, Value = 3.37},
                new DistributionPair() {FreedomDegree = 180, Value = 3.28},
                new DistributionPair() {FreedomDegree = 240, Value = 3.2},
                new DistributionPair() {FreedomDegree = 480, Value = 3.05},
                new DistributionPair() {FreedomDegree = 960, Value = 2.81},
                new DistributionPair() {FreedomDegree = 1920, Value = 2.3},
                new DistributionPair() {FreedomDegree = 3840, Value = 1.35},
                new DistributionPair() {FreedomDegree = 7680, Value = 0.5},
                new DistributionPair() {FreedomDegree = 15360, Value = 0.01},
                new DistributionPair() {FreedomDegree = 30720, Value = 0.005},
                new DistributionPair() {FreedomDegree = int.MaxValue, Value = 0.0005},
            }}
        };

        public static double GetCriticalValue(int freedomDegree, double significanceLevel)
        {
            if (!_distributionTables.ContainsKey(significanceLevel))
            {
                throw new Exception($"There is not a distribution table for significance level = {significanceLevel}! Use 0.05, 0.001 instead.");
            }

            var distributionTable = _distributionTables[significanceLevel];

            foreach (var distribution in distributionTable)
            {
                if (freedomDegree <= distribution.FreedomDegree)
                {
                    return distribution.Value;
                }
            }

            throw new Exception($"There is not a value for the freedom degree: {freedomDegree}!");
        }

        private struct DistributionPair
        {
            public int FreedomDegree { get; set; }
            public double Value { get; set; }
        }
    }
}
