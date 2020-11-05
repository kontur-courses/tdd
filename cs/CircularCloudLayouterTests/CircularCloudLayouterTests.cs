using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Drawing;
using System.IO;

namespace CircularCloudLayouterTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter circularCloudLayouter;
        private Painter painter;
        private int pictureWidth = 700;
        private int pictureHeight = 500;
        private Rectangle[] rectangles;
        private readonly int rectanglesCount = 70;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2));
            painter = new Painter(pictureWidth, pictureHeight);
            rectangles = new Rectangle[rectanglesCount];
            for (int i = 0; i < rectanglesCount; i++)
            {
                rectangles[i] = circularCloudLayouter.PutNextRectangle(MakeRandomSize());
                painter.PaintRectangle(rectangles[i]);
            }
        }

        private Size MakeRandomSize()
        {
            var rand = new Random(Environment.TickCount);
            return new Size(rand.Next(10, 50), rand.Next(10, 30));
        }

        [Test]
        public void PutNextRectangle_PlaceFirstRectangleOnCenter()
        {
            rectangles[0].X.Should().Be(pictureWidth / 2);
            rectangles[0].Y.Should().Be(pictureHeight / 2);
        }

        [Test]
        public void PutNextRectangle_PlaceFirstRectangleOnCenter_WhenOnlyOneRectangle()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2));
            var rectangle = circularCloudLayouter.PutNextRectangle(MakeRandomSize());
            rectangle.X.Should().Be(pictureWidth / 2);
            rectangle.Y.Should().Be(pictureHeight / 2);
        }

        [Test]
        public void PutNextRectangle_ThrowsException_RectangleSizeLessOrEqualZero()
        {
            Assert.Throws<ArgumentException>(() => circularCloudLayouter.PutNextRectangle(new Size(0, 0)));
        }

        [Test]
        public void PutNextRectangle_NotCross_PreviousRectangle()
        {
            for (var i = 1; i < rectanglesCount; i++)
            {
                rectangles[i - 1].IntersectsWith(rectangles[i]).Should().BeFalse();
            }
        }

        [Test]
        public void RectanglesNotCrossed()
        {
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void CloudIsDense()
        {
            var squareOfRectangles = 0.0;
            var radius = 0.0;
            for (int i = 0; i < rectanglesCount; i++)
            {
                radius = Math.Max(GetRadiusOfFramingCircle(rectangles[i]), radius);
                squareOfRectangles += rectangles[i].Height * rectangles[i].Width;
            }

            painter.PaintCircle(rectangles[0].Location, radius);
            var squareOfFramingCircle = Math.PI * radius * radius;
            (squareOfRectangles / squareOfFramingCircle).Should().BeInRange(0.6, 1);
        }

        private double GetRadiusOfFramingCircle(Rectangle rectangle)
        {
            var center = rectangles[0].Location;
            var distanceBetweenCenterAndLeftTopAngle = center.GetDistanceTo(rectangle.Location);
            var distanceBetweenCenterAndLeftBottomAngle = center.GetDistanceTo(new Point(rectangle.Left,
                rectangle.Bottom));
            var distanceBetweenCenterAndRightBottomAngle = center.GetDistanceTo(new Point(rectangle.Right,
                rectangle.Bottom));
            var distanceBetweenCenterAndRightTopAngle =
                center.GetDistanceTo(new Point(rectangle.Right,
                    rectangle.Top));
            return Math.Max(
                Math.Max(distanceBetweenCenterAndLeftTopAngle, distanceBetweenCenterAndLeftBottomAngle),
                Math.Max(distanceBetweenCenterAndRightBottomAngle, distanceBetweenCenterAndRightTopAngle));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                painter.SavePicture("failed.jpeg");
                Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath("failed.jpeg")}");
            }
        }
    }
}