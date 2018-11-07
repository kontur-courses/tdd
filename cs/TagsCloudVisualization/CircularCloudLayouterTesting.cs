using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using NUnit.Framework;

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
                Assert.AreEqual(new List<Rectangle>(), cloud.ListRectangles);
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
            public void Should_CompressCloud()
            {
                var listSizes = new List<Size>()
                {
                    new Size(272,118),new Size(270,69),new Size(184,115),
                    new Size(198,77),new Size(299,76),new Size(230,100),
                };
                var expectedListRect = new List<Rectangle>()
                {
                    new Rectangle(864,941,272,118),new Rectangle(971,1059,270,69),
                    new Rectangle(953,1128,184,115),new Rectangle(1136,982,198,77),
                    new Rectangle(1036,865,299,76),new Rectangle(1137,1128,230,100)
                };
                var listRect = listSizes.Select(size => cloud.PutNextRectangle(size)).ToList();
                listRect.Should().BeEquivalentTo(expectedListRect);
            }
        }
    }
}