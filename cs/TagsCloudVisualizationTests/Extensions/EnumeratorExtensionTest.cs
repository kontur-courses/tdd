using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualizationTests.Extensions;

[TestFixture]
[TestOf(typeof(EnumeratorExtension))]
public class EnumeratorExtensionTest
{

    [Test]
    public void ToIEnumerable_ShouldReturnExpectedEnumerable()
    {
        const int elementsNumber = 1000;
        var centerPoint = new Point(0, 0);
        var expectedEnumerable = new FermatSpiralPointsGenerator(1, 0.5)
            .GeneratePoints(centerPoint);
        var actualEnumerable = expectedEnumerable
            .GetEnumerator()
            .ToIEnumerable();
        
        actualEnumerable.Take(elementsNumber).Should()
            .BeEquivalentTo(expectedEnumerable.Take(elementsNumber));
    }

    [Test]
    public void ToIEnumerable_ShouldContinueEnumeration()
    {
        const int elementsNumber = 10;
        
        var startEnumerable = Enumerable.Range(0, elementsNumber);
        using var movedEnumerator = startEnumerable.GetEnumerator();
        movedEnumerator.MoveNext();
        var actualEnumerable = movedEnumerator.ToIEnumerable();
        var expectedEnumerable = Enumerable.Range(1, elementsNumber - 1);

        actualEnumerable.Should().BeEquivalentTo(expectedEnumerable);
        
    }
}