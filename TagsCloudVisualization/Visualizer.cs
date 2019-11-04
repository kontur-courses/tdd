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
        public readonly float Thickness;
        public readonly Point Center;
        public readonly int MinRectSize;
        public readonly int MaxRectSize;
        public readonly int AmountOfRectangles;

        private readonly CircularCloudLayouter layouter;
        private readonly Random random;

        public Visualizer(float thickness, int minRectSize, int maxRectSize, int amountOfRectangles)
        {
            Thickness = thickness;
            Center = new Point(ImageWidth/2, ImageHeight/2);
            MinRectSize = minRectSize;
            MaxRectSize = maxRectSize;
            AmountOfRectangles = amountOfRectangles;
            layouter = new CircularCloudLayouter(Center, thickness);
            random = new Random();
        }

        public Bitmap DrawRectangles()
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, ImageWidth, ImageHeight);
            for (var i = 0; i < AmountOfRectangles; i++)
            {
                var randomValue1 = random.Next(MinRectSize, MaxRectSize);
                var randomValue2 = random.Next(MinRectSize, MaxRectSize);
                graphics.DrawRectangle(Pens[random.Next(Pens.Count)],
                    layouter.PutNextRectangle(
                        new Size(Math.Max(randomValue1, randomValue2), Math.Min(randomValue1, randomValue2))));
            }
            return bitmap;
        }

        static void Main(string[] args)
        {
            var onSmallRectangleDifferenceAndSmallThickness = new Visualizer(1, 15, 60, 100);
            var onSmallRectangleDifferenceAndLargeThickness = new Visualizer(5, 15, 60, 100);
            var onBigRectangleDifferenceAndSmallThickness = new Visualizer(1, 10, 150, 100);
            var onBigRectangleDifferenceAndLargeThickness = new Visualizer(5, 10, 150, 100);
            onSmallRectangleDifferenceAndSmallThickness.DrawRectangles().Save("onSmallRectangleDifferenceAndSmallThickness.png");
            onSmallRectangleDifferenceAndLargeThickness.DrawRectangles().Save("onSmallRectangleDifferenceAndLargeThickness.png");
            onBigRectangleDifferenceAndSmallThickness.DrawRectangles().Save("onBigRectangleDifferenceAndSmallThickness.png");
            onBigRectangleDifferenceAndLargeThickness.DrawRectangles().Save("onBigRectangleDifferenceAndLargeThickness.png");
        }
    }
}
