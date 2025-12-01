using System.Drawing;

public class TagCloudVisualizationConfig
{
    public int CanvasWidth { get; set; }
    public int CanvasHeight { get; set; }

    public Color CanvasBackgroundColor { get; set; } = Color.White;
    public Color ShapeBorderColor { get; set; } = Color.Blue;
    public int ShapeBorderThickness { get; set; } = 2;

    public Color ShapeFillColor { get; set; } = Color.FromArgb(80, Color.CornflowerBlue);
    public bool ShapeUseFill { get; set; } = true;

    public bool AutoResize { get; set; } = true; 
    public int Padding { get; set; } = 30; 
    public int MaxCanvasWidth { get; set; } = 8000; 
    public int MaxCanvasHeight { get; set; } = 8000;
    public int MinCanvasWidth { get; set; } = 100;
    public int MinCanvasHeight { get; set; } = 100;
}