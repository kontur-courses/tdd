using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Tests
{
    public class VisualizationContext
    {
        private readonly HashSet<Rectangle> allRectangles = new HashSet<Rectangle>();

        public HashSet<Rectangle> AllRectangles
        {
            get
            {
                allRectangles.UnionWith(WrongRectangles);
                return allRectangles;
            }
        }

        public HashSet<Rectangle> WrongRectangles { get; } = new HashSet<Rectangle>();

        public bool IsEmpty => allRectangles.Count == 0;

        public Image GetTestVisualization()
        {
            if (IsEmpty)
                throw new Exception($"'{nameof(AllRectangles)}' property is empty - method has nothing to draw.");

            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            using var backgroundBrush = new SolidBrush(Color.Snow);

            graphics.FillRectangle(backgroundBrush, new Rectangle(Point.Empty, bitmap.Size));

            using var simpleRectanglesPen = new Pen(Color.LightGray);
            using var wrongRectanglePen = new Pen(Color.Crimson);

            graphics.DrawRectangles(simpleRectanglesPen, AllRectangles.ToArray());
            graphics.DrawRectangles(wrongRectanglePen, WrongRectangles.ToArray());

            return bitmap;
        }
    }
}