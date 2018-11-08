using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Enter folder path");
            var folderPath = Console.ReadLine();
            Console.WriteLine(@"Enter min and max edge length of a rectangle");
            var inputStr = Console.ReadLine().Split();
            var minEdgeRectangle = int.Parse(inputStr[0]);
            var maxEdgeRectangle = int.Parse(inputStr[1]);
            Console.WriteLine(@"Enter center coordinates");
            inputStr = Console.ReadLine().Split();
            var x = int.Parse(inputStr[0]);
            var y = int.Parse(inputStr[1]);
            var center = new Point(x,y);
            Console.WriteLine(@"Enter the number of rectangles");
            var numberRectangles = int.Parse(Console.ReadLine());
            DrawRectanglesInBitmap(folderPath,minEdgeRectangle,maxEdgeRectangle,center,numberRectangles);
        }

        static void DrawRectanglesInBitmap(string folderPath, int minValue, int maxValue, Point center,int numberRectangles)
        {
            var cloud = new CircularCloudLayouter(center);
            var bmp = new Bitmap(cloud.WindowSize.Width, cloud.WindowSize.Height);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.AliceBlue);
            var pen = new Pen(Brushes.DarkOrchid, 8);
            var rnd = new Random();
            for (int i = 0; i < numberRectangles; i++)
            {
                var size = new Size(rnd.Next(minValue, maxValue), rnd.Next(minValue, maxValue));
                Rectangle rect = cloud.PutNextRectangle(size);
                gr.DrawRectangle(pen, rect);
            }
            gr.DrawEllipse(pen,center.X,center.Y,3,3);
            bmp.Save(folderPath);
        }
    }
}
