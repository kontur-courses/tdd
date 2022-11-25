namespace TagsCloudVisualization;

public static class PictureSaver
{
    public static void SaveRectanglesAsPicture(List<Rectangle> rectangles,
        string path, string fileName = "pic.jpg")
    {
        if (rectangles.Count == 0)
            throw new ArgumentException("List of rectangles must not be empty");
        Point center = rectangles[0].GetCenter();
        int radius = Math.Min(center.X, center.Y);
        Bitmap bitmap = new Bitmap(radius * 2, radius * 2);
        bitmap = Drawer.FillBackground(bitmap);
        bitmap = Drawer.DrawRectangles(bitmap, rectangles);
        bitmap.Save(path + "\\" + fileName);
    }
}