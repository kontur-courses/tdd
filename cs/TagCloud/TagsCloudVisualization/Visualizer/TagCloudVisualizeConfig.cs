using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizationConfig
{
    public string OutputFileName { get; set; }
    public Size? ImageSize { get; set; }
    public Color BackgroundColor { get; set; }
    public Color RectangleColor { get; set; }
    public Color CenterColor { get; set; }
    public int PenWidth { get; set; }
    public int CenterPointSize { get; set; }
    public int RectangleFillAlpha { get; set; }

    public TagCloudVisualizationConfig(
        string outputFileName,
        Size? imageSize = null,
        Color? backgroundColor = null,
        Color? rectangleColor = null,
        Color? centerColor = null,
        int penWidth = 2,
        int centerPointSize = 5,
        int rectangleFillAlpha = 50)
    {
        OutputFileName = outputFileName ?? throw new ArgumentNullException(nameof(outputFileName));
        ImageSize = imageSize;
        BackgroundColor = backgroundColor ?? Color.White;
        RectangleColor = rectangleColor ?? Color.Blue;
        CenterColor = centerColor ?? Color.Red;
        PenWidth = penWidth;
        CenterPointSize = centerPointSize;
        RectangleFillAlpha = rectangleFillAlpha;
    }
}