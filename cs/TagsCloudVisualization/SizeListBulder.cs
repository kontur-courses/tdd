using System.Drawing;

namespace TagsCloudVisualization
{
    public class SizeListBulder
    {
        public static List<Size> GetCustomSizes()
        {
            var list = new List<Size>();
            list.Add(new Size(100, 75));
            Enumerable.Range(0, 3).ToList().ForEach(x => list.Add(new Size(70, 55)));
            Enumerable.Range(0, 11).ToList().ForEach(x => list.Add(new Size(50, 30)));
            Enumerable.Range(0, 55).ToList().ForEach(x => list.Add(new Size(20, 15)));
            Enumerable.Range(0, 75).ToList().ForEach(x => list.Add(new Size(20, 15)));
            Enumerable.Range(0, 265).ToList().ForEach(x => list.Add(new Size(15, 10)));
            return list;
        }
    }
}