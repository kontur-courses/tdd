using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class RectanglesVisualizatorSettings
    {
        public readonly string filename;
        public readonly Size bitmapSize;
        public readonly List<Color> colors;
        public readonly Color backgroundColor;
        public readonly float minMargin;
        public readonly bool fillRectangles;
        public readonly Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex;

        public RectanglesVisualizatorSettings(string filename) : 
            this(filename, new Size(800,800))
        {

        }

        public RectanglesVisualizatorSettings(string filename, Size bitmapSize) :
            this(filename, bitmapSize, new List<Color> {Color.DarkGreen})
        {

        }

        public RectanglesVisualizatorSettings(string filename,
            Size bitmapSize,
            List<Color> colors) :
            this(filename, bitmapSize, colors, Color.White)
        {

        }

        public RectanglesVisualizatorSettings(string filename,
            Size bitmapSize,
            List<Color> colors,
            Color backgroundColor,
            float minMargin = 10,
            bool fillRectangles = false,
            Func<int, IRectanglesCloud, List<RectangleF>> getRectanglesByColorIndex = null)
        {
            this.filename = filename;
            this.bitmapSize = bitmapSize;
            this.colors = colors;
            this.backgroundColor = backgroundColor;
            this.minMargin = minMargin;
            this.fillRectangles = fillRectangles;
            if (getRectanglesByColorIndex == null)
                getRectanglesByColorIndex = GetRectanglesByColorIndexDefaultFunc(colors);
            this.getRectanglesByColorIndex = getRectanglesByColorIndex;

        }

        private Func<int, IRectanglesCloud, List<RectangleF>> GetRectanglesByColorIndexDefaultFunc
            (List<Color> colors)
        {
            return (colorIndex, cloud) =>
                cloud.Rectangles.Where((t, i) => i % colors.Count == colorIndex)
                    .ToList();
        }
    }
}
