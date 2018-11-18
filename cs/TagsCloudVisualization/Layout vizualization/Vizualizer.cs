using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Layout_vizualization
{
    public class Vizualizer
    {
        public Bitmap GetLayoutImage(IEnumerable<Rectangle> rectangles, Size sizeBackground)
        {
            var image = new Bitmap(sizeBackground.Width, sizeBackground.Height);
            var pen = new Pen(Color.Black, 2);
            var graphics = Graphics.FromImage(image);
            graphics.FillRectangle(Brushes.Wheat, new Rectangle(new Point(0, 0), sizeBackground));
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