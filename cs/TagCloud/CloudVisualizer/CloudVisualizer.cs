using System;
using System.Drawing;
using System.Linq;
using TagCloud;

public class CloudVisualizer : ICloudVisualizer
{
    public DrawSettings Settings { get; set; }

    public Bitmap CreatePictureWithItems(TagItem[] tagItems)
    {
        if (tagItems == null) throw new ArgumentException("Array can't be null");
        if (tagItems.Length == 0) throw new ArgumentException("Array can't be empty");

        var bounds =
            tagItems
                .SelectMany(t => new[] { t.Rectangle.Location, new Point(t.Rectangle.Right, t.Rectangle.Bottom) })
                .ToArray()
                .GetBounds();
        var picture = new Bitmap(bounds.Width, bounds.Height);

        using (var graphics = Graphics.FromImage(picture))
        {
            graphics.TranslateTransform(-bounds.X, -bounds.Y);
            Draw(graphics, tagItems);
        }
        return picture;
    }

    private void Draw(Graphics graphics, TagItem[] tagItems)
    {
        var rectangles = tagItems.Select(t => t.Rectangle).ToArray();
        var words = tagItems.Select(t => t.Word).ToArray();

        if (Settings != DrawSettings.OnlyWords)
            graphics.DrawRectangles(Pens.Black, rectangles);
        if (Settings == DrawSettings.OnlyWords || Settings == DrawSettings.WordsInRectangles)
            for (var i = 0; i < words.Length; i++)
            {
                graphics.DrawString(words[i],
                    SystemFonts.DefaultFont,
                    Brushes.Black,
                    rectangles[i].GetCenter());
            }
        if (Settings == DrawSettings.RectanglesWithNumeration)
            for (var i = 0; i < words.Length; i++)
            {
                graphics.DrawString(
                    i.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black,
                    rectangles[i].GetCenter());
            }
    }

    public Bitmap CreatePictureWithRectangles(Rectangle[] rectangles)
    {
        return CreatePictureWithItems(
            rectangles
                .Select(rectangle => new TagItem(null, rectangle))
                .ToArray());
    }
}