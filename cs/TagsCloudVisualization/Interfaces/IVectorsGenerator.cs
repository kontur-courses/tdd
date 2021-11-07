﻿using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IVectorsGenerator
    {
        Point GetNextVector();
    }
}