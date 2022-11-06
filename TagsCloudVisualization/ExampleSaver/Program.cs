using System.Drawing.Drawing2D;
using System.Globalization;
using TagCloud;

namespace ExampleSaver
{
    internal static class Program
    {
        static void Main()
        {
            var size = new Size(400, 400);
            var layouter = new CircularCloudLayouter(new Point(size.Width / 2, size.Height / 2));

            for (int i = 0; i < 2; i++)
            {
                var img = new Bitmap(size.Width, size.Height);
                var g = Graphics.FromImage(img);
                g.Clear(Color.SlateBlue);
                for (int j = 0; j < 100; j++)
                {
                    var rand = new Random();
                    var rect = layouter.PutNextRectangle(new Size(10 + rand.Next(20), 10 + rand.Next(20)));
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Orange), 1) { Alignment = PenAlignment.Inset }, rect);
                }

                var localPath = string.Format(CultureInfo.InvariantCulture, "../../../../Resources/imgs/Density{0}_AngleStep{1}.jpg", layouter.Density, layouter.AngleStep);
                img.Save(Path.GetFullPath(localPath));
                layouter.Density *= 100;
                layouter.AngleStep *= 100;
                layouter.Rectangles.Clear();
            }
        }
    }
}