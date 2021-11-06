using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Layouting;
using TagCloud.Saving;
using TagCloud.Visualization;

namespace TagCloud
{
    public class TagCloud
    {
        private IBitmapToDesktopSaver bitmapSaver;
        private ICloudLayouter layouter;
        private IVisualizer visualizer;

        private Bitmap canvas;

        public TagCloud(ICloudLayouter layouter,
            IBitmapToDesktopSaver bitmapSaver,
            IVisualizer visualizer)
        {
            this.layouter = layouter;
            this.bitmapSaver = bitmapSaver;
            this.visualizer = visualizer;
        }

        public void Save()
        {
            if (canvas == null)
                throw new NullReferenceException();

            bitmapSaver.Save(canvas, true);
        }

        public void PutNextTag(Size tagRectangleSize)
        {
            layouter.PutNextRectangle(tagRectangleSize);
        }

        public void Visualize()
        {
            visualizer.VisualizeCloud();
        }

        public void MarkupVisualize()
        {
            var rectangles = layouter.GetRectangles();
            canvas = CreateCanvas();
            var circleCloudRadius = layouter.GetCloudRadius();
            //RelocateRectangles(rectangles);

            var g = Graphics.FromImage(canvas);
            var imgSize = canvas.Size;
            var cloudCenter = layouter.GetCloudCenter();
            var cloudCircleRadius = layouter.GetCloudRadius();

            visualizer.VisualizeDebuggingMarkup(g, imgSize, cloudCenter, cloudCircleRadius);
        }

        private Bitmap CreateCanvas()
        {
            var canvasSize = layouter.GetRectanglesBoundaryBox();

            return new Bitmap(canvasSize.Width * 2, canvasSize.Height * 2);
        }

        private void RelocateRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var newX = rectangles[i].X + canvas.Width / 2;
                var newY = rectangles[i].Y + canvas.Height / 2;

                var newRect = new Rectangle(new Point(newX, newY), rectangles[i].Size);
                rectangles[i] = newRect;
            }
        }
    }
}