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
            new Size(size.Width /= by, size.Height /= by);

        public static Point Center(this Rectangle rectangle)=>
            rectangle.Location + rectangle.Size.Divide(2);

        public static Size HeightSize(this Size size) =>
            new Size(0,size.Height);

        public static Size WidthSize(this Size size) =>
            new Size(0, size.Width);

        public static IEnumerable<Point> Points(this Rectangle rect)
        {
            yield return rect.Location;
            yield return rect.Location + rect.Size.HeightSize();
            yield return rect.Location + rect.Size.WidthSize();
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

        public static int Space(this Size size) =>
            size.Height * size.Width;

        public static double DistanceTo(this Point from, Point to)=>
            Math.Sqrt((to.X-from.X)*(to.X-from.X) + (to.Y-from.Y)*(to.Y-from.Y));

        public static IEnumerable<TOut> ForAllPairs<TIn,TOut>(this IList<TIn> coll, Func<TIn,TIn,TOut> func)
        {
            for (int i = 0; i < coll.Count; i++)
            for (int j = 0; j < i; j++)
                yield return func(coll[i], coll[j]);
        }
        
        public static void ForAllPairs<TIn>(this IList<TIn> coll, Action<TIn,TIn> func)
        {
            for (int i = 0; i < coll.Count; i++)
            for (int j = 0; j < i; j++)
            func(coll[i], coll[j]);
        }
        
        
        public static void ForAllPairs<TIn>(this IEnumerable<TIn> enumerable, Action<TIn,TIn> func)
        {
            var coll = enumerable.ToArray();
            for (int i = 0; i < coll.Length; i++)
            for (int j = 0; j < i; j++)
                func(coll[i], coll[j]);
        }


        public static Rectangle Unite(this IEnumerable<Rectangle> enumerable)
        {
            var coll = enumerable.ToArray();
            var x1 = coll.Select(x => x.Left).Min();
            var x2 = coll.Select(x => x.Right).Max();
            var y1 = coll.Select(x => x.Top).Min();
            var y2 = coll.Select(x => x.Bottom).Max();
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
    }
}
