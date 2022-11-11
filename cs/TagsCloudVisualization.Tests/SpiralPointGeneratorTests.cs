using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

public class SpiralPointGeneratorTests
{
    [TestCase(0, 0.2f, TestName = "{m}ZeroParameter")]
    [TestCase(2, 0f, TestName = "{m}ZeroStep")]
    public void Constructor_ThrowArgumentException_On(int parameter, float step)
    {
        Action act = () => new SpiralPointGenerator(Point.Empty, parameter, step);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_DoesntThrowArgumentException_OnCorrectArguments()
    {
        Action act = () => new SpiralPointGenerator(Point.Empty, 1, 0.2f);

        act.Should().NotThrow<ArgumentException>();
    }

    [TestCaseSource(nameof(NextCases))]
    public void Next_ShouldReturnCorrectNextPoint_With(Point center, int parameter, double step, Point[] expectedPoints)
    {
        var spiral = new SpiralPointGenerator(center, parameter, step);

        var actual = new List<Point>();
        for (int i = 0; i < expectedPoints.Length; i++)
        {
            actual.Add(spiral.Next());
        }

        actual.Should().BeEquivalentTo(expectedPoints);
    }

    private static TestCaseData[] NextCases =
    {
        new TestCaseData(new Point(100, 100), 2, 0.4, new Point[]
        {
            new Point(100, 100), new Point(101, 100), new Point(101, 101), new Point(101, 102)
        }).SetName("{m}PositiveArguments"),

        new TestCaseData(new Point(100, 100), -2, 0.4, new Point[]
        {
            new Point(100, 100), new Point(99, 100), new Point(99, 99), new Point(99, 98)
        }).SetName("{m}NegativeParameter"),

        new TestCaseData(new Point(100, 100), 2, -0.4, new Point[]
        {
            new Point(100, 100), new Point(99, 100), new Point(99, 101), new Point(99, 102)
        }).SetName("{m}NegativeStep"),

        new TestCaseData(new Point(100, 100), -2, -0.4, new Point[]
        {
            new Point(100, 100), new Point(101, 100), new Point(101, 99), new Point(101, 98)
        }).SetName("{m}NegativeParameters"),
    };
}