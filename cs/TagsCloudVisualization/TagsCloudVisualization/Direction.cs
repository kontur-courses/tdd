namespace TagsCloudVisualization
{
    public class Direction : IDirection<double>
    {
        private const double AngleShift = 1;
        private double _currentAlpha;

        public double GetNextDirection()
        {
            var oldAlpha = _currentAlpha;
            _currentAlpha = (_currentAlpha + AngleShift).AngleToStandardValue();

            return oldAlpha;
        }
    }
}