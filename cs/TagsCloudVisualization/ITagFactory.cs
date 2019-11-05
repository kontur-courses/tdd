using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ITagFactory
    {
        IEnumerable<Tag> GetTags(string[] cloudStrings, Graphics graphics, CircularCloudLayouter circularCloudLayouter);
    }
}