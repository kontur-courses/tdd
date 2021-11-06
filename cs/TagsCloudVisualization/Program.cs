using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var tags = new List<(string, Font)>();
            for (var i = 0; i < 150; i++)
            {
                tags.Add(("3", new Font("Arial", 35)));
            }
            Visualizer.Draw(tags, new Size(1200,900), "example.png");
        }
    }
}