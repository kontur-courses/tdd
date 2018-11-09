using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Math = System.Math;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTesting
    {
        [TestFixture]
        private class Constructor
        {
            [TestCase(45, 100, TestName = "Correct center point")]
            public void Should_NotThrowArgumentException_When(int x, int y)
            {
                var center = new Point(x, y);
                var cloud = new CircularCloudLayouter(center);
                Assert.AreEqual(new Point(x, y), cloud.Center);
            }

            [TestCase(-1, 2, TestName = "x is negative")]
            [TestCase(2, -1, TestName = "y is negative")]
            [TestCase(10000, 2, TestName = "x larger than window size")]
            [TestCase(-1, 30000, TestName = "y larger than window size")]
            public void Should_ThrowArgumentException_When(int x, int y)
            {
                var center = new Point(x, y);
                Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(center));
            }

            [Test]
            public void Should_InitializeListRectangles()
            {
                var cloud = new CircularCloudLayouter(new Point(0, 0));
                Assert.AreEqual(new List<Rectangle>(), cloud.Rectangles);
            }
        }

        [TestFixture]
        private class PutNextRectangle
        {
            private CircularCloudLayouter cloud;
            private double stepAngle, paramArchimedesSpiral;
            [SetUp]
            public void Init()
            {
                stepAngle = CircularCloudLayouter.StepAngle;
                paramArchimedesSpiral = CircularCloudLayouter.ParameterArchimedesSpiral;
                cloud = new CircularCloudLayouter(new Point(1000, 1000));
            }

            [TearDown]
            public void Dispose()
            {
                cloud = new CircularCloudLayouter(new Point(1000, 1000));
            }

            [Test]
            public void Should_ReturnCorrectFirstRectangle()
            {
                var rectangle = cloud.PutNextRectangle(new Size(5, 20));

                Assert.AreEqual(new Rectangle(998, 990, 5, 20), rectangle);
            }

            [Test]
            public void Should_CorrectAngleChange()
            {
                cloud.PutNextRectangle(new Size(5, 20));
                Assert.AreEqual(stepAngle, cloud.Angle);
            }

            [Test]
            public void Should_CorrectlyPositionTwoRectangles()
            {
                var numberSteps = 38;
                var distance = stepAngle * numberSteps * paramArchimedesSpiral;
                var expectedLocation = new Point((int)(cloud.Center.X + distance * Math.Cos(stepAngle * numberSteps)),
                    (int)(cloud.Center.Y - distance * Math.Sin(stepAngle * numberSteps)));
                var expectedResult = new Rectangle(expectedLocation, new Size(4, 4));

                cloud.PutNextRectangle(new Size(4, 4));
                var nextRectangle = cloud.PutNextRectangle(new Size(4, 4));

                Assert.AreEqual(expectedResult, nextRectangle);
            }

            [Test]
            public void Should_HaveCircleShape()
            {
                const int lengthEdge = 40;
                var sizes = new List<Size>();
                for (var i = 0; i < 30; i++)
                {
                    sizes.Add(new Size(lengthEdge, lengthEdge));
                }

                var area = lengthEdge * lengthEdge * sizes.Count;
                var radius = Math.Sqrt(area / Math.PI);
                var listRect = sizes.Select(size => cloud.PutNextRectangle(size)).ToList();
                var maxDistance = listRect.SelectMany(GetListRectangleTops).Max();

                maxDistance.Should().BeLessThan(radius * 1.4);
            }

            private List<double> GetListRectangleTops(Rectangle rect)
            {
                return new List<double>
                    {
                        CalculateDistanceToPoint(rect.Location),
                        CalculateDistanceToPoint(new Point(rect.X + rect.Width, rect.Y)),
                        CalculateDistanceToPoint(new Point(rect.X, rect.Y + rect.Height)),
                        CalculateDistanceToPoint(new Point(rect.X + rect.Width, rect.Y + rect.Height))
                    };
            }

            private double CalculateDistanceToPoint(Point point)
            {
                return Math.Sqrt(Math.Pow(point.X - cloud.Center.X, 2) + Math.Pow(point.Y - cloud.Center.Y, 2));
            }

            [Test]
            public void Should_CompressCloud()
            {
                const int lengthEdge = 40;
                var sizes = new List<Size>();
                for (var i = 0; i < 60; i++)
                {
                    sizes.Add(new Size(lengthEdge, lengthEdge));
                }

                var rectangles = sizes.Select(size => cloud.PutNextRectangle(size)).ToList();
                var hasNeighbor = rectangles.All(rect => rectangles.Any(rect1 => TwoRectanglesTouch(rect,rect1)));

                hasNeighbor.Should().BeTrue();
            }

            private bool TwoRectanglesTouch(Rectangle rect, Rectangle rect1)
            {
                return rect.X == rect1.X + rect1.Width ||
                       rect.X + rect.Width == rect1.X ||
                       rect.Y == rect1.Y + rect1.Height ||
                       rect.Y + rect.Height == rect1.Y;
            }
        }
    }
}