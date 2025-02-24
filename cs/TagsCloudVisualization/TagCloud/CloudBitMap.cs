using System.Drawing;
using System.Drawing.Imaging;
using TagCloud2;

namespace TagsCloudVisualization;

public class CloudBitMap : ITagCloudImage
{
    private readonly Bitmap bitmap;
    private readonly string filePath;
    private readonly Graphics graphics;
    private readonly Pen pen = new(Color.Red);
    private bool isDisposed;
    private bool isSave;
    private string nameFile;

    public CloudBitMap(SettingsTagCloud settingsTagCloud, string nameFile)
    {
        this.nameFile = nameFile;
        Validate(settingsTagCloud.PathDirectory, settingsTagCloud.Size.Width, settingsTagCloud.Size.Height);

        bitmap = new Bitmap(settingsTagCloud.Size.Width, settingsTagCloud.Size.Height);
        graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Black);
        filePath = settingsTagCloud.PathDirectory;
    }

    public Size Size() => bitmap.Size;

    public void Draw(Rectangle rec)
    {
        graphics.DrawRectangle(pen, rec);
    }

    public void Draw(RectangleTagCloud rec)
    {
        graphics.DrawString(rec.text, rec.font, Brushes.Blue, rec.Rectangle);

        graphics.DrawRectangle(pen, rec.Rectangle);
    }

    public SizeF GetSizeWord(WordPopular word)
    {
        return graphics.MeasureString(word.Word, new Font("Aria", 24, FontStyle.Bold));
    }

    public void Save()
    {
        if (isSave)
        {
            Console.WriteLine("уже сохранена");
            return;
        }

        var saveFilePath = string.Join("",filePath, $"/tagCloud-({nameFile}).png");
        bitmap.Save(saveFilePath, ImageFormat.Png);
        Console.WriteLine($"file saved in {saveFilePath}");
        isSave = true;
    }


    private static void Validate(string filePath, int width, int height)
    {
        if (!Directory.Exists(filePath)) throw new DirectoryNotFoundException($"not correct path {filePath}");
        if (filePath[^1] == '/') throw new ArgumentException("PathShouldBeWithout \"\\\"");
        if (width <= 0 || height <= 0) throw new ArgumentException("size of image should be with positive number");
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool fromMethod)
    {
        if (!isDisposed)
        {
            if (fromMethod)
            {
                Save();
            }

            bitmap.Dispose();
            graphics.Dispose();
            pen.Dispose();

            isDisposed = true;
        }
    }
}