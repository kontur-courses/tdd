using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private int currentSpiralAngle;

        private void IncreaseSpiralAngle()
        {
            currentSpiralAngle++;
        }

        public Point GenerateRectangleLocation()
        {
            //For generating rectangle location (left-upper corner)
            //I'm using Archimedean Spiral

            var distanceBetweenTurnings = 1;
            var radius = distanceBetweenTurnings * currentSpiralAngle;

            var x = (int)(radius * Math.Cos(currentSpiralAngle));
            var y = (int)(radius * Math.Sin(currentSpiralAngle));
            IncreaseSpiralAngle();
            return new Point(x,y);
        }


    }
}