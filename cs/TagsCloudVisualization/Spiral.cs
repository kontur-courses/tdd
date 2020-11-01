
using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private Point center;
        private int spiralPitch;
        private double angleStepRadian;
        
        
        public Spiral(Point center, int spiralPitch, double angleStepRadian)
        {
            this.center = center;
            this.spiralPitch = spiralPitch;
            this.angleStepRadian = angleStepRadian;
        }

        public Point GetNextPoint()
        {
            throw new NotImplementedException();
        }
    }
}