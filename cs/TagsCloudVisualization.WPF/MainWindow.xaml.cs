using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private const double DefaultDpi = 96.0;

        private CircularCloudLayouter? circularCloud;
        
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
                speed = 0.1;
            
            timer.Interval = TimeSpan.FromSeconds(speed);
            MyCanvas.Children.Clear();
        }

        private void StepSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StepSlider is not null && TbSteps is not null)
                TbSteps.Text = StepSlider.Value.ToString(CultureInfo.InvariantCulture).Split('.')[0];
        }

        private void SavePicture(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Image", 
                DefaultExt = ".png",
                Filter = "PNG File (.png)|*.png"
            };

            var result = dlg.ShowDialog();

            if (result != true) 
                return;
            var filename = dlg.FileName;
            SaveCanvasToFile(this, MyCanvas, DefaultDpi, filename);
        }

        private static void SaveCanvasToFile(FrameworkElement window, UIElement canvas, double dpi, string filename)
        {
            var size = new System.Windows.Size(window.Width, window.Height);
            canvas.Measure(size);
 
            var rtb = new RenderTargetBitmap(
                (int)window.Width,
                (int)window.Height,
                dpi,
                dpi,
                PixelFormats.Pbgra32
            );
            rtb.Render(canvas);
 
            SaveRtbAsPngbmp(rtb, filename);
        }
 
        private static void SaveRtbAsPngbmp(BitmapSource bmp, string filename)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using var stm = File.Create(filename);
            enc.Save(stm);
        }
    }
}