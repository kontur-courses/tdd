using System;
using System.Drawing;
using TagsCloudVisualization.Graphic;
using TagsCloudVisualization.Infrastructure.Environment;
using TagsCloudVisualization.Infrastructure.Layout;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ILayoutStrategy strategy;
        private readonly Environment<Rectangle> environment;
        private readonly IEnvironmentToImageConverter<Rectangle> environmentToImageConverter;
        private readonly IImageSaver imageSaver;

        public CircularCloudLayouter(Point center)
        {
            strategy = new SpiralPlacing(center, 1);
            environment = new PlainEnvironment();
            environmentToImageConverter = new BitmapDrawer();
            imageSaver = new BitmapSaver(@"TagsCloudVisualization/Pictures", @"picture");
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(Point.Empty, rectangleSize);
            var rectangleMiddleOffset = new Size(rectangle.Width / 2, rectangle.Height / 2);

            bool IsValid(Point possiblePoint)
            {
                rectangle.Location = possiblePoint - rectangleMiddleOffset;
                var isValid = !environment.IsColliding(rectangle);
                return isValid;
            }

            var location = strategy.GetPlace(IsValid);
            rectangle.Location = location - rectangleMiddleOffset;
            
            environment.Add(rectangle);
            return rectangle;
        }

        public void SaveImage()
        {
            var image = environmentToImageConverter.GetEnvironmentImage(environment);
            imageSaver.Save(image);
        }
    }
}