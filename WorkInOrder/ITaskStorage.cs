using System;

namespace WorkInOrder
{
    public interface ITaskStorage
    {
        void Create(DateTime listDate, string content);
        Task[] GetTasks(DateTime today);
    }

    public class Task
    {
        public Task(DateTime createdOn, string name, Status status)
        {
            CreatedOn = createdOn;
            Name = name;
            Status = status;
        }

        public DateTime CreatedOn { get; }
        public string Name { get; }
        public Status Status { get; }
    }

    public enum Status
    {
        Pending,
        Current,
        Done,
        Skipped
    }
}