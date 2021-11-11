using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class GeneratorOfRectangles
    {
        Random rnd = new Random();

        public Size GetRectangleSize(int minValue, int maxValue)
        {
             return new Size(rnd.Next(minValue, maxValue), rnd.Next(minValue, maxValue));
        }

    }
}
