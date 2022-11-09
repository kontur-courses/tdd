// using System.Drawing;
// using NUnit.Framework;
//
// namespace TagsCloudVisualization.Tests
// {
//     [TestFixture]
//     public class RectanglePlacerShould
//     {
//         private RectanglePlacer rectanglePlacer;
//
//         [SetUp]
//         public void SetUp()
//         {
//             rectanglePlacer = new RectanglePlacer();
//         }
//
//         [TestCaseSource(typeof(TestData), nameof(TestData.DefaultPointsAndSizeForPlace))]
//         [Parallelizable(scope: ParallelScope.All)] 
//         public Rectangle Place_DefaultPointsAndSize_MiddlePlace(int x, int y, int width, int height) =>
//             rectanglePlacer.Place(new Point(x, y), new Size(width, height));
//     }
// }