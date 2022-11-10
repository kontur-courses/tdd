using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        private static CloudGenerator GetCloudGenerator()
        {
            var minSize = new Size(50, 50);
            var maxSize = new Size(200, 200);
            var center = new Point(1500, 1500);
            return new CloudGenerator(100, minSize, maxSize, center, 4, 4, 0);
        }
        
        public static void Main(string[] args)
        {
            var cg = GetCloudGenerator();
            LayoutDrawer layoutDrawer = new LayoutDrawer(3000, 3000, Color.Black, 2);
            layoutDrawer.DrawLayout(cg);
            
        }
    }
}