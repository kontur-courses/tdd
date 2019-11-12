using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public static class TestsHelper
    {
        public static List<Rectangle> PutRandomRectanglesUsingLayouter(int rectanglesCount,
            CircularCloudLayouter layouter, Size minSize, Size maxSize)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < rectanglesCount; ++i)
                rectangles.Add(layouter.PutNextRectangle(
                    CreateRandomSize(minSize, maxSize)
                ));
            return rectangles;
        }

        public static Size CreateRandomSize(Size minSize, Size maxSize)
        {
            var random = new Random();
            return new Size(random.Next(minSize.Width, maxSize.Width), random.Next(minSize.Height, maxSize.Height));
        }

        public static int GetRadiusOfCircleIncludingAllRectangles(IReadOnlyCollection<Rectangle> rectangles,
            Point center)
        {
            var (left, right, top, bottom) = GetEdgesOfRectanglesSet(rectangles);

            var radius = 0;

            if (Math.Abs(left - center.X) > radius)
                radius = Math.Abs(left - center.X);
            if (Math.Abs(right - center.X) > radius)
                radius = Math.Abs(right - center.X);
            if (Math.Abs(top - center.Y) > radius)
                radius = Math.Abs(top - center.Y);
            if (Math.Abs(bottom - center.Y) > radius)
                radius = Math.Abs(bottom - center.Y);

            return radius;
        }

        public static bool IsRectangleInCircle(Rectangle rectangle, Point circleCenter, int circleRadius)
        {
            return IsPointInCircle(new Point(rectangle.Left, rectangle.Top), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Left, rectangle.Bottom), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Right, rectangle.Top), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Right, rectangle.Bottom), circleCenter, circleRadius);
        }

        public static void SaveFailedTagsCloudAndNotify(string imageName, TagsCloudImage tagsCloudImage)
        {
            var fileName = imageName + "failed.png";
            var exactPath = Path.GetFullPath(fileName);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Tag cloud visualization saved to file {0}", exactPath);
        }

        private static bool IsPointInCircle(Point point, Point circleCenter, int circleRadius)
        {
            var pointXRelative = point.X - circleCenter.X;
            var pointYRelative = point.Y - circleCenter.Y;
            return pointXRelative * pointXRelative + pointYRelative * pointYRelative <= circleRadius * circleRadius;
        }

        private static (int left, int right, int top, int bottom) GetEdgesOfRectanglesSet(
            IReadOnlyCollection<Rectangle> rectangles)
        {
            var left = rectangles.Aggregate(0,
                (leftmost, rectangle) => rectangle.Left < leftmost ? rectangle.Left : leftmost);
            var right = rectangles.Aggregate(0,
                (rightmost, rectangle) => rectangle.Right > rightmost ? rectangle.Right : rightmost);

            var top = rectangles.Aggregate(0,
                (topmost, rectangle) => rectangle.Top < topmost ? rectangle.Top : topmost);
            var bottom = rectangles.Aggregate(0,
                (bottommost, rectangle) => rectangle.Bottom > bottommost ? rectangle.Bottom : bottommost);

            return (left, right, top, bottom);
        }
    }
}