using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagCloudTests;

public class CircularCloudLayouterVisualizationTests
{
    private Point center;
    private CircularCloudVisualizer visualizer;

    [SetUp]
    public void SetUp()
    {
        center = new Point(750, 750);
        visualizer = new CircularCloudVisualizer(GenerateRectangles(center, 100, 30, 100));
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

    private static List<Rectangle> GenerateRectangles(Point center, int maxSize, int minSize, int count)
    {
        var rnd = new Random();
        var storage = new List<Rectangle>();
        var layouter = CircularCloudLayouter.CreateLayouterWithStartRectangles(center, storage);
        for (var i = 0; i < count; i++) layouter.PutNextRectangle(new Size(rnd.Next(minSize, maxSize),
            rnd.Next(minSize, maxSize)));

        return storage;
    }
}