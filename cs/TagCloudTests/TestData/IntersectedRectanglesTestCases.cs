using System.Collections;
using System.Drawing;

namespace TagCloudTests.TestData;

public class IntersectedRectanglesTestCases : IEnumerable
{
    private static TestCaseData[] data = new[]
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

    public IEnumerator GetEnumerator()
    {
        foreach (var testCase in data)
            yield return testCase.Returns(true);
    }
}