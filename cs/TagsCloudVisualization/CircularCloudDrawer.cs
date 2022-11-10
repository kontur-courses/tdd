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
            var image = CreateBitmap();
            DrawRectangles(image);
            DrawCircleInCenterOfImage(image);
            return image;
        }

        private Bitmap CreateBitmap()
        {
            var imageSize = FindBounds();
            return new Bitmap(imageSize.Width * BoundScale, imageSize.Height * BoundScale);
        }
        
        private Size FindBounds()
        {
            var leftBound = _layouter.Rectangles.Min(rectangle => rectangle.Left);
            var rightBound = _layouter.Rectangles.Max(rectangle => rectangle.Right);
            var bottomBound = _layouter.Rectangles.Max(rectangle => rectangle.Bottom);
            var topBound = _layouter.Rectangles.Min(rectangle => rectangle.Top);
            
            var width = rightBound - leftBound;
            var height = bottomBound - topBound;

            return new Size(width, height);
        }

        private void DrawRectangles(Bitmap image)
        {
            var graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (var rectangle in _layouter.Rectangles)
            {
                _pen.Color = GetRandomColor();
                var offsetRectangle = GetRectangleOffsetToCenter(rectangle, image);
                graphics.DrawRectangle(_pen, offsetRectangle);
            }
        }

        private Rectangle GetRectangleOffsetToCenter(Rectangle rectangle, Bitmap image)
        {
            var rectanglePosition = rectangle.Location - (Size)_layouter.Center + image.Size.Multiply(0.5);
            return new Rectangle(rectanglePosition, rectangle.Size);
        }

        private void DrawCircleInCenterOfImage(Bitmap image)
        {
            var graphics = Graphics.FromImage(image);
            var imageCenter = (Point)image.Size.Multiply(0.5);
            float radius = Math.Max(image.Width, image.Height) / BoundScale;
            _pen.Color = Color.Black;
            graphics.DrawCircle(_pen, imageCenter, radius);
        }

        private Color GetRandomColor()
        {
            var R = 25 + _random.Next() % 215;
            var G = 25 + _random.Next() % 215;
            var B = 25 + _random.Next() % 215;

            return Color.FromArgb( R, G, B);
        }
    }
}