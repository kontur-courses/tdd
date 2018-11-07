using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Spiral
    {
        public readonly Point Center;
        private Point currentLocation;

        private Direction currentDirection;
        private int stepCount;

        private IEnumerator<Point> locations;

        public Spiral(Point center)
        {
            Center = center;
            currentLocation = center;
            currentDirection = Direction.Up;
            stepCount = 1;

            locations = GetLocations();
        }

        private IEnumerator<Point> GetLocations()
        {
            yield return currentLocation;

            while (true)
            {
                for (var i = 0; i < stepCount; i++)
                {
                    MoveOnDirection();
                    yield return currentLocation;
                }

                ChangeDirection();

                for (var i = 0; i < stepCount; i++)
                {
                    MoveOnDirection();
                    yield return currentLocation;
                }

                stepCount++;
                ChangeDirection();
            }
        }

        private void MoveOnDirection()
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    currentLocation.Y--;
                    break;
                case Direction.Right:
                    currentLocation.X++;
                    break;
                case Direction.Down:
                    currentLocation.Y++;
                    break;
                case Direction.Left:
                    currentLocation.X--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeDirection()
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    currentDirection = Direction.Right;
                    break;
                case Direction.Right:
                    currentDirection = Direction.Down;
                    break;
                case Direction.Down:
                    currentDirection = Direction.Left;
                    break;
                case Direction.Left:
                    currentDirection = Direction.Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Point GetNextLocation()
        {
            locations.MoveNext();
            return locations.Current;
        }
    }
}