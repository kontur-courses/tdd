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
            float minMargin = 10,
            bool fillRectangles = false,
            Func<int, List<RectangleF>> getRectanglesByColorIndex = null)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var pens = colors.Select(c => new Pen(c, 1))
                .ToList();
            if (getRectanglesByColorIndex == null)
                getRectanglesByColorIndex = GetRectanglesByColorIndexDefaultFunc(colors);
            var gr = Graphics.FromImage(bitmap);
            var k = CalculateScaleModifier(bitmapSize, minMargin);

            gr.TranslateTransform(bitmapSize.Width / 2, bitmapSize.Height / 2);
            gr.ScaleTransform(k, k);

            gr.Clear(backgroundColor);
            VisualizeRectangles(fillRectangles, getRectanglesByColorIndex, pens, gr);
            VisualizeCenter(gr);

            bitmap.Save(filename);
        }

        private float CalculateScaleModifier(Size bitmapSize, float minMargin)
        {
            var cloudBoundingRectangle = cloud.GetCloudBoundingRectangle();

            var imageBoundingRectangle = RectangleFExtensions
                .GetRectangleByCenter(bitmapSize, new PointF());
            var offset = imageBoundingRectangle
                .GetRectanglesBoundsMaxOffset(cloudBoundingRectangle);
            var offsetWidth = offset.X + minMargin;
            var offsetHeight = offset.Y + minMargin;
            return Math.Min(1, Math.Min(
                bitmapSize.Width / (bitmapSize.Width + 2 * offsetWidth),
                bitmapSize.Height / (bitmapSize.Height + 2 * offsetHeight)));
        }

        private void VisualizeCenter(Graphics gr)
        {
            var brush = new SolidBrush(Color.White);
            var centerRect = RectangleFExtensions
                .GetRectangleByCenter(new Size(8, 8), cloud.Center);
            gr.FillEllipse(brush, centerRect);
        }

        private void VisualizeRectangles(bool fillRectangles,
            Func<int, List<RectangleF>> getRectanglesByColorIndex,
            List<Pen> pens,
            Graphics gr)
        {
            if (fillRectangles)
                for (var i = 0; i < pens.Count; i++)
                    gr.FillRectangles(new SolidBrush(pens[i].Color),
                        GetRectanglesToDraw(getRectanglesByColorIndex, i));
            else
                for (var i = 0; i < pens.Count; i++)
                    gr.DrawRectangles(pens[i],
                        GetRectanglesToDraw(getRectanglesByColorIndex, i));
        }

        private Func<int, List<RectangleF>> GetRectanglesByColorIndexDefaultFunc
            (List<Color> colors)
        {
            return colorIndex =>
                cloud.Rectangles.Where((t, i) => i % colors.Count == colorIndex)
                    .ToList();
        }

        private RectangleF[] GetRectanglesToDraw
            (Func<int, List<RectangleF>> getRectanglesByColorIndex, int i)
            => getRectanglesByColorIndex(i)
                .Select(r => new RectangleF(r.Location, r.Size))
                .ToArray();
    }
}
