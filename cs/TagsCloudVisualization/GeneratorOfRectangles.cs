using System;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class GeneratorOfRectangles : IGenerataorFigure
    {
        private Random rnd = new Random();

        public Size GetSize(int minValue, int maxValue)
        {
            return new Size(rnd.Next(minValue, maxValue), rnd.Next(minValue, maxValue));
        }
    }
}