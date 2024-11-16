using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class RandomExtensionsTests
{
    private const int Seed = 43435534;
    private readonly Random random = new(Seed);
    
    [TestCase(0, TestName = "MinLengthEqualZero")]
    [TestCase(-5, TestName = "MinLengthLessThanZero")]
    [TestCase(100000, TestName = "MinLengthGreaterMaxLength")]
    public void NextSize_ShouldThrowException(int minLength)
    {
        var random = new Random(Seed);
        var nextSizeInvoke = () => random.NextSize(minLength, 1000);
        
        nextSizeInvoke.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Test]
    [Repeat(10)]
    public void NextSize_ShouldReturnExpectedRandomSize()
    {
        var seed = this.random.Next();
        var random = new Random(seed);
        var checkRandom = new Random(seed);
        
        var actualSize = random.NextSize(1, int.MaxValue);
        var expectedSize = new Size(GetRandomInt(checkRandom), GetRandomInt(checkRandom));
        
        actualSize.Should().BeEquivalentTo(expectedSize);
    }
    
    [Test]
    [Repeat(10)]
    public void NextPoint_ShouldReturnExpectedRandomPoint()
    {
        var seed = this.random.Next();
        var random = new Random(seed);
        var checkRandom = new Random(seed);
        
        var actualSize = random.NextPoint(1, int.MaxValue);
        var expectedSize = new Point(GetRandomInt(checkRandom), GetRandomInt(checkRandom));
        
        actualSize.Should().BeEquivalentTo(expectedSize);
    }

    private static int GetRandomInt(Random random) => random.Next(1, int.MaxValue);
}