using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class RectanglesVisualizator
    {
        public void Visualize(RectanglesVisualizatorSettings settings, IRectanglesCloud cloud)
        {
            var bitmapSize = settings.bitmapSize;
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var pens = settings.colors.Select(c => new Pen(c, 1))
                .ToList();
            var gr = Graphics.FromImage(bitmap);
            var k = CalculateScaleModifier(cloud, bitmapSize, settings.minMargin);

            gr.TranslateTransform(bitmapSize.Width / 2, bitmapSize.Height / 2);
            gr.ScaleTransform(k, k);

            gr.Clear(settings.backgroundColor);
            VisualizeRectangles
                (settings.fillRectangles, settings.getRectanglesByColorIndex, cloud, pens, gr);
            VisualizeCenter(cloud, gr);

            bitmap.Save(settings.filename);
        }

        private float CalculateScaleModifier(IRectanglesCloud cloud, Size bitmapSize, float minMargin)
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

        private void VisualizeCenter(IRectanglesCloud cloud,Graphics gr)
        {
            var brush = new SolidBrush(Color.White);
            var centerRect = RectangleFExtensions
                .GetRectangleByCenter(new Size(8, 8), cloud.Center);
            gr.FillEllipse(brush, centerRect);
        }

        private void VisualizeRectangles(bool fillRectangles,
            Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex,
            IRectanglesCloud cloud,
            List<Pen> pens,
            Graphics gr)
        {
            if (fillRectangles)
                for (var i = 0; i < pens.Count; i++)
                {
                    gr.FillRectangles(new SolidBrush(pens[i].Color),
                        GetRectanglesToDraw(getRectanglesByColorIndex, cloud, i));

                }
            else
                for (var i = 0; i < pens.Count; i++)
                {
                    gr.DrawRectangles(pens[i],
                        GetRectanglesToDraw(getRectanglesByColorIndex, cloud, i));
                }
        }

        private RectangleF[] GetRectanglesToDraw
            (Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex,
                IRectanglesCloud cloud,
                int i)
            => getRectanglesByColorIndex(i, cloud)
                .Select(r => new RectangleF(r.Location, r.Size))
                .ToArray();
    }
}
