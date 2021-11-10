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
            var layouter = new SimpleCircularCloudLayouter(new Point(1000, 1000));
            var visualizer = new Visualizer(layouter, new Random(12345), 2000, 2000);
            CreateImageFromJPGS(visualizer);
            Console.WriteLine("Hello World!");
        }
        
        static void CreateImageFromRectangles(Visualizer visualizer)
        {
            var rectanglesCount = 500;
            var size = new Size(100, 100);
            var rectangles = Enumerable.Range(1, rectanglesCount).Select(x => size).ToArray();
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