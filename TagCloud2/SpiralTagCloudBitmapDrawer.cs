using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud2.Interfaces;

namespace TagCloud2
{
    public class SpiralTagCloudBitmapDrawer : ITagCloudBitmapDrawer
    {
        public Bitmap Bitmap { get; }
        private readonly Graphics _graphics;
        private readonly string _fontName;
        

        public SpiralTagCloudBitmapDrawer(Size size, string fontName)
        {
            _fontName = fontName;
            Bitmap = new Bitmap(size.Width, size.Height);
            _graphics = Graphics.FromImage(Bitmap);
        }

        public void DrawRectangles(Rectangle[] rectangles)
        {
            _graphics.DrawRectangles(Pens.Black, rectangles);
        }
        
        public void DrawRectanglesWithTags(Rectangle[] rectangles, Tuple<string, int>[] tags)
        {
            for (var i = 0; i < rectangles.Length; i++)
            {
                _graphics.DrawRectangle(Pens.Black, rectangles[i]);
                _graphics.DrawString(tags[i].Item1, new Font("Consolas", tags[i].Item2), Brushes.Blue, rectangles[i]);
            }
        }

        public SizeF GetStringInRectangleSize(string s, int fontSize)
        {
            var sizeF = _graphics.MeasureString(s, new Font(_fontName, fontSize));
            return new SizeF(sizeF.Width + 1, sizeF.Height + 1);
        }
    }
}