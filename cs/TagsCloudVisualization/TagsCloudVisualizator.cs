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
        private Size imageSize;

        public TagsCloudVisualizator(CircularCloudLayouter layouter, Size imageSize)
        {
            this.layouter = layouter;
            this.imageSize = imageSize;
        }

        public void DrawAndSaveCloud(string fileName)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            foreach (var rect in layouter.Rectangles)
            {
                graphics.FillRectangle(Brushes.Red, rect);
                graphics.DrawRectangle(Pens.Black, rect);
            }
            graphics.Save();
            image.Save(fileName);

        }
    }
}
