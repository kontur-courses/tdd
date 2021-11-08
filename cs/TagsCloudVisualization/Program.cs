using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var list = new SortedSingleLinkedList<Point>((x, y) => x.X > y.X);
            list.Add(new Point(3, 1));
            list.Add(new Point(1, 1));
            list.Add(new Point(2, 1));
            var res = list.ToEnumerable().ToList();
            list.Remove(new Point(2, 1));
            list.Remove(new Point(1, 1));
            list.Remove(new Point(3, 1));
            Console.WriteLine(1);
        }
    }
}