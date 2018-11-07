using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Api;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));

            var rnd = new Random();
            var count = 50;
            while (count-- > 0)
                circularCloudLayouter.PutNextRectangle(new Size(rnd.Next(100, 300), rnd.Next(50, 80)));

            circularCloudLayouter.Generate();
        }
    }
}
