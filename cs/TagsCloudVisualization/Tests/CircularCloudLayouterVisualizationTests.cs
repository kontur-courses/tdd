using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

public class CircularCloudLayouterVisualizationTests
{
    private Size imageSize;
    private Point center;
    private CircularCloudVisualizer visualizer;

    [SetUp]
    public void SetUp()
    {
        imageSize = new(1000, 1000);
        center = new Point(imageSize.Width / 2, imageSize.Height / 2);
        visualizer = new CircularCloudVisualizer(GenerateRectangles(center, 100, 10, 100), imageSize);
    }

    [Test]
    public void GenerateImage()
    {
        visualizer.CreateImage();
    }

    [Test]
    public void GenerateImageWithSaveEveryStep()
    {
        visualizer.CreateImage(withSaveSteps: true);
    }

    private static List<RectangleWrapper> GenerateRectangles(Point center, int maxSize, int minSize, int count)
    {
        var rnd = new Random();
        var storage = new List<RectangleWrapper>();
        var layouter = new CircularCloudLayouter(center, storage);
        for (var i = 0; i < count; i++) layouter.PutNextRectangle(new Size(rnd.Next(minSize, maxSize),
            rnd.Next(minSize, maxSize)));

        return storage;
    }
}