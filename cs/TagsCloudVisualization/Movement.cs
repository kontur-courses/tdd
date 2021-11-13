using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Movement
    {
        private readonly MovementType movementType;
        private readonly int distance;

        public Movement(MovementType movementType, int distance)
        {
            this.movementType = movementType;
            this.distance = distance;
        }

        public bool CanDoMovement()
        {
            return distance > 0 && distance != int.MaxValue;
        }

        public Rectangle MoveRectangle(Rectangle rectangle)
        {
            switch (movementType)
            {
                case MovementType.Down:
                    rectangle.Y += distance;
                    break;
                case MovementType.Up:
                    rectangle.Y -= distance;
                    break;
                case MovementType.Left:
                    rectangle.X -= distance;
                    break;
                case MovementType.Right:
                    rectangle.X += distance;
                    break;
                default:
                    throw new Exception("Unknown MovementType");
            }

            return rectangle;
        }
    }
}