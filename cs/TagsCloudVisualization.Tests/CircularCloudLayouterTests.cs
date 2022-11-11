using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Abstractions;

namespace TagsCloudVisualization.Tests;

public class CircularCloudLayouterTests
{
    private ICloudLayouter cloudLayouter;
    private List<Rectangle> rectangles;
    private readonly Point center = new Point(100, 100);
    private static Random random = Random.Shared;

    [SetUp]
    public void Setup()
    {
        cloudLayouter = new CircularCloudLayouter(center);
        rectangles = new List<Rectangle>();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
            return;

        var directoryPath = Path.Join(Environment.CurrentDirectory, "Images");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var filename = $"{TestContext.CurrentContext.Test.Name}.png";
        var fullpath = Path.Combine(directoryPath, filename);
        using var image = new ImageGenerator().Generate(rectangles);
        image.Save(fullpath, ImageFormat.Png);
        TestContext.Error.WriteLine($"Tag cloud visualization saved to file {fullpath}");
    }

    [TestCase(1, -1, TestName = "{m}NegativeHeight")]
    [TestCase(-1, 1, TestName = "{m}NegativeWidth")]
    [TestCase(-11, -1, TestName = "{m}NegativeHeightAndWidth")]
    public void PutNextRectangle_ThrowArgumentException_On(int width, int height)
    {
        Action act = () => cloudLayouter.PutNextRectangle(new Size(width, height));

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ReturnCentralRectangle_WhenPutFirstRectangle()
    {
        var size = new Size(100, 100);
        var expected = new Rectangle(center, size);

        var rectangle = cloudLayouter.PutNextRectangle(size);

        rectangle.Should().Be(expected);
    }

    [Test]
    public void PutNextRectangle_ReturnRectangle_WithDeclaredSize()
    {
        var size = GetRandomSize(0, int.MaxValue);

        var rectangle = cloudLayouter.PutNextRectangle(size);

        rectangle.Size.Should().Be(size);
    }

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_RandomRectangleDoesNotIntersect_WithOthers(int count)
    {
        var result = false;
        for (int i = 0; i < count; i++)
        {
            var rectangle = cloudLayouter.PutNextRectangle(GetRandomSize(40, 100));

            result = rectangle.IsIntersectWith(rectangles);
            rectangles.Add(rectangle);
        }

        result.Should().BeFalse();
    }

    [TestCase(50)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_ShouldCreateCloudLikeCircle(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var rectangle = cloudLayouter.PutNextRectangle(new Size(50, 50));
            rectangles.Add(rectangle);
        }

        var rectanglesArea = rectangles.Sum(r => r.Width * r.Height);
        var circleDiameter = GetCircleDiameter();

        ((double)rectanglesArea / (circleDiameter * circleDiameter) - Math.PI / 4d).Should().BeLessOrEqualTo(0.01);
    }

    [TestCase(50)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_ShouldPlaceRectanglesTightlyPacked_WithSameRectangles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var rectangle = cloudLayouter.PutNextRectangle(new Size(50, 50));
            rectangles.Add(rectangle);
        }

        var rectanglesArea = rectangles.Sum(r => r.Width * r.Height);
        var circleDiameter = GetCircleDiameter();
        var circleArea = (circleDiameter * circleDiameter * Math.PI) / 4d;
        var tightness = rectanglesArea / circleArea;
        tightness.Should().BeGreaterOrEqualTo(0.8);
    }

    private Size GetRandomSize(int minValue, int maxValue)
    {
        return new Size(random.Next(minValue, maxValue), random.Next(minValue, maxValue));
    }

    private int GetCircleDiameter()
    {
        var maxX = int.MinValue;
        var minX = int.MaxValue;
        var maxY = int.MinValue;
        var minY = int.MaxValue;

        foreach (var rectangle in rectangles)
        {
            if (rectangle.Bottom > maxY)
                maxY = rectangle.Bottom;
            if (rectangle.Top < minY)
                minY = rectangle.Top;
            if (rectangle.Right > maxX)
                maxX = rectangle.Right;
            if (rectangle.Bottom < minX)
                minX = rectangle.Left;
        }

        return maxX - minX > maxY - minY
            ? (maxX - minX)
            : (maxY - minY);
    }
}