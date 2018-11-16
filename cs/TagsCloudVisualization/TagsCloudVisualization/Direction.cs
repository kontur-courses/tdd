namespace TagsCloudVisualization
{
    public class Direction: IDirection<double>
    {
        private double _currentAlpha;
        private const double AngleShift = 1;

        public double GetNextDirection()
        {
            var oldAlpha = _currentAlpha;
            _currentAlpha = Tools.AngleToStandardValue(_currentAlpha + AngleShift);

            return oldAlpha;
        }
    }
}
