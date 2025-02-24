using System.Drawing;
using TagCloud2;

namespace TagsCloudVisualization;

// хочется сразу заиспользовать паттерн мост. использовать класс TagCloud, как абстракцию для создания всех деталей TagCloud
// замечу, что tagCloud не имеет интерфейса, ибо он по существу никогда не будет иметь другую реализацию, ибо вся логика внутри интерефейсов, а этот класс просто обёртка, которая ничего сама делать и не может. композиция на максиум кароче.
// здесь бы еще паттерн строитель, но это уже overhead.
public class TagCloud : IDisposable
{
    private readonly ICloudLayouter cloudLayouter;

    private readonly ITagCloudImage tagCloudImage;

    private bool IsDisposed;

    public TagCloud(ICloudLayouter cloudLayouter, ITagCloudImage tagCloudImage)
    {
        Validate(cloudLayouter, tagCloudImage);
        this.cloudLayouter = cloudLayouter;
        this.tagCloudImage = tagCloudImage;
    }


    private static void Validate(ICloudLayouter cloudLayouter, ITagCloudImage tagCloudImage)
    {
        if (cloudLayouter.Start.Y > tagCloudImage.Size().Height ||
            cloudLayouter.Start.X > tagCloudImage.Size().Width)
            throw new ArgumentException("the start position is abroad of image");
    }

    // было красиво было бы если, в аргументах принимали бы массив tags, по их популярности, а не random, но по задачи этого делать не нужно
    public void GenerateCloud(List<WordPopular> words)
    {
        for (var i = 0; i < words.Count; i++)
        {
            var sizeF = tagCloudImage.GetSizeWord(words[i]);
            var size = (sizeF with { Height = sizeF.Height, Width = sizeF.Width + 10}).ToSize();
            
            var rec = cloudLayouter.PutNextRectangle(size);
            var RecCloud = new RectangleTagCloud(rec, words[i].Word);
            tagCloudImage.Draw(RecCloud);
        }
    }

    public void Save()
    {
        tagCloudImage.Save();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool fromMethod)
    {
        if (!IsDisposed)
        {
            if (fromMethod)
            {
                // потенциально это может в будущем пригодится, еще есть tag 2 и 3.
            }

            tagCloudImage.Dispose();

            IsDisposed = true;
        }
    }
}