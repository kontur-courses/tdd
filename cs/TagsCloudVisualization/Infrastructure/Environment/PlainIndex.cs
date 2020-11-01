using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Environment
{
    public class PlainIndex : Index<Rectangle>
    {
        public PlainIndex()
        {
            Elements = new List<Rectangle>();
        }

        public override void Add(Rectangle element)
        {
            Elements.Add(element);
        }

        public override void Remove(Rectangle element)
        {
            throw new System.NotImplementedException();
        }
    }
}