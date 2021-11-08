using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualizationTests
{
    public static class VisualizerTestHelper
    {
        public static void AssertBitmap(IVisualizer visualizer, Size bitmapSize, List<Point> bitmapPoints)
        {
            var expected = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            bitmapPoints.ForEach(point => expected.SetPixel(point.X, point.Y, Color.Red));
            var output = new VisualOutput(visualizer);

            var actual = output.DrawToBitmap();

            actual.ToEnumerable().Should().Equal(expected.ToEnumerable());
        }
    }
}