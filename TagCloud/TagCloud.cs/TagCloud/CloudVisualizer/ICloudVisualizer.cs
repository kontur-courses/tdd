using System.Drawing;
using TagCloud;

public interface ICloudVisualizer
{
    DrawSettings Settings { get; set; }
    Bitmap CreatePictureWithRectangles(Rectangle[] rectangles);
    Bitmap CreatePictureWithItems(TagItem[] rectangles);
}