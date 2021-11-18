using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CloudLayouterParameters_Tests
    {
        private CloudLayoutParameters cloudLayoutParameters;

        [SetUp]
        public void SetParameters()
        {
            cloudLayoutParameters = new CloudLayoutParameters();
        }

        [Test]
        public void BoostRadius_ShouldChange_CurrentRadius()
        {
            var firstRadius = cloudLayoutParameters.Radius;

            cloudLayoutParameters.BoostRadius();

            cloudLayoutParameters.Radius.Should().NotBe(firstRadius);
        }

        [Test]
        public void BoostRadius_ShouldChange_StepAngle()
        {
            cloudLayoutParameters = new CloudLayoutParameters(startAngle: 2, countBoostsForChangeAngle: 2);
            
            cloudLayoutParameters.BoostRadius();
            cloudLayoutParameters.BoostRadius();

            cloudLayoutParameters.StepAngle.Should().BeApproximately(1f, 0.001f);
        }

        [Test]
        public void ResetRadius_ShouldSetCurrentRadius_ToFirstRadius()
        {
            var firstRadius = cloudLayoutParameters.Radius;

            cloudLayoutParameters.BoostRadius();
            cloudLayoutParameters.BoostRadius();
            cloudLayoutParameters.ResetRadius();

            cloudLayoutParameters.Radius.Should().Be(firstRadius);
        }

        [Test]
        public void CurrentAngle_ShouldChangeSelf_AfterInvoke()
        {
            var firstAngle = cloudLayoutParameters.NextAngle;
            
            cloudLayoutParameters.NextAngle.Should().NotBe(firstAngle);
        }
    }
}