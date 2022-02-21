using System;

namespace WorkInOrder.Tests.BusinessLogic
{
    internal class TestTask : Task
    {
        public static Task Active()
        {
            return new TestTask(Status.Current);
        }

        private TestTask(Status status) 
            : base(DateTime.Now, Guid.NewGuid().ToString(), status)
        {
        }
    }
}