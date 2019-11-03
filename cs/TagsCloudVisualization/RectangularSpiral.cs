using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class RectangularSpiral : ISpiral
    {
        static Point[] directions = new Point[]
        {
            new Point(0, 1),
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
        };

        public IEnumerable<Point> GetPoints()
        {
            var stepNumber = 0;
            var curentPosition = new Point(0, 0);
            yield return curentPosition;

            while (true)
            {
                var countStepsInCurentDirection = (stepNumber / 2) + 1;
                var curentDirection = directions[stepNumber % 4];
                for (int i = 0; i < countStepsInCurentDirection; i++)
                {
                    curentPosition = new Point(curentPosition.X + curentDirection.X, curentPosition.Y + curentDirection.Y);
                    yield return curentPosition;
                }
                stepNumber += 1;
            }
        }
    }
}
