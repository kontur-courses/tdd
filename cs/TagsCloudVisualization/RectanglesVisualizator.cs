using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class RectanglesVisualizator
    {
        private readonly IRectanglesCloud cloud;

        public RectanglesVisualizator(IRectanglesCloud cloud)
        {
            this.cloud = cloud;
        }

        public void DefaultVisualize(string filename)
        {
            var size = new Size(800, 800);
            var colors = new List<Color> { Color.DarkGreen };
            CustomVisualize(filename, size, colors, Color.White);
        }

        public void CustomVisualize(string filename,
            Size bitmapSize,
            List<Color> colors,
            Color backgroundColor,
            bool fillRectangles = false,
            Func<int, List<RectangleF>> getRectanglesByColorIndex = null)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var referenceCenter = GetCloudCenterOnImage(bitmap);
            var pens = colors.Select(c => new Pen(c, 1))
                .ToList();
            if (getRectanglesByColorIndex == null)
                getRectanglesByColorIndex = GetRectanglesByColorIndexDefaultFunc(colors);
            var gr = Graphics.FromImage(bitmap);

            gr.Clear(backgroundColor);
            VisualizeCenter(gr, referenceCenter);
            VisualizeRectangles(fillRectangles, getRectanglesByColorIndex, referenceCenter, pens, gr);

            bitmap.Save(filename);
        }

        private PointF GetCloudCenterOnImage(Bitmap image)
        {
            var imageCenter = new PointF(image.Width / 2, image.Height / 2);
            return new PointF(imageCenter.X + cloud.Center.X, imageCenter.Y + cloud.Center.Y);
        }

        private void VisualizeCenter(Graphics gr, PointF refCenter)
        {
            var brush = new SolidBrush(Color.Black);
            var centerRect = RectangleFExtensions
                .GetRectangleByCenter(new Size(1, 1), refCenter);
            gr.FillEllipse(brush, centerRect);
        }

        private void VisualizeRectangles(bool fillRectangles,
            Func<int, List<RectangleF>> getRectanglesByColorIndex,
            PointF refCenter,
            List<Pen> pens,
            Graphics gr)
        {
            if (fillRectangles)
                for (var i = 0; i < pens.Count; i++)
                    gr.FillRectangles(new SolidBrush(pens[i].Color),
                        GetRectanglesToDraw(getRectanglesByColorIndex, refCenter, i));
            else
                for (var i = 0; i < pens.Count; i++)
                    gr.DrawRectangles(pens[i],
                        GetRectanglesToDraw(getRectanglesByColorIndex, refCenter, i));
        }

        private Func<int, List<RectangleF>> GetRectanglesByColorIndexDefaultFunc
            (List<Color> colors)
        {
            return colorIndex =>
                cloud.Rectangles.Where((t, i) => i % colors.Count == colorIndex)
                    .ToList();
        }

        private RectangleF[] GetRectanglesToDraw(Func<int, List<RectangleF>> getRectanglesByColorIndex,
            PointF refCenter,
            int i)
            => getRectanglesByColorIndex(i)
                .Select(r => new RectangleF(
                    (r.Location.ToVector() + refCenter.ToVector()).ToPointF(), r.Size))
                .ToArray();
    }
}
