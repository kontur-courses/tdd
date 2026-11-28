using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private const string FailedLayoutsDirectory = "FailedLayouts";

    private static readonly Point Center = new(500, 500);

    private readonly TagCloudVisualizer visualizer = new(VisualizationSettings.Default);

    private CircularCloudLayouter layouter;
    private List<Rectangle> placedRectangles;

    [SetUp]
    public void SetUp()
    {
        throw new NotImplementedException();
    }

    [TearDown]
    public void TearDown()
    {
        throw new NotImplementedException();
    }

    private List<Rectangle> PutRectangles(IEnumerable<Size> rectangleSizes)
    {
        throw new NotImplementedException();
    }

    private static bool IsCurrentTestFailed()
    {
        throw new NotImplementedException();
    }

    private void SaveLayoutImage()
    {
        throw new NotImplementedException();
    }
}
