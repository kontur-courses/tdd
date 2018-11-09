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
            var visualisator = new Visualiser(new Size(1000, 1000));
            visualisator.MakeExampleImage("output.bmp", 
                rectangleCount: 1000, 
                maxRectangleSize:new Size(10, 10));
        }
    }
}
