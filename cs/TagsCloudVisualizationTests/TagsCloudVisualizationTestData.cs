using System.Drawing;

namespace TagsCloudVisualizationTests;

public class TagsCloudVisualizationTestData
{
    public static IEnumerable<TestCaseData> IntersectionData
    {
        get
        {
            yield return new TestCaseData(
                new Point(0, 0),
                new Rectangle(100, 100, 15, 15),
                false
            ).SetName("False_When_NotIntersect");

            yield return new TestCaseData(
                new Point(0, 0),
                new Rectangle(1, 1, 15, 15),
                true
            ).SetName("True_When_Intersect");
        }
    }

    public static IEnumerable<TestCaseData> PutRectanglesData
    {
        get
        {
            yield return new TestCaseData(
                new List<Point> { new(0, 0) },
                new[] { true }
            ).SetName("True_When_CanPutRectangle");

            yield return new TestCaseData(
                new List<Point> { new(0, 0), new(0, 0) },
                new[] { true, false }
            ).SetName("False_When_CanNotPutRectanglesInOnePlace");
        }
    }
}