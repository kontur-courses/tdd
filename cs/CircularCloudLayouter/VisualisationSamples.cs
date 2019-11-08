using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;


namespace TagsCloudVisualization

{
    [TestFixture]
    class VisualisationSamples
    {
        [Test]
        public void GetCloud()
        {
            var cloud = new CloudVisualization(1000, 1000);
            var layouter = new CircularCloudLayouter(new Point(500,500));
            Random random = new Random();
            for (int i = 0; i < 150; i++)
                layouter.PutNextRectangle(new Size(random.Next(3,50), random.Next(3, 50)));
           var result= cloud.DrawRectangles(layouter.Rectangles);
           result.Save("C:\\Users\\Igor\\Pictures\\result1.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    public class Programm
    {
        public static void Main()
        {

        }
    }
}
