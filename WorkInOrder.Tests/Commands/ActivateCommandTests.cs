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
        private readonly Session _session = new Session();

        public ActivateCommandTests()
        {
            _factory = new CommandFactory(_board.Object);
        }

        [Fact]
        public void MarksGivenTaskAsCurrent()
        {
            var result = ActivateTheTask();

            _board.Verify(x => x.Activate(PendingTask));
            result.Expect(string.Format(Messages.TaskActivated, PendingTask), Format.Neutral);
        }

        [Fact]
        public void FailsToActivateTaskThatDoesNotExist()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskNotFoundException>();

            var result = ActivateTheTask();

            result.Expect(string.Format(Messages.TaskNotFound, PendingTask), Format.Negative);
        }

        [Fact]
        public void DoesNotUpdateTheTaskThatIsAlreadyActive()
        {
            _board.Setup(x => x.Activate(PendingTask)).Throws<TaskAlreadyActiveException>();

            var result = ActivateTheTask();

            result.Expect(string.Format(Messages.TaskAlreadyActive, PendingTask), Format.Negative);
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
            result.Expect(string.Format(Messages.AnotherTaskAlreadyActive, PendingTask,activeTask), Format.Negative);
        }

        [Fact]
        public void ShowsAllTasksFoundAlongWithUniqueNumbersAssignedToNarrowDownTheSearch()
        {
            // Arrange
            var expectedTasks = new[] {"bob", "bob1"};
            _board.Setup(x => x.Activate(PendingTask)).Throws(new NonUniqueNameException(expectedTasks));
            var command = new ActivateCommand(_board.Object, PendingTask, _session);

            // Act
            var results = command.Run();

            // Assert
            results[0].Expect(string.Format(Messages.MoreThanOneTaskFound, PendingTask), Format.Neutral);
            results[1].Expect($"1. {expectedTasks[0]}", Format.Neutral);
            results[2].Expect($"2. {expectedTasks[1]}", Format.Neutral);
        }

        [Fact]
        public void AllowsYouToNarrowDownTheSearchUsingResultNumber()
        {
            // Arrange
            var expectedTasks = new[] { "bob", "bob1" };
            _board.Setup(x => x.Activate(PendingTask)).Throws(new NonUniqueNameException(expectedTasks));
            var command = new ActivateCommand(_board.Object, PendingTask, _session);
            command.Run();

            const string bob1Identifier = "2";
            var narrowingCommand = new ActivateCommand(_board.Object, bob1Identifier, _session);

            // Act
            narrowingCommand.Run();

            // Assert
            _board.Verify(x=>x.Activate(expectedTasks[1]));
        }

        private OutputMessage ActivateTheTask()
        {
            var command = _factory.Identify($"activate {PendingTask}");

            return command.Run().Single();
        }
    }
}