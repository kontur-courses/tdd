using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization
{
    class CircularCloudLayouter : IEnumerable<Rectangle>
    {
        public Point Center { get; }
        private readonly Random random;
        private int radiusSetting;
        private int stepRadiusSetting = 5;
        private double deflectionAngle = Math.PI*2;
        private const double stepDeflectionAngle = Math.PI / 12;
        private readonly List<Rectangle> existingRectangles;

        public CircularCloudLayouter(Point center)
        {
            random = new Random();
            existingRectangles = new List<Rectangle>();
            this.Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {  
            deflectionAngle = 2 * Math.PI * random.NextDouble(); 
            while (true)
            {
                while (deflectionAngle > 0)
                {
                    int x = Center.X + (int) (radiusSetting*Math.Cos(deflectionAngle)) - rectangleSize.Width / 2;
                    int y = Center.Y + (int) (radiusSetting*Math.Sin(deflectionAngle)) - rectangleSize.Height / 2;
                    var newRectangle = new Rectangle(new Point(x, y), rectangleSize);
                    if (!IsIntersectionWithRectangles(newRectangle))
                    {
                        var resultRectangle = ShiftedToCenter(newRectangle);
                        existingRectangles.Add(resultRectangle);
                        return resultRectangle;
                    }
                    deflectionAngle -= stepDeflectionAngle;
                }
                deflectionAngle = 2 * Math.PI * random.NextDouble();
                radiusSetting += stepRadiusSetting;
            }
        }

        private bool TryShiftToDirection(int directionX, int directionY, Rectangle rectangle, out Rectangle resultrectangle)
        {
            var newLocation = new Point(rectangle.X + directionX, rectangle.Y + directionY);
            resultrectangle = new Rectangle(newLocation, rectangle.Size);
            return !IsIntersectionWithRectangles(resultrectangle);
        }

        private Rectangle ShiftedToCenter(Rectangle rectangle)
        {
            var currentRectangle = rectangle;
            var rectengleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var directionX = Math.Sign(Center.X - rectengleCenter.X);
            var directionY = Math.Sign(Center.Y - rectengleCenter.Y);
            while (directionY != 0 || directionX != 0)
            {
                Rectangle newRectangle;
                if (directionX != 0 && TryShiftToDirection(directionX, 0, currentRectangle, out newRectangle))
                {
                    currentRectangle = newRectangle;
                }
                else
                {
                    if (directionY != 0 && TryShiftToDirection(0, directionY, currentRectangle, out newRectangle))
                    {
                        currentRectangle = newRectangle;
                    }
                    else
                    {
                        break;
                    }
                }
                rectengleCenter = new Point(
                    currentRectangle.X + currentRectangle.Width / 2,
                    currentRectangle.Y + currentRectangle.Height / 2);
                directionY = Math.Sign(Center.Y - rectengleCenter.Y);
                directionX = Math.Sign(Center.X - rectengleCenter.X);
            }
            return currentRectangle;
        }

        private bool IsIntersectionWithRectangles(Rectangle rectangle)
        {
            return existingRectangles.Any(r => r.IntersectsWith(rectangle));
        }

        public IEnumerator<Rectangle> GetEnumerator()
        {
            return existingRectangles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;
        private Random random = new Random();
        private int defaultWidthRectangle = 200;
        private int defaultHeightRectangle = 200;

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));
        }

        private void AssertDoNotIntersect(List<Rectangle> rectangles)
        { 
            var intersectingRectangles = GetPairIntersectingRectangles(rectangles);
            string message = "";
            if (intersectingRectangles != null)
                message = String.Format("{0}: {1} {2}",
                    "intersecting rectangles",
                    intersectingRectangles.Item1.ToString(),
                    intersectingRectangles.Item2.ToString());
            Assert.IsNull(intersectingRectangles, message);

        }

        private Tuple<Rectangle, Rectangle> GetPairIntersectingRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return Tuple.Create(rectangles[i], rectangles[j]);
                }
            }
            return null;
        }

        [Test]
        public void returnRectangleWithCorrectSize_AfterPut()
        {
            Size size = new Size(100, 150);
            cloud.PutNextRectangle(size).Size.Should().Be(size);
        }

        [Test]
        public void returnRectangleInCenter_AfterFirstPutting()
        {
            var center = cloud.Center;
            var rectangleSize = new Size(59, 37);
            var rectangle = cloud.PutNextRectangle(rectangleSize);
            var expectedLocation = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            rectangle.Location.Should().Be(expectedLocation);
            rectangle.Size.Should().Be(rectangleSize);
        }

        private List<Rectangle> PutRectanglesToCloud(int numberOfRectangles)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < numberOfRectangles; i++)
            {
                var rectangleSize = new Size(
                (int)(defaultWidthRectangle * random.NextDouble()),
                (int)(defaultHeightRectangle * random.NextDouble()));
                var rect = cloud.PutNextRectangle(rectangleSize);
                rectangles.Add(rect);
            }
            return rectangles;
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(300)]
        [TestCase(1000)]
        public void returnNotOverlappingRectangles_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
        }


        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void shiftRectangleToCenter_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            foreach (var rectangle in rectangles)
            {
                var rectengleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
                var directionY = Math.Sign(cloud.Center.Y - rectengleCenter.Y);
                var directionX = Math.Sign(cloud.Center.X - rectengleCenter.X);
                if (directionX != 0 || directionY != 0)
                {
                    var newLocation = new Point(rectangle.X + directionX, rectangle.Y + directionY);
                    var newRectangle = new Rectangle(newLocation, rectangle.Size);
                    var numberOfIntersection = rectangles.Where(r => r != rectangle).Count(r => r.IntersectsWith(newRectangle));
                    numberOfIntersection.Should().BeGreaterThan(0);
                }
            }
        }

        [TestCase(100)]
        [TestCase(500)]
        [TestCase(1000)]
        public void placeRectanglesTightly_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
            var totalSquare = rectangles.Sum(r => r.Width*r.Height);
            var minX = rectangles.Min(r => r.X);
            var maxX = rectangles.Max(r => r.Right);
            var minY = rectangles.Min(r => r.Y);
            var maxY = rectangles.Max(r => r.Bottom);
            int squareBoundingRectangle = (maxX - minX)*(maxY - minY);
            ((double) totalSquare/squareBoundingRectangle).Should().BeGreaterThan(0.5);
        }
    }
}