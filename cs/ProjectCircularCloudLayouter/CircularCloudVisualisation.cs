using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProjectCircularCloudLayouter
{
    public class CircularCloudVisualisation
    {
        private Bitmap _circular_cloud_bitmap;
        private CircularCloudLayouter _cloudLayouter;
        private Random _random;

        public CircularCloudVisualisation(CircularCloudLayouter cloudLayouter)
        {
            _cloudLayouter = cloudLayouter;
            _circular_cloud_bitmap = new Bitmap(_cloudLayouter.CloudRadius * 2, _cloudLayouter.CloudRadius * 2);
            _random = new Random();
        }

        public void MakeImageTagsCircularCloud(string pathToSave, ImageFormat imageFormat)
        {
            if (_cloudLayouter.GetRectangles.Count == 0)
                throw new Exception("IsNotContainsRectanglesForDraw");
            DrawCircularCloud(Graphics.FromImage(_circular_cloud_bitmap));
            SaveBitmap(pathToSave, imageFormat);
        }

        private void DrawCircularCloud(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.White, new Rectangle(new Point(0,0), 
                new Size(_cloudLayouter.CloudRadius * 2, _cloudLayouter.CloudRadius * 2)));
            foreach (var rectangle in _cloudLayouter.GetRectangles)
            {
                var brush = GetNewRandomBrush();
                var currentLocation = rectangle.Location
                                      + new Size(_cloudLayouter.CloudRadius+5, _cloudLayouter.CloudRadius+5);
                graphics.FillRectangle(brush, new Rectangle(currentLocation, rectangle.Size));
            }
        }

        private Brush GetNewRandomBrush() => new SolidBrush(Color.FromArgb(_random.Next(255), 
            _random.Next(255), _random.Next(255)));

        private void SaveBitmap(string path, ImageFormat imageFormat)
        {
            try
            {
                _circular_cloud_bitmap.Save(path, imageFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Failed to save the file");
            }
        }
    }
}