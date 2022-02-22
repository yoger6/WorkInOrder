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
            var nextTask = _storage.FindFirstAvailableSince(CreatedOn);
            _storage.UpdateStatus(Name, Status.Skipped);
            if (nextTask != null)
            {
                nextTask.Activate();
            }
        }

        public void Activate()
        {
            throw new NotImplementedException();
        }
    }
}