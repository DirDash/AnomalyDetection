using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WaveletAnomalyDetection;

namespace AnomalyDetectionApplication
{
    public partial class MainWindow : Window
    {
        private DetectionEngine _detectionEngine = new DetectionEngine();

        private string _filename;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void HandleDataLoaded()
        {
            _detectionEngine.ClearResults();

            WaveletComboBox.Items.Clear();
            foreach (var wavelet in _detectionEngine.AllWavelets)
            {
                WaveletComboBox.Items.Add(wavelet);
            }
            WaveletComboBox.SelectedIndex = 1;

            _detectionEngine.ComparisonWindowStart = 0;
            _detectionEngine.ComparisonWindowEnd = _detectionEngine.Data.Count;
        
            _detectionEngine.DetectionWindowStart = 3 * (_detectionEngine.Data.Count / 4);
            _detectionEngine.DetectionWindowEnd = _detectionEngine.Data.Count;

            MessageTextBox.Clear();

            FirstCriterionValueLabel.Content = "0";
            FirstCriterionLimitLabel.Content = "0";
            SecondCriterionValueLabel.Content = "0";
            SecondCriterionLimitLabel.Content = "0";
            ThirdCriterionValueLabel.Content = "0";
            ThirdCriterionLimitLabel.Content = "0";

            WaveletComboBox.IsEnabled = true;

            SensivityTextBox.IsEnabled = true;

            ComparisonWindowStartTextBox.IsEnabled = true;
            ComparisonWindowEndTextBox.IsEnabled = true;

            DetectionWindowStartTextBox.IsEnabled = true;
            DetectionWindowEndTextBox.IsEnabled = true;

            DetectAnomalyButton.IsEnabled = true;
            SaveToFileButton.IsEnabled = false;
        }

        private void VisualizeData()
        {
            var elipseRadius = VisualizationHelper.CalculateElipseRadius(_detectionEngine.Data.Count);

            VisualizationCanvas.Children.Clear();

            for (var i = 0; i < _detectionEngine.Data.Count; i++)
            {
                var x = VisualizationHelper.CalculateCoordinate(0, _detectionEngine.Data.Count - 1, elipseRadius, VisualizationCanvas.Width - elipseRadius, i);
                var y = VisualizationHelper.CalculateCoordinate(_detectionEngine.DataMinValue, _detectionEngine.DataMaxValue, elipseRadius, VisualizationCanvas.Height - elipseRadius, _detectionEngine.Data[i]);

                VisualizationHelper.DrawPoint(VisualizationCanvas, x, y, elipseRadius, Brushes.DarkGray, _detectionEngine.Data[i].ToString());
            }
        }

        private void LoadFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.DefaultExt = ".txt";
                fileDialog.Filter = "Text documents (.txt)|*.txt";
                
                if (fileDialog.ShowDialog() == true)
                {
                    SetStatus("Загрузка данных...");
                    _filename = Path.GetFileName(fileDialog.FileName);

                    _detectionEngine.LoadDataFromFile(fileDialog.FileName);

                    HandleDataLoaded();

                    UpdateInterface();

                    SetStatus("Визуализация...");

                    VisualizeData();

                    SetStatus("");
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка при попытке загрузки файла.{Environment.NewLine}Убедитесь, что файл имеет корректный формат.{Environment.NewLine}Подробнее: https://github.com/DirDash/AnomalyDetection");
            }
            SetStatus("");
        }

        private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new SaveFileDialog();
                fileDialog.DefaultExt = ".txt";
                fileDialog.Filter = "Text documents (.txt)|*.txt";
                fileDialog.FileName = $"{Path.GetFileNameWithoutExtension(_filename)}_anomaly_detection_results.txt";
                
                if (fileDialog.ShowDialog() == true)
                {
                    SetStatus("Сохранение результатов...");

                    _detectionEngine.SaveDataToFile(fileDialog.FileName);
                    
                    MessageBox.Show("Сохрание успешно завершено");

                    SetStatus("");
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка при попытке сохранения файла.{Environment.NewLine}Повторите попытку или перезагрузите приложение.");
            }
            SetStatus("");
        }

        private void DetectAnomalyButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Поиск аномалий...");

            try
            {
                _detectionEngine.CheckOnAnomaly();
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка в процессе поиска аномалий.{Environment.NewLine}Повторите попытку или перезагрузите приложение.");
            }

            UpdateInterface();
            SetStatus("");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateInterface()
        {
            if (!IsInitialized)
            {
                return;
            }

            MaxTimeLabel.Content = _detectionEngine.Data.Count;
            MinValueLabel.Content = _detectionEngine.DataMinValue;
            MaxValueLabel.Content = _detectionEngine.DataMaxValue;

            ComparisonWindowStartTextBox.Text = _detectionEngine.ComparisonWindowStart.ToString();
            ComparisonWindowEndTextBox.Text = _detectionEngine.ComparisonWindowEnd.ToString();

            DetectionWindowStartTextBox.Text = _detectionEngine.DetectionWindowStart.ToString();
            DetectionWindowEndTextBox.Text = _detectionEngine.DetectionWindowEnd.ToString();

            if (string.IsNullOrEmpty(_filename))
            {
                FilenameLabel.Content = "файл не выбран";
            }
            else
            {
                FilenameLabel.Content = _filename;
            }

            if (_detectionEngine.AnomalyDetectionResults != null && _detectionEngine.AnomalyDetectionResults.Count > 0)
            {
                SaveToFileButton.IsEnabled = true;
            }

            if (_detectionEngine.Data != null && _detectionEngine.Data.Count > 0)
            {
                var comparisonWindowStart = VisualizationHelper.CalculateCoordinate(0, _detectionEngine.Data.Count, 0, VisualizationCanvas.Width, _detectionEngine.ComparisonWindowStart);
                var comparisonWindowEnd = VisualizationHelper.CalculateCoordinate(0, _detectionEngine.Data.Count, 0, VisualizationCanvas.Width, _detectionEngine.ComparisonWindowEnd);
                ComparisonWindowGrid.Margin = new Thickness(comparisonWindowStart, 0, 0, 0);
                ComparisonWindowGrid.Width = comparisonWindowEnd - comparisonWindowStart;

                var detectionWindowStart = VisualizationHelper.CalculateCoordinate(0, _detectionEngine.Data.Count, 0, VisualizationCanvas.Width, _detectionEngine.DetectionWindowStart);
                var detectionWindowEnd = VisualizationHelper.CalculateCoordinate(0, _detectionEngine.Data.Count, 0, VisualizationCanvas.Width, _detectionEngine.DetectionWindowEnd);
                DetectionWindowGrid.Margin = new Thickness(detectionWindowStart, 0, 0, 0);
                DetectionWindowGrid.Width = detectionWindowEnd - detectionWindowStart;
            }

            if (_detectionEngine.AnomalyDetectionResults != null && _detectionEngine.AnomalyDetectionResults.Count > 0)
            {
                FirstCriterionLabel.Content = _detectionEngine.AnomalyDetectionResults[0].Source;
                FirstCriterionValueLabel.Content = _detectionEngine.AnomalyDetectionResults[0].StatisticsValue;
                FirstCriterionLimitLabel.Content = _detectionEngine.AnomalyDetectionResults[0].StatisticsLimit;
                switch (_detectionEngine.AnomalyDetectionResults[0].Type)
                {
                    case AnomalyDetectionResultType.Anomaly:
                        FirstCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(239, 154, 154));
                        break;
                    case AnomalyDetectionResultType.HighProbabilityOfAnomaly:
                        FirstCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(255, 204, 128));
                        break;
                    default:
                        FirstCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));
                        break;
                }

                SecondCriterionLabel.Content = _detectionEngine.AnomalyDetectionResults[1].Source;
                SecondCriterionValueLabel.Content = _detectionEngine.AnomalyDetectionResults[1].StatisticsValue;
                SecondCriterionLimitLabel.Content = _detectionEngine.AnomalyDetectionResults[1].StatisticsLimit;
                switch (_detectionEngine.AnomalyDetectionResults[1].Type)
                {
                    case AnomalyDetectionResultType.Anomaly:
                        SecondCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(239, 154, 154));
                        break;
                    case AnomalyDetectionResultType.HighProbabilityOfAnomaly:
                        SecondCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(255, 204, 128));
                        break;
                    default:
                        SecondCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));
                        break;
                }

                ThirdCriterionLabel.Content = _detectionEngine.AnomalyDetectionResults[2].Source;
                ThirdCriterionValueLabel.Content = _detectionEngine.AnomalyDetectionResults[2].StatisticsValue;
                ThirdCriterionLimitLabel.Content = _detectionEngine.AnomalyDetectionResults[2].StatisticsLimit;
                switch (_detectionEngine.AnomalyDetectionResults[2].Type)
                {
                    case AnomalyDetectionResultType.Anomaly:
                        ThirdCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(239, 154, 154));
                        break;
                    case AnomalyDetectionResultType.HighProbabilityOfAnomaly:
                        ThirdCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(255, 204, 128));
                        break;
                    default:
                        ThirdCriterionGrid.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));
                        break;
                }

                var noAnomalyDetetcted = true;
                MessageTextBox.Clear();
                foreach (var result in _detectionEngine.AnomalyDetectionResults)
                {
                    if (result.Type != AnomalyDetectionResultType.Normal)
                    {
                        noAnomalyDetetcted = false;

                        MessageTextBox.Text += $"({result.Source}) {result.Message}.";
                        MessageTextBox.Text += Environment.NewLine;
                        MessageTextBox.Text += Environment.NewLine;
                    }
                }

                if (noAnomalyDetetcted)
                {
                    MessageTextBox.Text += "Аномалии не обнаружены.";
                }
            }
        }

        private void SetStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                StatusLbl.Content = "";
                StatusLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                StatusLbl.Content = status.ToUpper();
                StatusLbl.Visibility = Visibility.Visible;
                AllowInterfaceToUpdate();
            }
        }

        private void AllowInterfaceToUpdate()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                return null;
            }), null);

            Dispatcher.PushFrame(frame);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        private void WaveletComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _detectionEngine.ChosenWavelet = (IWavelet)WaveletComboBox.SelectedItem;
        }

        private void SensivityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(SensivityTextBox.Text, out var value))
            {
                if (value < _detectionEngine.MinSensivity)
                {
                    value = _detectionEngine.MinSensivity;
                    SensivityTextBox.Text = value.ToString();
                }
                _detectionEngine.Sensivity = value;
            }
        }

        private void ComparisonWindowStartTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ComparisonWindowStartTextBox.Text, out int content))
            {
                _detectionEngine.ComparisonWindowStart = content;
            }
            else
            {
                ComparisonWindowStartTextBox.Text = content.ToString();
            }

            UpdateInterface();
        }

        private void ComparisonWindowEndTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ComparisonWindowEndTextBox.Text, out int content))
            {
                _detectionEngine.ComparisonWindowEnd = content;
            }
            else
            {
                ComparisonWindowEndTextBox.Text = content.ToString();
            }

            UpdateInterface();
        }

        private void DetectionWindowStartTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(DetectionWindowStartTextBox.Text, out int content))
            {
                _detectionEngine.DetectionWindowStart = content;
            }
            else
            {
                DetectionWindowStartTextBox.Text = content.ToString();
            }

            UpdateInterface();
        }

        private void DetectionWindowEndTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(DetectionWindowEndTextBox.Text, out int content))
            {
                _detectionEngine.DetectionWindowEnd= content;
            }
            else
            {
                DetectionWindowEndTextBox.Text = content.ToString();
            }

            UpdateInterface();
        }        
    }
} 