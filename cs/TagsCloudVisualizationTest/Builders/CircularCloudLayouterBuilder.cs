using System.Drawing;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualizationTest.Builders
{
    public class CircularCloudLayouterBuilder
    {
        private Point center;
        private double? densityParameter;
        private int? degreesDelta;

        private readonly PointSpiralBuilder pointSpiralBuilder;

        private CircularCloudLayouterBuilder()
        {
            pointSpiralBuilder = PointSpiralBuilder.APointSpiral();
        }
        
        public static CircularCloudLayouterBuilder ACircularCloudLayouter()
        {
            return new CircularCloudLayouterBuilder();
        }

        public CircularCloudLayouterBuilder WithCenterAt(Point position)
        {
            center = position;
            return this;
        }
        
        public CircularCloudLayouterBuilder WithDensityParameter(double density)
        {
            densityParameter = density;
            pointSpiralBuilder.WithDensityParameter(density);
            return this;
        }
        
        public CircularCloudLayouterBuilder WithDegreesDelta(int degreesDeltaParameter)
        {
            degreesDelta = degreesDeltaParameter;
            pointSpiralBuilder.WithDegreesDelta(degreesDeltaParameter);
            return this;
        }

        public CircularCloudLayouter Build()
        {
            return degreesDelta is null && densityParameter is null
                ? new CircularCloudLayouter(center)
                : new CircularCloudLayouter(pointSpiralBuilder.Build());
        }
    }
}