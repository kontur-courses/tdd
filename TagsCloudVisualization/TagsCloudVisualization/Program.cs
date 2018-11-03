using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(200, 200));
            for (int i = 0; i < 20; i++)
            {
                layouter.PutNextRectangle(new Size(40, 24));
                layouter.PutNextRectangle(new Size(24, 24));
            }

            layouter.SaveBitmap("1", 400, 400);
        }
    }
}
