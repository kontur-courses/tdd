using System;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagsCloudVisualizationTests")]
namespace TagsCloudVisualization.Layouters
{
    internal class Spiral
    {
        private float radius = 0;
        private float angle = 0;

        private readonly Point center;

        public PointF CurrentPoint =>
            new ((float) Math.Cos(angle) * radius + center.X,
                (float) Math.Sin(angle) * radius + center.Y);


        public Spiral(Point center)
        {
            this.center = center;
        }

        public void IncreaseSize(float radiusIncreaseValue = 0.1f, float angleIncreaseValue = 0.1f)
        {
            radius += radiusIncreaseValue;
            angle += angleIncreaseValue;
        }
    }
}