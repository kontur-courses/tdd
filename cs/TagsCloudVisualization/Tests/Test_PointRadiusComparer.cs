using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Test_PointRadiusComparer
    {
        private PointRadiusComparer standardComparer;

        [SetUp]
        public void CreateComparer()
        {
            standardComparer = new PointRadiusComparer(new Point(0, 0));
        }

        private static object[] caseOnEqualsPoint =
        {
            new object[] {new Point(1, 1), new Point(1, 1)},
            new object[] {new Point(2, 1), new Point(-2, 1)},
            new object[] {new Point(3, 4), new Point(4, 3)}
        };

        [TestCaseSource(nameof(caseOnEqualsPoint))]
        public void PointerRadiusComparer_ReturnZero_WhenPointsHaveSimilarRadius(Point first, Point second)
        {
            standardComparer.Compare(first, second).Should().Be(0);
        }

        private static object[] caseOnFirstPointCloserThanSecond =
        {
            new object[] {new Point(1, 1), new Point(2, 2)},
            new object[] {new Point(-1, 1), new Point(12, 25)},
            new object[] {new Point(12, -12), new Point(30, 25)},
            new object[] {new Point(-3, -12), new Point(13, 1)}
        };

        [TestCaseSource(nameof(caseOnFirstPointCloserThanSecond))]
        public void PointerRadiusComparer_ReturnLessThanZero_WhenFirstPointCloserThanSecond(Point first, Point second)
        {
            standardComparer.Compare(first, second).Should().BeLessThan(0);
        }

        private static object[] caseOnSecondPointCloserThanFirst =
        {
            new object[] {new Point(12, 12), new Point(2, 2)},
            new object[] {new Point(10, 1), new Point(-1, 5)},
            new object[] {new Point(12, -12), new Point(3, -5)},
            new object[] {new Point(1000, 1000), new Point(-900, -900)}
        };

        [TestCaseSource(nameof(caseOnSecondPointCloserThanFirst))]
        public void PointerRadiusComparer_ReturnMoreThanZero_WhenSecondPointCloserThanFirst(Point first, Point second)
        {
            standardComparer.Compare(first, second).Should().BePositive();
        }

        [Test]
        public void PointerRadiusCompare_ReturnRightAnswer_WhenCentreNotStandard()
        {
            var comparer = new PointRadiusComparer(new Point(2, 2));

            comparer.Compare(new Point(0, 0), new Point(4, 4)).Should().Be(0);
            comparer.Compare(new Point(-1, -1), new Point(4, 4)).Should().BePositive();
            comparer.Compare(new Point(-1, -1), new Point(7, 7)).Should().BeNegative();
        }
    }
}