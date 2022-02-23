using System;

namespace WorkInOrder
{
    public class AnotherTaskAlreadyActiveException : Exception
    {
        public AnotherTaskAlreadyActiveException(string taskName)
        {
            TaskName = taskName;
        }

        public string TaskName { get; }
    }
}