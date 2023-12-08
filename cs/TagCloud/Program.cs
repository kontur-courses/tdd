using System.Drawing;

namespace TagCloud
{
    public class Program
    {
        private const int Width = 1920;
        private const int Height = 1080;

        static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new SpiralGenerator(new Point(Width / 2, Height / 2), 1, 0.01));

            var random = new Random();

            for (var i = 0; i < 150; i++)
            {
                layouter.PutNextRectangle(new Size(50 + random.Next(0, 100), 50 + random.Next(0, 100)));
            }

            var filename = "Sample";
            var path = @$"{Environment.CurrentDirectory}\..\..\..\Samples";
            var absPath = Path.GetFullPath(path);

            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }

            absPath += @$"\{filename}.png";

            using var bitmap = CloudDrawer.DrawTagCloud(layouter.Rectangles);
            bitmap.Save(absPath);
        }
    }
}