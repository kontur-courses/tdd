using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TagCloudTests;

public class HasIntersectedRectangleTests_Should
{
    #region TestData

    public static IEnumerable<TestCaseData> intersectedRectanglesTestCases =
        new[]
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
        }.Select(testCase => testCase.Returns(true));

    public static IEnumerable<TestCaseData> notIntersectedRectanglesTestCases =
        new[]
        {
            new TestCaseData(new Rectangle(0, 0, 1, 1), new Rectangle(1, 0, 1, 1))
                .SetName("WhenFirstRightBorder_СoincidesWithSecondLeftBorder"),
            new TestCaseData(new Rectangle(0, 0, 1, 1), new Rectangle(0, 1, 1, 1))
                .SetName("WhenFirstTopBorder_СoincidesWithSecondBottomBorder"),
        }.Select(testCase => testCase.Returns(false));

    #endregion

    [TestCaseSource(nameof(intersectedRectanglesTestCases))]
    [TestCaseSource(nameof(notIntersectedRectanglesTestCases))]
    public bool ReturnValue(Rectangle first, Rectangle second)
    {
        return new[] { first, second }.HasIntersectedRectangles()
            && new[] { second, first }.HasIntersectedRectangles();
    }

    [Test]
    public void ThrowException_ThenEmptyOrConainsOneElement()
    {
        Assert.Throws<ArgumentException>(() => Array.Empty<Rectangle>().HasIntersectedRectangles());
        Assert.Throws<ArgumentException>(() => new[] { new Rectangle(0, 0, 1, 1) }.HasIntersectedRectangles());
    }
}