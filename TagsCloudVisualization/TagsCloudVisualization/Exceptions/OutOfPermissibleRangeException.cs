using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Exceptions
{
    class OutOfPermissibleRangeException: Exception
    {
        public OutOfPermissibleRangeException(string massage) : base(massage)
        {

        }
    }
}
