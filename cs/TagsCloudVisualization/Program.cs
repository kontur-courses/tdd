using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rnd = new Random();
            var canvasSize = new Size(800, 800);
            var visualisation = new CircularCloudVisualizator(canvasSize);
            var center = new Point(canvasSize.Width / 2, canvasSize.Height / 2);
            var layouter = new CircularCloudLayouter(center, new Spiral(center, 1, 1));
            for (int i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(rnd.Next(10, 100), rnd.Next(10, 50)));
            }

            visualisation.DrawRectangles(layouter.Rectangles);
            visualisation.SaveCanvas("../visualization.jpg");
        }
    }
}