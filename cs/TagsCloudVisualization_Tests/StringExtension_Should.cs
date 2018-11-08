using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class StringExtension_Should
    {
        [Test]
        public void GetFrequency_ReturnCorrectFrecuency()
        {
            var frequency = "one two two three three three".GetFrequency();
            frequency["one"].Should().Be(1);
            frequency["two"].Should().Be(2);
            frequency["three"].Should().Be(3);
        }

        [Test]
        public void GetFrequency_CorrectlyTreatWhiteSpaceDelimetrs()
        {
            var frequency = "one\ntwo\ttwo\r\nthree three  three".GetFrequency();
            frequency["one"].Should().Be(1);
            frequency["two"].Should().Be(2);
            frequency["three"].Should().Be(3);
        }

        [Test]
        public void GetFrequency_CorrectlyTreatNotLetterCharacters()
        {
            var frequency = "one, two/ two 2three three three".GetFrequency();
            frequency["one"].Should().Be(1);
            frequency["two"].Should().Be(2);
            frequency["three"].Should().Be(3);
        }
    }
}