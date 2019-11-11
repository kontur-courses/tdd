using System.Drawing;

namespace TagCloud
{
    public static class Program
    {
        public static void Main()
        {
            new TagCloudVisualization(200, new Size(1920, 1080), "result").MakeTagCloud();
        }
    }
}