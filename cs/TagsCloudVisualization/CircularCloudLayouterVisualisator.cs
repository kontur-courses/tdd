using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterVisualisator
    {
        private readonly CircularCloudLayouter _layouter;
        private readonly Pen _pen = new Pen(Color.Red, 15);
        private readonly Random _random = new Random(1);
        public CircularCloudLayouterVisualisator(CircularCloudLayouter layouter)
        {
            _layouter = layouter;
        }
        
        public Bitmap CreateImage()
        {
            Size imageSize = FindBounds();
            Bitmap image = new Bitmap(imageSize.Width * 3, imageSize.Height * 3);
            DrawRectangles(image);
            DrawCircleAroundRectangles(image);
            return image;
        }
        
        private Size FindBounds()
        {
            int leftBound = _layouter.Rectangles.Min(rectangle => rectangle.Left);
            int rightBound = _layouter.Rectangles.Max(rectangle => rectangle.Right);
            int width = rightBound - leftBound;
            
            int bottomBound = _layouter.Rectangles.Max(rectangle => rectangle.Bottom);
            int topBound = _layouter.Rectangles.Min(rectangle => rectangle.Top);
            int height = bottomBound - topBound;

            return new Size(width, height);
        }

        private void DrawRectangles(Bitmap image)
        {
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (Rectangle rectangle in _layouter.Rectangles)
            {
                _pen.Color = GetRandomColor();
                Rectangle offsetRectangle = GetRectangleOffsetToCenter(rectangle, image);
                graphics.DrawRectangle(_pen, offsetRectangle);
            }
        }

        private Rectangle GetRectangleOffsetToCenter(Rectangle rectangle, Bitmap image)
        {
            Point rectanglePosition = rectangle.Location - (Size)_layouter.Center + image.Size.Multiply(0.5);
            return new Rectangle(rectanglePosition, rectangle.Size);
        }

        private void DrawCircleAroundRectangles(Bitmap image)
        {
            Graphics graphics = Graphics.FromImage(image);
            Point imageCenter = (Point)image.Size.Multiply(0.5);
            float radius = Math.Max(image.Width, image.Height) * 0.25f;
            _pen.Color = Color.Black;
            graphics.DrawCircle(_pen, imageCenter, radius);
        }

        private Color GetRandomColor()
        {
            int R = _random.Next() % 255;
            int G = _random.Next() % 255;
            int B = _random.Next() % 255;
            return Color.FromArgb( R, G, B);
        }

        public void Save(Bitmap image, string filename="image.png")
        {
            image.Save(filename, ImageFormat.Png);
        }
    }
}