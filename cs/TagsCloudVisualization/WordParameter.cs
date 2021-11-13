using System.Drawing;
using FluentAssertions.Equivalency;

namespace TagsCloudVisualization
{
    public class WordParameter
    {
        private RectangleF wordRectangleF;
        public string Word { get; set; }
        public SizeF Size => wordRectangleF.Size;
        public PointF Location => wordRectangleF.Location;
    }
}