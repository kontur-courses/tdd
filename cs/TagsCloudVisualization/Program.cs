using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static readonly Point Center = new Point(512, 360);
        private static readonly Size ImageSize = new Size(Center.X * 2, Center.Y * 2);
        private static readonly Size MinRectangleSize = new Size(20, 10);
        private static readonly Size MaxRectangleSize = new Size(70, 60);

        private static void Main()
        {
            DrawTagCloudWithEqualRectangles();
            DrawTagCloudWithIncreasingRectangles();
            DrawTagCloudWithDecreasingRectangles();
        }

        private static void DrawTagCloudWithEqualRectangles()
        {
            var size = new Size(MaxRectangleSize.Width - MinRectangleSize.Width,
                MaxRectangleSize.Height - MinRectangleSize.Height);
            using (var visualizer = CreateVisualizer())
            {
                for (var i = 0; i < 100; i++)
                {
                    visualizer.DrawRectangle(size);
                }
                visualizer.Save("equal_rectangles.png");
            }
        }

        private static void DrawTagCloudWithIncreasingRectangles()
        {
            using (var visualizer = CreateVisualizer())
            {
                for (var i = 0; i < 50; i++)
                {
                    var font = new Font(FontFamily.GenericMonospace, 8 + i);
                    visualizer.DrawText($"#{i + 1}", font);
                }
                visualizer.Save("increasing_rectangles.png");
            }
        }

        private static void DrawTagCloudWithDecreasingRectangles()
        {
            using (var visualizer = CreateVisualizer())
            {
                for (var i = 0; i < 50; i++)
                {
                    var font = new Font(FontFamily.GenericMonospace, 58 - i);
                    visualizer.DrawText($"#{i + 1}", font);
                }
                visualizer.Save("decreasing_rectangles.png");
            }
        }

        private static CircularCloudVisualizer CreateVisualizer() =>
            new CircularCloudVisualizer(new CircularCloudLayouter(Center), ImageSize);
    }
}
