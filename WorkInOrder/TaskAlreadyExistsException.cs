using System;

namespace WorkInOrder
{
    public class TaskAlreadyExistsException : Exception
    {
        public TaskAlreadyExistsException(string name) : base($"Task {name} already exists")
        {
        }
    }
}