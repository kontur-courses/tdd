using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using TagCloudTests.TestData;

namespace TagCloudTests;

public class HasIntersectedRectangleTests_Should
{
    [TestCaseSource(typeof(IntersectedRectanglesTestCases))]
    [TestCaseSource(typeof(NotIntersectedRectanglesTestCases))]
    public bool ReturnValue(Rectangle first, Rectangle second)
    {
        return new[] { first, second }.HasIntersectedRectangles()
            && new[] { second, first }.HasIntersectedRectangles();
    }

    [Test]
    public void ThrowException_ThenConainsOneElement()
    {
        Assert.Throws<ArgumentException>(() => new[] { new Rectangle(0, 0, 1, 1) }.HasIntersectedRectangles());
    }
    
    [Test]
    public void ThrowException_ThenEmpty()
    {
        Assert.Throws<ArgumentException>(() => Array.Empty<Rectangle>().HasIntersectedRectangles());
    }
}