using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cloud = new CircularCloudLayouter(new Point(450, 450));
            var rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                cloud.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(5, 20)));
            }
            var bitMap = new Bitmap(900, 900);
            bitMap = cloud.Visualization(bitMap);
            bitMap.Save("firsImage.png");
        }
    }
}