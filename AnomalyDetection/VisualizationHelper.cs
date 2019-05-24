using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnomalyDetectionApplication
{
    /// <summary>
    /// Класс для работы с визуализацией, в частности - с отрисовкой точек.
    /// </summary>
    static class VisualizationHelper
    {
        public static double CalculateCoordinate(double currentMin, double currentMax, double realMin, double realMax, double current)
        {
            var positionCoefficient = Math.Abs(current - currentMin) / (currentMax - currentMin);
            return positionCoefficient * (realMax - realMin) + realMin;
        }

        public static void DrawPoint(Canvas canvas, double x, double y, double radius, Brush color, string tooltip)
        {
            var point = new Point(x, canvas.Height - y);

            var elipse = new Ellipse();
            elipse.StrokeThickness = 1;
            elipse.Stroke = Brushes.DarkSlateGray;
            elipse.Width = 2 * radius;
            elipse.Height = 2 * radius;
            elipse.Margin = new Thickness(point.X - radius, point.Y - radius, 0, 0);
            elipse.Fill = color;

            if (!string.IsNullOrEmpty(tooltip))
            {
                elipse.ToolTip = tooltip;
            }

            canvas.Children.Add(elipse);
        }

        public static int CalculateElipseRadius(int elipseTotalAmount)
        {
            var elipseRadius = (int)Math.Floor(10000 * 0.75 / elipseTotalAmount);

            if (elipseRadius > 3)
            {
                elipseRadius = 3;
            }
            else if (elipseRadius < 1)
            {
                elipseRadius = 1;
            }

            return elipseRadius;
        }
    }
}