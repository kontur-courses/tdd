using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HomeExerciseTDD
{
    class Program
    {
        class Helper
        {
            public Size GetRandomSize(int leftBorder, int rightBorder)
            {
                var randomazer = new Random();
                var width = randomazer.Next(leftBorder, rightBorder);
                var height = randomazer.Next(leftBorder, rightBorder);
                return new Size(width, height);
            }
            private double GetDistantBetweenPoints(Point first, Point second)
            {
                return Math.Sqrt(Math.Pow(second.X-first.X,2)+Math.Pow(second.Y-first.Y,2));
            }

            public double GetMaxDistance(Point center, Rectangle rectangle)
            {
                var maxDistant = double.MinValue;
                var rectanglePoints = new []
                {
                    new Point(rectangle.Left, rectangle.Top),
                    new Point(rectangle.Left, rectangle.Bottom),
                    new Point(rectangle.Right, rectangle.Top),
                    new Point(rectangle.Right, rectangle.Bottom)
                };

                foreach (var point in rectanglePoints)
                {
                    var distance = GetDistantBetweenPoints(point, center);
                    maxDistant = distance > maxDistant ? distance : maxDistant;
                }
                return maxDistant;
            }
        }

        static void Main(string[] args)
        {
            var helper = new Helper();
            var center = new Point(35, 35);
            var layouter = new CircularCloudLayouter(center);
            var radius = double.MinValue;

            for (var j = 0; j < 500; j++)
            {
                var newRectangle = layouter.PutNextRectangle(helper.GetRandomSize(30, 40));
                var distant = helper.GetMaxDistance(center, newRectangle);
                radius = distant > radius ? distant : radius;
            }

            var imageSize = (int)(radius + 20)*2;

            var format = ImageFormat.Png;
            layouter.DrawCircularCloud(imageSize, imageSize, "2.png", format);
        }
    }
}