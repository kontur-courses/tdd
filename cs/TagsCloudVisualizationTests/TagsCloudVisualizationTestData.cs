using System.Drawing;

namespace TagsCloudVisualizationTests;

public class TagsCloudVisualizationTestData
{
    public static IEnumerable<TestCaseData> RectanglesIntersection
    {
        get
        {
            yield return new TestCaseData(
                new Dictionary<string, Rectangle>
                {
                    { "Table", new(0, 0, 10, 15) },
                    { "Board", new(15, 20, 15, 15) }
                },
                new Rectangle(10, 15, 5, 5),
                false
            ).SetName("False_When_Not_Intersect");

            yield return new TestCaseData(
                new Dictionary<string, Rectangle>
                {
                    { "Table", new(0, 0, 10, 15) },
                    { "Board", new(15, 20, 15, 15) }
                },
                new Rectangle(5, 5, 1, 1),
                true
            ).SetName("True_When_Intersect");
        }
    }

    public static IEnumerable<TestCaseData> PutRectangle
    {
        get
        {
            yield return new TestCaseData(
                new List<Point>
                {
                    new(0, 0)
                },
                new[]
                {
                    true
                }
            ).SetName("True_When_CanPutRectangle");

            yield return new TestCaseData(
                new List<Point>
                {
                    new(0, 0),
                    new(0, 0)
                },
                new[]
                {
                    true,
                    false
                }
            ).SetName("False_When_CanNotPutRectanglesInOnePlace");
        }
    }

    public static IEnumerable<TestCaseData> Algorithm
    {
        get
        {
            yield return new TestCaseData(
                "../../../../TagsCloudVisualization/words.txt",
                true
            ).SetName("PlaceAllWordsInScreen");
        }
    }
}