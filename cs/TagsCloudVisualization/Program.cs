using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter();
            GenerateRectangles(50, layouter);
            
            RectangleVisualisator visualisator = new RectangleVisualisator(layouter);
            visualisator.Paint();
            visualisator.Save("Rectangles.png");
        }

        public static void GenerateRectangles(int amount, CircularCloudLayouter layouter)
        {
            if (amount <= 0)
                throw new ArgumentException();
            for (int i = 0; i < amount; i++)
            {
                layouter.PutNextRectangle(new Size(amount - i, amount - i));
            }
        }
    }
}