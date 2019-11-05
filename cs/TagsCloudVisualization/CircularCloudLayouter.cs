using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class MyCell
    {
        public MyCell Next { get; set; }
        public Rectangle Place { get; set; }
        private int LowLever { get; set; } 
       public List<Rectangle> IternalRectangles { get; set; }

       public MyCell(int x, int y,int width,int height)
       {
            Place=new Rectangle(x,y,width,height);
            LowLever = Place.Y + Place.Height;
       }
       public Rectangle? ToPutOne(Size rectangleSize)
       {
          if(rectangleSize.Width>Place.Width||rectangleSize.Height>Place.Height)
              throw new ArgumentException();
          if (LowLever - Place.Y >= rectangleSize.Height)
          {
              LowLever -= rectangleSize.Height;
              return new Rectangle(Place.X,LowLever,rectangleSize.Width,rectangleSize.Height);
          }
          return null;
       }
    }

    public class CircularCloudLayouter
    {
        public MyCell Head { get; private set; }
        public MyCell[,] Map { get; set; }
        public List<Rectangle> Rectangles { get; private set; }
        public Point Center { get; private set; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            Head = new MyCell(Center.X,Center.Y,30,30);
            Map=new MyCell[21,21];
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var current = Head;
        }
    }
}
