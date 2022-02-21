using System;
using System.Linq;
using Moq;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests
{
    public class SkipCommandTests
    {
        private const string ExistingTask = "task";
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public SkipCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
            _storage.Setup(x => x.GetTasks()).Returns(new[] { new Task(DateTime.Now, ExistingTask, Status.Pending) });
        }

        [Fact]
        public void CannotSkipIfThereIsNoCurrentTask()
        {
            var result = Run();

            result.Single().Expect("There's not active task to skip.", Format.Negative);
        }

        [Fact]
        public void MarksCurrentTaskAsSkipped()
        {
            _storage.Setup(x => x.GetTasks()).Returns(new[] { new Task(DateTime.Now, ExistingTask, Status.Current) });

            Run();

            _storage.Verify(x=>x.UpdateStatus(ExistingTask, Status.Skipped));
        }

        [Fact]
        public void ActivatesSubsequentTask()
        {
            const string freshTask = "aah fresh meat";
            _storage.Setup(x => x.GetTasks()).Returns(new[] {new Task(DateTime.Now, freshTask, Status.Pending), new Task(DateTime.Now.AddDays(-1), ExistingTask, Status.Current)});

            Run();

            _storage.Verify(x => x.UpdateStatus(freshTask, Status.Current));
        }

        private OutputMessage[] Run()
        {
            var command = _factory.Identify("skip");

            return command.Run();
        }
    }
}