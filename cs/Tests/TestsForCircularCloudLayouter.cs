using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    public class TestsForCircularCloudLayouter
    {
        private CircularCloudLayouter circularCloudLayouter;
        private Painter painter;
        private TestContext testContext;
        private int pictureWidth = 700;
        private int pictureHeight = 500;
        private Rectangle[] rectangles;
        private readonly int rectanglesCount = 70;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2));
            painter = new Painter(pictureWidth, pictureHeight);
            testContext = new TestContext(TestExecutionContext.CurrentContext);
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
            return new Size(rand.Next(10, 50), rand.Next(5, 30));
        }


        private bool RectanglesAreCrossed(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (Math.Min(rectangle1.Right, rectangle2.Right) - Math.Max(rectangle1.Left, rectangle2.Left) > 0)
                   && (Math.Min(rectangle1.Bottom, rectangle2.Bottom) - Math.Max(rectangle1.Top, rectangle2.Top) > 0);
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
                RectanglesAreCrossed(rectangles[i - 1], rectangles[i]).Should().BeFalse();
            }
        }

        [Test]
        public void RectanglesNotCrossed()
        {
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    RectanglesAreCrossed(rectangles[i], rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void CloudIsDense()
        {
            var squareOfRectangles = 0.0;
            var center = rectangles[0].Location;
            squareOfRectangles += rectangles[0].Height * rectangles[0].Width;
            for (int i = 1; i < rectanglesCount; i++)
                squareOfRectangles += rectangles[i].Height * rectangles[i].Width;

            var distanceBetweenCenterAndLeftTopAngle = GetDistanceBetweenPoints(center, rectangles[rectanglesCount-1].Location);
            var distanceBetweenCenterAndLeftBottomAngle =
                GetDistanceBetweenPoints(center, new Point(rectangles[69].Left, rectangles[69].Bottom));
            var distanceBetweenCenterAndRightBottomAngle =
                GetDistanceBetweenPoints(center, new Point(rectangles[69].Right, rectangles[69].Bottom));
            var distanceBetweenCenterAndRightTopAngle =
                GetDistanceBetweenPoints(center, new Point(rectangles[69].Right, rectangles[69].Top));
            var radius = Math.Max(
                Math.Max(distanceBetweenCenterAndLeftTopAngle, distanceBetweenCenterAndLeftBottomAngle),
                Math.Max(distanceBetweenCenterAndRightBottomAngle, distanceBetweenCenterAndRightTopAngle));
            var squareOfFramingCircle = Math.PI * radius * radius;
            (squareOfRectangles / squareOfFramingCircle).Should().BeInRange(0.25, 1);
        }

        private double GetDistanceBetweenPoints(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        [TearDown]
        public void TearDown()
        {
            if (testContext.Result.Outcome.Status == TestStatus.Failed)
            {
                painter.SavePicture("failed.jpeg");
                Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath("failed.jpeg")}");
            }
        }

    }
}
