using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NUnit.Framework.Interfaces;

namespace TagsCloudTests
{
    [TestFixture]
    public class TagCloudTests
    {
        private static CircularCloudLayouter tagCloud;
        [SetUp]
        public void SetUp()
        {
            var center = new Point(0, 0);
            tagCloud = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void CircularCloudLayouter_CreateLayouter_ShouldReturnEmptyLayouter()
        {
            tagCloud.tags.Count.Should().Be(0);
        }
        
        [Test]
        public void PutNewRectangle_ReturnsRectangleWithCenterCoordinates_ForFirstCall()
        {
            var rectangleSize = new Size(10, 20);
            tagCloud.PutNextRectangle(rectangleSize).Location.Should().BeEquivalentTo(tagCloud.Center);
        }

        [Test]
        public void PutNewRectangle_ChangesCloudBoundaries_WhenAddingRectangle()
        {
            var rectangleSize = new Size(10, 5);
            tagCloud.PutNextRectangle(rectangleSize).Size.Should().BeEquivalentTo(new Size(tagCloud.GetWidth, tagCloud.GetHeight));
        }

        [Test]
        public void PutNewRectangle_SetRectanglesWithoutIntersect_ForTwoRectangles()
        {
            var rectangleSize = new Size(10, 5);
            var firstRectangle = tagCloud.PutNextRectangle(rectangleSize);
            var secondRectangle = tagCloud.PutNextRectangle(rectangleSize);
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [TestCase(100, 0.7)]
        [TestCase(1000, 0.8)]
        public void PutNewRectangle_MakeDenseCloud_WhenAddRectangle(int rectangleCount, double density)
        {
            var rectangleSize = new Size(10, 5);
            for (var i = 0; i < rectangleCount; i++)
                tagCloud.PutNextRectangle(rectangleSize);

            double squareOfAllRectangles = 0;
            
            foreach (var tag in tagCloud.tags)
            {
                squareOfAllRectangles += tag.GetRectangleArea();
            }

            var cloudSquare = Math.PI * Math.Pow(FindCloudRadius(tagCloud), 2);

            double cloudDensity = squareOfAllRectangles / cloudSquare;

            cloudDensity.Should().BeGreaterThan(density);
        }

        private double FindCloudRadius(CircularCloudLayouter cloud)
        {
            double radius = 0;
            foreach (var rectangle in cloud.tags)
            {
                foreach (var corner in rectangle.GetCornersCoordinates())
                {
                    var distance = DistanceBetweenPoints(cloud.Center, corner);
                    if (distance > radius)
                        radius = distance;
                }
            }

            return radius;
        }

        private double DistanceBetweenPoints(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }
        
        [TearDown]
        public static void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                var directory = TestContext.CurrentContext.TestDirectory;
                var filename = TestContext.CurrentContext.Test.Name;
                var path = $"{directory}\\..\\..\\FailedTestsImages\\{filename}.png";
                var painter = CloudPainter.CreateNewTagCloud(tagCloud, filename);
                TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
                painter.Save(path, ImageFormat.Png);
            }
        }
    }
}