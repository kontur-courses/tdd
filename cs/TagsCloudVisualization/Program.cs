using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://github.com/kontur-courses/tdd/blob/master/HomeExercise.md
            var layouter = new CircularCloudLayouter(new Point(1000, 1000));
            var visualizer = new Visualizer(new Random(12345), 2000, 2000);
            CreateImageFromJPGS(visualizer, layouter);
        }

        static void CreateImageFromRectangles(Visualizer visualizer, ILayouter layouter)
        {
            var rectanglesCount = 300;
            var rnd = new Random(123);
            var sizes = Enumerable.Range(1, rectanglesCount)
                .Select(x => new Size(100, rnd.Next(50, 200)))
                .ToArray();
            var res = visualizer.CreateImageFromSizes(sizes, layouter);
            File.WriteAllBytes("result.jpg", res);
        }

        static void CreateImageFromSquares(Visualizer visualizer, ILayouter layouter, int squaresCount, int squareSize)
        {
            var size = new Size(squareSize, squareSize);
            var sizes = Enumerable.Repeat(size, squaresCount).ToArray();
            var res = visualizer.CreateImageFromSizes(sizes, layouter);
            File.WriteAllBytes("result.jpg", res);
        }

        static void CreateImageFromJPGS(Visualizer visualizer, ILayouter layouter)
        {
            var path = Path.Join(Environment.CurrentDirectory, "Pictures");
            var res = visualizer.CreateImageFromJPGS(path, layouter);
            File.WriteAllBytes("result.jpg", res);
        }
    }
}