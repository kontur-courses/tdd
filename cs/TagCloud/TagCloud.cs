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
        private readonly IBitmapToDesktopSaver bitmapSaver;
        private readonly ICloudLayouter layouter;
        private readonly IVisualizer visualizer;

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
            InitCanvasIfNotExist();

            var g = Graphics.FromImage(canvas);
            var cloudCenter = layouter.GetCloudCenter();
            var rectangles = RelocateRectangles(layouter.GetRectangles());

            visualizer.VisualizeCloud(g, cloudCenter, rectangles);
        }

        public void VisualizeMarkup()
        {
            InitCanvasIfNotExist();

            var g = Graphics.FromImage(canvas);
            var imgSize = canvas.Size;
            var cloudCenter = layouter.GetCloudCenter();
            var cloudCircleRadius = layouter.GetCloudRadius();

            visualizer.VisualizeDebuggingMarkup(g, imgSize, cloudCenter, cloudCircleRadius);
        }

        private void InitCanvasIfNotExist()
        {
            if (canvas != null)
                return;

            var canvasSize = layouter.GetRectanglesBoundaryBox();
            canvas =  new Bitmap(canvasSize.Width * 2, canvasSize.Height * 2);
        }

        private List<Rectangle> RelocateRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var newX = rectangles[i].X + canvas.Width / 2;
                var newY = rectangles[i].Y + canvas.Height / 2;

                var newRect = new Rectangle(new Point(newX, newY), rectangles[i].Size);
                rectangles[i] = newRect;
            }

            return rectangles;
        }
    }
}