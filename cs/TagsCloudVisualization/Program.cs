using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://github.com/kontur-courses/tdd/blob/master/HomeExercise.md
            var layouter = new CircularCloudLayouter(new Point(1000, 1000));
            var visualizer = new Visualizer(layouter, new Random(12345), 2000, 2000);
            CreateImageFromJPGS(visualizer);
            Console.WriteLine("Hello World!");
        }
        
        static void CreateImageFromRectangles(Visualizer visualizer)
        {
            var rectanglesCount = 300;
            var rnd = new Random(123);
            var rectangles = Enumerable.Range(1, rectanglesCount)
                .Select(x => new Size(100, rnd.Next(50, 200)))
                .ToArray();
            var res = visualizer.CreateImageFromRectangles(rectangles);
            File.WriteAllBytes("result.jpg", res);
        }
        
        static void CreateImageFromSquares(Visualizer visualizer, int rectanglesCount, int squareSize)
        {
            var size = new Size(squareSize, squareSize);
            var rectangles = Enumerable.Repeat(size, rectanglesCount).ToArray();
            var res = visualizer.CreateImageFromRectangles(rectangles);
            File.WriteAllBytes("result.jpg", res);
        }
        
        static void CreateImageFromJPGS(Visualizer visualizer)
        {
            var path = Path.Join(Environment.CurrentDirectory, "Pictures");
            var res = visualizer.CreateImageFromJPGS(path);
            File.WriteAllBytes("result.jpg", res);
        }
    }
}