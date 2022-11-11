using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private const string ImagesFolder = "../../../result/";

        private static void Main(string[] args)
        {
            Painter.CreatePicture(new Point(70, -70), 300, ImagesFolder + "300.png");
            Painter.CreatePicture(new Point(0, 0), 10000, ImagesFolder + "10000.png");
        }
    }
}