using System;
using System.Drawing;
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
        private CircularCloudLayouter circularCloudLayouter = new(new Point(400, 225));
        private readonly DispatcherTimer timer = new();
        
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Focus();
            timer.Interval = TimeSpan.FromSeconds(.2);
            timer.Start();
        }

        private void DrawRectangle(object? sender, EventArgs e)
        {
            customColor = GetRandomColor();
            
            var rectangleFromCloud =
                circularCloudLayouter.PutNextRectangle(new Size(random.Next(25, 50), random.Next(25, 50)));
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
            if (string.CompareOrdinal((string?) StartButton.Content, "Start") == 0)
            {
                StartButton.Content = "Stop";
                timer.Tick += DrawRectangle;
            }
            else
            {
                 StartButton.Content = "Start";
                 timer.Tick -= DrawRectangle;               
            }
        }
    }
}