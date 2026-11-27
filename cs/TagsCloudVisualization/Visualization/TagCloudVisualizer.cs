using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualization;

public class TagCloudVisualizer
{
    private readonly VisualizationSettings settings;

    public TagCloudVisualizer(VisualizationSettings settings)
    {
        ValidateSettings(settings);
        this.settings = settings;
    }

    public Bitmap Draw(IReadOnlyCollection<Rectangle> rectangles)
    {
        throw new NotImplementedException();
    }

    public void SaveToFile(IReadOnlyCollection<Rectangle> rectangles, string filePath)
    {
        throw new NotImplementedException();
    }

    private Rectangle GetCanvasBounds(IReadOnlyCollection<Rectangle> rectangles)
    {
        throw new NotImplementedException();
    }

    private void DrawRectangle(Graphics graphics, Rectangle rectangle, Color fillColor)
    {
        throw new NotImplementedException();
    }

    private Color GetFillColor(int rectangleIndex)
    {
        throw new NotImplementedException();
    }

    private static void ValidateSettings(VisualizationSettings settings)
    {
        throw new NotImplementedException();
    }
}
