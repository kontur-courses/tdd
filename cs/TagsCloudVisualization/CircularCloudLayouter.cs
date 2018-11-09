using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<RowLayout> layout = new List<RowLayout>();
        private int firstIndex;
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Point Center { get; }
        public IEnumerable<Rectangle> Layout => layout.SelectMany(x=>x.Body);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (TryAddNewRow(rectangleSize,out var rect))
                return rect;

            return layout.Where(x => x.Bounds.Height >= rectangleSize.Height)
                            .OrderBy(x => x.Bounds.Width)
                            .First()
                            .Add(rectangleSize);
        }

        private bool TryAddNewRow(Size rectangleSize,out Rectangle rect)
        {
            if (layout.Count == 0)
            {
                rect = new Rectangle(Center - rectangleSize.Divide(2), rectangleSize);
                layout.Add(new RowLayout(rect));
                firstIndex = 0;
                return true;
            }

            if (CaseForNewRow(rectangleSize.Height))
            {
                var heights = layout.Select(x => x.Bounds.Width).ToArray();
                rect =  heights.Take(firstIndex).Sum() > heights.Skip(firstIndex + 1).Sum() ?
                    AddFirstRow(rectangleSize) : AddLastRow(rectangleSize);
                return true;
            }

            rect = default(Rectangle);
            return false;
        }

        private bool CaseForNewRow(int height) =>
            layout.Sum(x => x.Bounds.Height) < layout.Max(x => x.Bounds.Width) ||
            layout.Max(x => x.Bounds.Height) < height;

        private Rectangle AddLastRow(Size rectangleSize)
        {
            var height = layout.Last().Bounds.Bottom;
            var rect = new Rectangle(new Point(Center.X - rectangleSize.Width / 2, height), rectangleSize);
            layout.Add(new RowLayout(rect));
            firstIndex++;
            return rect;
        }
        
        private Rectangle AddFirstRow(Size rectangleSize)
        {
            var height = layout.First().Bounds.Top - rectangleSize.Height;
            var rect = new Rectangle(new Point(Center.X - rectangleSize.Width / 2, height), rectangleSize);
            layout.Insert(0,new RowLayout(rect));
            return rect;
        }
    }
}
