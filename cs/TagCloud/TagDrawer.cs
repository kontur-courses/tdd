using System;
using System.Collections.Generic;
//пришлось скачивать через нугет так как на нетКоре не было битмапа)
using System.Drawing;

namespace TagCloud
{
    class TagDrawer
    {
        public static void Draw(string fileName, CircularCloudLayouter layouter)
        {
            var pens = new List<Pen>
            {
                new Pen(new SolidBrush(Color.Blue)),
                new Pen(new SolidBrush(Color.Green)),
                new Pen(new SolidBrush(Color.Black)),
                new Pen(new SolidBrush(Color.Red)),
                new Pen(new SolidBrush(Color.Yellow)),
                new Pen(new SolidBrush(Color.Magenta))
            };
            var border = 10;
            var width = layouter.SizeOfCloud.Width + border;
            var height = layouter.SizeOfCloud.Height + border;
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            var whiteRectangle =new Rectangle(0, 0, width, height);
            graphics.FillRegion(new SolidBrush(Color.White), new Region(whiteRectangle));

            var penTeration = 0;
            layouter.Rectangles.ForEach(rect =>
            {
                rect.Offset(new Point(border/2-layouter.LeftDownPointOfCloud.X, border/2-layouter.LeftDownPointOfCloud.Y));
                penTeration = penTeration >= pens.Count - 1 ? 0 : penTeration + 1;
                graphics.DrawRectangle(pens[penTeration], rect);
            });
            bitmap.Save(fileName);
        }
    }
}