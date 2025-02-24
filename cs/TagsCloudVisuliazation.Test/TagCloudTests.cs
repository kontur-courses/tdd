using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisuliazation.Test;

public class TagCloudTests
{
    private const string PathPhoto = "./../../../photos/WithErrorFrom";
    private TagCloud tagCloud;

    [SetUp]
    public void Setup()
    {
        var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
        var tagImage = new CloudBitMap(1000, 1000, PathPhoto);
        tagCloud = new TagCloud(circularCloudLayouter, tagImage);
    }

    [Test]
    public void CreateTagCloud_WithoutIntersect()
    {
        var rectangles = new List<Rectangle>();
        
        tagCloud.GenerateCloud(50, 100, 100);

        CheckIntercets(rectangles);
    }

    [Test]
    public void TagCloud_StartPosition_ShouldBe_In_Image()
    {
        var circularCloudLayouter = new CircularCloudLayouter(new Point(6, 3));
        var tagImage = new CloudBitMap(5, 5, "./../../../photos/ph");
        Action action = () => new TagCloud(circularCloudLayouter, tagImage);
        action.Should().Throw<ArgumentException>().WithMessage("the start position is abroad of image");
    }
    
    [TearDown]
    public void CheckResult()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure && 
            TestContext.CurrentContext.Test.MethodName!.StartsWith("Create"))
        {
            TestContext.WriteLine($"Tag cloud visualization saved to file {PathPhoto}/" +
                                  $" {TestContext.CurrentContext.Test.MethodName}");
        }
        
        tagCloud.Dispose();
    }

    private static void CheckIntercets(List<Rectangle> rectangles)
    {
        for (int i = 0; i < rectangles.Count; i++)
        for (int j = i + 1; j < rectangles.Count; j++)
        {
            rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }
    }
}