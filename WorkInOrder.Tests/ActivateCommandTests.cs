using System;
using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests
{
    public class ActivateCommandTests
    {
        private const string PendingTask = "task";
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public ActivateCommandTests()
        {
            _factory = new CommandFactory(_storage.Object, _board.Object);
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, PendingTask, Status.Pending)});
        }

        [Fact]
        public void MarksGivenTaskAsCurrent()
        {
            var command = _factory.Identify($"activate {PendingTask}");

            command.Run();

            _storage.Verify(x=>x.UpdateStatus(PendingTask, Status.Current));
        }

        [Fact]
        public void SkipsTaskThatWasPreviouslySetAsCurrent()
        {
            const string previousTask = "boringTask";
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, previousTask, Status.Current), new Task(DateTime.Now, PendingTask, Status.Pending)});
            var command = _factory.Identify($"activate {PendingTask}");

            command.Run();

            _storage.Verify(x=>x.UpdateStatus(previousTask, Status.Skipped));
        }

        [Fact]
        public void FailsToActivateTaskThatDoesNotExist()
        {
            const string taskName = "stuff";
            var command = _factory.Identify($"activate {taskName}");

            var result = command.Run();

            result.Single().Expect($"{taskName} not found", Format.Negative);
        }

        [Fact]
        public void DoesNotUpdateTheTaskThatIsAlreadyActive()
        {
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, PendingTask, Status.Current)});
            var command = _factory.Identify($"activate {PendingTask}");

            var result = command.Run();

            result.Single().Expect($"{PendingTask} is already active", Format.Neutral);
        }
    }
}