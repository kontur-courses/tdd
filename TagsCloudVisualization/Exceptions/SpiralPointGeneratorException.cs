using System;

namespace TagsCloudVisualization
{
    public class SpiralPointGeneratorException : Exception
    {
        public SpiralPointGeneratorException() : base("Failed searching suitable point") {}
    }
}