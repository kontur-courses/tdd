using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.TagsCloudVisualization
{
    class CloudVisualization : ITagsCloudVisualization<Rectangle>
    {
        public Bitmap Draw(IEnumerable<Rectangle> figuresToDraw, int imageWidth, int imageHeight)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var random = new Random();
            using (var drawPlace = Graphics.FromImage(image))
            {
                foreach (var rectangle in figuresToDraw)
                {
                    var color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    drawPlace.FillRectangle(new SolidBrush(color), rectangle);
                }
            }
            return image;
        }
    }
}
