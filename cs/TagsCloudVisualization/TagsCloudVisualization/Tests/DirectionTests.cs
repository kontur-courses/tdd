using System;
using NUnit.Framework;


namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class DirectionTests
    {
        [TestCase(ExpectedResult = 0, TestName = "BeZeroWhenTakeFirstValue")]
        public double DirectionShould()
        {
            return new Direction().GetNextDirection();
        }

        [TestCase(ExpectedResult = 1, TestName = "BeOneOnSecondStep")]
        public double AlphaShould()
        {
            var direction = new Direction();
            direction.GetNextDirection();

            var alpha = Math.Atan(direction.GetNextDirection());

            return alpha;
        }

    }
}
