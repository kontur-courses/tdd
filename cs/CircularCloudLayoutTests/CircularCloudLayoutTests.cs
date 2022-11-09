using TagsCloudVisualization;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace CircularCloudLayoutTests
{
    [TestFixture]
    public class CircularCloudLayoutTests
    {
        private SetUpBuilder? setup;
        private Bitmap picture;
        private Graphics g;

        [Test]
        public void PlacedRectangles_Should_Fill_80Percent_OfCircleSpace()
        {
            setup = new SetUpBuilder().WithCenter(new Point(200, 200)).WithCustomSizes();
            var layoutRadius = setup.GetRadius();
            var placedRectangles = setup.GetRectangles();
            var area = 0;
            var expectedCoveredArea = (int)(Math.PI * layoutRadius * layoutRadius * 0.8);

            placedRectangles.ForEach(x => area += x.GetArea());

            area.Should().BeGreaterOrEqualTo(expectedCoveredArea);
        }

        [Test]
        [Description(
            "Тест проверяет, что два наименьших квадрата (меньше чем шаг спирали) будут помещены с наименее возможным расстоянием друг от друга")]
        public void PlacedRectangles_PutTwoSmallestSquares_ShouldBePlacedPointBlank()
        {
            var layout = new CircularCloudLayout(new Point(5, 5));
            var smallestSquareSize = new Size(1, 1);
            Rectangle squareOne;
            Rectangle squareTwo;
            int distanceBetweenSquares;

            layout.PutNextRectangle(smallestSquareSize, out squareOne);
            layout.PutNextRectangle(smallestSquareSize, out squareTwo);
            distanceBetweenSquares = Math.Abs(squareOne.Left - squareTwo.Left + squareOne.Top - squareTwo.Top);

            distanceBetweenSquares.Should().Be(1);
        }

        [Test]
        public void PlacedRectangles_Should_Not_IntersectEachOther()
        {
            setup = new SetUpBuilder().WithCenter(new Point(200, 200)).WithCustomSizes();
            var placedRectangles = setup.GetRectangles();
            var doIntersects = false;

            for (int i = 0; i < placedRectangles.Count - 1; i++)
            {
                for (int b = i + 1; b < placedRectangles.Count; b++)
                {
                    doIntersects = doIntersects || placedRectangles[i].IntersectsWith(placedRectangles[b]);
                }
            }

            doIntersects.Should().BeFalse();
        }

        [Test]
        public void PlacedRectangles_ActualCenter_ShouldNotDeviate_MoreThanFivePercent_From_CenterPoint()
        {
            setup = new SetUpBuilder().WithCenter(new Point(200, 200)).WithCustomSizes();
            var center = setup.GetCenter();
            var placedRectangles = setup.GetRectangles();
            var maxX = placedRectangles.Select(x => x.Right).Max();
            var maxY = placedRectangles.Select(x => x.Bottom).Max();
            var minX = placedRectangles.Select(x => x.Left).Min();
            var minY = placedRectangles.Select(x => x.Top).Min();
            var deviationX = center.X * 0.05;
            var deviationY = center.Y * 0.05;


            var actualCenter = new Point((maxX + minX) / 2, (maxY + minY) / 2);
            double actualDeviationX = Math.Abs(center.X - actualCenter.X);
            double actualDeviationY = Math.Abs(center.Y - actualCenter.Y);

            actualDeviationX.Should().BeLessOrEqualTo(deviationX);
            actualDeviationY.Should().BeLessOrEqualTo(deviationY);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_GivePoint_WithNonPositiveX_Should_ThrowArgumentException(int x)
        {
            Action act = () => new CircularCloudLayout(new Point(x, 1));
            act.Should().Throw<ArgumentException>().WithMessage("X sould be positive number");
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_GivePoint_WithNonPositiveY_Should_ThrowArgumentException(int y)
        {
            Action act = () => new CircularCloudLayout(new Point(1, y));
            act.Should().Throw<ArgumentException>().WithMessage("Y sould be positive number");
        }

        [Description("Layout size based on center point")]
        [TestCase(250, 100)]
        [TestCase(100, 250)]
        public void PutNextRectangle_AddSizeBiggerThanLayoutSize_ShouldNotPlaceRectangle(int length, int height)
        {
            var layout = new CircularCloudLayout(new Point(100, 100));
            var size = new Size(length, height);

            layout.PutNextRectangle(size, out Rectangle rectangle).Should().BeFalse();
        }

        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        public void PutNextRectangle_GiveSizeWhithNonpositiveDimensions_Should_ThrowArgumentException(int length,
            int height)
        {
            var layout = new CircularCloudLayout(new Point(100, 100));

            Action act = () => layout.PutNextRectangle(new Size(length, height), out Rectangle rectangle);

            act.Should().Throw<ArgumentException>().WithMessage("Both dimensions must be above zero");
        }

        [Test]
        [Description("Circle based on center point")]
        public void PlacedRectangles_Should_LieInLimitingCircle()
        {
            setup = new SetUpBuilder().WithCenter(new Point(200, 200)).WithCustomSizes();
            var layoutRadius = setup.GetRadius();
            var placedRectangles = setup.GetRectangles();
            var center = setup.GetCenter();
            var isOutside = false;

            foreach (var rectangle in placedRectangles)
            {
                var x1 = rectangle.Left - center.X;
                var y1 = rectangle.Top - center.Y;
                var x2 = rectangle.Right - center.X;
                var y2 = rectangle.Bottom - center.Y;
                isOutside = isOutside || Math.Sqrt(x1 * x1 + y1 * y1) > layoutRadius
                                      || Math.Sqrt(x2 * x2 + y1 * y1) > layoutRadius
                                      || Math.Sqrt(x1 * x1 + y2 * y2) > layoutRadius
                                      || Math.Sqrt(x2 * x2 + y2 * y2) > layoutRadius;
            }

            isOutside.Should().BeFalse();
        }

        [TearDown]
        public void Cleanup()
        {
            var context = TestContext.CurrentContext;
            if (!(context.Result.FailCount > 0) || setup == null)
                return;
            picture = new(setup.GetCenter().X * 2 + 5, setup.GetCenter().Y * 2 + 5);
            g = Graphics.FromImage(picture);
            g.Clear(Color.White);
            setup.GetRectangles().ForEach(x => g.DrawRectangle(Pens.Black, x));
            g.DrawEllipse(Pens.Black, setup.GetCenter().X - setup.GetRadius(), setup.GetCenter().Y - setup.GetRadius(),
                setup.GetRadius() * 2, setup.GetRadius() * 2);
            var time = DateTime.Now.ToString("dd/MM/yyyy_HH-mm-ss");
            var path = Path.Combine(context.WorkDirectory,
                $"..\\..\\..\\CircularCloudLayout_{context.Test.Name}_{time}.bmp");
            Console.WriteLine($"Tag cloud visualization saved to file CircularCloudLayout_{context.Test.Name}_{time}");
            picture.Save(path);
        }
    }
}