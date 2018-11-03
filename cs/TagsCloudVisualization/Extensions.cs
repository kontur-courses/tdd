using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class Extensions
    {
        public static Size Divide(this Size size, int by) =>
            new Size(size.Height /= by, size.Width /= by);

        public static Point Center(this Rectangle rectangle)=>
            rectangle.Location + rectangle.Size.Divide(2);

    }
}
