using System.Drawing;
using TagsCloudVisualization;


var generator = new LayoutGenerator(new Point(0, 0), new Size(800, 600));

generator.GenerateLayout("layout1.png", 100, random => new Size(random.Next(20, 100), random.Next(10, 50)));
generator.GenerateLayout("layout2.png", 150, random => new Size(50, 20));
generator.GenerateLayout("layout3.png", 200, random => new Size(random.Next(10, 30), random.Next(10, 30)));