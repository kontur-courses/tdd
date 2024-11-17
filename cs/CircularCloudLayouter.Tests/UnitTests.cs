using FluentAssertions;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Drawing;
using TagsCloudVisualization;


namespace CircularCloudLayouterTests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {

        private CircularCloudLayouter CCL;

        [SetUp]
        public void Setup()
        {
            CCL = new CircularCloudLayouter(new Point(100,100));
        }

        [TearDown]
        public void Teardown() 
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var testName = TestContext.CurrentContext.Test.Name;
            if (Equals(testResult, ResultState.Failure) ||
                Equals(testResult == ResultState.Error))
            {
                var drawer = new ImageCreater(CCL.Center.X * 2, CCL.Center.Y * 2);
                var directory = AppDomain.CurrentDomain.BaseDirectory + $"FailedTestTagCloud.{testName}.jpeg";
                drawer.Generate(CCL.Rectangles, directory);
                Console.WriteLine($"Tag cloud visualization saved to file {directory}");
            }
        }

        [Test]
        public void CircularCloudLayouter_Constructor_CorrectlySetCenter()
        {
            CCL.Center.Should().Be(new Point(100,100));
        }

        [Test]
        public void PutNextRectangle_PlacesFirstRectangleToCenter()
        {
            var size = new Size(50,50);
            var centerRecLocation = CircularCloudLayouter.
                GetCornerPoint(CCL.Center, size);
            CCL.PutNextRectangle(size);
            CCL.Rectangles.First().Location.Should().Be(centerRecLocation);
        }

        [Test]
        public void PutNextRectangle_RectanglesDoesNotIntersect()
        {
            var Sizes = new List<Size>();
            for (int i = 10; i < 100; i += 10)
                for (int j = 5; j < 50; j += 5)
                    CCL.PutNextRectangle(new Size(i, j));
            var length = CCL.Rectangles.Count;
            for (int i = 0; i < length - 1; i++)
                for (int j = 0; j < length - 1; j++)
                {
                    if (i != j)
                        CCL.Rectangles[i].IntersectsWith(CCL.Rectangles[j])
                            .Should().BeFalse();
                }   
        }

        [Test]
        public void PutNextRectangle_IncreaseDistanceFromCenter_WithMoreRectangles()
        {
            var firstRectangle = CCL.PutNextRectangle(new Size(20, 10));
            for (int i = 10; i < 100; i += 10)
                for (int j = 10; j < 50; j += 10)
                    CCL.PutNextRectangle(new Size(i, j));
            var lastRectangle = CCL.PutNextRectangle(new Size(20, 30));

            var firstDistance = FindDistance(CCL.Center, firstRectangle.Location);
            var lastDistance = FindDistance(CCL.Center, lastRectangle.Location);

            lastDistance.Should().BeGreaterThan(firstDistance);
        }

        private double FindDistance(Point r1, Point r2)
        {
            return Math.Sqrt(Math.Pow(r2.X - r1.X, 2) + Math.Pow(r2.Y - r1.Y, 2));
        }

        [Test]
        public void PutNextRectangle_ShouldPlaceRectanglesInSpiral()
        { 
            var firstRectangle = CCL.PutNextRectangle(new Size(10, 10));
            CCL.PutNextRectangle(new Size(10, 10));
            CCL.PutNextRectangle(new Size(10, 10));
            CCL.PutNextRectangle(new Size(10, 10));
            CCL.PutNextRectangle(new Size(10, 10));

            CircularCloudLayouter.GetCenterPoint(firstRectangle).Should().Be(CCL.Center);
            for (int i = 1; i < 4; i++)
            {
                var rect = CCL.Rectangles[i];
                FindDistance(firstRectangle.Location, rect.Location).Should().BeLessThan(30);
            }
        }
    }
}