using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class BasePointsCloudLayouter_Should : ICloudLayouter_Should
    {
        protected override ICloudLayouter CreateLayouterInstance(Point center)
        {
            return new BasePointsCloudLayouter(center);
        }
    }
}