using System;
using Moq;

namespace WorkInOrder.Tests.BusinessLogic
{
    internal class TestTask : Task
    {
        public static Task Pending()
        {
            return new TestTask(Status.Pending);
        }

        public static Task Active()
        {
            return new TestTask(Status.Current);
        }

        public static Task Pending(ITaskStorage storage)
        {
            return new TestTask(Status.Pending, storage);
        }

        public static ITask Active(ITaskStorage storage)
        {
            return new TestTask(Status.Current, storage);
        }

        public static Mock<ITask> Mocked() => new Mock<ITask>();

        private TestTask(Status status) 
            : base(DateTime.Now, Guid.NewGuid().ToString(), status)
        {
        }

        private TestTask(Status status, ITaskStorage storage)
            : base(DateTime.Now, Guid.NewGuid().ToString(), status, null, storage)
        {
        }
    }
}