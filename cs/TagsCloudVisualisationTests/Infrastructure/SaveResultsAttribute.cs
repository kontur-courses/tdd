using System;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualisationTests.Infrastructure
{
    public class SaveResultsAttribute : Attribute
    {
        public readonly TestStatus[] ValidStatuses;

        public SaveResultsAttribute(params TestStatus[] validStatuses)
        {
            ValidStatuses = validStatuses;
        }
    }
}