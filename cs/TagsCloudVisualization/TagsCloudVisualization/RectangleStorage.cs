using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class RectangleStorage
    {
        private readonly IDirection<double> _direction;
        private readonly List<Sector> _sectors;

        public RectangleStorage(Point center, IDirection<double> direction)
        {
            _direction = direction;
            _sectors = new List<Sector>()
            {
                new Sector(Quadrant.First, center),
                new Sector(Quadrant.Second, center),
                new Sector(Quadrant.Third, center),
                new Sector(Quadrant.Fourth, center)
            };
        }

        public Rectangle PlaceNewRectangle(Size rectangleSize)
        {
            var direction = _direction.GetNextDirection();
            var sector = ChoseSectorByDirection(direction);

            return sector.PlaceRectangle(direction, rectangleSize);
        }

        private Sector ChoseSectorByDirection(double direction)
        {
            var quadrant = Tools.DetermineQuadrantByDirection(direction);

            return _sectors[(int)quadrant];
        }
    }
}
