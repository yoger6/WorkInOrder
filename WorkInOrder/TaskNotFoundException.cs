using System;

namespace WorkInOrder
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string name) : base($"Task {name} doesn't exist")
        {
        }
    }
}