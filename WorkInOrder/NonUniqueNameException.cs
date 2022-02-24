using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkInOrder
{
    public class NonUniqueNameException : Exception
    {
        public string[] TasksFound { get; }
        public NonUniqueNameException(IEnumerable<string> taskNamesFound)
        {
            TasksFound = taskNamesFound.ToArray();
        }
    }
}