using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly string path;
        private readonly string samplesDirectory = "Samples";
        private readonly SpiralPoints spiralPoints;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private int right;
        private int bottom;

        public CircularCloudLayouter(Point center)
        {
            spiralPoints = new SpiralPoints(center);
            var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (directoryInfo == null) return;
            path = $"{directoryInfo.FullName}\\{samplesDirectory}";
            if(!Directory.Exists(path))
                directoryInfo.CreateSubdirectory(samplesDirectory);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            CheckRectangleBorders(rectangle);
            return rectangle;
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(path)) return null;
            var penWidth = 5;
            var pen = new Pen(Color.Purple, penWidth);
            var bitmap = new Bitmap(right + penWidth, bottom + penWidth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in rectangles)
            {
                graphics.DrawRectangle(pen, rectangle);
            }

            graphics.Save();
            var imagePath = $"{path}\\{rectangles.Count}rectangles.jpg";
            bitmap.Save(imagePath);
            return imagePath;
        }
        
        private bool HaveIntersection(Rectangle rectangle) => rectangles.Any(other => other.IntersectsWith(rectangle));

        private void CheckRectangleBorders(Rectangle rectangle)
        {
            if (rectangle.Bottom > bottom)
                bottom = rectangle.Bottom;
            if (rectangle.Right > right)
                right = rectangle.Right;
        }
        
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var halfSize = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);
            var points = spiralPoints.GetSpiralPoints().GetEnumerator();
            points.MoveNext();
            var rectangle = new Rectangle(points.Current - halfSize, rectangleSize); 
            while (HaveIntersection(rectangle) || rectangle.Left < 0 || rectangle.Top < 0)
            {
                points.MoveNext();
                rectangle.Location = points.Current - halfSize;
            }
            points.Dispose();
            return rectangle;
        }
    }
}