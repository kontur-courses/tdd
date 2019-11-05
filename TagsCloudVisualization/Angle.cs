using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class Angle
    {
        public Point coordinates;
        public int xLen;
        public int yLen;
        public bool isInner;
        public Angle(Point coord, int xLen, int yLen, bool isInner)
        {
            this.coordinates = coord;
            this.xLen = xLen;
            this.yLen = yLen;
            this.isInner = isInner;
        }

        public Quadrant GetQuadrant()
        {
            if (xLen > 0 && yLen > 0)
                return Quadrant.Fourth;
            else if (xLen < 0 && yLen > 0)
                return Quadrant.Third;
            else if (xLen < 0 && yLen < 0)
                return Quadrant.Second;
            else
                return Quadrant.First;
        }
    }

    public enum Quadrant
    {
        First=1,
        Second,
        Third,
        Fourth
    }
}
