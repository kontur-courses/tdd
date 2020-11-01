
using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private Point center;
        private int spiralPitch;
        private double angleStepRadian;
        private double currentAngle;
        
        public Spiral(Point center, int spiralPitch, double angleStepRadian)
        {
            this.center = center;
            this.spiralPitch = spiralPitch;
            this.angleStepRadian = angleStepRadian;

            currentAngle = 0;
        }

        public Point GetNextPoint()
        {
            var distance = getDistance();
            var point = convertFromPolarToPoint(distance, currentAngle);
            currentAngle += angleStepRadian;
            return point;
        }

        private double getDistance()
        {
            return currentAngle * spiralPitch / (2 * Math.PI);
        }

        private Point convertFromPolarToPoint(double distance, double angleRadian)
        {
            var pointX = center.X 
                         + Convert.ToInt32(Math.Round(distance * Math.Cos(angleRadian)));
            var pointY = center.Y 
                         + Convert.ToInt32(Math.Round(distance * Math.Sin(angleRadian)));
            return new Point(pointX, pointY);
        }
    }
}