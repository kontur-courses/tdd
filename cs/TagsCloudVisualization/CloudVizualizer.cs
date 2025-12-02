using System.Drawing;
using System.Runtime.Versioning;

namespace TagsCloudVisualization;

[SupportedOSPlatform("windows")]
public static class CloudVisualizer
{
    public static void GenerateRectanglesCloud(
        string fileName,
        List<Size> rectSizes,
        Point? center = null,
        Size? imageSize = null,
        Color? backgroundColor = null,
        Color? baseRectangleColor = null,
        Color? rectangleBorderColor = null,
        string? outputDirectory = null)
    {
        try
        {
            center ??= new Point(0, 0);
            imageSize ??= new Size(0, 0); 
            backgroundColor ??= Color.Black;
            baseRectangleColor ??= Color.Blue;
            rectangleBorderColor ??= Color.Black;

            outputDirectory = string.IsNullOrWhiteSpace(outputDirectory)
                ? Environment.CurrentDirectory
                : outputDirectory;

            ValidateInputs(fileName, rectSizes);

            Directory.CreateDirectory(outputDirectory);
            var fullPath = Path.Combine(outputDirectory, fileName);

            var layouter = new CircularCloudLayouter(center.Value);
            foreach (var size in rectSizes)
                layouter.PutNextRectangle(size);

            using var pen = new Pen(rectangleBorderColor.Value);

            SaveLayoutImage(
                layouter.PlacedRectangles,
                fullPath,
                imageSize.Value,
                backgroundColor.Value,
                baseRectangleColor.Value,
                pen
            );

            Console.WriteLine($"Cloud saved: {fullPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Cloud generation failed: {ex.Message}");
        }
    }


    private static void ValidateInputs(string fileName, List<Size> rectSizes)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));

        var ext = Path.GetExtension(fileName);
        if (!string.IsNullOrEmpty(ext))
        {
            var validExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
            if (!validExtensions.Contains(ext.ToLowerInvariant()))
                throw new ArgumentException(
                    $"Invalid file extension. Allowed: {string.Join(", ", validExtensions)}",
                    nameof(fileName)
                );
        }

        ArgumentNullException.ThrowIfNull(rectSizes);
        switch (rectSizes.Count)
        {
            case 0:
                throw new ArgumentException("No rectangles provided");
            case > 10000:
                throw new ArgumentException("Too many rectangles (max 10000)");
        }
    }
    
    private static void SaveLayoutImage(
        IReadOnlyList<Rectangle> rectangles,
        string fileName,
        Size imageSize,
        Color backgroundColor,
        Color baseRectangleColor,
        Pen? pen = null,
        int padding = 50)
    {
        try
        {
            var directory = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Console.WriteLine($"Creating directory: {directory}");
                Directory.CreateDirectory(directory);
            }
            
            pen ??= Pens.Black;

            var minX = rectangles.Min(r => r.Left);
            var maxX = rectangles.Max(r => r.Right);
            var minY = rectangles.Min(r => r.Top);
            var maxY = rectangles.Max(r => r.Bottom);

            var width = imageSize.Width > 0 ? imageSize.Width : (maxX - minX) + padding * 2;
            var height = imageSize.Height > 0 ? imageSize.Height : (maxY - minY) + padding * 2;
            
            if (width <= 0 || height <= 0)
                throw new InvalidOperationException($"Invalid image dimensions: {width}x{height}");
                
            if (width > 10000 || height > 10000)
                throw new InvalidOperationException($"Image dimensions too large: {width}x{height}");

            using var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);

            g.Clear(backgroundColor);

            var colors = PaletteGenerator.GenerateColorPalette(rectangles.Count - 1, baseRectangleColor);
            var colorIndex = 0;

            foreach (var rect in rectangles)
            {
                var shifted = rect with
                {
                    X = rect.Left - minX + padding,
                    Y = rect.Top - minY + padding
                };

                if (rect == rectangles[0])
                {
                    using var centerBrush = new SolidBrush(baseRectangleColor);
                    g.FillRectangle(centerBrush, shifted);
                    continue;
                }

                using var brush = new SolidBrush(colors[colorIndex++]);
                g.FillRectangle(brush, shifted);
                g.DrawRectangle(pen, shifted);
            }

            Console.WriteLine($"Saving image to: {fileName}");
            bitmap.Save(fileName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save image '{fileName}': {ex.Message}", ex);
        }
    }
}