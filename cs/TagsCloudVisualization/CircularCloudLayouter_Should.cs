using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _circularCloudLayouter;

        [SetUp]
        public void SetCircularCloudFieldToNull()
        {
            _circularCloudLayouter = null;
        }

        [TearDown]
        public void CreateLayoutImage_IfTestFailed()
        {
            if (TestContext.CurrentContext.Result.FailCount < 1) return;
            _circularCloudLayouter.CreateImageOfLayout(TestContext.CurrentContext.Test.Name, TestContext.CurrentContext.WorkDirectory);
            var filePath = TestContext.CurrentContext.WorkDirectory + @"\" + TestContext.CurrentContext.Test.Name + @".png";
            TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
            TestContext.AddTestAttachment(TestContext.CurrentContext.Test.Name + @".png");
        }

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
            _circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectSize = new Size(rectWidth, rectHeight);
            var rectangleCreation = () => _circularCloudLayouter.PutNextRectangle(rectSize);
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
            _circularCloudLayouter = new CircularCloudLayouter(new Point(centerX, centerY));
            var rectangle = _circularCloudLayouter.PutNextRectangle(new Size(rectWidth, rectHeight));
            var halfWidth = (int)Math.Floor(rectWidth / 2.0);
            var halfHeight = (int)Math.Floor(rectHeight / 2.0);

            var expectedRectLeft = _circularCloudLayouter.CenterPoint.X - halfWidth;
            var expectedRectRight = _circularCloudLayouter.CenterPoint.X + halfWidth + (rectWidth % 2);
            var expectedRectTop = _circularCloudLayouter.CenterPoint.Y - halfHeight;
            var expectedRectBottom = _circularCloudLayouter.CenterPoint.Y + halfHeight + (rectHeight % 2);

            rectangle.Left.Should().Be(expectedRectLeft);
            rectangle.Right.Should().Be(expectedRectRight);
            rectangle.Top.Should().Be(expectedRectTop);
            rectangle.Bottom.Should().Be(expectedRectBottom);
        }

        [Test]
        [Timeout(1000)]
        public void Puts5SameSquares_InPlusShape_WhenAngleDeltaIsHalfPi()
        {
            _circularCloudLayouter = new CircularCloudLayouter(new Point(), 1, Math.PI / 2);
            var firstRect = _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            var secondRect = _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            var thirdRect = _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            var fourthRect = _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            var fifthRect = _circularCloudLayouter.PutNextRectangle(new Size(10, 10));

            firstRect.GetRectangleCenterPoint().Should().Be(new Point(0, 0));
            secondRect.GetRectangleCenterPoint().Should().Be(new Point(10, 0));
            thirdRect.GetRectangleCenterPoint().Should().Be(new Point(0, 10));
            fourthRect.GetRectangleCenterPoint().Should().Be(new Point(-10, 0));
            fifthRect.GetRectangleCenterPoint().Should().Be(new Point(0, -10));
        }

        [Test]
        [Timeout(1000)]
        public void Puts11SameSquaresCloseToEachOther_WhenAngleDeltaIsPiOver60()
        {
            _circularCloudLayouter = new CircularCloudLayouter(new Point());
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
                var currentRectangle = _circularCloudLayouter.PutNextRectangle(new Size(4, 4));
                (int rectNumber, Point centerPoint) expectedTuple = (rectNumber, expectedRectCenter);
                (int rectNumber, Point centerPoint) actualTuple = (rectNumber, currentRectangle.GetRectangleCenterPoint());
                actualTuple.Should().Be(expectedTuple);
                rectNumber++;
            }
        }
    }
}