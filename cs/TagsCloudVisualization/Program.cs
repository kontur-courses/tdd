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
                var bmp = new Bitmap(1000, 1000);
                var rnd = new Random();
                var layouter = CircularCloudLayouterBuilder
                    .ACircularCloudLayouter()
                    .WithCenterAt(new Point(500, 500))
                    .Build();
            
                using (var graphics = Graphics.FromImage(bmp))
                {
                    using (var pen = new Pen(Brushes.Black, 1))
                    {
                        for (var i = 0; i < 50; i++)
                        {
                            pen.Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                            graphics.DrawRectangle(pen, layouter.PutNextRectangle(new Size(75, rnd.Next(10, 75 - i * 1))));
                        }
                    }
                }
                
                bmp.Save($"..\\..\\CloudTagSample{j}.jpg", ImageFormat.Jpeg);
            }
        }
    }
}
