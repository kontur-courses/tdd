using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualizator
    {
        private CircularCloudLayouter layouter;
        private Bitmap image;
        private Graphics graphics;

        public TagsCloudVisualizator(CircularCloudLayouter layouter, Size imageSize)
        {
            this.layouter = layouter;
            image = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(image);
        }

        public void drawCloud()
        {
            graphics.Clear(Color.White);
            foreach (var rect in layouter.Rectangles)
            {
                graphics.FillRectangle(Brushes.Red, rect);
                graphics.DrawRectangle(Pens.Black, rect);
            }
            graphics.Save();

        }

        public void saveImage(string fileName)
        {
            image.Save(fileName);
        }
    }
}
