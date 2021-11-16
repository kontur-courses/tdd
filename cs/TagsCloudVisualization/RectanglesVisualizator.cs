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

        public void Visualize(RectanglesVisualizatorSettings settings)
        {

            var bitmapSize = settings.bitmapSize;
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var pens = settings.colors.Select(c => new Pen(c, 1))
                .ToList();
            var gr = Graphics.FromImage(bitmap);
            var k = CalculateScaleModifier(bitmapSize, settings.minMargin);

            gr.TranslateTransform(bitmapSize.Width / 2, bitmapSize.Height / 2);
            gr.ScaleTransform(k, k);

            gr.Clear(settings.backgroundColor);
            VisualizeRectangles
                (settings.fillRectangles, settings.getRectanglesByColorIndex, pens, gr);
            VisualizeCenter(gr);

            bitmap.Save(settings.filename);
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
            Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex,
            List<Pen> pens,
            Graphics gr)
        {
            if (fillRectangles)
                for (var i = 0; i < pens.Count; i++)
                {
                    gr.FillRectangles(new SolidBrush(pens[i].Color),
                        GetRectanglesToDraw(getRectanglesByColorIndex, i));

                }
            else
                for (var i = 0; i < pens.Count; i++)
                {
                    gr.DrawRectangles(pens[i],
                        GetRectanglesToDraw(getRectanglesByColorIndex, i));
                }
        }

        private RectangleF[] GetRectanglesToDraw
            (Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex, int i)
            => getRectanglesByColorIndex(i, cloud)
                .Select(r => new RectangleF(r.Location, r.Size))
                .ToArray();
    }
}
