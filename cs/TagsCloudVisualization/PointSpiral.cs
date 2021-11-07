﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class PointSpiral : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly float dtheta;
        private readonly float densityParameter;

        public PointSpiral(Point center, float densityParameter = 1, int degreesParameter = 5)
        {
            if (densityParameter <= 0f)
                throw new ArgumentException("densityParameter should be positive");
            if (degreesParameter <= 0)
                throw new ArgumentException("degreesParameter should be positive");
            
            this.center = center;
            this.densityParameter = densityParameter;
            
            dtheta = (float)(degreesParameter * Math.PI / 180);
        }

        public IEnumerable<Point> GetPoints()
        {
            var theta = 0f;
            
            while (true)
            {
                var radius = densityParameter * theta;
                yield return new Point(
                    (int)(radius * Math.Cos(theta)) + center.X,
                    (int)(radius * Math.Sin(theta)) + center.Y
                );
                theta += dtheta;
            }
        }

        public IEnumerator<Point> GetEnumerator() => GetPoints().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}