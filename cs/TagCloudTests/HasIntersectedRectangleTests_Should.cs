using System.Drawing;
using FluentAssertions;

namespace TagCloudTests;

public class HasIntersectedRectangleTests_Should
{
    public static TestCaseData[] intersectedRectanglesTestCases =
    {
        new TestCaseData(new Rectangle(1, 1, 1, 1), new Rectangle(1, 1, 1, 1))
            .SetName("WhenFirstEqualsSecond"),
        new TestCaseData(new Rectangle(1, 1, 1, 1), new Rectangle(0, 0, 3, 3))
            .SetName("WhenFirstInSecond"),
        new TestCaseData(new Rectangle(-1, 1, 2, 1), new Rectangle(0, 0, 3, 3))
            .SetName("WhenFirstEnterInSecond_ByLeftSide"),
        new TestCaseData(new Rectangle(1, 1, 2, 1), new Rectangle(0, 0, 3, 3))
            .SetName("WhenFirstEnterInSecond_ByRightSide"),
        new TestCaseData(new Rectangle(1, 1, 1, 2), new Rectangle(0, 0, 3, 3))
            .SetName("WhenFirstEnterInSecond_ByTopSide"),
        new TestCaseData(new Rectangle(1, -1, 1, 2), new Rectangle(0, 0, 3, 3))
            .SetName("WhenFirstEnterInSecond_ByBottomSide"),
        new TestCaseData(new Rectangle(0, 0, 2, 2), new Rectangle(1, 1, 2, 2))
            .SetName("WhenFirstRightTopAngle_IntersectWithSecondLeftBottomAngle"),
        new TestCaseData(new Rectangle(0, 1, 2, 2), new Rectangle(0, 1, 2, 2))
            .SetName("WhenFirstRightBottomAngle_IntersectWithSecondLeftTopAngle")
    };

    public static TestCaseData[] notIntersectedRectanglesTestCases =
    {
        new TestCaseData(new Rectangle(0, 0, 1, 1), new Rectangle(1, 0, 1, 1))
            .SetName("WhenFirstRightBorder_СoincidesWithSecondLeftBorder"),
        new TestCaseData(new Rectangle(0,0, 1, 1), new Rectangle(0, 1, 1, 1))
            .SetName("WhenFirstTopBorder_СoincidesWithSecondBottomBorder"),
    };
        
    [TestCaseSource(nameof(intersectedRectanglesTestCases))]
    public void ReturnTrue(Rectangle first, Rectangle second)
    {
        new[] { first, second }.HasIntersectedRectangles().Should().BeTrue();
        new[] { second, first }.HasIntersectedRectangles().Should().BeTrue();
    }
    
    [TestCaseSource(nameof(notIntersectedRectanglesTestCases))]
    public void ReturnFalse(Rectangle first, Rectangle second)
    {
        new[] { first, second }.HasIntersectedRectangles().Should().BeFalse();
        new[] { second, first }.HasIntersectedRectangles().Should().BeFalse();
    }

    [Test]
    public void ThrowException_ThenEmptyOrConainsOneElement()
    {
        Assert.Throws<ArgumentException>(() => Array.Empty<Rectangle>().HasIntersectedRectangles());
        Assert.Throws<ArgumentException>(() => new[] { new Rectangle(0, 0, 1, 1) }.HasIntersectedRectangles());
    }
}