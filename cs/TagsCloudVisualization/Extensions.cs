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

        public static Size HeightSize(this Size size) =>
            new Size(0,size.Height);

        public static Size WidhtSize(this Size size) =>
            new Size(0, size.Width);

        public static IEnumerable<Point> Points(this Rectangle rect)
        {
            yield return rect.Location;
            yield return rect.Location + rect.Size.HeightSize();
            yield return rect.Location + rect.Size.WidhtSize();
            yield return rect.Location + rect.Size;
        }
        public static void Times(this int count, Action act)
        {
            for (int i = 0; i < count; i++)
                act();
        }
        public static IEnumerable<T> Times<T>(this int count, Func<T> act)
        {
            for (int i = 0; i < count; i++)
                yield return act();
        }

    }
}
