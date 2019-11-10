using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;


namespace TagsCloudVisualization

{
    [TestFixture]
    class VisualisationTest
    {
        [TestCase(400,1000,1000,1,TestName = "Get400Rectangles")]
        [TestCase(150,500,500,2,TestName = "Get150Rectangles")]
        public void GetCloud(int count,int width, int height,int number)
        {
            var cloud = new CloudVisualization(width, height);
            var layouter = new CircularCloudLayouter(new Point(width/2,height/2));
            Random random = new Random();
            for (int i = 0; i < count; i++)
                layouter.PutNextRectangle(new Size(random.Next(3,50), random.Next(3, 50)));
           var result= cloud.DrawRectangles(layouter.Rectangles);
           result.Save(string.Format("layouter{0}.png",number), System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    public class Programm
    {
        public static void Main()
        {

        }
    }
}
