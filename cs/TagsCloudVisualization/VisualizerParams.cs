

using Aspose.Drawing;

namespace TagsCloudVisualization;

public class VisualizerParams
{
    private int _width;
    private int _height;
    private string _pathToFile;
    private string _fileName;
    
    public VisualizerParams(int width = 500, int height = 500, string fileName = "TagsCloud.png")
    {
        Width = width;
        Height = height;
        PathToFile = "../../../TagCloudImages";
        BgColor = Color.Black;
        RectangleColor = Color.Chocolate;
        FileName = fileName;
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