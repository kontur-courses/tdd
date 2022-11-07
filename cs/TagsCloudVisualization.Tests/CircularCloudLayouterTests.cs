using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests;

public class CircularCloudLayouterTests
{
    private ICloudLayouter sut;
    private List<Rectangle> rectangles;
    private readonly Point center = new Point(100, 100);

    [SetUp]
    public void Setup()
    {
        sut = new CircularCloudLayouter(center);
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

        var filename = string.Concat(TestContext.CurrentContext.Test.Name, ".png");
        using var image = ImageGenerator.Generate(rectangles.ToArray(), center);
        image.Save(Path.Combine(directoryPath, filename), ImageFormat.Png);
    }

    [TestCase(1, -1, TestName = "{m}NegativeHeight")]
    [TestCase(-1, 1, TestName = "{m}NegativeWidth")]
    [TestCase(-11, -1, TestName = "{m}NegativeHeightAndWidth")]
    public void PutNextRectangle_ThrowArgumentException_On(int width, int height)
    {
        Action act = () => sut.PutNextRectangle(new Size(width, height));

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ReturnCentralRectangle_WhenPutFirstRectangle()
    {
        var rectangle = sut.PutNextRectangle(new Size(100, 100));
        rectangle.Location.Should().Be(center);
    }

    [Test]
    public void PutNextRectangle_ReturnRectangle_WithDeclaredSize()
    {
        var size = new Size(Random.Shared.Next(0, int.MaxValue), Random.Shared.Next(0, int.MaxValue));

        var rectangle = sut.PutNextRectangle(size);

        rectangle.Size.Should().Be(size);
    }

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_RectangleDoesNotIntersect_WithOthers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var rectangle = sut.PutNextRectangle(new Size(50, 50));
            if (rectangles.Any(x => x.IntersectsWith(rectangle)))
            {
                Assert.Fail("Rectangle has intersection");
            }

            rectangles.Add(rectangle);
        }
    }

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_RandomRectangleDoesNotIntersect_WithOthers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var rectangle = sut.PutNextRectangle(new Size(Random.Shared.Next(40, 100), Random.Shared.Next(40, 100)));
            if (rectangles.Any(x => x.IntersectsWith(rectangle)))
            {
                Assert.Fail("Rectangle has intersection");
            }

            rectangles.Add(rectangle);
        }
    }
}