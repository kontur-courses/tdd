using System;
using System.Drawing;

namespace TagCloud2.Interfaces
{
    public interface ITagCloduBitmapDrawer
    {
        public void DrawRectangles(Rectangle[] rectangles);
        public void DrawTags(Rectangle[] rectangles, Tuple<string, int>[] tags);
        public SizeF GetStringInRectangleSize(string s, int fontSize);
        public Bitmap Bitmap { get; }
    }
}