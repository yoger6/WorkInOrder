using System;

namespace WorkInOrder
{
    public interface ITaskStorage
    {
        void Create(DateTime createdOn, string content);
        Task[] GetTasks();
        void UpdateStatus(string name, Status status);
    }
}