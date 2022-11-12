using System.Drawing;
using System.Windows.Forms;


// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var spiral = new Spiral(1, Point.Empty);
            var cloudLayouter = new CloudLayouter(spiral);
            var drawer = new CloudLayouterDrawer(Color.Black, cloudLayouter);
            Application.Run(new CloudLayouterForm(drawer, 100));
        }
    }
}