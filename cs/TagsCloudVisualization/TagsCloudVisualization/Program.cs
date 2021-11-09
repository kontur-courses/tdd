using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
  internal class Program
  {
    public static void Main(string[] args)
    {
        var pointsGenerator = new SpiralPointsGenerator(new Point(1000, 1000), 10, 0, Math.PI / 180, 0.01);
        var bmt = CloudVisualizer.Draw(new CircularCloudLayouter(pointsGenerator),
            1500, new List<Color>()
            {
                Color.Red, Color.Brown, Color.Firebrick, Color.Tomato, Color.Maroon, Color.Salmon, Color.DeepPink
            }, new Size(30, 30), Color.Black);
        bmt.Save("1500_tags.png", ImageFormat.Png);
    }
  }
}