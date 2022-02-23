using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
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
            var result = ActivateTheTask();

            _board.Verify(x => x.Activate(PendingTask));
            result.Expect($"{PendingTask} is now active", Format.Neutral);
        }

        [Fact]
        public void FailsToActivateTaskThatDoesNotExist()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskNotFoundException>();

            var result = ActivateTheTask();

            result.Expect($"{PendingTask} not found", Format.Negative);
        }

        [Fact]
        public void DoesNotUpdateTheTaskThatIsAlreadyActive()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskAlreadyActiveException>();

            var result = ActivateTheTask();

            result.Expect($"{PendingTask} is already active", Format.Negative);
        }

        [Fact]
        public void DoesNotActivateTaskWhenAnotherOneIsActive()
        {
            // Arrange
            const string activeTask = "I'm the active one!";
            _board.Setup(x => x.Activate(PendingTask)).Throws(new AnotherTaskAlreadyActiveException(activeTask));

            // Act
            var result = ActivateTheTask();

            // Assert
            result.Expect($"Cannot activate {PendingTask} as there's another active task: {activeTask}. To switch active task use the switch command.", Format.Negative);
        }

        private OutputMessage ActivateTheTask()
        {
            var command = _factory.Identify($"activate {PendingTask}");

            return command.Run().Single();
        }
    }
}