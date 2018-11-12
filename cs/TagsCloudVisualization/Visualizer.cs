using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        public static Bitmap GetImageFromRectangles(List<Rectangle> rects, Size canvasSize, bool isDrawRectNumber = false)
        {
            var image = new Bitmap(canvasSize.Width, canvasSize.Height);
            var canvas = Graphics.FromImage(image);
            canvas.FillRectangle(Brushes.Black, 0, 0, canvasSize.Width, canvasSize.Height);

            var strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            for (var i = 0; i < rects.Count; i++)
            {
                canvas.FillRectangle(Brushes.DarkSeaGreen, rects[i]);
                canvas.DrawRectangle(new Pen(Brushes.White, 1.0F), rects[i]);

                if (isDrawRectNumber)
                {
                    canvas.DrawString(i.ToString(), new Font("Arial", 8), Brushes.Black,
                        new PointF(rects[i].X + rects[i].Width / 2, rects[i].Y + rects[i].Height / 2), strFormat);
                }
            }
            return image;
        }

        // TODO Добавить создание облака тэгов из набора слов
    }
}