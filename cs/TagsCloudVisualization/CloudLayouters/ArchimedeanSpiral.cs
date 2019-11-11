namespace TagsCloudVisualization.CloudLayouters
{
    /// <summary>
    ///     Class implement archimedean spiral function in polar coordinate system: r(f) = a + bf
    /// </summary>
    public class ArchimedeanSpiral
    {
        private readonly double freeCoefficient;
        private readonly double azimuthCoefficient;
        private readonly double azimuthDelta;

        /// <param name="freeCoefficient">It's "a" param from r(f) = a + bf</param>
        /// <param name="azimuthCoefficient">It's "b" param from r(f) = a + bf</param>
        /// <param name="azimuthDelta">It's value which added to azimuth to provide function growth (f2 = f1 + df)</param>
        private ArchimedeanSpiral(double freeCoefficient, double azimuthCoefficient, double azimuthDelta)
        {
            this.freeCoefficient = freeCoefficient;
            this.azimuthCoefficient = azimuthCoefficient;
            this.azimuthDelta = azimuthDelta;
        }

        /// <summary>
        ///     Create function with a = 0 and b = 1: r(f) = f
        /// </summary>
        public ArchimedeanSpiral(double azimuthDelta) : this(0, 1, azimuthDelta) { }

        /// <summary>
        ///     Azimuth is variable f from r(f) = a + bf. Measured in radians.
        /// </summary>
        public double Azimuth { get; private set; }

        /// <summary>
        ///     Radius is r from r(f) = a + bf.
        /// </summary>
        public double Radius => freeCoefficient + azimuthCoefficient * Azimuth;

        /// <summary>
        ///     Increase azimuth on specified azimuth delta.
        /// </summary>
        public void IncreaseAzimuth() => Azimuth += azimuthDelta;
    }
}