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

    public static Bitmap FillBackground(Bitmap bitmap)
    {
        Bitmap bm = (Bitmap)bitmap.Clone();
        Graphics graph = Graphics.FromImage(bm);
        Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
        return bm;
    }
}