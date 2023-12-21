﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public interface ILayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}
