using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloudTests;

public class CloudTests_Should
{
    private const string RelativePathToFailDirectory = @"..\..\..\Fails";

    private CircularCloudLayouter layouter;
    private Point center = new Point(0, 0);

    [SetUp]
    public void Setup()
    {
        layouter = new CircularCloudLayouter(center);
    }

    [TearDown]
    public void Tear_Down()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            SaveIncorrectResultsToJpg();
    }

    [Test]
    public void ReturnEmptyList_WhenCreated()
    {
        layouter.Rectangles.Should().BeEmpty();
    }

    [Test]
    public void ReturnOneElementList_WhenAddOne()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(1);
    }

    [Test]
    public void ReturnTwoElementList_WhenAddTwo()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(2);
        NotIntersectedAssetration();
    }

    [TestCase(1, 1, 100, TestName = "WithSquareShape")]
    [TestCase(20, 10, 100, TestName = "WithRectangleShape")]
    public void AddManyRectangles_WithConstantSize(int width, int height, int count)
    {
        var size = new Size(width, height);
        for (int i = 0; i < count; i++)
            layouter.PutNextRectangle(size);

        layouter.Rectangles.Count().Should().Be(count);
        NotIntersectedAssetration();
    }

    [TestCase(5, 10, 2, 4, 100)]
    public void AddManyRectangles_WithVariableSize(int widthMin, int widthMax, int heightMin, int heightMax, int count)
    {
        var rnd = new Random(DateTime.Now.Microsecond);

        for (int i = 0; i < count; i++)
        {
            var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
            layouter.PutNextRectangle(size);
        }

        layouter.Rectangles.Count().Should().Be(count);
        NotIntersectedAssetration();
    }

    [TestCase(5, 10, 10, 20, 100)]
    public void AlwaysFail(int widthMin, int widthMax, int heightMin, int heightMax, int count)
    {
        var rnd = new Random(DateTime.Now.Microsecond);
        for (int i = 0; i < count; i++)
        {
            var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
            layouter.PutNextRectangle(size);
        }

        Assert.Fail();
    }

    public void NotIntersectedAssetration()
    {
        layouter
            .Rectangles
            .HasIntersectedRectangles()
            .Should()
            .BeFalse();
    }

    private void SaveIncorrectResultsToJpg()
    {
        var rectangles = layouter.Rectangles;
        if (rectangles.Count() == 0)
            return;

        var minX = rectangles.Min(rect => rect.X);
        var maxX = rectangles.Max(rect => rect.Right);
        var minY = rectangles.Min(rect => rect.Top);
        var maxY = rectangles.Max(rect => rect.Bottom);

        var bitmap = new System.Drawing.Bitmap(maxX - minX + 2, maxY - minY + 2);
        var graphics = Graphics.FromImage(bitmap);
        graphics.DrawRectangles(new Pen(Color.Red, 1),
            rectangles.Select(rect => rect with { X = -minX + rect.X, Y = -minY + rect.Y })
                .ToArray());

        var pathToFile = @$"{RelativePathToFailDirectory}\{TestContext.CurrentContext.Test.FullName}.jpg";
        var absolutePath = Path.GetFullPath(pathToFile);
        bitmap.Save(pathToFile);
        Console.WriteLine($"Tag cloud visualization saved to file {absolutePath}");
    }
}