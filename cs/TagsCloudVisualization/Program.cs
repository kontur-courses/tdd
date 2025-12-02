using System.Drawing;
using TagsCloudVisualization;

var workingDirectory = Environment.CurrentDirectory;
var projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;
var layouter = new CircularCloudLayouter(new Point(0, 0));
var visualizer = new CloudVisualizer(new ImageCreator(new ImageOptions()), layouter);
visualizer.CreateAndSave(1000, 1, projectDirectory);
visualizer.CreateAndSave(500, 1, projectDirectory);
visualizer.CreateAndSave(250, 1, projectDirectory);