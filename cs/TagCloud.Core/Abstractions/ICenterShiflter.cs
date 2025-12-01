using System.Drawing;

namespace TagsCloudLib.Abstractions;

public interface ICenterShifter
{
    Rectangle ShiftToCenter(Rectangle rectangle, Point center, IEnumerable<Rectangle> others);
}