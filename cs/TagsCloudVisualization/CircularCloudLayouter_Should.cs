using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should : ICloudLayouter_Should
    {
        protected override ICloudLayouter CreateLayouterInstance(Point center)
        {
            return new CircularCloudLayouter(center);
        }
    }
}