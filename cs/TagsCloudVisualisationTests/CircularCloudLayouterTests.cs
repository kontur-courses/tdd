using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Drawing;
using System.IO;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Entities.Layouters;
using TagsCloudVisualisation.Entities.Visualizer;

namespace TagsCloudVisualisationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        public CircularCloudLayouter cloudLayouter;
        public List<Rectangle> rectangles;
        public BasicCloudVisalizer basicVisualizer;
        public Point center;

        [SetUp]
        public void Setup()
        {
            center = new Point(100, 100);
            cloudLayouter = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
            basicVisualizer = new BasicCloudVisalizer();
        }

        [TearDown]
        public void SaveVisualizationForFailedTest() 
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    string testName = TestContext.CurrentContext.Test.Name;
                    string fileName = $"FAILED_{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

                    var tempDir = Path.Combine(Path.GetTempPath(), "CloudLayouts");
                    Directory.CreateDirectory(tempDir);

                    var filePath = Path.Combine(tempDir, fileName);

                    basicVisualizer.Visualize(rectangles, center, filePath);

                    Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
                }
                catch (Exception ex) 
                {
                    TestContext.WriteLine($"Failed to save visualization: {ex.Message}");
                }
            }

            rectangles.Clear();
        }

        [Test]
        public void Constructor_CreatesInstance_WithValidCenter()
        {
            var center = new Point(100, 100);
            Assert.DoesNotThrow(() => new CircularCloudLayouter(center));
        }

        [Test]
        public void PutNextRectangle_ThrowsException_WithNegativeSize()
        {
            var negativeSize = new Size(-10, -5);
            Assert.Throws<ArgumentException>(() => cloudLayouter.PutNextRectangle(negativeSize));
        }

        [Test]
        public void PutNextRectangle_ThrowsException_WithZeroSize()
        {
            var zeroSize = new Size(0, 0);
            Assert.Throws<ArgumentException>(() => cloudLayouter.PutNextRectangle(zeroSize));
        }

        [Test]
        public void PutNextRectangle_ProducesSameResults_ForSameInputSequence()
        {
            var sizes = new[]
            {
                new Size(20, 20),
                new Size(30, 10),
                new Size(15, 25)
            };

            rectangles = sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();

            var anotherLayouter = new CircularCloudLayouter(center);
            var anotherLayout = sizes.Select(size => anotherLayouter.PutNextRectangle(size)).ToList();

            for (int i = 0; i < rectangles.Count; i++)
            {
                Assert.That(anotherLayout[i], Is.EqualTo(rectangles[i]),
                    $"Rectangle {i} should be at same position");
            }
        }

        [Test]
        public void PutNextRectangle_PutsFirstRectangleProperly_AtCenter()
        {
            var expectedPosition = new Point(90, 90);
            
            var rectangle = cloudLayouter.PutNextRectangle(new Size(20, 20));

            Assert.That(rectangle.Location, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void PutNextRectangle_DoNotIntersectRectangles_WithTwoRectangles()
        {
            var first = cloudLayouter.PutNextRectangle(new Size(20, 20));
            var second = cloudLayouter.PutNextRectangle(new Size(20, 20));

            Assert.That(first.IntersectsWith(second), Is.False);
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesInCircleShape_WithRectanglesOfOneSize()
        {
            var rectangleSize = new Size(20, 20);
            int count = 50;

            for (int i = 0; i < count; i++)
            {
                rectangles.Add(cloudLayouter.PutNextRectangle(rectangleSize));
            }

            double packingDensity = CalculatePackingDensity(rectangles, center);
            Assert.That(packingDensity, Is.GreaterThan(0.60),
                $"Packing density {packingDensity:P2} is too low for circular shape");
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesInCircleShape_WithRandomSizeRectangles()
        {
            int count = 50;

            //Set seed for constant pattern of random generation
            var random = new Random(42);

            for (int i = 0; i < count; i++)
            {
                var width = random.Next(10, 51);
                var height = random.Next(10, 51);
                var rectangleSize = new Size(width, height);

                rectangles.Add(cloudLayouter.PutNextRectangle(rectangleSize));
            }

            double packingDensity = CalculatePackingDensity(rectangles, center);

            Assert.That(packingDensity, Is.GreaterThan(0.40),
                $"Packing density {packingDensity:P2} is too low for circular shape with random rectangles");
        }

        private double CalculatePackingDensity(List<Rectangle> rectangles, Point center)
        {
            var allPoints = new List<Point>();
            foreach (var rect in rectangles)
            {
                allPoints.AddRange(new[]
                {
                new Point(rect.Left, rect.Top),
                new Point(rect.Right, rect.Top),
                new Point(rect.Left, rect.Bottom),
                new Point(rect.Right, rect.Bottom)
                });
            }

            double circleRadius = FindMinimalEnclosingCircleRadius(allPoints, center);

            double circleArea = Math.PI * circleRadius * circleRadius;
            double totalRectanglesArea = rectangles.Sum(r => r.Width * r.Height);

            return totalRectanglesArea / circleArea;
        }

        private double FindMinimalEnclosingCircleRadius(List<Point> points, Point center)
        {
            if (points.Count == 0) return 0;

            double maxDistance = 0;
            foreach (var point in points)
            {
                double distance = CalculateDistance(point, center);
                if (distance > maxDistance)
                    maxDistance = distance;
            }

            return maxDistance;
        }
        private double CalculateDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}