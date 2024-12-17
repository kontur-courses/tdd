using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectedWithAnyFrom(this Rectangle forInsertion, List<Rectangle> storage)
        {
            if (storage.Count == 0) return false;
            return storage.FirstOrDefault(r => forInsertion != r
                                                 && forInsertion.IntersectsWith(r)) != default;
        }
    }
}
