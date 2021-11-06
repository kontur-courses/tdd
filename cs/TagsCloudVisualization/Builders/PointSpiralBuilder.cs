using System.Drawing;

namespace TagsCloudVisualization
{
    public class PointSpiralBuilder
    {
        private Point center;
        private float densityParameter = 1;
        private int degreesParameter = 1;

        public static PointSpiralBuilder APointSpiral()
        {
            return new PointSpiralBuilder();
        }

        public PointSpiralBuilder WithCenter(Point center)
        {
            this.center = center;
            return this;
        }
        
        public PointSpiralBuilder WithDensityParameter(float densityParameter)
        {
            this.densityParameter = densityParameter;
            return this;
        }
        
        public PointSpiralBuilder WithDegreesParameter(int degreesParameter)
        {
            this.degreesParameter = degreesParameter;
            return this;
        }
        
        public PointSpiralBuilder But()
        {
            return APointSpiral()
                .WithCenter(center)
                .WithDegreesParameter(degreesParameter)
                .WithDensityParameter(densityParameter);
        }
        
        public PointSpiral Build()
        {
            return new PointSpiral(center, densityParameter, degreesParameter);
        }
    }
}