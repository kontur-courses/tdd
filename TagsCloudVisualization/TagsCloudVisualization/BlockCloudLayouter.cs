using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class BlockCloudLayouter : ICloudLayouter
    {
        public List<Rectangle> FreeRectangles = new List<Rectangle>();
        public List<Rectangle> PlacedRectangles { get; } = new List<Rectangle>();

        private Point leftUpperCorner;
        private Point rightBottomCorner;
        private Point center;

        private AddRectangleState currentAddState = AddRectangleState.RightUp;

        private int rectangleCount = 0;

        public BlockCloudLayouter(Point center)
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

                PlacedRectangles.Add(
                    new Rectangle(
                        leftUpperCorner.X + center.X,
                        leftUpperCorner.Y + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height));
                return PlacedRectangles[0];
            }


            Rectangle rect = new Rectangle(0, 0, 0, 0);
            int suitableRectIndex = SmallestSuitableFreeRectangleIndex(rectangleSize);
            if (suitableRectIndex != -1)
            {
                rect = CutOutFromFreeRectangleByIndex(suitableRectIndex, rectangleSize);
                rect.X += center.X;
                rect.Y += center.Y;
            }
            else
            {
                rect = InitNextRectangle(rectangleSize);
                AddFreeRectangle(rectangleSize);
                ResizeBorders(rectangleSize);

                rectangleCount++;
                if (currentAddState == AddRectangleState.UpLeft) currentAddState = AddRectangleState.RightUp;
                else currentAddState += 1;
            }

            PlacedRectangles.Add(rect);
            return rect;
        }

        private int SmallestSuitableFreeRectangleIndex(Size rectangleSize)
        {
            int index = -1;
            int minSquare = -1;
            for (int i = 0; i < FreeRectangles.Count; i++)
            {
                var rect = FreeRectangles[i];
                if (rect.Width >= rectangleSize.Width && rect.Height >= rectangleSize.Height)
                {
                    if (index == -1 || minSquare > rect.Width * rect.Height)
                    {
                        index = i;
                        minSquare = rect.Width * rect.Height;
                    }
                }
            }

            return index;
        }

        private Rectangle CutOutFromFreeRectangleByIndex(int index, Size rectangleSize)
        {
            Rectangle freeRect = FreeRectangles[index];
            FreeRectangles.RemoveAt(index);
            if (freeRect.Width > rectangleSize.Width)
            {
                FreeRectangles.Add(new Rectangle(freeRect.X + rectangleSize.Width, freeRect.Y,
                    freeRect.Width - rectangleSize.Width, rectangleSize.Height));
            }

            if (freeRect.Height > rectangleSize.Height)
            {
                FreeRectangles.Add(new Rectangle(freeRect.X, freeRect.Y + rectangleSize.Height,
                    freeRect.Width, freeRect.Height - rectangleSize.Height));
            }

            return new Rectangle(freeRect.X, freeRect.Y, rectangleSize.Width, rectangleSize.Height);
        }

        private void AddFreeRectangle(Size rectangleSize)
        {
            switch (currentAddState)
            {
                case AddRectangleState.RightUp:
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

                case AddRectangleState.BottomRight:
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

                case AddRectangleState.LeftBottom:
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

                case AddRectangleState.UpLeft:
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
                case AddRectangleState.RightUp:
                {
                    rightBottomCorner.Y = Math.Max(rightBottomCorner.Y, leftUpperCorner.Y + rectangleSize.Height);
                    rightBottomCorner.X = rightBottomCorner.X + rectangleSize.Width;
                }
                    break;

                case AddRectangleState.BottomRight:
                {
                    rightBottomCorner.Y = rightBottomCorner.Y + rectangleSize.Height;
                    leftUpperCorner.X = Math.Min(leftUpperCorner.X, rightBottomCorner.X - rectangleSize.Width);
                }
                    break;

                case AddRectangleState.LeftBottom:
                {
                    leftUpperCorner.Y = Math.Min(leftUpperCorner.Y, rightBottomCorner.Y - rectangleSize.Height);
                    leftUpperCorner.X = leftUpperCorner.X - rectangleSize.Width;
                }
                    break;

                case AddRectangleState.UpLeft:
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
                case AddRectangleState.RightUp:
                {
                    initialisedRectangle = new Rectangle(
                        rightBottomCorner.X + center.X,
                        leftUpperCorner.Y + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

                case AddRectangleState.BottomRight:
                {
                    initialisedRectangle = new Rectangle(
                        rightBottomCorner.X - rectangleSize.Width + center.X,
                        rightBottomCorner.Y + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

                case AddRectangleState.LeftBottom:
                {
                    initialisedRectangle = new Rectangle(
                        leftUpperCorner.X - rectangleSize.Width + center.X,
                        rightBottomCorner.Y - rectangleSize.Height + center.Y,
                        rectangleSize.Width,
                        rectangleSize.Height);
                }
                    break;

                case AddRectangleState.UpLeft:
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

        public Rectangle GetBorders()
        {
            return new Rectangle(leftUpperCorner.X + center.X, leftUpperCorner.Y + center.Y,
                rightBottomCorner.X - leftUpperCorner.X, rightBottomCorner.Y - leftUpperCorner.Y);
        }

        private enum AddRectangleState
        {
            RightUp,
            BottomRight,
            LeftBottom,
            UpLeft
        }
    }
}