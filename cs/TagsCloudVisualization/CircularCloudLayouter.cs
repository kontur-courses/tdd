using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter 
    {
        public List<RectangleF> Rectangles { get; set; }
        public readonly PointF Center;
        private readonly double offsetPoint;
        private readonly double spiralStep;
        private int lastNumberPoint;
        public bool IsOffsetToCenter { get; set; }

        public CircularCloudLayouter(PointF center, bool isOffsetToCenter, double offsetPoint, double spiralStep)
        {
            if (center.X < 0 || center.Y < 0) throw new ArgumentException();
            this.spiralStep = spiralStep;
            this.offsetPoint = offsetPoint;
            Center = center;
            IsOffsetToCenter = isOffsetToCenter;
            Rectangles = new List<RectangleF>();
            lastNumberPoint = 0;
        }

        public CircularCloudLayouter(PointF center) : this(center, false, 0.01, -0.3)
        {
        }

        public CircularCloudLayouter(PointF center, bool isOffsetToCenter) : this(center, isOffsetToCenter, 0.01, -0.3)
        {
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            RectangleF rect;
            for (; ; lastNumberPoint++)
            {
                var phi = lastNumberPoint * spiralStep;
                var r = offsetPoint * lastNumberPoint;
                var x = (int)(r * Math.Cos(phi)) + Center.X;
                var y = (int)(r * Math.Sin(phi)) + Center.Y;
                var point = new PointF(x - rectangleSize.Width / 2, y - rectangleSize.Height / 2);
                rect = new RectangleF(point, rectangleSize);
                if (!rect.AreIntersected(Rectangles))
                {
                    if (IsOffsetToCenter) rect = OffsetToCenter(rect);
                    break;
                }
            }
            Rectangles.Add(rect);
            return rect;
        }

        private RectangleF OffsetToCenter(RectangleF rect)
        {
            var point = rect.Location;
            while (rect.CanBeShiftedToPointX(Center))
            {
                var newX = ((rect.Center().X < Center.X) ? 1 : -1) + point.X;
                var pointNew = new PointF(newX, point.Y);
                var rectNew = new RectangleF(pointNew, rect.Size);
                if (rectNew.AreIntersected(Rectangles)) break;
                point = pointNew;
                rect = rectNew;
            }
            while (rect.CanBeShiftedToPointY(Center))
            {
                var newY = ((rect.Center().Y < Center.Y) ? 1 : -1) + point.Y;
                var pointNew = new PointF(point.X, newY);
                var rectNew = new RectangleF(pointNew, rect.Size);
                if (rectNew.AreIntersected(Rectangles)) break;
                point = pointNew;
                rect = rectNew;
            }
            return rect;
        }

        public void SaveBitmap(string btmName)
        {
            var bmp = new Bitmap(800, 500);
            using Graphics gph = Graphics.FromImage(bmp);
            var blackPen = new Pen(Color.Black, 1);
            gph.DrawRectangles(blackPen, Rectangles.ToArray());
            bmp.Save(btmName + ".bmp");
        }
    }
}
