using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = Console.ReadLine();
            DrawRectanglesInBitmap(folderPath);
        }

        static void DrawRectanglesInBitmap(string folderPath)
        {
            var cloud = new CircularCloudLayouter(new Point(1000, 1000));
            var bmp = new Bitmap(cloud.WindowSize.Width, cloud.WindowSize.Height);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.AliceBlue);
            var pen = new Pen(Brushes.DarkOrchid, 8);
            var rnd = new Random();
            for (int i = 0; i < 70; i++)
            {
                var size = new Size(rnd.Next(100, 300), rnd.Next(50, 120));
                Rectangle rect = cloud.PutNextRectangle(size);
                gr.DrawRectangle(pen, rect);
            }
            bmp.Save(folderPath);
        }
    }
}
