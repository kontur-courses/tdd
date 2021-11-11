using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Layouting;
using TagCloud.Saving;
using TagCloud.Visualization;

namespace TagCloud
{
    public class TagCloud : ITagCloud

    {
        private readonly IBitmapSaver bitmapSaver;
        private readonly ICloudLayouter layouter;
        private readonly IVisualizer visualizer;

        public TagCloud(ICloudLayouter layouter,
            IBitmapSaver bitmapSaver,
            IVisualizer visualizer)
        {
            this.layouter = layouter;
            this.bitmapSaver = bitmapSaver;
            this.visualizer = visualizer;
        }

        public void SaveToBitmap(bool shouldShowLayout, bool shouldShowMarkup)
        {
            var canvas = Visualize(shouldShowLayout, shouldShowMarkup);
            bitmapSaver.Save(canvas, true);
        }

        public void PutNextTag(Size tagRectangleSize)
        {
            layouter.PutNextRectangle(tagRectangleSize);
        }

        private Bitmap Visualize(bool shouldShowLayout, bool shouldShowMarkup)
        {
            var canvas = CreateCanvas();
            var rectangles = RelocateRectangles(layouter.GetRectanglesCopy(), canvas.Size);

            using (var g = Graphics.FromImage(canvas))
            {
                if (shouldShowLayout)
                    visualizer.VisualizeCloud(g, layouter.Center, rectangles);

                if (shouldShowMarkup)
                {
                    var cloudRadius = layouter.GetCloudBoundaryRadius();
                    visualizer.VisualizeDebuggingMarkup(g, canvas.Size, layouter.Center, cloudRadius);
                }
            }

            return canvas;
        }

        private Bitmap CreateCanvas()
        {
            var canvasSize = layouter.GetRectanglesBoundaryBox();

            if (canvasSize.Width < 1 || canvasSize.Height < 1)
                throw new ArgumentException("image width and height can't be lower than 1");

            var sizeMultiplier = 1.5;
            return new Bitmap((int)(canvasSize.Width * sizeMultiplier), 
                (int)(canvasSize.Height * sizeMultiplier));
        }

        private static List<Rectangle> RelocateRectangles(List<Rectangle> rectangles, Size imgSize)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var newX = rectangles[i].X + imgSize.Width / 2;
                var newY = rectangles[i].Y + imgSize.Height / 2;

                var newRect = new Rectangle(new Point(newX, newY), rectangles[i].Size);
                rectangles[i] = newRect;
            }

            return rectangles;
        }
    }
}