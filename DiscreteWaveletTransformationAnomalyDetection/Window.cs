using System.Collections.Generic;
using System.Linq;

namespace DiscreteWaveletTransformationAnomalyDetection
{
    class Window
    {
        public int Start { get; set; }
        public int End { get; set; }

        public List<double> Points
        {
            get
            {
                var pointsInWindow = new List<double>();

                for (int i = Start; i < End; i++)
                {
                    pointsInWindow.Add(_allPoints[i]);
                }

                return pointsInWindow;
            }
        }

        private double[] _allPoints;

        public Window(IEnumerable<double> allPoints, int start, int end)
        {
            _allPoints = allPoints.ToArray();
            Start = start;
            End = end;
        }
    }
}