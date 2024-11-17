using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class RandomSizeRectangle : IRectangleGenerator
    {
        public IEnumerable<Size> GenerateRectangles(int quantity)
        {
            var rnd = new Random();
            for (var i = 0; i < quantity; i++)
                yield return new Size(rnd.Next(5, 30), rnd.Next(5, 30));
        }
    }
}
