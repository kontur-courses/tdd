using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloudVisualization;

namespace TagsCloud.Tests;

[TestFixture]
public class PointFExtensionsTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        random = new Random();
    }

    private Random random;

    [Test]
    public void Center_Should_CenterPoint()
    {
        for (var i = 0; i < 10000; i++)
        {
            var actual = new PointF(random.Next(1, 5000), random.Next(1, 5000));
            var center = new PointF(random.Next(1, 5000), random.Next(1, 5000));
            var expected = new PointF(actual.X + center.X, actual.Y + center.Y);

            actual = actual.Center(center);
            actual.Should().Be(expected);
        }
    }

    [Test]
    public void ApplyOffset_Should_ApplyOffset()
    {
        for (var i = 0; i < 10000; i++)
        {
            var actual = new PointF(random.Next(1, 5000), random.Next(1, 5000));
            var (offsetX, offsetY) = (random.Next(1, 5000), random.Next(1, 5000));

            var expected = new PointF(actual.X + offsetX, actual.Y + offsetY);
            actual = actual.ApplyOffset(offsetX, offsetY);

            actual.Should().Be(expected);
        }
    }

    [Test]
    public void ConvertToCartesian_Should_ReturnCorrectValues()
    {
        for (var i = 0; i < 10000; i++)
        {
            var actual = new PointF(random.Next(1, 5000), random.Next(1, 5000));

            var (x, y) = (actual.X * (float)Math.Cos(actual.Y),
                actual.X * (float)Math.Sin(actual.Y));
            var expected = new PointF(x, y);

            actual = actual.ConvertToCartesian();
            actual.Should().Be(expected);
        }
    }
}