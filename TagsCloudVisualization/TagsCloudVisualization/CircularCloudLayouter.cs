using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public bool FillFreeRectangles = true;
        public List<Rectangle> FreeRectangles = new List<Rectangle>();

        private Point leftUpperCorner;
        private Point rightBottomCorner;
        private Point center;

        private AddRectangleState currentAddState = AddRectangleState.Rigth_Up;

        private int rectangleCount = 0;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleCount == 0)
            {
                rectangleCount++;
                leftUpperCorner = new Point(
                    -rectangleSize.Width / 2,
                    -rectangleSize.Height / 2);
                rightBottomCorner = new Point(
                    rectangleSize.Width + leftUpperCorner.X,
                    rectangleSize.Height + leftUpperCorner.Y);
                
                return new Rectangle(
                    leftUpperCorner.X + center.X,
                    leftUpperCorner.Y+center.Y,
                    rectangleSize.Width,
                    rectangleSize.Height);
            }


            Rectangle rect = new Rectangle(0, 0, 0, 0);
            if (FillFreeRectangles && CheckForSuitableFreeRectangle(rectangleSize, ref rect))
            {
                FreeRectangles.Remove(rect);
                rect.Width = rectangleSize.Width;
                rect.Height = rectangleSize.Height;
            }
            else
            {
                rect = InitNextRectangle(rectangleSize);
                AddFreeRectangle(rectangleSize);
                ResizeBorders(rectangleSize);

                rectangleCount++;
                if (currentAddState == AddRectangleState.Up_Left) currentAddState = AddRectangleState.Rigth_Up;
                else currentAddState += 1;
            }

            return rect;
        }

        private bool CheckForSuitableFreeRectangle(Size rectangleSize, ref Rectangle rectangle)
        {
            foreach (var rect in FreeRectangles)
            {
                if (rect.Width >= rectangleSize.Width && rect.Height >= rectangleSize.Height)
                {
                    rectangle = rect;
                    return true;
                }
            }

            return false;
        }

        private void AddFreeRectangle(Size rectangleSize)
        {
            switch (currentAddState)
            {
                case AddRectangleState.Rigth_Up:
                {
                    if (rightBottomCorner.Y > leftUpperCorner.Y + rectangleSize.Height)
                    {
                        Rectangle rect =
                            new Rectangle(rightBottomCorner.X, leftUpperCorner.Y + rectangleSize.Height,
                                rectangleSize.Width, rightBottomCorner.Y - leftUpperCorner.Y - rectangleSize.Height);
                        FreeRectangles.Add(rect);
                    }
                    else if (rightBottomCorner.Y < leftUpperCorner.Y + rectangleSize.Height)
                    {
                        Rectangle rect =
                            new Rectangle(leftUpperCorner.X, rightBottomCorner.Y,
                                rightBottomCorner.X - leftUpperCorner.X,
                                leftUpperCorner.Y + rectangleSize.Height - rightBottomCorner.Y);
                        FreeRectangles.Add(rect);
                    }
                }
                    break;

                case AddRectangleState.Bottom_Right:
                {
                    if (leftUpperCorner.X < rightBottomCorner.X - rectangleSize.Width)
                    {
                        Rectangle rect =
                            new Rectangle(leftUpperCorner.X, rightBottomCorner.Y,
                                rightBottomCorner.X - leftUpperCorner.X - rectangleSize.Width, rectangleSize.Height);
                        FreeRectangles.Add(rect);
                    }
                    else if (leftUpperCorner.X > rightBottomCorner.X - rectangleSize.Width)
                    {
                        Rectangle rect =
                            new Rectangle(rightBottomCorner.X - rectangleSize.Width, leftUpperCorner.Y,
                                rectangleSize.Width - rightBottomCorner.X + leftUpperCorner.X,
                                rightBottomCorner.Y - leftUpperCorner.Y);
                        FreeRectangles.Add(rect);
                    }
                }
                    break;

                case AddRectangleState.Left_Bottom:
                {
                    if (leftUpperCorner.Y < rightBottomCorner.Y - rectangleSize.Height)
                    {
                        Rectangle rect =
                            new Rectangle(leftUpperCorner.X - rectangleSize.Width, leftUpperCorner.Y,
                                rectangleSize.Width, rightBottomCorner.Y - leftUpperCorner.Y - rectangleSize.Height);
                        FreeRectangles.Add(rect);
                    }
                    else if (leftUpperCorner.Y > rightBottomCorner.Y - rectangleSize.Height)
                    {
                        Rectangle rect =
                            new Rectangle(leftUpperCorner.X, rightBottomCorner.Y - rectangleSize.Height,
                                rightBottomCorner.X - leftUpperCorner.X,
                                rectangleSize.Height - rightBottomCorner.Y - leftUpperCorner.Y);
                        FreeRectangles.Add(rect);
                    }
                }
                    break;

                case AddRectangleState.Up_Left:
                {
                    if (rightBottomCorner.X > leftUpperCorner.X + rectangleSize.Width)
                    {
                        Rectangle rect =
                            new Rectangle(leftUpperCorner.X + rectangleSize.Width,
                                leftUpperCorner.Y - rectangleSize.Height,
                                rightBottomCorner.X - leftUpperCorner.X - rectangleSize.Width,
                                rectangleSize.Height);
                        FreeRectangles.Add(rect);
                    }
                    else if (rightBottomCorner.X < leftUpperCorner.X + rectangleSize.Width)
                    {
                        Rectangle rect =
                            new Rectangle(rightBottomCorner.X, leftUpperCorner.Y,
                                leftUpperCorner.X + rectangleSize.Width - rightBottomCorner.X,
                                rightBottomCorner.Y - leftUpperCorner.Y);
                        FreeRectangles.Add(rect);
                    }
                }
                    break;
            }
        }

        private void ResizeBorders(Size rectangleSize)
        {
            switch (currentAddState)
            {
                case AddRectangleState.Rigth_Up:
                    {
                        rightBottomCorner.Y = Math.Max(rightBottomCorner.Y, leftUpperCorner.Y + rectangleSize.Height);
                        rightBottomCorner.X = rightBottomCorner.X + rectangleSize.Width;
                    }
                    break;

                case AddRectangleState.Bottom_Right:
                    {
                        rightBottomCorner.Y = rightBottomCorner.Y + rectangleSize.Height;
                        leftUpperCorner.X = Math.Min(leftUpperCorner.X, rightBottomCorner.X - rectangleSize.Width);
                    }
                    break;

                case AddRectangleState.Left_Bottom:
                    {
                        leftUpperCorner.Y = Math.Min(leftUpperCorner.Y, rightBottomCorner.Y - rectangleSize.Height);
                        leftUpperCorner.X = leftUpperCorner.X - rectangleSize.Width;
                    }
                    break;

                case AddRectangleState.Up_Left:
                    {
                        rightBottomCorner.X = Math.Max(rightBottomCorner.X, leftUpperCorner.X + rectangleSize.Width);
                        leftUpperCorner.Y = leftUpperCorner.Y - rectangleSize.Height;
                    }
                    break;
            }
        }
        private Rectangle InitNextRectangle(Size rectangleSize)
        {
            Rectangle initialisedRectangle = new Rectangle(0, 0, 0, 0);
            switch (currentAddState)
            {
                case AddRectangleState.Rigth_Up:
                {
                    initialisedRectangle = new Rectangle(
                        rightBottomCorner.X + center.X,
                        leftUpperCorner.Y + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

                case AddRectangleState.Bottom_Right:
                {
                    initialisedRectangle = new Rectangle(
                        rightBottomCorner.X - rectangleSize.Width + center.X,
                        rightBottomCorner.Y + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);

                }
                    break;

                case AddRectangleState.Left_Bottom:
                {
                    initialisedRectangle = new Rectangle(
                        leftUpperCorner.X - rectangleSize.Width + center.X,
                        rightBottomCorner.Y - rectangleSize.Height + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

                case AddRectangleState.Up_Left:
                {
                    initialisedRectangle = new Rectangle(
                        leftUpperCorner.X + center.X,
                        leftUpperCorner.Y - rectangleSize.Height + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

            }

            return initialisedRectangle;
        }

        private enum AddRectangleState
        {
            Rigth_Up,
            Bottom_Right,
            Left_Bottom,
            Up_Left
        }
    }
}
