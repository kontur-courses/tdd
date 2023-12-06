using System.Drawing;

namespace TagCloud
{
    public class Program
    {
        private const int Width = 1920;
        private const int Height = 1080;

        static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(Width / 2, Height / 2));

            var random = new Random();

            for (var i = 0; i < 150; i++)
            {
                layouter.PutNextRectangle(new Size(50 + random.Next(0, 100), 50 + random.Next(0, 100)));
            }

            var filename = "Sample";
            var bitmap = CloudDrawer.DrawTagCloud(layouter);
            var path = @$"{Environment.CurrentDirectory}\..\..\..\Samples";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += @$"\{filename}.png";
            bitmap.Save(path);
        }
    }
}