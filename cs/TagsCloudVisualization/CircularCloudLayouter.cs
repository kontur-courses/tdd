using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<RectangleF> rectangles;
        public List<RectangleF> Rectangles { get => rectangles; }
        private PointF center;
        private Spiral spiral;

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            rectangles = new List<RectangleF>();
            spiral = new Spiral(center);
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size should be valid: " +
                    "width and height must be positive");
            var rectangle = GetRectangle(rectangleSize, center);
            var positionedRectangle = FindPositionToRectangle(rectangle);
            rectangles.Add(positionedRectangle);
        }

        public void Visualise(string filename)
        {
            var bitmap = new Bitmap((int)(center.X * 2), (int)(center.Y * 2));
            var gr = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.DarkGreen, 1);
            gr.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save(filename);
        }

        private RectangleF GetRectangle(SizeF rectSize, PointF center)
            => RectangleFExtensions.GetRectangleByCenter(rectSize, center);

        private RectangleF GetShiftedRectangle(SizeF rectSize,
            PointF center, Vector2 offset)
            => GetRectangle(rectSize, new PointF
                (center.X + offset.X, center.Y + offset.Y));

        private RectangleF FindPositionToRectangle(RectangleF rect)
        {
            while (IsRectangleIntersectedByAnother(rect))
            {
                var p = spiral.GetNextPointOnSpiral();
                rect = GetRectangle(rect.Size, p);
            }
            return ShiftRectangleToCenter(rect);
        }

        private RectangleF ShiftRectangleToCenter(RectangleF rect)
        {
            var shifted = new RectangleF();
            var rectCenter = rect.GetCenter();
            var normal = GetNormalToCenter(rectCenter);
            if (float.IsNaN(normal.X) || float.IsNaN(normal.Y))
                return rect;
            var k = 1;
            while (!IsRectangleIntersectedByAnother(rect))
            {
                shifted = rect;
                rect = GetShiftedRectangle(rect.Size, rectCenter, normal * k);
                k += 1;
            }
            return shifted;
        }

        

        private Vector2 GetNormalToCenter(PointF point)
        {
            var direction = new Vector2(center.X - point.X,
                center.Y - point.Y);
            var length = direction.Length();
            return new Vector2(direction.X / length,
                direction.Y / length);
        }

        private bool IsRectangleIntersectedByAnother(RectangleF rect)
            => rectangles.Where(r => r.IntersectsWith(rect))
                .Any();
    }
}
