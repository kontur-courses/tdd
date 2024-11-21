using System.Drawing;
using Random = System.Random;

namespace TagsCloudVisualization.Visualisation
{
    public class SizeBuilder : ICountConfigurator, IWidthConfigurator, IHeightConfigurator, IGenerator<Size>
    {
        private int generateRectangleCount = 0;

        private int _minWidth = 0;

        private int _maxWidth = 0;

        private int _minHeight = 0;

        private int _maxHeight = 0;

        public static ICountConfigurator Configure()
        {
            return new SizeBuilder();
        }

        public IEnumerable<Size> Generate()
        {
            var random = new Random();

            for (int i = 0; i < generateRectangleCount; i++)
            {
                // Генерация случайной ширины и высоты в заданных пределах
                int width = random.Next(_minWidth, _maxWidth + 1);
                int height = random.Next(_minHeight, _maxHeight + 1);

                yield return new Size(width, height);
            }
        }

        public IWidthConfigurator SetCount(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count should be positive");

            generateRectangleCount = count;
            return this;
        }

        public IHeightConfigurator SetWidth(int minWidth, int maxWidth)
        {
            if (minWidth <= 0 || maxWidth <= 0)
                throw new ArgumentException("Arguments should be positive");

            _minWidth = minWidth;
            _maxWidth = maxWidth;

            return this;
        }

        public IGenerator<Size> SetHeight(int minHeight, int maxHeight)
        {
            if (minHeight <= 0 || maxHeight <= 0)
                throw new ArgumentException("Arguments should be positive");

            _minHeight = minHeight;
            _maxHeight = maxHeight;

            return this;
        }
    }

    public interface ICountConfigurator
    {
        public IWidthConfigurator SetCount(int count);
    }

    public interface IWidthConfigurator
    {
        public IHeightConfigurator SetWidth(int minWidth, int maxWidth);
    }

    public interface IHeightConfigurator
    {
        public IGenerator<Size> SetHeight(int minHeight, int maxHeight);
    }

    public interface IGenerator<T>
    {
        public IEnumerable<T> Generate();
    }
}
