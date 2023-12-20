

using Aspose.Drawing;

namespace TagsCloudVisualization;

public class VisualizerParams
{
    private int _width;
    private int _height;
    private string _pathToFile;
    private string _fileName;

    private VisualizerParams(string fileName = "TagsCloud.png")
    {
        PathToFile = "../../../TagCloudImages";
        BgColor = Color.Black;
        RectangleColor = Color.Chocolate;
        FileName = fileName;
    }
    
    public VisualizerParams(int width, int height, string fileName = "TagsCloud.png") 
        : this(fileName)
    {
        Width = width;
        Height = height;
    }
    
    public VisualizerParams() : this(500, 500)
    {
    }
    
    public VisualizerParams(IEnumerable<Rectangle> rectangles, string fileName = "TagsCloud.png") 
        : this(fileName)
    {
        var (maxRight, maxBottom, minLeft, minTop) = (int.MinValue, int.MinValue, int.MaxValue, int.MaxValue);

        foreach (var rectangle in rectangles)
        {
            maxRight = Math.Max(maxRight, rectangle.Right);
            maxBottom = Math.Max(maxBottom, rectangle.Bottom);
            minLeft = Math.Min(minLeft, rectangle.Left);
            minTop = Math.Min(minTop, rectangle.Top);
        }

        Width = maxRight + minLeft;
        Height = maxBottom + minTop;
    }
    
    public VisualizerParams(CircularCloudLayouter layouter, string fileName = "TagsCloud.png") 
        : this(layouter.GetRectangles, fileName)
    {
    }

    public int Width
    {
        get => _width;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Width must be positive number");

            _width = value;
        }
    }

    public int Height { 
        get => _height;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Height must be positive number");

            _height = value;
        } 
    }
    
    public string PathToFile
    {
        get => _pathToFile;
        set
        {
            if (Path.GetInvalidPathChars().Any(value.Contains))
                throw new ArgumentException($"Path {value} is invalid");

            _pathToFile = value;
        }
    }

    public string FileName
    {
        get => _fileName;
        set
        {
            if (Path.GetInvalidFileNameChars().Any(value.Contains))
                throw new ArgumentException($"Name {value} is invalid");

            _fileName = value;
        }
    }
    
    public Color BgColor { get; set; }
    public Color RectangleColor { get; set; }
    public string PathWithFileName => Path.Combine(PathToFile, FileName);
}