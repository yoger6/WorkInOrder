using System;

namespace WorkInOrder
{
    public class Task : ITask
    {
        private readonly ITaskStorage _storage;

        public Task(DateTime createdOn, string name, Status status, DateTime? completedOn = null)
        {
            CreatedOn = createdOn;
            Name = name;
            Status = status;
            CompletedOn = completedOn;
        }

        protected Task(DateTime createdOn, string name, Status status, DateTime? completedOn, ITaskStorage storage)
        : this(createdOn, name, status, completedOn)
        {
            _storage = storage;
        }

        public DateTime CreatedOn { get; }
        public DateTime? CompletedOn { get; }
        public string Name { get; }
        public Status Status { get; }
        public bool IsCompleted => Status == Status.Done;

        void ITask.Skip()
        {
            _storage.UpdateStatus(Name, Status.Skipped);
        }

        public void Activate()
        {
            _storage.UpdateStatus(Name, Status.Current);
        }
    }
}