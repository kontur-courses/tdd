using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var j = 0; j < 3; j++)
            {
                var layouter = CircularCloudLayouterBuilder
                    .ACircularCloudLayouter()
                    .WithCenterAt(new Point(25, 25))
                    .Build();
                
                RectanglePainter
                    .GetBitmapWithRectangles(GetRectangles(layouter, 100))
                    .Save($"..\\..\\CloudTagSample{j}.jpg", ImageFormat.Jpeg);
            }
        }

        public static IEnumerable<Rectangle> GetRectangles(CircularCloudLayouter layouter, int count)
        {
            var rnd = new Random();
            
            for (var i = 0; i < count; i++)
            {
                yield return layouter.PutNextRectangle(new Size(count + 25, rnd.Next(10, count + 25 - i * 1)));
            }
        }
    }
}
