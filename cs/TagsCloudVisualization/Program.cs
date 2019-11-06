using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        private const string Path = @"\..\..\Examples\";
        public static void Main(string[] args)
        {
            MakeExample1();
            MakeExample2();
            MakeExample3();
        }

        private static void PutRectangles(Point center, string name, int count, int startWidth, int startHeight, int step)
        {
            var layouter = new CircularCloudLayouter(center);
            var visualizer = new CircularCloudVisualizer(layouter, name, Path);
            var width = startWidth;
            var height = startHeight;
            for (var i = 0; i < count; i++)
            {
                width += step;
                height += step;
                layouter.PutNextRectangle(new Size(width, height));
            }
            visualizer.VisualizeLayout();
        }
        private static void MakeExample1()
        {
            PutRectangles(new Point(1000, 1000), "Example1", 30, 20, 10, 10);
        }

        private static void MakeExample2()
        {
            PutRectangles(new Point(1000, 1000), "Example2", 30, 320, 310, -10);
        }

        private static void MakeExample3()
        {
            PutRectangles(new Point(500, 500), "Example3", 200, 30, 20, 0);
        }

    }
}