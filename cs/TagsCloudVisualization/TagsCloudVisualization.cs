using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class TagsCloudVisualization
    {
        static void Main(string[] args)
        {
            var allSizes = new List<Size>();
            var rnd = new Random();
            const int height = 1080;
            const int width = 1920;
            for (var i = 0; i < 60; i++)
            {
                var nextHeight = rnd.Next(40, 50);
                //var nextHeight = 40;
                var nextWidth = rnd.Next(nextHeight * 2, nextHeight * 6);
                //var nextWidth = 160;
                allSizes.Add(new Size(nextWidth, nextHeight));
            }

            var rectangles = new List<Rectangle>();
            var circularCloudLayouter = new CircularCloudLayouter(new Point(width / 2 - 100, height / 2));
            foreach (var r in allSizes)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(r));
            }

            var render = new TagsCloudRenderer();
            render.RenderIntoFile("img.png", circularCloudLayouter.TagsCloud, new Size(width, height));
        }
    }
}