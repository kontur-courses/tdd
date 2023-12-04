using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudVisualization;

var center = new PointF((float)1920 / 2, (float)1080 / 2);
var random = new Random();

var sizes = Enumerable
    .Range(0, 120)
    .Select(rect => new Size(random.Next(50, 120), random.Next(50, 120)))
    .ToArray();

var layout = new CircularCloudLayout(center, 1f, (float)Math.PI / 180);
var rectangles = sizes.Select(size => layout.PutNextRectangle(size)).ToList();

using var image = new Image<Rgba32>(1920, 1080);

image.Mutate(ctx =>
{
    ctx.Clear(Color.White);
    rectangles.ForEach(rect => { ctx.Draw(Color.Black, 1f, rect); });
});

image.SaveAsJpeg("spiral");