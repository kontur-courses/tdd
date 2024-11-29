using System.Drawing;

namespace TagsCloudVisualization;

public class SizeParser
{
    public bool TryParse(string str, out Size size)
    {
        var lineParts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (lineParts.Length == 2
            && int.TryParse(lineParts[0], out var width)
            && int.TryParse(lineParts[1], out var height))
        {
            size = new Size(width, height);
            return true;
        }

        size = default;
        return false;
    }
}
