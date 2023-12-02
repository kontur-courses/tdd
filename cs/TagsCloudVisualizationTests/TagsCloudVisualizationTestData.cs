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
                new Rectangle(500, 500, 1, 1),
                false
            ).SetName("False_When_NotIntersect");

            yield return new TestCaseData(
                new Point(1, 1),
                new Rectangle(0, 0, 2, 2),
                true
            ).SetName("True_When_Intersect");
        }
    }
}