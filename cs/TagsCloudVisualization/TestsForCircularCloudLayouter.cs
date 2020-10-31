using System;
using System.Drawing;
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

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2));
            painter = new Painter(pictureWidth, pictureHeight);
            testContext = new TestContext(TestExecutionContext.CurrentContext);
            rectangles = new Rectangle[70];
            for (int i = 0; i < 70; i++)
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
        public void PutNextRectangle_ThrowsException_RectangleSizeLessOrEqualZero()
        {
            Assert.Throws<ArgumentException>(() => circularCloudLayouter.PutNextRectangle(new Size(0, 0)));
        }

        [Test]
        public void PutNextRectangle_NotCross_PreviousRectangle()
        {
            for (var i = 1; i < 70; i++)
            {
                RectanglesAreCrossed(rectangles[i - 1], rectangles[i]).Should().BeFalse();
            }
        }

        [Test]
        public void RectanglesNotCrossed()
        {
            for (int i = 0; i < 70; i++)
            {
                for (int j = i+1; j < 70; j++)
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
            for (int i = 1; i < 70; i++)
            {
                squareOfRectangles += rectangles[i].Height * rectangles[i].Width;
            }

            var radius = Math.Max(Math.Max(GetDistanceBetweenPoints(center, rectangles[69].Location),
                    GetDistanceBetweenPoints(center, new Point(rectangles[69].Left, rectangles[69].Bottom))),
                Math.Max(GetDistanceBetweenPoints(center, new Point(rectangles[69].Right, rectangles[69].Bottom)),
                    GetDistanceBetweenPoints(center, new Point(rectangles[69].Right, rectangles[69].Top))));
            (squareOfRectangles / (Math.PI * radius * radius)).Should().BeGreaterThan(0.25);
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
                Console.WriteLine("Tag cloud visualization saved to file \"failed.jpeg\"");
            }
        }

    }
}
