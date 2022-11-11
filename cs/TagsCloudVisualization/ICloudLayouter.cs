namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(List<Rectangle> rectangles, Size rectangleSize);
    
}