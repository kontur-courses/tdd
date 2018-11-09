using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    internal class RowLayout
    {
        private Point center;
        private readonly List<Rectangle> body;
        
        public RowLayout(Rectangle initial)
        {
            Bounds = initial;
            body = new List<Rectangle>{initial};
            center = initial.Center();
        }
        
        public Rectangle Bounds { get; private set; }
        public IEnumerable<Rectangle> Body => body;
        //public double Density => Body.Sum(x => x.Size.Area()) / (double)Bounds.Size.Area();

        private bool isLeft;
        public Rectangle Add(Size rectangleSize)
        {
            if (rectangleSize.Height > Bounds.Height)
                throw new ArgumentException("Size does not fit into row.");
            
            Rectangle rect;
            if (isLeft)
            {
                rect = new Rectangle(new Point(Bounds.Left - rectangleSize.Width, Bounds.Top), rectangleSize);
                body.Insert(0, rect);
                Bounds = new []{rect,Bounds}.EnclosingRectangle(); 
            }
            else
            {
                rect = new Rectangle(new Point(Bounds.Right, Bounds.Top), rectangleSize);
                body.Add(rect);
                Bounds = new []{rect,Bounds}.EnclosingRectangle();
            }
            isLeft = !isLeft;
            return rect;
        }
    }
}