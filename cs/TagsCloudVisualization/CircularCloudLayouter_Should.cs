using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void CreatesClassInstance_WithoutException()
        {
            var createCloudLayouter = () => new CircularCloudLayouter(new Point(0, 0));
            createCloudLayouter.Should().NotThrow();
        }

        [TestCase(-1, 0, TestName = "Negative width")]
        [TestCase(0, -1, TestName = "Negative height")]
        [TestCase(-5, -5, TestName = "Negative width and height")]
        public void PutNextRectangle_ThrowsArgumentException_WhenNegativeParameters(int rectWidth, int rectHeight)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectSize = new Size(rectWidth, rectHeight);
            var rectangleCreation = () => cloudLayouter.PutNextRectangle(rectSize);
            rectangleCreation.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 0, 100, 100)]
        [TestCase(0, 0, 1, 1)]
        [TestCase(0, 0, 5, 4)]
        [TestCase(5, 4, 2, 1)]
        [TestCase(5, 4, 3, 6)]
        public void PutsFirstRectangle_InTheCenter(int centerX, int centerY, int rectWidth, int rectHeight)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(centerX, centerY));
            var rectangle = cloudLayouter.PutNextRectangle(new Size(rectWidth, rectHeight));
            var halfWidth = (int)Math.Floor(rectWidth / 2.0);
            var halfHeight = (int)Math.Floor(rectHeight / 2.0);

            var expectedRectLeft = cloudLayouter.CenterPoint.X - halfWidth;
            var expectedRectRight = cloudLayouter.CenterPoint.X + halfWidth + (rectWidth % 2);
            var expectedRectTop = cloudLayouter.CenterPoint.Y - halfHeight;
            var expectedRectBottom = cloudLayouter.CenterPoint.Y + halfHeight + (rectHeight % 2);

            rectangle.Left.Should().Be(expectedRectLeft);
            rectangle.Right.Should().Be(expectedRectRight);
            rectangle.Top.Should().Be(expectedRectTop);
            rectangle.Bottom.Should().Be(expectedRectBottom);
        }

        [Test]
        [Timeout(1000)]
        public void Puts5SameSquares_InPlusShape_WhenAngleDeltaIsHalfPi()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(), 1, Math.PI / 2);
            var firstRect = cloudLayouter.PutNextRectangle(new Size(10, 10));
            var secondRect = cloudLayouter.PutNextRectangle(new Size(10, 10));
            var thirdRect = cloudLayouter.PutNextRectangle(new Size(10, 10));
            var fourthRect = cloudLayouter.PutNextRectangle(new Size(10, 10));
            var fifthRect = cloudLayouter.PutNextRectangle(new Size(10, 10));

            firstRect.GetRectangleCenterPoint().Should().Be(new Point(0, 0));
            secondRect.GetRectangleCenterPoint().Should().Be(new Point(10, 0));
            thirdRect.GetRectangleCenterPoint().Should().Be(new Point(0, 10));
            fourthRect.GetRectangleCenterPoint().Should().Be(new Point(-10, 0));
            fifthRect.GetRectangleCenterPoint().Should().Be(new Point(0, -10));
        }

        [Test]
        [Timeout(1000)]
        public void Puts11SameSquaresCloseToEachOther_WithDefaultDeltaAngle()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point());
            var expectedRectCenters = new List<Point>()
            {
                new (0, 0),
                new (4, 0),
                new (2, 4),
                new (-2, 4),
                new (-4, 0),
                new (-2, -4),
                new (2, -4),
                new (6, 4),
                new (-6, 4),
                new (-6, -4),
                new (6, -4),
            };
            var rectNumber = 0;
            foreach (var expectedRectCenter in expectedRectCenters)
            {
                var currentRectangle = cloudLayouter.PutNextRectangle(new Size(4,4));
                // Вот это бы хотелось зарефакторить так, чтобы fluentassertions явно говорил
                // На каком номере прямоугольника он свалился, пока что это выглядит так
                //  Expected actualTuple to be equal to
                //  {
                //      Item1 = 7, 
                //      Item2 = { X = 6,Y = -4}
                //  }, but found
                //  {
                //      Item1 = 7, 
                //      Item2 = { X = 6,Y = 4}
                //  }
                (int rectNumber, Point centerPoint) expectedTuple = (rectNumber, expectedRectCenter);
                (int rectNumber, Point centerPoint) actualTuple = (rectNumber, currentRectangle.GetRectangleCenterPoint());
                actualTuple.Should().Be(expectedTuple);
                rectNumber++;
            }
            
        }
    }
}