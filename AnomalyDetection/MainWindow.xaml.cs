using DiscreteWaveletTransformationAnomalyDetection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnomalyDetectionApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var points = new List<double>();
            var fileName = "sample_500_25_5_3,5.txt";

            using (var streamReader = new StreamReader(fileName))
            {
                var line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    points.Add(double.Parse(line));
                }
            }

            var detector = new DiscreteWaveletTransformationAnomalyDetector();
            detector.ComparisonWindowLeftLimit = 0;
            detector.ComparisonWindowRightLimit = 400;
            detector.DetectionWindowLeftLimit = 400;
            detector.DetectionWindowRightLimit = 500;

            var results = detector.CheckOnAnomaly(points).ToList();

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    OutputLabel.Content = result.Message;
                }
            }
            else
            {
                OutputLabel.Content = "аномалии не обнаружены.";
            }
        }
    }
}
