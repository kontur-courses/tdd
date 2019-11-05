namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private readonly double freeCoefficient;
        private readonly double azimuthCoefficient;

        private ArchimedeanSpiral(double freeCoefficient, double azimuthCoefficient)
        {
            this.freeCoefficient = freeCoefficient;
            this.azimuthCoefficient = azimuthCoefficient;
        }

        public ArchimedeanSpiral() : this(0, 1) { }

        public double Azimuth { get; set; }
        public double Radius => freeCoefficient + azimuthCoefficient * Azimuth;
    }
}