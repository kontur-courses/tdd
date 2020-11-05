using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Vizualizer
    {
        public void MakeNewImage(Point center, List<Size> sizes)
        {
            var bitmap = new Bitmap(2 * center.X, 2 * center.Y);
            var layouter = new CircularCloudLayouter(center);
            var graphics = Graphics.FromImage(bitmap);
            var random = new Random();
            
            var brush = new SolidBrush(Color.Aqua);
            
            foreach (var size in sizes)
            {
                graphics.FillRectangle(brush, 
                    layouter.PutNextRectangle(size));
                brush.Color = Color.FromArgb(random.Next(0, 256),random.Next(0, 256),random.Next(0, 256));
            }
            
            bitmap.Save("C:/Users/Belyaev Ilya/Desktop/projects/output.bmp");
        }
    }
}