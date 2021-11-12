using System;
using System.Drawing;

namespace CloudLayouter
{
    public class RandomRectangleGenerator
    {
        private readonly Size minRectangle;
        private readonly Size maxRectangle;
        
        private readonly Random randomNumbers;

        public RandomRectangleGenerator(Size minRectangle, Size maxRectangle, int seed = 20)
        {
            this.minRectangle = minRectangle;
            this.maxRectangle = maxRectangle;
            randomNumbers = new Random(seed);
        }
        
        public Size GetRandomRectangle()
        {
            var width = randomNumbers.Next(minRectangle.Width, maxRectangle.Width);
            var height = randomNumbers.Next(minRectangle.Height, maxRectangle.Height);

            return new Size(width, height);
        }
    }
}