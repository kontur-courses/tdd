using System;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rectangles = TagsCloudExample.GetExample1();
            RectanglePainter.SaveToFile($"{Environment.CurrentDirectory}\\ex1.png", rectangles);
        }
    }
}