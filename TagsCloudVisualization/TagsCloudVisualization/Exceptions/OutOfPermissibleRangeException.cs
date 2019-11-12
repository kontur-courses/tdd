using System;

namespace TagsCloudVisualization.Exceptions
{
    class OutOfPermissibleRangeException: Exception
    {
        public OutOfPermissibleRangeException(string massage) : base(massage)
        {

        }
    }
}
