using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProjectCircularCloudLayouter
{
    public static class CircularCloudVisualisation
    {
        public static void MakeImageTagsCircularCloud(this CircularCloudLayouter circularCloudLayouter,
            string pathToSave, ImageFormat imageFormat)
        {
            var bitmap = new Bitmap(circularCloudLayouter.CloudRadius * 2, circularCloudLayouter.CloudRadius * 2);
            if (circularCloudLayouter.GetRectangles.Count == 0)
                throw new Exception("IsNotContainsRectanglesForDraw");
            DrawCircularCloud(Graphics.FromImage(bitmap), circularCloudLayouter);
            SaveBitmap(pathToSave, imageFormat, bitmap);
        }

        private static void DrawCircularCloud(Graphics graphics, CircularCloudLayouter layouter)
        {
            graphics.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0),
                new Size(layouter.CloudRadius * 2, layouter.CloudRadius * 2)));
            foreach (var rectangle in layouter.GetRectangles)
            {
                var brush = GetNewRandomBrush();
                var currentLocation = rectangle.Location
                                      + new Size(layouter.CloudRadius + 5, layouter.CloudRadius + 5);
                graphics.FillRectangle(brush, new Rectangle(currentLocation, rectangle.Size));
            }

            graphics.Flush();
        }

        private static Brush GetNewRandomBrush()
        {
            var random = new Random();
            return new SolidBrush(Color.FromArgb(random.Next(255),
                random.Next(255), random.Next(255)));
        }

        private static void SaveBitmap(string path, ImageFormat imageFormat, Bitmap circularCloudBitmap)
        {
            try
            {
                circularCloudBitmap.Save(path, imageFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Failed to save the file");
            }
        }
    }
}