using System;

namespace WorkInOrder
{
    public class Task : ITask, IEquatable<Task>
    {
        private readonly ITaskStorage _storage;

        protected Task(DateTime createdOn, string name, Status status, DateTime? completedOn = null)
        {
            CreatedOn = createdOn;
            Name = name;
            Status = status;
            CompletedOn = completedOn;
        }

        public Task(DateTime createdOn, string name, Status status, DateTime? completedOn, ITaskStorage storage)
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

        public void Complete()
        {
            _storage.UpdateStatus(Name, Status.Done);
            _storage.UpdateCompletionDate(Name, DateTime.Now);
        }

        public void Deactivate()
        {
            _storage.UpdateStatus(Name, Status.Pending);
        }

        public bool Equals(Task other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Task) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}