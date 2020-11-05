using System;
using System.Drawing;

namespace TagsCloudVisualisation
{
    partial class CircularCloudLayouter
    {
        private class CandidatePoint
        {
            public CandidatePoint(int x, int y, Point cloudCenter, PointDirection direction)
            {
                X = x;
                Y = y;
                Direction = direction;
                var relativeX = x - cloudCenter.X;
                var relativeY = y - cloudCenter.Y;
                CloudCenterDistance = Math.Sqrt(relativeX * relativeX + relativeY * relativeY);
            }

            public int X { get; }
            public int Y { get; }
            public double CloudCenterDistance { get; }
            public PointDirection Direction { get; }
        }

        private enum PointDirection
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}