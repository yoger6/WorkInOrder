using System;

namespace WorkInOrder
{
    public interface ITask
    {
        DateTime CreatedOn { get; }
        DateTime? CompletedOn { get; }
        string Name { get; }
        Status Status { get; }
        bool IsCompleted { get; }

        void Skip();
        void Activate();
    }
}