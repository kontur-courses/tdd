using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

internal static class CircularCloudLayouterTestCases
{
    internal static readonly IEnumerable<TestCaseData> GetRectanglesWithZeroSizesTestData =
    [
        new TestCaseData(new Point(0, 0), new Size(0, 0))
            .SetName("AllZero"),
        new TestCaseData(new Point(0, 0), new Size(1, 0))
            .SetName("WidthZero"),
        new TestCaseData(new Point(0, 0), new Size(0, 1))
            .SetName("HeightZero"),
    ];

    internal static readonly IEnumerable<TestCaseData> GetCorrectRectangleSizesWithEndsLocationTestData =
    [
        new TestCaseData(new Point(10, 10), new List<Size>() {new(10, 10)}, new Point(10, 10) - new Size(10, 10) / 2)
            .SetName("FirstRectangleInCenter"),
    ];

    internal static readonly IEnumerable<TestCaseData> GetCorrectUnusualRectanglesSizeTestData =
    [
        new TestCaseData(new Point(100, 100), Array.Empty<Size>())
            .SetName("ArraySizeEmpty"),
    ];

    internal static readonly IEnumerable<TestCaseData> GetCorrectRectangleSizesTestData =
    [
        new TestCaseData(new Point(100, 100), new List<Size>() {new(10, 10)})
            .SetName("OneSize"),
        new TestCaseData(new Point(100, 100), new List<Size>()
                {new(10, 10), new(20, 20), new(15, 15), new(5, 7), new(3, 1), new(15, 35)})
            .SetName("MoreSizes"),
    ];
}