using System;
using System.Linq;
using Moq;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests
{
    public class DoneCommandTests
    {
        private const string ExistingTask = "task";
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public DoneCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
            _storage.Setup(x => x.GetAll()).Returns(new[] { new Task(DateTime.Now, ExistingTask, Status.Pending) });
        }

        [Fact]
        public void CannotCompleteTaskWhenThereIsNoActiveOne()
        {
            var result = Complete();

            result.Single().Expect("There's not active task to complete", Format.Negative);
        }

        [Fact]
        public void MarkCurrentTaskAsDone()
        {
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, ExistingTask, Status.Current)});

            Complete();

            _storage.Verify(x => x.UpdateStatus(ExistingTask, Status.Done));
        }

        [Fact]
        public void ActivateSubsequentTask()
        {
            const string nextTask = "something to get busy with;";
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, ExistingTask, Status.Current), new Task(DateTime.Now.AddDays(1), nextTask, Status.Pending)});

            Complete();

            _storage.Verify(x => x.UpdateStatus(nextTask, Status.Current));
        }

        private OutputMessage[] Complete()
        {
            var command = _factory.Identify("done");
            return command.Run();
        }
    }
}