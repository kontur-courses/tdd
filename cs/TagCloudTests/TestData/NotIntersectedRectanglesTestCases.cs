using System.Collections;
using System.Drawing;

namespace TagCloudTests.TestData;

public class NotIntersectedRectanglesTestCases: IEnumerable
{
    private static TestCaseData[] data = new[]
    {
        new TestCaseData(new Rectangle(0, 0, 1, 1), new Rectangle(1, 0, 1, 1))
            .SetName("WhenFirstRightBorder_СoincidesWithSecondLeftBorder"),
        new TestCaseData(new Rectangle(0, 0, 1, 1), new Rectangle(0, 1, 1, 1))
            .SetName("WhenFirstTopBorder_СoincidesWithSecondBottomBorder"),
    };

    public IEnumerator GetEnumerator()
    {
        foreach (var testCase in data)
            yield return testCase.Returns(false);
    }
}