using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TagsCloudVisualization.CloudLayouter;

public class CloudVisualizer
{
    public void SaveVisualization(List<RectangleF> rectangles, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory)) 
            throw new DirectoryNotFoundException("The directory does not exist.");

        var maxWidth = rectangles.Max(r => r.Right);
        var maxHeight = rectangles.Max(r => r.Bottom);

        var width = (int)Math.Max(maxWidth, 1200);
        var height = (int)Math.Max(maxHeight, 900);

        using (var image = new Image<Rgba32>(width, height))
        {
            image.Mutate(ctx => 
            {
                ctx.Fill(Color.White);
                foreach (var rectangle in rectangles)
                {
                    ctx.Fill(Color.CornflowerBlue, rectangle);
                    ctx.Draw(Color.Black, 1, rectangle);
                }
            });

            image.Save(filePath);
        }
    }
}