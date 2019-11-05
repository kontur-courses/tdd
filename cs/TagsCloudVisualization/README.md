```c#
    var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
    for (var i = 0; i < 50; i++)
        circularCloudLayouter.PutNextRectangle(new Size(50, 50));
    var cloudImageCreator = new TagCloudImageCreator(circularCloudLayouter);
    cloudImageCreator.Save();
```
<img src="Clouds\1.jpg">

```c#
    var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
    for (var i = 0; i < 50; i++)
        circularCloudLayouter.PutNextRectangle(new Size(50 + i, 50 - i));
    var cloudImageCreator = new TagCloudImageCreator(circularCloudLayouter);
    cloudImageCreator.Save();
```
<img src="Clouds\2.jpg">