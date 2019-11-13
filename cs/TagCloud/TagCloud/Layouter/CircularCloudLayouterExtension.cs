using System;
using System.Drawing;
using System.Linq;

namespace TagCloud.Layouter
{
    public static class CircularCloudLayouterExtension
    {
        public static Rectangle GetInscribedSquare(this CircularCloudLayouter layouter)
        {
            // Abs because our coordinates may be negative
            var width = Math.Min(Math.Abs(layouter.GetMaxXCoord() - layouter.GetMinXCoord()),
                Math.Abs(layouter.GetMaxYCoord() - layouter.GetMinYCoord()));
            var height = width;
            return new Rectangle(layouter.Center.X - width / 2, layouter.Center.Y - height / 2, width, height);
        }

        public static Rectangle GetCircumscribedSquare(this CircularCloudLayouter layouter)
        {
            // Abs because our coordinates may be negative
            var width = Math.Max(Math.Abs(layouter.GetMinXCoord() - layouter.GetMinXCoord()),
                Math.Abs(layouter.GetMaxYCoord() - layouter.GetMinYCoord()));
            var height = width;
            return new Rectangle(layouter.Center.X - width / 2, layouter.Center.Y - height / 2, width, height);
        }

        public static int GetMinXCoord(this CircularCloudLayouter layouter) =>
            layouter.GetAllRectangles().Min(r => r.X);

        public static int GetMinYCoord(this CircularCloudLayouter layouter) =>
            layouter.GetAllRectangles().Min(r => r.Y);

        public static int GetMaxXCoord(this CircularCloudLayouter layouter) =>
            layouter.GetAllRectangles().Max(r => r.X + r.Width);

        public static int GetMaxYCoord(this CircularCloudLayouter layouter) =>
            layouter.GetAllRectangles().Max(r => r.Y + r.Height);
    }
}