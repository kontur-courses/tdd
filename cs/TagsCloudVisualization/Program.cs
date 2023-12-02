// See https://aka.ms/new-console-template for more information

using TagsCloudVisualization;

var dict = WordsDataSet.CreateFrequencyDict(
    "../../../../TagsCloudVisualization/src/words.txt"
);

var circularCloudLayouter = new CircularCloudLayouter(dict);

circularCloudLayouter.GenerateTagCloud("out/rectangles");