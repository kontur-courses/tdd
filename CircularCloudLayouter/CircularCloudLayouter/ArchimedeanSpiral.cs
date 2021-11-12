using System;
using System.Drawing;

namespace TagsCloudVisualizer
{
    class ArchimedeanSpiral
    {
        private double currentAngle = 0;
        private readonly double angleSpeed;
        private double currentRadius;
        private readonly double linearSpeed;
        private Point center;
        public Point CurrentPoint { get; private set; }
        public ArchimedeanSpiral(Point center, double angleSpeed = 0.108, double linearSpeed = 0.032)
        {
            if (angleSpeed == 0 && linearSpeed == 0)
            {
                throw new ArgumentException();
            }
            this.linearSpeed = linearSpeed;
            this.angleSpeed = angleSpeed;
            this.center = center;
        }

        public Point GetNextPoint()
        {
            CurrentPoint = GeometryHelper
                .ConvertFromPolarToDecartWithFlooring(currentAngle, currentRadius)
                .Displace(center.X, center.Y);
            currentRadius += linearSpeed;
            currentAngle += angleSpeed;
            return CurrentPoint;
        }
    }
}
