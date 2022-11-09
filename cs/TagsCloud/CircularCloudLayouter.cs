using System.Drawing;

namespace TagsCloud
{
    public sealed class CircularCloudLayouter : CloudLayouter<Rectangle>
    {
        private readonly IPlacer<Rectangle> placer;
        private readonly IFigurePattern figurePattern;

        public CircularCloudLayouter(Point center, double figureStep = 1)
        {
            figurePattern = new SpiralPattern(center, figureStep);
            placer = new RectanglePlacer();
        }

        public override Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Incorrect width or height", nameof(rectangleSize));
            
            var figure = GetNextFigure(rectangleSize);
            Figures.Add(figure);
            return figure;
        }

        private Rectangle GetNextFigure(Size size)
        {
            while (true)
            {
                var point = figurePattern.GetNextPoint();
                var figure = placer.Place(point, size);
                if (Figures.Any(fig => fig.IntersectsWith(figure)))
                    continue;

                figurePattern.Restart();
                return figure;
            }
        }
    }
}