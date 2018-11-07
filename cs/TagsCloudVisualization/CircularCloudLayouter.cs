using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        //TODO remove public
        public readonly List<RowLayout> layout = new List<RowLayout>();
        private int firstIndex;
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Point Center { get; }
        public IEnumerable<Rectangle> Layout => layout.SelectMany(x=>x.Body);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle? rect = TryAddNewRow(rectangleSize);
            if (rect != null) //TODO Remove rect variable
                return rect.Value;

            return layout.Where(x => x.Bounds.Height >= rectangleSize.Height)
                            .OrderBy(x => x.Bounds.Width)
                            .First()
                            .Add(rectangleSize);
        }

        private Rectangle? TryAddNewRow(Size rectangleSize)
        {
            if (layout.Count == 0)
            {
                var rect = new Rectangle(Center - rectangleSize.Divide(2), rectangleSize);
                layout.Add(new RowLayout(rect));
                firstIndex = 0;
                return rect;
            }

            if (layout.Sum(x => x.Bounds.Height) < layout.Max(x => x.Bounds.Width) ||
                layout.Max(x => x.Bounds.Height) < rectangleSize.Height)
            {
                var heights = layout.Select(x => x.Bounds.Width).ToArray();
                return heights.Take(firstIndex).Sum() > heights.Skip(firstIndex + 1).Sum() ?
                    AddFirstRow(rectangleSize) : AddLastRow(rectangleSize);
            }
            return null;
        }

        private Rectangle AddLastRow(Size rectangleSize)
        {
            var height = layout.Last().Bounds.Bottom;
            var rect = new Rectangle(Center.X - rectangleSize.Width / 2, height, rectangleSize.Width, rectangleSize.Height);
            layout.Add(new RowLayout(rect));
            firstIndex++;
            return rect;
        }
        //TODO DRY Maybe..
        private Rectangle AddFirstRow(Size rectangleSize)
        {
            var height = layout.First().Bounds.Top - rectangleSize.Height;
            var rect = new Rectangle(Center.X - rectangleSize.Width / 2, height, rectangleSize.Width, rectangleSize.Height);
            layout.Insert(0,new RowLayout(rect));
            return rect;
        }
    }
}
