using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class Cloud
    {
        private readonly List<RectangleF> rectangles;
        public IReadOnlyList<RectangleF> Rectangles => rectangles;
        public readonly PointF Center;

        public Cloud(PointF center)
        {
            Center = center;
            rectangles = new List<RectangleF>();
        }

        public void AddRectangle(RectangleF rectangle) 
            => rectangles.Add(rectangle);

        public void DefaultVisualize(string filename)
        {
            var bitmap = new Bitmap
                ((int)(Center.X * 2), (int)(Center.Y * 2));
            var gr = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.DarkGreen, 1);

            gr.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save(filename);
        }

        public void CustomVisualize(string filename,
            Size bitmapSize, 
            List<Color> colors,
            Color backgroundColor,
            bool fillRectangles = false,
            Func<int, List<RectangleF>> getRectanglesByColorIndex = null)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var gr = Graphics.FromImage(bitmap);
            var pens = colors.Select(c => new Pen(c, 1))
                .ToList();
            if(getRectanglesByColorIndex == null) 
                getRectanglesByColorIndex = GetRectanglesByColorIndexDefaultFunc(colors);

            gr.FillRectangle(new SolidBrush(backgroundColor), 
                new Rectangle(new Point(), bitmapSize));
            VisualiseCenter(gr);
            VisualiseRectangles(fillRectangles, getRectanglesByColorIndex, pens, gr);

            bitmap.Save(filename);
        }

        private void VisualiseCenter(Graphics gr)
        {
            var brush = new SolidBrush(Color.Black);
            var centerRect = RectangleFExtensions
                .GetRectangleByCenter(new Size(1, 1), Center);
            gr.FillEllipse(brush, centerRect);
        }

        private void VisualiseRectangles(bool fillRectangles,
            Func<int, List<RectangleF>> getRectanglesByColorIndex,
            List<Pen> pens,
            Graphics gr)
        {
            if (fillRectangles)
                for (var i = 0; i < pens.Count; i++)
                    gr.FillRectangles(new SolidBrush(pens[i].Color),
                        getRectanglesByColorIndex(i).ToArray());
            else
                for (var i = 0; i < pens.Count; i++)
                    gr.DrawRectangles(pens[i], getRectanglesByColorIndex(i).ToArray());
        }

        private Func<int, List<RectangleF>> GetRectanglesByColorIndexDefaultFunc
            (List<Color> colors)
        {
            return colorIndex =>
                rectangles.Where((t, i) => i % colors.Count == colorIndex)
                    .ToList();
        }
    }
}
