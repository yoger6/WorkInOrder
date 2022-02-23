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
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public ActivateCommandTests()
        {
            _factory = new CommandFactory(Mock.Of<ITaskStorage>(), _board.Object);
        }

        [Fact]
        public void MarksGivenTaskAsCurrent()
        {
            var command = _factory.Identify($"activate {PendingTask}");

            var result = command.Run();

            _board.Verify(x => x.Activate(PendingTask));
            result.Single().Expect($"{PendingTask} is now active", Format.Neutral);
        }

        [Fact]
        public void FailsToActivateTaskThatDoesNotExist()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskNotFoundException>();
            var command = _factory.Identify($"activate {PendingTask}");

            var result = command.Run();

            result.Single().Expect($"{PendingTask} not found", Format.Negative);
        }

        [Fact]
        public void DoesNotUpdateTheTaskThatIsAlreadyActive()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskAlreadyActiveException>();
            var command = _factory.Identify($"activate {PendingTask}");

            var result = command.Run();

            result.Single().Expect($"{PendingTask} is already active", Format.Neutral);
        }
    }
}