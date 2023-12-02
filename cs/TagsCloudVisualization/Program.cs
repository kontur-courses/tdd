// See https://aka.ms/new-console-template for more information

using TagsCloudVisualization;

var dict = WordsDataSet.CreateFrequencyDict(
    "../../../../TagsCloudVisualizationTests/bigAmountOfWords.txt"
);

var circularCloudLayouter = new CircularCloudLayouter(dict);

circularCloudLayouter.GenerateTagCloud("rectangles");