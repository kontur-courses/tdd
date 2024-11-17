using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main()
        {
            Drawings.DrawPicture(new RandomSizeRectangle(), 300, new ArchemedianSpiral(), new Point(1000, 1000), "spiral.bmp");
            Drawings.DrawPicture(new RandomSizeRectangle(), 300, new HeartShaped(), new Point(1000, 1000), "heart.bmp");
            Drawings.DrawPicture(new RandomSizeRectangle(), 300, new DeltaSHaped(), new Point(1000, 1000), "delta.bmp");
        }
    }
}
