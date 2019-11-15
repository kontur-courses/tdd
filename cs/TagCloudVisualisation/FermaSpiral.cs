using System;
using System.Drawing;

namespace TagCloudVisualization
{
   public class FermaSpiral
    {
        private readonly float coefficient;
        private readonly Point center;
        private int spiralCounter;

        public FermaSpiral(float coefficient, Point center)
        {
            this.coefficient = coefficient;
            this.center = center;
            spiralCounter = 0;
        }

        public Point GetSpiralNext()
        {
            var x = coefficient * MathF.Pow(spiralCounter, 1f / 2) * MathF.Cos(spiralCounter);
            var y = coefficient * MathF.Pow(spiralCounter, 1f / 2) * MathF.Sin(spiralCounter);
            var pos = new Point((int)(x + center.X), (int)(y + center.Y));
            spiralCounter++;
            return pos;
        }

        public void Reset()
        {
            spiralCounter = 0;
        }
    }
}