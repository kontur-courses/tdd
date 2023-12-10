using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralDistributionTests
    {
        private Point center;
        private SpiralDistribution spiralDistribution;

        [SetUp]
        public void SetUp()
        {
            center = new Point();
            spiralDistribution = new SpiralDistribution(center);
        }


        [Test]
        public void SprialDistribution_Initialize_Params()
        {
            Assert.AreEqual(center, spiralDistribution.Center);
            Assert.AreEqual(0, spiralDistribution.Angle);
            Assert.AreEqual(0, spiralDistribution.Radius);
        }

        [Test]
        public void SpiralDistribution_First_Call_GetNextPoint_Should_Return_Center()
        {
            Assert.AreEqual(center, spiralDistribution.GetNextPoint());
        }

        [Test]
        public void SpiralDistribution_Should_Increase_Angle_After_Call_GetNextPoint()
        {
            spiralDistribution.GetNextPoint();
            Assert.AreEqual(0.1, spiralDistribution.Angle);
        }

        [Test]
        public void SpiralDistribution_Should_Increase_Radius_And_Reset_Angle_When_Angle_More_Than_2Pi()
        {
            for (int i = 0; i < 63; i++)
            {
                spiralDistribution.GetNextPoint();
            }
            Assert.AreEqual(0,spiralDistribution.Angle);
            Assert.AreEqual(0.1,spiralDistribution.Radius);
        }
    }
}