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
            var visualisator = new Visualisator(new Size(600, 600));
            visualisator.MakeImage("output.bmp", 30);
        }
    }
}
