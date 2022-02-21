using System;

namespace WorkInOrder
{
    public interface ITaskStorage
    {
        void Create(DateTime createdOn, string content, Status status = Status.Pending);
        Task[] GetAll();
        void UpdateStatus(string name, Status status);
        Task Find(Status status);
    }
}