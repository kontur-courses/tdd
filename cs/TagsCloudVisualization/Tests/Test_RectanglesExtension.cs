using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Test_RectanglesExtension
    {
        private static object[] rectangles =
        {
            new object[] {1, 1, 2, 2},
            new object[] {0, 1, 3, 3},
            new object[] {-1, -1, 10, 10}
        };

        [TestCaseSource(nameof(rectangles))]
        public void CornersOfRectangles_ShouldReturnFourCornerOnDifferentRectangle(int x, int y, int width, int height)
        {
            RectanglesHelper.GetCorners(new Rectangle(x, y, width, height)).Should().HaveCount(4);
        }

        [TestCaseSource(nameof(rectangles))]
        public void CornersOfRectangles_ShouldReturnCorrectCorner(int x, int y, int width, int height)
        {
            RectanglesHelper.GetCorners(new Rectangle(x, y, width, height))
                .Should().Contain(new Point(x, y))
                .And.Contain(new Point(x + width, y + height))
                .And.Contain(new Point(x, y + height))
                .And.Contain(new Point(x + width, y));
        }

        [TestCaseSource(nameof(rectangles))]
        public void GetAllPossiblePosition_ShouldReturnFourDifferentPosition(int x, int y, int width, int height)
        {
            RectanglesHelper.GetAllPossibleRectangles(new Point(x, y), new Size(width, height)).Should()
                .HaveCount(4).And.OnlyHaveUniqueItems();
        }
        
        [TestCaseSource(nameof(rectangles))]
        public void GetAllPossiblePosition_ShouldReturnPositionThatHaveCornerInLocation(int x, int y, int width, int height)
        {
            RectanglesHelper.GetAllPossibleRectangles(new Point(x, y), new Size(width, height))
                .Should().Contain(new Rectangle(x, y, width, height))
                .And.Contain(new Rectangle(x - width, y, width, height))
                .And.Contain(new Rectangle(x, y - height, width, height))
                .And.Contain(new Rectangle(x - width, y - height, width, height));
        }
        
    }
}