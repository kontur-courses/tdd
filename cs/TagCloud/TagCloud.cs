using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagCloud
{
    public class TagCloud
    {
        public Point Center { get; private set; }

        public List<Rectangle> Rectangles { get; private set; }

        public TagCloud(Point center)
        {
            Center = center;

            Rectangles = new List<Rectangle>();
        }

        public int GetWidth()
        {
            if (Rectangles.Count == 0)
                return 0;

            return Rectangles.Max(r => r.Right) - Rectangles.Min(r => r.Left);
        }

        public int GetHeight()
        {
            if (Rectangles.Count == 0)
                return 0;

            return Rectangles.Min(r => r.Bottom) - Rectangles.Max(r => r.Top);
        }
    }
}
