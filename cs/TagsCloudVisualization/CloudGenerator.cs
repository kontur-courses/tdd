using System.Drawing;

namespace TagsCloudVisualization
{
    public class CloudGenerator
    {
        private readonly int _rectanglesCount;
        private readonly Size _minSize;
        private readonly Size _maxSize;
        private readonly CircularCloudLayouter _ccl;

        public CloudGenerator(int count, Size minSize, Size maxSize, Point center, double step, double density, double start)
        {
            _rectanglesCount = count;
            _minSize = minSize;
            _maxSize = maxSize;
            
            _ccl = new CircularCloudLayouter(center, step, density, start);
        }
        
        public IEnumerable<Rectangle> GetNextRectangle()
        {
            var width = new Random();
            var height = new Random();
            
            for (var i = 0; i < _rectanglesCount; i++)
            {
                var size = new Size(width.Next(_minSize.Width, _maxSize.Width), height.Next(_minSize.Height, _maxSize.Height));
                yield return _ccl.PutNextRectangle(size);
            }
        }
    }
}

