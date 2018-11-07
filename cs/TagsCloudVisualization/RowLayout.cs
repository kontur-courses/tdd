using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RowLayout
    {
        private Point center;
        
        public RowLayout(Rectangle initial)
        {
            Bounds = initial;
            Body = new List<Rectangle>{initial};
            center = initial.Center();
        }
        
        public Rectangle Bounds { get; private set; }
        public List<Rectangle> Body { get; }
        public double Fillness => Body.Select(x => x.Size.Space()).Sum() / (double)Bounds.Size.Space();

        private bool isLeft;
        public Rectangle Add(Size rectangleSize)
        {
            Rectangle rect;
            if (isLeft)
            {
                rect = new Rectangle(Bounds.Left - rectangleSize.Width, Bounds.Top,
                    rectangleSize.Width, rectangleSize.Height);
                Body.Insert(0, rect);
                //TODO Refactor
                Bounds = new Rectangle(new Point(Bounds.X - rectangleSize.Width, Bounds.Y),
                                       new Size(Bounds.Width + rectangleSize.Width, Bounds.Height));
            }
            else
            {
                rect = new Rectangle(Bounds.Right, Bounds.Top, rectangleSize.Width,
                    rectangleSize.Height);
                Body.Add(rect);
                //TODO Refactor
                Bounds = new Rectangle(Bounds.Location,new Size(Bounds.Width+rectangleSize.Width,Bounds.Height));
            }
            isLeft = !isLeft;
            return rect;
        }
    }
}