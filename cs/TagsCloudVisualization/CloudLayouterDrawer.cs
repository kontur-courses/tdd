using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudLayouterDrawer
    {
        private readonly Random random;
        public int Margin { get; private set; }


        public CloudLayouterDrawer(int margin)
        {
            this.Margin = margin;
            random = new Random();
        }

        public void DrawCloud(string filename, IEnumerable<Rectangle> rectangles)
        {
            
            if (!rectangles.Any() || string.IsNullOrEmpty(filename))
                throw new ArgumentException();

            int imageWidth,imageHeight;
            SetImageSize(rectangles, out imageWidth, out imageHeight);

            var newRectanglesPositions = GetNewRectanglesPositions(rectangles,imageWidth,imageHeight);

            using (var bitmap = new Bitmap(imageWidth, imageHeight))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    foreach (var rectangle in newRectanglesPositions)
                    {
                        var color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                        var brush = new SolidBrush(color);
                        graphics.FillRectangle(brush, rectangle);
                    }
                    SaveImageToFile(bitmap,filename);
                }
            }
        }


        private void SaveImageToFile(Bitmap bitmap,string filename)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var filePath = Path.Combine(projectDirectory, "images", filename);
            bitmap.Save(filePath, ImageFormat.Png);
        }
        private void SetImageSize(IEnumerable<Rectangle> rectangles,out int imageWidth,out int imageHeight)
        {
            var maxX = rectangles.Select(rec => rec.X + rec.Width / 2).Max();
            var minX = rectangles.Select(rec => rec.X - rec.Width / 2).Min();
            var maxY = rectangles.Select(rec => rec.Y + rec.Height / 2).Max();
            var minY = rectangles.Select(rec => rec.Y - rec.Height / 2).Min();

            imageWidth = maxX - minX + Margin;
            imageHeight = maxY - minY + Margin;
        }

        private IEnumerable<Rectangle> GetNewRectanglesPositions(IEnumerable<Rectangle> rectangles,int imageWidth, int imageHeight)
        {
            return rectangles.Select(rec =>
                new Rectangle(rec.X + imageWidth / 2, rec.Y + imageHeight / 2, rec.Width, rec.Height));
        }
    }
}