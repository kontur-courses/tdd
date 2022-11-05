using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

public class SpiralTests
{
    [TestCase(0, 0.2f, TestName = "{m}ZeroParameter")]
    [TestCase(2, 0f, TestName = "{m}ZeroStep")]
    public void Constructor_ThrowArgumentException_On(int parameter, float step)
    {
        Action act = () => new Spiral(Point.Empty, parameter, step);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_DoesntThrowArgumentException_OnCorrectArguments()
    {
        Action act = () => new Spiral(Point.Empty, 1, 0.2f);

        act.Should().NotThrow<ArgumentException>();
    }
    
    [TestCaseSource(nameof(Cases))]
    public void Next_ShouldReturnCorrectNextPoint_With(int parameter, float step, Point[] expectedPoints)
    {
        var spiral = new Spiral(new Point(100, 100), parameter, step);

        var actual = new List<Point>();
        for (int i = 0; i < expectedPoints.Length; i++)
        {
            actual.Add(spiral.Next());
        }

        actual.Should().BeEquivalentTo(expectedPoints);
    }

    private static object[] Cases =
    {
        new TestCaseData(2, 0.4f, new Point[]
        {
            new Point(100, 100), new Point(100, 100), new Point(102, 102), new Point(100, 102)
        }).SetName("{m}PositiveArguments"),
        new TestCaseData(-2, 0.4f, new Point[]
        {
            new Point(100, 100), new Point(100, 100), new Point(98, 98), new Point(100, 98)
        }).SetName("{m}NegativeParameter"),
        new TestCaseData(2, -0.4f, new Point[]
        {
            new Point(100, 100), new Point(100, 100), new Point(98, 102), new Point(100, 102)
        }).SetName("{m}NegativeStep"),
        new TestCaseData(-2, -0.4f, new Point[]
        {
            new Point(100, 100), new Point(100, 100), new Point(102, 98), new Point(100, 98)
        }).SetName("{m}NegativeParameters"),
    };
}