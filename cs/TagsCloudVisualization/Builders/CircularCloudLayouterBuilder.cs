using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterBuilder
    {
        private Point center;
        
        public static CircularCloudLayouterBuilder ACircularCloudLayouter()
        {
            return new CircularCloudLayouterBuilder();
        }

        public CircularCloudLayouterBuilder WithCenterAt(Point position)
        {
            center = position;
            return this;
        }

        public CircularCloudLayouterBuilder But()
        {
            return new CircularCloudLayouterBuilder()
                .WithCenterAt(center);
        }

        public CircularCloudLayouter Build()
        {
            return new CircularCloudLayouter(center);
        }
    }
}