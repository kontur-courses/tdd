using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace TagsCloudVisualisation;

public class ScreenShot
{
    public static string TakeScreenShot()
    {
        var screenName = Guid.NewGuid().ToString("N") + ".jpeg";
        var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
            Screen.PrimaryScreen.Bounds.Height,
            PixelFormat.Format32bppArgb);
        var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

        gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
            Screen.PrimaryScreen.Bounds.Y,
            0,
            0,
            Screen.PrimaryScreen.Bounds.Size,
            CopyPixelOperation.SourceCopy);
        bmpScreenshot.Save(
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, 
                screenName), 
            ImageFormat.Jpeg);
        
        return screenName;
    }
}