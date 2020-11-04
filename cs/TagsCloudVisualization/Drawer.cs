using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Drawer
    {       
        public static void DrawImage(List<Rectangle> rectangles, Point center ,string fileName)
        {
            if(!fileName.All(char.IsLetter))
                throw new ArgumentException("File name contains invalid characters");

            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("X or Y of center was negative");

            if (!rectangles.Any())
                throw new ArgumentException("The sequence contains no elements");

            var path = Directory.GetCurrentDirectory();

            var deltaXAndY = GetDeltaXAndY(rectangles);

            using var image = new Bitmap( center.X + deltaXAndY[0], center.Y + deltaXAndY[1]);
            using var graphics = Graphics.FromImage(image);

            graphics.DrawRectangles(new Pen(Color.Red), rectangles.ToArray());

            image.Save($"{path}\\{fileName}.bmp", ImageFormat.Bmp);
        }

        private static int[] GetDeltaXAndY(List<Rectangle> rectangles)
        {
            var maxX = -1; 
            var maxY = -1;

            foreach (var elem in rectangles)
            {
                if (elem.X > maxX)
                    maxX = elem.X;
                if (elem.Y > maxY)
                    maxY = elem.Y;
            }

            return new[] {maxX, maxY};
        }

    }
}
