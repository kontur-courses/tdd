using System.Drawing;
using System.Drawing.Imaging;


namespace TagsCloudVisualization;

public class CloudRunner
{
    
    private readonly CircularCloudLayouter layouter;
    private readonly List<Rectangle> placeRectangles;
    private readonly TextProcessor textProcessor;
    private readonly List<WordData> wordDataList;
    private readonly AppSettings appSettings;
    
    public CloudRunner(AppSettings appSettings)
    {
        this.appSettings = appSettings;
        
        layouter = new CircularCloudLayouter( 
            appSettings, new SpiralPointGenerator(new Point(appSettings.ImageSize.Width / 2, 
                appSettings.ImageSize.Height / 2), appSettings.SpiralDensity, appSettings.Angle));
        textProcessor = new TextProcessor(appSettings);
        wordDataList = new List<WordData>();
        placeRectangles = new List<Rectangle>();
    }

    public void Run()
    {
        textProcessor.Process();
        var processWords = textProcessor.ProcessWords;
        
        foreach (var word in processWords)
        {
            var rect = layouter.PutNextRectangle(word.Size);
            wordDataList.Add(new WordData(word.Word, word.WordFont, word.Size));
            placeRectangles.Add(rect);
        }
        var visualizer = new CreateCloud(placeRectangles, appSettings, processWords); 
        
        visualizer.SaveImage(appSettings.OutputPath, ImageFormat.Png);
        
    }
}