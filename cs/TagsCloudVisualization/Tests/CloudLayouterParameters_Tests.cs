using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CloudLayouterParameters_Tests
    {
        private CloudLayouterParameters cloudLayouterParameters;

        [SetUp]
        public void SetParameters()
        {
            cloudLayouterParameters = new CloudLayouterParameters();
        }

        [Test]
        public void BoostRadius_ShouldChange_CurrentRadius()
        {
            var firstRadius = cloudLayouterParameters.CurrentRadius;

            cloudLayouterParameters.BoostRadius();

            cloudLayouterParameters.CurrentRadius.Should().NotBe(firstRadius);
        }

        [Test]
        public void BoostRadius_ShouldChange_StepAngle()
        {
            cloudLayouterParameters = new CloudLayouterParameters(startAngle: 2, countRadiusBoostsForChangeAngle: 2);
            
            cloudLayouterParameters.BoostRadius();
            cloudLayouterParameters.BoostRadius();

            cloudLayouterParameters.StepAngle.Should().BeApproximately(1f, 0.001f);
        }

        [Test]
        public void ResetRadius_ShouldSetCurrentRadius_ToFirstRadius()
        {
            var firstRadius = cloudLayouterParameters.CurrentRadius;

            cloudLayouterParameters.BoostRadius();
            cloudLayouterParameters.BoostRadius();
            cloudLayouterParameters.ResetRadius();

            cloudLayouterParameters.CurrentRadius.Should().Be(firstRadius);
        }

        [Test]
        public void CurrentAngle_ShouldChangeSelf_AfterInvoke()
        {
            var firstAngle = cloudLayouterParameters.NextAngle;
            
            cloudLayouterParameters.NextAngle.Should().NotBe(firstAngle);
        }
    }
}