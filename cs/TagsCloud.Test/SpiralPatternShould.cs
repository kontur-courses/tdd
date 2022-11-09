using System.Drawing;
using FluentAssertions;

namespace TagsCloud.Test
{
    [TestFixture]
    public class SpiralPatternShould
    {
        private SpiralPattern spiralPattern;

        [SetUp]
        public void SetUp()
        {
            spiralPattern = new SpiralPattern(Point.Empty, 1);
        }
        
        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectStepCount))]
        [Parallelizable(scope: ParallelScope.All)] 
        public void Ctor_IncorrectStep_ArgumentException(int steps)
        {
            // ReSharper disable once ObjectCreationAsStatement
            var createSpiralPattern = (Action) (() => new SpiralPattern(Point.Empty, steps));
            createSpiralPattern.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectStepCount))]
        [Parallelizable(scope: ParallelScope.All)] 
        public void StepSetter_SetIncorrectStep_ArgumentException(int steps)
        {
            var setStep = (Action) (() => spiralPattern.Step = steps);
            setStep.Should().Throw<ArgumentException>();
        }
        
        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectStepCount))]
        [Parallelizable(scope: ParallelScope.None)] 
        public void Ctor_CorrectStep_EqualSteps(int steps)
        {
            var spiralInstance = new SpiralPattern(Point.Empty, steps);
            spiralInstance.Step.Should().Be(steps);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectStepCount))]
        [Parallelizable(scope: ParallelScope.None)] 
        public void StepGetter_CorrectStep_EqualSteps(int steps)
        {
            spiralPattern.Step = steps;
            spiralPattern.Step.Should().Be(steps);
        }
    }
}