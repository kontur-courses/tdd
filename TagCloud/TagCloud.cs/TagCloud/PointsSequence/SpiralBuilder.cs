using System.Drawing;

namespace TagCloud
{
    public class SpiralBuilder // fluent tests
    {
        private readonly Spiral spiral;

        public SpiralBuilder(Spiral spiral)
        {
            this.spiral = spiral;
        }

        public SpiralBuilder WithStepLength(double stepLength)
        {
            spiral.SetStepLength(stepLength);
            return this;
        }

        public SpiralBuilder WithCenterIn(Point center)
        {
            spiral.SetCenter(center);
            return this;
        }

        public static implicit operator Spiral(SpiralBuilder ps)
        {
            return ps.spiral;
        }
    }
}