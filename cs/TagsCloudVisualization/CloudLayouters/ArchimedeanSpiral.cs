namespace TagsCloudVisualization.CloudLayouters
{
    /// <summary>
    ///     Class implement archimedean spiral function in polar coordinate system: r(f) = a + bf
    /// </summary>
    public class ArchimedeanSpiral
    {
        private readonly double freeCoefficient;
        private readonly double azimuthCoefficient;

        /// <param name="freeCoefficient">It's "a" param from r(f) = a + bf</param>
        /// <param name="azimuthCoefficient">It's "b" param from r(f) = a + bf</param>
        private ArchimedeanSpiral(double freeCoefficient, double azimuthCoefficient)
        {
            this.freeCoefficient = freeCoefficient;
            this.azimuthCoefficient = azimuthCoefficient;
        }

        /// <summary>
        ///     Create function with a = 0 and b = 1: r(f) = f
        /// </summary>
        public ArchimedeanSpiral() : this(0, 1) { }

        /// <summary>
        ///     Azimuth is variable f from r(f) = a + bf. Measured in radians.
        /// </summary>
        public double Azimuth { get; set; }

        /// <summary>
        ///     Radius is r from r(f) = a + bf.
        /// </summary>
        public double Radius => freeCoefficient + azimuthCoefficient * Azimuth;
    }
}