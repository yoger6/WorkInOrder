using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using WorkInOrder.Tests.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class SwitchCommandTests
    {
        private readonly Task _existingTask;
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public SwitchCommandTests()
        {
            _factory = new CommandFactory(Mock.Of<ITaskStorage>(), _board.Object);
            _existingTask = TestTask.Active();
        }

        [Fact]
        public void SwitchesToNewTask()
        {
            var result = Run(_existingTask.Name);

            _board.Verify(x => x.Switch(_existingTask.Name));
        }

        [Fact]
        public void CannotSwitchToTaskThatDoesNotExist()
        {
            const string task = "abcd";
            _board.Setup(x => x.Switch(task)).Throws<TaskNotFoundException>();

            var result = Run(task);

            result.Expect($"{task} does not exist", Format.Negative);
        }
        
        [Fact]
        public void CannotSwitchWhenNoTaskIsActive()
        {
            const string task = "abcd";
            _board.Setup(x => x.Switch(task)).Throws<NoActiveTaskException>();

            var result = Run(task);

            result.Expect($"Cannot switch to {task} as there's no active task to switch from. Rather use Activate command.", Format.Negative);
        }

        [Fact]
        public void CannotSwitchToTaskThatIsAlreadyActive()
        {
            // Arrange
            const string task = "abcd";
            _board.Setup(x => x.Switch(task)).Throws<TaskAlreadyActiveException>();

            // Act
            var result = Run(task);

            // Assert
            result.Expect($"{task} is already active", Format.Neutral);
        }

        private OutputMessage Run(string task)
        {
            var command = _factory.Identify($"switch {task}");

            return command.Run().Single();
        }
    }
}