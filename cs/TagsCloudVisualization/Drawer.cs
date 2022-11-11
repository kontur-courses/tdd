namespace TagsCloudVisualization;

public static class Drawer
{
    public static Bitmap DrawRectangles(Bitmap bitmap, List<Rectangle> rectangles)
    {
        Bitmap bm = (Bitmap)bitmap.Clone();
        Graphics graph = Graphics.FromImage(bm);
        graph.DrawRectangles(new Pen(Color.Black), rectangles.ToArray());
        return bm;
    }
}