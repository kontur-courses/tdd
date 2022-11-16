using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualisation;

[TestFixture]
public class CircularCloudLayouter_Should
{
    [Test]
    public void CircularCloudLayouter_CorrectCenterPoint_ShouldNotThrowException()
    {
        var createCircularCLoudLayouter = 
            () => new CircularCloudLayouter(new Point(1, 1));
        createCircularCLoudLayouter
            .Should()
            .NotThrow();
    }
    
    [TestCase("Center can't be empty", 
        0, 
        0, 
        true, 
        TestName = "Empty center point should throw exception")]
    [TestCase("Center can't be located outside of drawing field", 
        -1, 
        -1, 
        false, 
        TestName = "Center point can't have negative values")]
    public void CircularCloudLayouter_IncorrectValues_ShouldThrowArgumentException(string message, int x, int y, bool isEmpty)
    {
        var point = isEmpty ? new Point() : new Point(x, y);
        var createCircularCLoudLayouter = 
            () => new CircularCloudLayouter(point);
        
        ThrowArgumentExceptionWithMessage(createCircularCLoudLayouter, message);
    }
    
    [Test]
    public void PutNextRectangle_CorrectSize_ShouldNoThrowException()
    {
        var func = () => new CircularCloudLayouter(new Point(1, 1))
                .PutNextRectangle(new Size(20, 20));

        func
            .Should()
            .NotThrow();
    }
    
    [TestCase("Rectangle size can't be empty", 
        0, 
        0, 
        true, 
        TestName = "Empty rectangle size should throw exception")]
    [TestCase("Rectangle size can't have negative side value", 
        -1, 
        -1, 
        false, 
        TestName = "Negative rectangle size should throw exception")]
    public void PutNextRectangle_IncorrectValues_ShouldThrowArgumentException(string message, int width, int height, bool isEmpty)
    {
        var size = isEmpty ? new Size() : new Size(width, height);
        var func = () => new CircularCloudLayouter(new Point(1, 1))
                .PutNextRectangle(size);
        
        ThrowArgumentExceptionWithMessage(func, message);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
        {
            GetDescriptionOfFailed();
        }
    }
    
    private void ThrowArgumentExceptionWithMessage<T>(Func<T> action, string message)
    {
        action 
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(message);
    }

    private void GetDescriptionOfFailed()
    {
        var screenName = ScreenShot.TakeScreenShot();
        var pathToScreenShot =
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, screenName);
        
        Console.WriteLine("Tag cloud visualization saved to file {0}", pathToScreenShot);
    }
}