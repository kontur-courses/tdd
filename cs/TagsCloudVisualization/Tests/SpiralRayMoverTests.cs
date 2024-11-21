using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.CloudClasses;

namespace TagsCloudVisualization.Tests
{
    public class SpiralRayMoverTests
    {
        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenNotValidSteps_ThrowArgumentException(int radiusStep, int angleSter)
        {
            Assert.Throws<ArgumentException>(() => new SpiralRayMover(new Point(10, 10), radiusStep, angleSter),
                "radiusStep and angleStep should be positive");
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenCenterWrong_ThrowArgumentException(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => new SpiralRayMover(new Point(x, y), 1, 5),
                "SpiralRayMover center Point should have positive X and Y");
        }
    }
}
