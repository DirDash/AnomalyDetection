using System.Collections.Generic;
using System.Linq;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    class Window
    {
        public int LeftLimit { get; set; }
        public int RightLimit { get; set; }

        public List<double> Points
        {
            get
            {
                var pointsInWindow = new List<double>();

                for (int i = LeftLimit; i < RightLimit; i++)
                {
                    pointsInWindow.Add(_allPoints[i]);
                }

                return pointsInWindow;
            }
        }

        private double[] _allPoints;

        public Window(IEnumerable<double> allPoints, int leftLimit, int rightLimit)
        {
            _allPoints = allPoints.ToArray();
            LeftLimit = leftLimit;
            RightLimit = rightLimit;
        }
    }
}