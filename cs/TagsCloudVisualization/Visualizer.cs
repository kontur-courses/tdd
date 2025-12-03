using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class Visualizer(ICircularCloudLayouter layouter)
{
    public void DrawCloud(
        IEnumerable<string> words,
        string outputPath,
        int imageWidth,
        int imageHeight,
        Color textColor,
        Color backgroundColor,
        Font font)
    { 
        var bitmap = new Bitmap(imageWidth, imageHeight);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(backgroundColor);
        
        var textBrush = new SolidBrush(textColor);

        foreach (var word in words)
        {
            var size = MeasureWord(word, graphics, font);
            var rectangle = layouter.PutNextRectangle(size);
            
            graphics.DrawString(word, font, textBrush, rectangle.Location);
        }

        bitmap.Save(outputPath, ImageFormat.Png);
    }

    private Size MeasureWord(string word, Graphics g, Font font)
    {
        var size = g.MeasureString(word, font);
        return new Size((int)size.Width, (int)size.Height);
    }
}