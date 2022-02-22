using System;

namespace WorkInOrder
{
    public interface ITaskStorage
    {
        void Create(DateTime createdOn, string content, Status status = Status.Pending);
        ITask[] GetAll();
        void UpdateStatus(string name, Status status);
        ITask Find(Status status);
        ITask Find(string name);
        ITask FindFirstAvailableSince(DateTime since);
        void UpdateCompletionDate(string content, DateTime date);
    }
}