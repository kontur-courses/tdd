using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;


// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            var size = new Size(1980, 1080);
            var cloud = new CircularCloudLayouter(new Point(size.Width / 2, size.Height / 2));
            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(new Size(random.Next(80, 100), random.Next(80, 100))));
            }

            var drawer = new DrawerCloudLayouter(Color.Black);

            //var bitMap = new Bitmap(size.Width, size.Height);
            //var g = Graphics.FromImage(bitMap); 
            //drawer.Draw(g, rectangles.ToArray());
            //bitMap.Save(); тут был полный путь сохранения
            Application.Run(new CloudLayouterForm(drawer, rectangles.ToArray()));
        }

    }
}
