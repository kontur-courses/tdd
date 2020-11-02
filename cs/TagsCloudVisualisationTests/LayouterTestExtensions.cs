using System.Drawing;
using FluentAssertions;
using TagsCloudVisualisation;

namespace TagsCloudVisualisationTests
{
    public static class LayouterTestExtensions
    {
        public static Rectangle PutAndTest(this ICircularCloudLayouter layouter,
            Size size, Point expectedPosition)
        {
            var expected = new Rectangle(expectedPosition, size);
            layouter.PutNextRectangle(size)
                .Should()
                .Be(expected);
            return expected;
        }

        public static ICircularCloudLayouter PutAndTest(this ICircularCloudLayouter layouter,
            Size size, Point expectedPosition, out Rectangle result)
        {
            layouter.Put(size, out result);
            result.Should().Be(new Rectangle(expectedPosition, size));
            return layouter;
        }

        public static ICircularCloudLayouter Put(this ICircularCloudLayouter layouter, Size size, out Rectangle result)
        {
            result = layouter.PutNextRectangle(size);
            return layouter;
        }
    }
}