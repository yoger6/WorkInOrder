using System;

namespace WorkInOrder
{
    public class Task : ITask
    {
        public Task(DateTime createdOn, string name, Status status, DateTime? completedOn = null)
        {
            CreatedOn = createdOn;
            Name = name;
            Status = status;
            CompletedOn = completedOn;
        }

        public DateTime CreatedOn { get; }
        public DateTime? CompletedOn { get; }
        public string Name { get; }
        public Status Status { get; }
        public bool IsCompleted => Status == Status.Done;
        public void Skip()
        {
            throw new NotImplementedException();
        }
    }
}