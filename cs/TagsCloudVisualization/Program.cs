using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var tags = new List<(string, Font)>();
            for (var i = 5; i < 500; i++)
            {
                tags.Add(("2", new Font("Arial", 15)));
            }

            var visualizer = new Visualizer(new Size(1200, 900));
            visualizer.Draw(tags, "example.png");
        }
    }
}