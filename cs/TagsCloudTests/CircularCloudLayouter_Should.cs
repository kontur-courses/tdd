using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;

namespace TagsCloudTests;

[TestFixture]
public class CircularCloudLayouter_Should : CircularCloudLayouterTestsBase
{
    [Test]
    public void CircularCloud_ShouldBeEmpty_WhenCreated()
    {
        Layouter.Rectangles.Should().BeEmpty();
    }

    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    [TestCase(0, 0)]
    [TestCase(-10, -10)]
    public void CircularCloud_ShouldThrowArgumentException_WhenInvalidRectangleSize(int width, int height)
    {
        var func = () => Layouter.PutNextRectangle(new SizeF(width, height));
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithSameSize()
    {
        var rectangleSize = new SizeF(20, 30);
        var rectangle = Layouter.PutNextRectangle(rectangleSize);

        rectangle.Size.Should().BeEquivalentTo(rectangleSize);
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceRectangleInCenter_WhenFirstRectangle()
    {
        var rectangleSize = new SizeF(10, 10);
        var expectedLocation = new PointF(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2);
        var rectangle = Layouter.PutNextRectangle(rectangleSize);

        rectangle.Location.Should().BeEquivalentTo(expectedLocation);
    }

    [Test]
    public void PutNextRectangle_ShouldNotIntersectWithPreviousRectangles()
    {
        foreach (var size in RectangleSizes)
        {
            Layouter.PutNextRectangle(size);
        }
        VerifyRectanglesDontIntersect(Layouter.Rectangles.ToList());
    }

    [Test]
    public void PutNextRectangle_ShouldNotIntersect_WhenPlacingIdenticalRectangles()
    {
        var rectangleSize = new SizeF(10, 10);
        var numberOfRectangles = 10;

        for (int i = 0; i < numberOfRectangles; i++)
        {
            Layouter.PutNextRectangle(rectangleSize);
        }
        VerifyRectanglesDontIntersect(Layouter.Rectangles.ToList());
    }

    [Test]
    public void CircularCloud_ShouldHaveAllPlacedRectangles()
    {
        foreach (var size in RectangleSizes)
        {
            Layouter.PutNextRectangle(size);
        }

        Layouter.Rectangles.Should().HaveCount(RectangleSizes.Length);
    }

    [Test]
    public void CircularCloudRectangles_ShouldBeCloseToCenter()
    {
        foreach (var size in RectangleSizes)
        {
            Layouter.PutNextRectangle(size);
        }

        var rectangles = Layouter.Rectangles.ToList();

        foreach (var rectangle in rectangles)
        {
            var distanceToCenter = Math.Sqrt(Math.Pow(rectangle.X + rectangle.Width / 2 - Center.X, 2) +
                                             Math.Pow(rectangle.Y + rectangle.Height / 2 - Center.Y, 2));
            distanceToCenter.Should().BeLessThan(MaxDistanceToCenter);
        }
    }

    private void VerifyRectanglesDontIntersect(List<RectangleF> rectangles)
    {
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
            }
        }
    }
}