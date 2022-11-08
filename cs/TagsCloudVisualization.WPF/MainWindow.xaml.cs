using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace TagsCloudVisualization.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random random = new();
        private Brush customColor = Brushes.Beige;
        private readonly DispatcherTimer timer = new();

        private CircularCloudLayouter circularCloud;
        
        public MainWindow()
        {
            InitializeComponent();
            UpdateCircularCloudFromTextBox();
            MyCanvas.Focus();
            timer.Interval = TimeSpan.FromSeconds(.2);
            timer.Start();
        }

        private void DrawRectangle(object? sender, EventArgs e)
        {
            customColor = GetRandomColor();
            
            var rectangleFromCloud =
                circularCloud.PutNextRectangle(new Size(random.Next(25, 50), random.Next(25, 50)));
            var canvasRect = new System.Windows.Shapes.Rectangle
            {
                Width = rectangleFromCloud.Width,
                Height = rectangleFromCloud.Height,
                Fill = customColor,
                StrokeThickness = 2,
                Stroke = Brushes.LightBlue,
            };

            Canvas.SetLeft(canvasRect, rectangleFromCloud.X);
            Canvas.SetTop(canvasRect, rectangleFromCloud.Y);

            MyCanvas.Children.Add(canvasRect);
        }

        private SolidColorBrush GetRandomColor() => new(Color.FromRgb((byte) random.Next(1, 255),
            (byte) random.Next(1, 255),
            (byte) random.Next(1, 255)));

        private void Start(object sender, RoutedEventArgs e)
        {
            if (string.CompareOrdinal((string?) StartButton.Header, "Start") == 0)
            {
                UpdateCircularCloudFromTextBox();
                StartButton.Header = "Stop";
                timer.Tick += DrawRectangle;
            }
            else
            {
                 StartButton.Header = "Start";
                 timer.Tick -= DrawRectangle;
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            UpdateCircularCloudFromTextBox();
            MyCanvas.Children.Clear();
        }

        private void UpdateCircularCloudFromTextBox()
        {
            var isNumber = int.TryParse(TbSteps.Text, out var steps);
            if (!isNumber)
                steps = 1;
            
            circularCloud =
                new CircularCloudLayouter(new Point((int) (MyWindow.Width / 2),
                    (int) (MyWindow.Height / 2)), steps);
        }

        private void UpdateInterval(object sender, TextChangedEventArgs e)
        {
            var isNumber = double.TryParse(TbSpeed.Text, out var speed);
            if (!isNumber)
                speed = 0.2;
            
            timer.Interval = TimeSpan.FromSeconds(speed);
            MyCanvas.Children.Clear();
        }

        private void StepSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StepSlider is not null && TbSteps is not null)
                TbSteps.Text = StepSlider.Value.ToString(CultureInfo.InvariantCulture).Split('.')[0];
        }
    }
}