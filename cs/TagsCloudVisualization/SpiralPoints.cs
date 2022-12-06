using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPoints : IEnumerator<Point>
    {
        private Point spiralCenter;
        private RadiusVector lastRadiusVector;
        private int distanceBetweenPoints = 300;
        private int spiralStep = 500;

        public SpiralPoints(Point spiralCenter)
        {
            this.spiralCenter = spiralCenter;
        }

        // public IEnumerable<Point> NextPointGetter()
        // {
        //     GetNextRadiusVector();
        //     yield return GetPointFromRadiusVector();
        // }

        private void GetNextRadiusVector()
        {
            if (lastRadiusVector == null)
            {
                lastRadiusVector = new RadiusVector(15,20);
                return;
            }

            var tangensOfRotationAngle = distanceBetweenPoints / lastRadiusVector.Length;
            var rotationAngel = Math.Atan(tangensOfRotationAngle);
            var lengthAddition = spiralStep / 360 * rotationAngel;
            var newRadiusVector = new RadiusVector(lengthAddition + lastRadiusVector.Length,
                rotationAngel + lastRadiusVector.Angle);
            lastRadiusVector = newRadiusVector;
        }

        private Point GetPointFromRadiusVector()
        {
            var alfa = lastRadiusVector.Angle;
            var r = lastRadiusVector.Length;
            var x = spiralCenter.X + r * Math.Cos(alfa);
            var y = spiralCenter.Y + r * Math.Sin(alfa);
            return new Point((int)x, (int)y);
        }

        public bool MoveNext()
        {
            GetNextRadiusVector();
            
            return true;
        }

        public void Reset()
        {
            lastRadiusVector = null;
        }

        object IEnumerator.Current => Current;

        public Point Current
        {
            get { return GetPointFromRadiusVector(); }
        }

        public void Dispose()
        {
        }
    }
}