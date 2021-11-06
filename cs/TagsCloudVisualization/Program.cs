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
        public static void Main()
        {
            var amount = 10;
            for (int i = 0; i < amount; i++)
                GenerateTagCloud(50);

            var spiral = new ArchimedeanSpiral(Point.Empty);
            for (int i = 0; i < 5; i++)
            {
                GenerateSpiral(i, spiral);
            }
        }

        static void GenerateTagCloud(int tagsCount)
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            var rectSises = new List<Size>();
            var rnd = new Random();
            for (int i = 0; i < tagsCount; i++)
            {
                var width = rnd.Next(14, 60);
                var height = rnd.Next(10, width);
                rectSises.Add(new Size(width, height));
            }
            rectSises.ToList().ForEach(s => layouter.PutNextRectangle(s));

            var drawer = new BitmapDrawer(layouter);
            drawer.Draw();
            drawer.Save();
        }

        static void GenerateSpiral(int count, ArchimedeanSpiral spiral)
        {
            var size = new Size(500, 500);
            var bitmap = new Bitmap(size.Width, size.Height);
            var gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, new Rectangle(Point.Empty, bitmap.Size));

            foreach (var p in spiral.Slide())
            {
                var point = new Point(p.X + size.Width / 2, p.Y + size.Height / 2);
                gr.DrawEllipse(Pens.Red, new Rectangle(point, new Size(1, 1)));
            }
            bitmap.Save($"spiral0{count}.png", ImageFormat.Png);
        }
    }
}
