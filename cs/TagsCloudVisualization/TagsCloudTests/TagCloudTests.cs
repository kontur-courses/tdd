using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class TagCloudTests
    {
        private static CircularCloudLayouter tagCloud;
        private static Point center;
        private static Size defaultSize;
        
        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            tagCloud = new CircularCloudLayouter(center);
            defaultSize = new Size(10,20);
        }
        
        [Test]
        public void CircularCloudLayouter_CreateLayouter_ShouldReturnEmptyLayouter()
        {
            tagCloud.tags.Should().BeEmpty();
        }
        
        [Test, Description("The center must be located at the point we passed in the constructor")]
        public void CircularCloudLayouter_CreateLayouter_ShouldReturnLayouterWithCorrectCenter()
        {
            tagCloud.Center.Should().BeEquivalentTo(center);
        }
        
        [Test, Description("Check on the correctness of the location of the first rectangle")]
        public void PutNewRectangle_ReturnsRectangleWithCenterCoordinates_ForFirstCall()
        {
            tagCloud.PutNextRectangle(defaultSize).Location.Should().BeEquivalentTo(center);
        }
        
        [Test, Description("The size of the rectangle should be the same as we passed in the constructor")]
        public void PutNewRectangle_ReturnsRectangle_WithCorrectSize()
        {
            var newRectangle = tagCloud.PutNextRectangle(defaultSize);
            newRectangle.Size.Should().BeEquivalentTo(defaultSize);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNewRectangle_ThrowsArgumentException_IfSizeIsNotPositive(int width, int height)
        {
            Action putRectangle = () => tagCloud.PutNextRectangle(new Size(width, height));
            putRectangle.Should().Throw<ArgumentException>().WithMessage("Width and height must be positive");
        }

        [Test]
        public void PutNewRectangle_ChangesCloudBoundaries_WhenAddingRectangle()
        {
            tagCloud.PutNextRectangle(defaultSize).Size.Should().BeEquivalentTo(new Size(tagCloud.GetWidth, tagCloud.GetHeight));
        }

        [Test, Description("Checking that none of the rectangles intersect with others when adding new rectangles")]
        public void PutNewRectangle_SetRectanglesWithoutIntersect_ForTwoRectangles()
        {
            var firstRectangle = tagCloud.PutNextRectangle(defaultSize);
            var secondRectangle = tagCloud.PutNextRectangle(defaultSize);
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [TestCase(100, 0.50), Description("Check that the cloud is dense enough")]
        [TestCase(500, 0.65)]
        [TestCase(1000, 0.7)]
        public void PutNewRectangle_MakeDenseCloud_WhenAddRectangle(int rectangleCount, double density)
        {
            var random = new Random();
            for (var i = 0; i < rectangleCount; i++)
                tagCloud.PutNextRectangle(new Size(random.Next(10, 20), random.Next(10, 20)));
            var cloudDensity = CalculateCloudDensity();
            
            cloudDensity.Should().BeGreaterThan(density);
        }

        private double FindCloudRadius(CircularCloudLayouter cloud)
        {
            double radius = 0;
            foreach (var rectangle in cloud.tags)
            {
                foreach (var corner in rectangle.GetCornersCoordinates())
                {
                    var distance = CalculateDistanceBetweenPoints(cloud.Center, corner);
                    if (distance > radius)
                        radius = distance;
                }
            }

            return radius;
        }

        private double CalculateSquareOfAllRectangles()
        {
            var squareOfAllRectangles = tagCloud.tags.Sum(rectangle => rectangle.GetRectangleArea());

            return squareOfAllRectangles;
        }

         public static double CalculateDistanceBetweenPoints(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }

        private double CalculateCloudDensity()
        {
            var squareOfAllRectangles = CalculateSquareOfAllRectangles();
            var cloudSquare = Math.PI * Math.Pow(FindCloudRadius(tagCloud), 2);
            return squareOfAllRectangles / cloudSquare;
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                var directory = TestContext.CurrentContext.TestDirectory;
                var filename = TestContext.CurrentContext.Test.Name;
                var path = Path.GetFullPath($"{directory}\\..\\..\\FailedTestsImages\\{filename}.png");
                var painter = new CloudPainter();
                var image = painter.CreateNewTagCloud(tagCloud);
                TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
                painter.SaveCloudImage(image, path);
            }
        }
    }
}