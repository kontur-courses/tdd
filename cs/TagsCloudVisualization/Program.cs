using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TagsCloudVisualization
{
    public class TheEasiestBenchmark
    {

        private void AddManyRectangles(CircularCloudLayouter circularCloudLayouter)
        {
            var size = new Size(10,10);
            for (int i = 0; i < 100; i++)
            {
                circularCloudLayouter.PutNextRectangle(size);
            }
        }

        [Benchmark(Description = "CirclePointGenerator")]
        public void Circle100()
        {
            var gP = new CirclePointGenerator(new Point(500, 500));
            var ccl = new CircularCloudLayouter(new Point(500, 500), gP);
            AddManyRectangles(ccl);
        }

        [Benchmark(Description = "ArchimedesSpiralPointGenerator")]
        public void Archimedes100()
        {
            var gP = new ArchimedesSpiralPointGenerator(new Point(500, 500));
            var ccl = new CircularCloudLayouter(new Point(500, 500), gP);
            AddManyRectangles(ccl);
        }
    }

    public class Program
    {
        
        private static Random rnd = new Random();
        private static Size GetRandomSize()
        {
            var h = rnd.Next(30, 50);
            var w = rnd.Next(50, 70);
            return new Size(w, h);
        }

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TheEasiestBenchmark>();
            Console.Read();

            //Example();

        }

        public static void Example()
        {
            //var gP = new CirclePointGenerator(new Point(500, 500), 0.1);
            var gP = new ArchimedesSpiralPointGenerator(new Point(500, 500));
            var rectangles = new List<Rectangle>();
            var ccl = new CircularCloudLayouter(new Point(500, 500), gP);
            for (int i = 0; i < 25; i++)
            {
                var size = GetRandomSize();
                rectangles.Add(ccl.PutNextRectangle(size));
            }
            DrawHandler.DrawRectangles(rectangles, new Point(500, 500), "new.bmp");

        }
    }
}