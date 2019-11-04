using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{

    public class CircularCloudLayouter:ITagsCloud
    {
        private class Angle
        {
            public Point coordinates;
            public int xLen;
            public int yLen;
            public bool isSquareInAngle;
            public Angle(Point coord, int xLen, int yLen, bool isSquareInAngle)
            {
                this.coordinates = coord;
                this.xLen = xLen;
                this.yLen = yLen;
                this.isSquareInAngle = isSquareInAngle;
            }
        }

        private Point center;
        private List<Rectangle> rectangles;
        private HashSet<Angle> angles;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.rectangles = new List<Rectangle>();
            this.angles = new HashSet<Angle>();
            angles.Add(new Angle(new Point(0,0), int.MaxValue, int.MaxValue, false));
            angles.Add(new Angle(new Point(0, 0), int.MaxValue, int.MinValue+1, false));
            angles.Add(new Angle(new Point(0, 0), int.MinValue + 1, int.MaxValue, false));
            angles.Add(new Angle(new Point(0, 0), int.MinValue + 1, int.MinValue+1, false));
            // added four initial angles
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var properAngle = FindMostSuitableAngle(rectangleSize);
            int xCoord;
            int yCoord;
            if (!properAngle.isSquareInAngle)
            {
                angles.RemoveWhere(a=>(a.coordinates==properAngle.coordinates && a.xLen==properAngle.xLen && a.yLen==properAngle.yLen));
                if (properAngle.xLen < 0)
                    xCoord = properAngle.coordinates.X - rectangleSize.Width;
                else
                    xCoord = properAngle.coordinates.X;
                if (properAngle.yLen < 0)
                    yCoord = properAngle.coordinates.Y - rectangleSize.Height;
                else
                    yCoord = properAngle.coordinates.Y;
                return new Rectangle(new Point(xCoord, yCoord), rectangleSize);
            }
            else
                throw new NotImplementedException();
        }

        private double distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - center.Y), 2));
        }

        private Angle FindMostSuitableAngle(Size rectangleSize)
        {
            return angles
                .Where(a=>(Math.Abs(a.xLen)>rectangleSize.Width && Math.Abs(a.yLen)>rectangleSize.Height))
                .OrderBy(a => distance(a.coordinates, center))
                .First();
        }

    }
}
