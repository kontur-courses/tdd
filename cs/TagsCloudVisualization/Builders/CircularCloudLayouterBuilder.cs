using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterBuilder
    {
        private Point center;
        private float? densityParameter;
        private int? degreesDelta;
        
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
        
        public CircularCloudLayouterBuilder WithDegreesDelta(int degreesDelta)
        {
            this.degreesDelta = degreesDelta;
            return this;
        }
        
        private CircularCloudLayouterBuilder WithDensityParameter(float? densityParameter)
        {
            this.densityParameter = densityParameter;
            return this;
        }
        
        private CircularCloudLayouterBuilder WithDegreesDelta(int? degreesDelta)
        {
            this.degreesDelta = degreesDelta;
            return this;
        }

        public CircularCloudLayouterBuilder But()
        {
            return new CircularCloudLayouterBuilder()
                .WithDensityParameter(densityParameter)
                .WithDegreesDelta(degreesDelta)
                .WithCenterAt(center);
        }

        public CircularCloudLayouter Build()
        {
            if (degreesDelta is null && densityParameter is null)
                return new CircularCloudLayouter(center);

            var spiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(center);
            if (degreesDelta != null)
                spiral.WithDegreesDelta((int) degreesDelta);
            if (densityParameter != null)
                spiral.WithDensityParameter((float) densityParameter);

            return new CircularCloudLayouter(spiral.Build());
        }
    }
}