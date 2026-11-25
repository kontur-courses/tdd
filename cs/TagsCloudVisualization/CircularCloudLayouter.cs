using System.Drawing;


namespace TagsCloudVisualization
{
    public  class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly ArchimedeanSpiral spiral;
        private readonly PlacementStrategy placement;
        private readonly CentripetalCompactor compactor;
        private readonly List<Rectangle> placed = new();


        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new ArchimedeanSpiral(center: center,radiusStep: 0.5, angleStep: Math.PI / 32);
            placement = new PlacementStrategy();
            compactor = new CentripetalCompactor();
        }


        public Rectangle PutNextRectangle(Size rectangleSize) 
        {
            throw new NotImplementedException();
        }
    }
}
