using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.LayoutGeneration;

namespace TagsCloudVisualization.LayoutVizualization
{
    public class Vizualizer
    {
        public Bitmap GetLayoutImage(ITagsCloud cloud, Size sizeBackground)
        {
            var rectangles = cloud.Rectangles;

            var image = new Bitmap(sizeBackground.Width, sizeBackground.Height);
            var pen = new Pen(Color.Black, 2);
            var graphics = Graphics.FromImage(image);
            var colors = new List<Brush>() {Brushes.Red, Brushes.Green, Brushes.Yellow};

            var random = new Random();
            foreach (var rectangle in rectangles)
            {
                var numberColor = random.Next(0, colors.Count);
                graphics.FillRectangle(colors[numberColor], rectangle);
                graphics.DrawRectangle(pen, rectangle);
            }

            return image;
        }

        public void SaveImage(string path, Bitmap img)
        {
            img.Save(path);
        }
    }
}