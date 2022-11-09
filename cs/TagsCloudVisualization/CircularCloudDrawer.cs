using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudDrawer
    {
        private readonly CircularCloudLayouter _layouter;
        private readonly Pen _pen = new Pen(Color.Red, 15);
        private readonly Random _random = new Random(1);
        private const int BoundScale = 3;
        public CircularCloudDrawer(CircularCloudLayouter layouter)
        {
            _layouter = layouter;
        }
        
        public Bitmap CreateImage()
        {
            Bitmap image = CreateBitmap();
            DrawRectangles(image);
            DrawCircleInCenterOfImage(image);
            return image;
        }

        private Bitmap CreateBitmap()
        {
            Size imageSize = FindBounds();
            return new Bitmap(imageSize.Width * BoundScale, imageSize.Height * BoundScale);
        }
        
        private Size FindBounds()
        {
            int leftBound = _layouter.Rectangles.Min(rectangle => rectangle.Left);
            int rightBound = _layouter.Rectangles.Max(rectangle => rectangle.Right);
            int bottomBound = _layouter.Rectangles.Max(rectangle => rectangle.Bottom);
            int topBound = _layouter.Rectangles.Min(rectangle => rectangle.Top);
            
            int width = rightBound - leftBound;
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

        private void DrawCircleInCenterOfImage(Bitmap image)
        {
            Graphics graphics = Graphics.FromImage(image);
            Point imageCenter = (Point)image.Size.Multiply(0.5);
            float radius = Math.Max(image.Width, image.Height) / BoundScale;
            _pen.Color = Color.Black;
            graphics.DrawCircle(_pen, imageCenter, radius);
        }

        private Color GetRandomColor()
        {
            int R = 25 + _random.Next() % 215;
            int G = 25 + _random.Next() % 215;
            int B = 25 + _random.Next() % 215;

            return Color.FromArgb( R, G, B);
        }
    }
}