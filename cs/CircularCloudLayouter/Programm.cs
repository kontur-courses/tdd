using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;


namespace TagsCloudVisualization

{
    public class Programm
    {
        public static void GetCloud(int count, int width, int height, int number)
        {
            var cloud = new CloudVisualization(width, height);
            var layouter = new CircularCloudLayouter(new Point(width / 2, height / 2));
            Random random = new Random();
            for (int i = 0; i < count; i++)
                layouter.PutNextRectangle(new Size(random.Next(3, 50), random.Next(3, 50)));
            var result = cloud.DrawRectangles(layouter.Rectangles);
            result.Save(string.Format(@"../../layouter{0}.png", number));
        }
        public static void Main()
        {
            GetCloud(400,1000,1000,1);
            GetCloud(200,500,500,2);
        }
    }
}
