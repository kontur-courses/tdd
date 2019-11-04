using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        public const int ImageWidth = 1920;
        public const int ImageHeight = 1080;
        public readonly List<Pen> Pens = new List<Pen>
        {
            new Pen(Color.Black),
            new Pen(Color.Red),
            new Pen(Color.Blue),
            new Pen(Color.Green)
        };
        public readonly Point Center;

        private readonly Random random;

        public Visualizer()
        {
            Center = new Point(ImageWidth/2, ImageHeight/2);
            random = new Random();
        }

        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, ImageWidth, ImageHeight);
            foreach (var rectangle in rectangles)
            {
                var shiftedRectangle = new Rectangle(Center, rectangle.Size);
                graphics.DrawRectangle(Pens[random.Next(Pens.Count)], shiftedRectangle);
            }
            return bitmap;
        }

        public Bitmap DrawRandomRectangles(float thickness, int minRectSize, int maxRectSize, int amountOfRectangles)
        {
            var layouter = new CircularCloudLayouter(Center, thickness);
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, ImageWidth, ImageHeight);
            for (var i = 0; i < amountOfRectangles; i++)
            {
                var randomValue1 = random.Next(minRectSize, maxRectSize + 1);
                var randomValue2 = random.Next(minRectSize, maxRectSize + 1);
                graphics.DrawRectangle(Pens[random.Next(Pens.Count)],
                    layouter.PutNextRectangle(
                        new Size(Math.Max(randomValue1, randomValue2), Math.Min(randomValue1, randomValue2))));
            }
            return bitmap;
        }

        static void Main(string[] args)
        {
            var visualizer = new Visualizer();
            visualizer.DrawRandomRectangles(1, 15, 60, 100).Save("onSmallRectangleDifferenceAndSmallThickness.png");
            visualizer.DrawRandomRectangles(5, 15, 60, 100).Save("onSmallRectangleDifferenceAndLargeThickness.png");
            visualizer.DrawRandomRectangles(1, 10, 150, 100).Save("onBigRectangleDifferenceAndSmallThickness.png");
            visualizer.DrawRandomRectangles(5, 10, 150, 100).Save("onBigRectangleDifferenceAndLargeThickness.png");
        }
    }
}
