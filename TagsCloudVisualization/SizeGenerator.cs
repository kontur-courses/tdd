using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class SizeGenerator
    {
        private static Random rnd = new Random();

        public static IEnumerable<Size> GenerateSimilarSquaresSize()
        {

            while (true)
            {
                var x = rnd.Next(100, 200);
                yield return new Size(x, x);
            }
        }
        public static IEnumerable<Size> GenerateRandomSize()
        {

            while (true)
            {
                var x = rnd.Next(1, 500);
                var y = rnd.Next(1, 500);
                yield return new Size(x, y);
            }
        }

        public static IEnumerable<Size> GenerateRandomSquarersSize()
        {
            while (true)
            {
                var x = rnd.Next(30, 600);
                yield return new Size(x, x);
            }
        }
        public static IEnumerable<Size> GenerateVerticalRectanglesSize()
        {
            while (true)
            {
                var x = rnd.Next(30, 100);
                var y = rnd.Next(100, 600);
                yield return new Size(x, y);
            }
        }
        public static IEnumerable<Size> GenerateHorizontalRectanglesSize()
        {
            while (true)
            {
                var x = rnd.Next(100, 600);
                var y = rnd.Next(30, 100);
                yield return new Size(x, y);
            }
        }
        public static IEnumerable<Size> GenerateRandomDecreasingRectanglesSize()
        {
            var counter = 1.0;
            while (true)
            {
                var x = rnd.Next(300, 500);
                var y = rnd.Next(300, 500);
                yield return new Size((int)(x * counter), (int)(y * counter));
                counter -= 0.005;
            }
        }
        public static IEnumerable<Size> GenerateRandomIncreasingRectanglesSize()
        {
            var counter = 1.0;
            while (true)
            {
                var x = rnd.Next(10, 30);
                var y = rnd.Next(10, 30);
                yield return new Size((int)(x * counter), (int)(y * counter));
                counter += 0.05;
            }
        }

    }
}
