using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloud = new CircularCloudLayouter(new Point());
            var rnd = new Random();

            var rectangle1 = new Rectangle(new Point(), new Size(3,7));
            var rectangle2 = new Rectangle(new Point(3,0), new Size(4,3));

            Console.WriteLine(rectangle1.IntersectsWith(rectangle2));
            Console.WriteLine(rectangle2.IntersectsWith(rectangle1));
            for (var e = 0; e < 10; e++)
                Console.WriteLine(cloud.PutNextRectangle(new Size(rnd.Next(1, 10), rnd.Next(1, 10))));


        }
    }
}
