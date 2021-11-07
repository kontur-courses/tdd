using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterBuilder
    {
        private Point center;
        private float? densityParameter;
        private int? degreesParameter;
        
        public static CircularCloudLayouterBuilder ACircularCloudLayouter()
        {
            return new CircularCloudLayouterBuilder();
        }

        public CircularCloudLayouterBuilder WithCenterAt(Point position)
        {
            center = position;
            return this;
        }
        
        public CircularCloudLayouterBuilder WithDensityParameter(float densityParameter)
        {
            this.densityParameter = densityParameter;
            return this;
        }
        
        public CircularCloudLayouterBuilder WithDegreesParameter(int degreesParameter)
        {
            this.degreesParameter = degreesParameter;
            return this;
        }
        
        private CircularCloudLayouterBuilder WithDensityParameter(float? densityParameter)
        {
            this.densityParameter = densityParameter;
            return this;
        }
        
        private CircularCloudLayouterBuilder WithDegreesParameter(int? degreesParameter)
        {
            this.degreesParameter = degreesParameter;
            return this;
        }

        public CircularCloudLayouterBuilder But()
        {
            return new CircularCloudLayouterBuilder()
                .WithDensityParameter(densityParameter)
                .WithDegreesParameter(degreesParameter)
                .WithCenterAt(center);
        }

        public CircularCloudLayouter Build()
        {
            if (degreesParameter is null && densityParameter is null)
                return new CircularCloudLayouter(center);

            var spiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(center);
            if (degreesParameter != null)
                spiral.WithDegreesParameter((int) degreesParameter);
            if (densityParameter != null)
                spiral.WithDensityParameter((float) densityParameter);

            return new CircularCloudLayouter(spiral.Build());
        }
    }
}