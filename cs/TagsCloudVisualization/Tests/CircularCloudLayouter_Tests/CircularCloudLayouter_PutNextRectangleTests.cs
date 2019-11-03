using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests.CircularCloudLayouter_Tests
{
    class CircularCloudLayouter_PutNextRectangleTests
    {
        CircularCloudLayouter layouterWithZeroCentralPoint;
        List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            layouterWithZeroCentralPoint = new CircularCloudLayouter(new Point(0, 0));
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                if (rectangles.Count != 0)
                {
                    var path = Environment.CurrentDirectory + "\\" +
                        nameof(CircularCloudLayouter_PutNextRectangleTests) + "." +
                        TestContext.CurrentContext.Test.Name + ".png";
                    if (TryDrawAndSaveResultImage(path))
                        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
                    else
                        TestContext.Out.WriteLine("Failed to save result image");
                }
            }
        }

        [TestCase(-1, 10)]
        [TestCase(10, -4)]
        [TestCase(-5, -7)]
        public void ShouldThrow_WhenAnyRectSizeIsNegative(int rectWidth, int rectHeight)
        {
            Action act = () => layouterWithZeroCentralPoint.PutNextRectangle(new Size(rectWidth, rectHeight));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 10)]
        [TestCase(0, 0)]
        public void ReturnedRectangleSize_ShouldBeEqualsSizeFromArgument(int rectWidth, int rectHeight)
        {
            var size = new Size(rectWidth, rectHeight);

            var rect = layouterWithZeroCentralPoint.PutNextRectangle(size);

            rect.Size.Should().Be(size);
        }

        [TestCase(5, 10)]
        [TestCase(6, 103)]
        public void CenterOfFirstRectangle_ShouldBeLayouterCentralPoint(int rectWidth, int rectHeight)
        {
            var center = new Point(101, 500);
            var layouter = new CircularCloudLayouter(center);

            var rect = layouter.PutNextRectangle(new Size(rectWidth, rectHeight));
            var rectCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            rectCenter.Should().Be(center);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(2)]
        [TestCase(200)]
        public void MultipleRectanglesWithRandomSizes_ShouldNotIntersectEachOther(int rectanglesCount)
        {
            var rnd = new Random();
            MultipleRectangles_ShouldNotIntersectEachOther(
                rectanglesCount, i => new Size(rnd.Next(1, 150), rnd.Next(1, 150)));
        }

        [TestCase(20)]
        [TestCase(30)]
        [TestCase(5)]
        public void MultipleRectangles_ShouldNotIntersectEachOther(int rectanglesCount)
        {
            MultipleRectangles_ShouldNotIntersectEachOther(
                rectanglesCount, i => new Size((i % 6 + 1) * 7, (i % 7 + 1) * 3));
        }

        void MultipleRectangles_ShouldNotIntersectEachOther(int rectanglesCount, Func<int, Size> getSize)
        {
            for (var i = 1; i <= rectanglesCount; i++)
                rectangles.Add(layouterWithZeroCentralPoint.PutNextRectangle(getSize(i)));

            foreach (var rect in rectangles)
                rectangles
                    .Where(r => !r.Equals(rect))
                    .Any(r => r.IntersectsWith(rect))
                    .Should().BeFalse();
        }

        [TestCase(50)]
        [TestCase(30)]
        [TestCase(10)]
        [TestCase(5)]
        public void MultipleRectangles_ShouldBeLocatedOnCircle(int rectanglesCount)
        {
            MultipleRectangles_ShouldBeLocatedOnCircle(
                rectanglesCount, i => new Size((i % 5 + 1) * 10, (i % 8 + 1) * 8));
        }

        [TestCase(50)]
        [TestCase(30)]
        [TestCase(10)]
        [TestCase(5)]
        public void MultipleRectanglesWithRandomSizes_ShouldBeLocatedOnCircle(int rectanglesCount)
        {
            var rnd = new Random();
            MultipleRectangles_ShouldBeLocatedOnCircle(
                rectanglesCount, i => new Size(rnd.Next(2, 100), rnd.Next(2, 100)));
        }

        void MultipleRectangles_ShouldBeLocatedOnCircle(int rectanglesCount, Func<int, Size> getSize)
        {
           for (var i = 1; i <= rectanglesCount; i++)
                rectangles.Add(layouterWithZeroCentralPoint.PutNextRectangle(getSize(i)));

            var (lengthByX, lengthByY) = GetMaxLengthsByXAndByY(rectangles);
            var radius = ((lengthByX + lengthByY) / 2) / 2;

            var totalRectanglesArea = rectangles.Sum(r => r.Width * r.Height);
            var circleArea = (int)(Math.PI * Math.Pow(radius, 2));

            Math.Abs(radius - lengthByX / 2).Should().BeLessThan(radius / 3);
            Math.Abs(radius - lengthByY / 2).Should().BeLessThan(radius / 3);
            Math.Abs(totalRectanglesArea - circleArea).Should().BeLessThan(circleArea / 2);
        }

        [TestCase(10)]
        [TestCase(30)]
        [TestCase(50)]
        [TestCase(150)]
        public void MultipleRectangles_ShouldBeCloserToCenter(int rectanglesCount)
        {
            MultipleRectangles_ShouldBeCloserToCenter(
                rectanglesCount, i => new Size((i % 5 + 1) * 3, (i % 7 + 1) * 5));
        }

        [TestCase(10)]
        [TestCase(30)]
        [TestCase(50)]
        [TestCase(150)]
        public void MultipleRectanglesWithRandomSizes_ShouldBeCloserToCenter(int rectanglesCount)
        {
            var rnd = new Random();
            MultipleRectangles_ShouldBeCloserToCenter(
                rectanglesCount, i => new Size(rnd.Next(2, 150), rnd.Next(2, 150)));
        }

        void MultipleRectangles_ShouldBeCloserToCenter(int rectanglesCount, Func<int, Size> getSize)
        {
            for (var i = 1; i <= rectanglesCount; i++)
                rectangles.Add(layouterWithZeroCentralPoint.PutNextRectangle(getSize(i)));

            var (lengthByX, lengthByY) = GetMaxLengthsByXAndByY(rectangles);
            var radius = ((lengthByX + lengthByY) / 2) / 2;
            var radiusSquared = Math.Pow(radius, 2);

            var freePointsCount = 0;
            for (var y = -radius; y <= radius; y++)
                for (var x = -radius; x <= radius; x++)
                    // неравенство, описывающее все точки внутри окружности,
                    // центр окружности не учитывается, потому что он равен (0, 0)
                    if (Math.Pow(x, 2) + Math.Pow(y, 2) <= radiusSquared)
                        if (!rectangles.Any(r => r.Contains(x, y)))
                            freePointsCount++;
            var circleArea = (int)(Math.PI * Math.Pow(radius, 2));

            freePointsCount.Should().BeLessThan(circleArea / 2);
        }

        (int lengthByX, int lengthByY) GetMaxLengthsByXAndByY(List<Rectangle> rectangles)
        {
            var minY = rectangles.Min(r => r.Top);
            var minX = rectangles.Min(r => r.Left);
            var maxY = rectangles.Max(r => r.Bottom);
            var maxX = rectangles.Max(r => r.Right);

            var lengthByX = maxX - minX + 1;
            var lengthByY = maxY - minY + 1;

            return (lengthByX, lengthByY);
        }

        bool TryDrawAndSaveResultImage(string pathToSave)
        {
            var frameWidth = 100;

            var (lengthByX, lengthByY) = GetMaxLengthsByXAndByY(rectangles);
            lengthByX += 2 * frameWidth;
            lengthByY += 2 * frameWidth;

            var imgSize = new Size(lengthByX, lengthByY);

            var minX = rectangles.Min(r => r.X);
            var minY = rectangles.Min(r => r.Y);

            var bm = new Bitmap(imgSize.Width, imgSize.Height);
            var gr = Graphics.FromImage(bm);
            gr.Clear(Color.Black);
            var rects = rectangles
                .Select(r => new Rectangle(
                    new Point(r.X - minX + frameWidth, r.Y - minY + frameWidth),
                    r.Size))
                .ToArray();
            DrawRectangles(rects, gr);

            try
            {
                bm.Save(pathToSave);
                return true;
            }
            catch
            {
                return false;
            }
        }

        void DrawRectangles(Rectangle[] rectangles, Graphics graphics)
        {
            var solidBrushRed = new SolidBrush(Color.Red);
            var solidBrushCyan = new SolidBrush(Color.Cyan);
            var penDarkRed = new Pen(Color.DarkRed);
            for (var i = 0; i < rectangles.Length; i++)
            {
                if (i == 0)
                    graphics.FillRectangle(solidBrushRed, rectangles[i]);
                else
                    graphics.FillRectangle(solidBrushCyan, rectangles[i]);
                graphics.DrawRectangle(penDarkRed, rectangles[i]);
            }
        }
    }
}