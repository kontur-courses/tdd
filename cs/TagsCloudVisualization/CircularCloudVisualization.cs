using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualization
    {
        readonly CircularCloudLayouter layouter;
        readonly Bitmap bitmap;
        public CircularCloudVisualization(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
            var layoutSize = layouter.GetLayoutSize();
            bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
        }

        public void DrawRectanglesOnImage()
        {
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Red, 5);
            graphics.DrawRectangles(pen, layouter.Rectangles.ToArray());
        }

        public void SaveImage(string path = @"D:\ProjectC#\tdd\tdd\cs\TagsCloudVisualization\LayoutImage\")
        {
            var name = $"ImageAt{layouter.Rectangles.Count}.png";
            bitmap.Save(path + name, ImageFormat.Png);
        }
    }
}
