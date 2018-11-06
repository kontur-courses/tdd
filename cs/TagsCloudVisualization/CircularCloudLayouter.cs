using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    interface ICircularCloudLayoter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }


    public class CircularCloudLayouter : ICircularCloudLayoter
    {
        private readonly List<Rectangle> addedRectangles = new List<Rectangle>();
        private readonly IGeneratorCloudPoints generatorCloudPoints;

        public CircularCloudLayouter(IGeneratorCloudPoints generatorCloudPoints)
        {
            this.generatorCloudPoints = generatorCloudPoints;
            var centerPoint = generatorCloudPoints.GetCenterPoint();
            if (centerPoint.X < 0 || centerPoint.Y < 0)
                throw new ArgumentException("Center of circle should have non-negative coordinates");
        }

        private void RaiseIfRectangleSizeIsIncorrect(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height should be positive integers");
        }

        private bool IntersectsWithPreviousRectangles(Rectangle rectangle)
        {
            for (var index = addedRectangles.Count - 1; index >= 0; index--)
                if (addedRectangles[index].IntersectsWith(rectangle))
                    return true;
            return false;
        }

        private Rectangle CreateRectangle(Size rectangleSize, Point centerOfRectangle)
        {
            var topBottomPoint = new Point(centerOfRectangle.X - rectangleSize.Width / 2,
                centerOfRectangle.Y - rectangleSize.Height / 2);
            return new Rectangle(topBottomPoint, rectangleSize);
        }

        private bool IsCorrectRectangle(Rectangle rectangle)
            => rectangle.X >= 0 && rectangle.Y >= 0 && !IntersectsWithPreviousRectangles(rectangle);

        private Rectangle GetRectangleAtFreePlace(Size rectangleSize)
        {
            while (true)
            {
                var rectangle = CreateRectangle(rectangleSize, generatorCloudPoints.GetNextPoint());
                if (IsCorrectRectangle(rectangle))
                    return rectangle;
            }
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            RaiseIfRectangleSizeIsIncorrect(rectangleSize);
            var rectangle = GetRectangleAtFreePlace(rectangleSize);
            addedRectangles.Add(rectangle);
            return rectangle;
        }
    }
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [TestCase(0, 0, 10, 10, TestName = "add square")]
        [TestCase(0, 0, 5, 3, TestName = "add not a square rectangle")]
        [TestCase(0, 0, 5, 10, TestName = "layouter center at zero point")]
        [TestCase(1, 3, 2, 5, TestName = "layouter center at point with positive x and y")]
        [TestCase(0, 4, 2, 5, TestName = "layouter center at point with zero x and positive y")]
        [TestCase(5, 0, 8, 6, TestName = "layouter center at point with positive x and zero y")]
        [TestCase(1000, 1000, 10, 2, TestName = "layouter center has y and x quite big")]
        [TestCase(10, 5, 1000, 1000, TestName = "rectangle has height and width quite big")]
        public void PutNextRectangle_ReturnRectangleWithCorrectSize_WhenSendOneRectangleSize(int startX, int startY,
        int width, int height)
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(startX, startY);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var size = new Size(width, height);

            var rectangle = cloudLayouter.PutNextRectangle(size);

            rectangle.Size
            .ShouldBeEquivalentTo(size);
        }

        [TestCase(0, -1, TestName = "center point has negative y")]
        [TestCase(-1, 0, TestName = "center point has negative x")]
        public void Creation_ShouldThrowArgumentException_WhenGetsIncorrectCenterPoint(int x, int y)
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(x, y);
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(generatorCirclePoints));
        }

        [TestCase(0, 10, TestName = "width is zero")]
        [TestCase(10, 0, TestName = "height is zero")]
        [TestCase(-1, 10, TestName = "width is negative")]
        [TestCase(10, -1, TestName = "height is negative")]
        public void PutNextRectangle_ShouldThrowArgumentException_WhenGetsIncorrectSize(int width, int height)
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(0, 0);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var size = new Size(width, height);

            Assert.Throws<ArgumentException>(() => cloudLayouter.PutNextRectangle(size));
        }

        [TestCase(0, 0, 10, 10, TestName = "center point at zero point")]
        [TestCase(11, 0, 10, 10, TestName = "center point has property x is greater than rectangle width")]
        [TestCase(0, 11, 10, 10, TestName = "center point has property y is greater than rectangle height")]
        [TestCase(10, 20, 9, 20, TestName = "center point has property x is greater than rectangle width, but less than height")]
        [TestCase(10, 10, 20, 9, TestName = "center point has property y is greater than rectangle height, but less than width")]
        public void PutNextRectangle_ShouldBeCloseToCenterPoint_WhenSendOneRectangleSize(int startX,
            int startY,
            int width,
            int height)
        {
            var centerPoint = new Point(startX, startY);
            var generatorCirclePoints = new EternityGeneratorCirclePoints(centerPoint);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var size = new Size(width, height);

            var square = cloudLayouter.PutNextRectangle(size);

            centerPoint.DistanceTo(square).Should().BeLessOrEqualTo(10);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectanglesWithNonNegativeCoordinates_WhenAddManyRectangles()
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(0, 0);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var rectangleSize = new Size(10, 10);
            var maximumSquare = new Rectangle(new Point(0, 0), new Size(10000000, 10000000));

            var rectangles = Enumerable.Range(0, 1000)
            .Select(x => cloudLayouter.PutNextRectangle(rectangleSize))
            .ToList();

            foreach (var rectangle in rectangles)
                maximumSquare.Contains(rectangle).Should().BeTrue();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnDisjointRectangles_WhenSendManyRectangles()
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(0, 0);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);

            var rectangles = Enumerable.Range(0, 100)
            .Select(x => cloudLayouter.PutNextRectangle(new Size(10 + x % 10, 10 + 2 * x % 10)))
            .ToList();

            rectangles.SelectMany(x => rectangles.Select(y => Tuple.Create(x, y)))
            .Where(x => x.Item1 != x.Item2)
            .Any(x => x.Item1.IntersectsWith(x.Item2))
            .Should()
            .BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnDenseRectangles_WhenAddedALotOfRectangles()
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(400, 400);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);

            var rectangles = Enumerable.Range(0, 200)
            .Select(x => cloudLayouter.PutNextRectangle(new Size(10 + x % 10, 10 + (3 * x) % 10)))
            .ToList();

            rectangles.SelectMany(x => rectangles.Select(y => Tuple.Create(x, y)))
            .Select(x => x.Item1.DistanceToRectangle(x.Item2))
            .Max()
            .Should()
            .BeLessThan(300);
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_WorksFast_WhenAddedALotOfRectangles()
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(0, 0);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);

            for (int iterationIndex = 0; iterationIndex < 2000; iterationIndex++)
                cloudLayouter.PutNextRectangle(new Size(10 + iterationIndex % 10, 10 + 3 * iterationIndex % 10));
        }

        [Test]
        public void PutNextRectangle_ReturnRectanglesWithCorrectSize_WhenSendALotOfRectangles()
        {

            var generatorCirclePoints = new EternityGeneratorCirclePoints(400, 400);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var countRectangles = 50;
            var sizes = Enumerable.Range(1, countRectangles)
            .Select(x => new Size(x * 3, x * 5))
            .ToList();

            var rectanglesSizes = sizes.Select(x => cloudLayouter.PutNextRectangle(x))
            .Select(x => x.Size)
            .ToList();

            rectanglesSizes.ShouldBeEquivalentTo(sizes,
                AssertionOptions => AssertionOptions.WithStrictOrdering());
        }
    }
}