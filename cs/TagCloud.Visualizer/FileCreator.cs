using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud.Visualizer
{
    public static class FileCreator
    {
        internal static void CreateBitmapFile(List<Rectangle> rectangles, string name, params string[] endPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", Path.Combine(endPath));
            BitmapCreator.DrawAndSaveBitmap(rectangles.ToList(), name, path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}