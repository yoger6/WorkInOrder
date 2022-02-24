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

        public static ITask Active(DateTime creationDate)
        {
            return new TestTask(creationDate, Status.Current);
        }

        public static Task Pending(ITaskStorage storage)
        {
            return new TestTask(Status.Pending, storage);
        }

        public static Task Pending(DateTime creationDate)
        {
            return new TestTask(creationDate, Status.Pending);
        }

        public static ITask Active(ITaskStorage storage)
        {
            return new TestTask(Status.Current, storage);
        }


        public static ITask Done(DateTime creationDate)
        {
            return new TestTask(creationDate, Status.Done);
        }

        public static ITask Done(DateTime creationDate, DateTime completionDate)
        {
            return new TestTask(creationDate, Status.Done, completionDate);
        }

        public static ITask Skipped(DateTime creationDate)
        {
            return new TestTask(creationDate, Status.Skipped);
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

        public TestTask(DateTime creationDate, Status status, DateTime? completionDate = null)
            : base(creationDate, Guid.NewGuid().ToString(), status, completionDate, Mock.Of<ITaskStorage>())
        {

        }
    }
}