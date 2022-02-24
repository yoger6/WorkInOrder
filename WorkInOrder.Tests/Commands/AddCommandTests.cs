using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class AddCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public AddCommandTests()
        {
            _factory = new CommandFactory(_board.Object);
        }

        [Fact]
        public void AddCommand_AddsNewTaskToTheBoard()
        {
            const string something = "Something";
            var command = _factory.Identify($"add {something}");

            var result = command.Run().Single();

            _board.Verify(x => x.Add(something));
            result.Expect($"{something} has been added to current todo list", Format.Neutral);
        }

        [Fact]
        public void AddCommand_ErrorWhenEmptyName()
        {
            _board.Setup(x => x.Add(string.Empty)).Throws<MissingContentException>();
            var command = _factory.Identify("add");

            var result = command.Run().Single();

            result.Expect("Missing task description", Format.Negative);
        }
        
        [Fact]
        public void AddCommand_AllowWhitespacesInTaskDescription()
        {
            const string task = "this is at test";
            var command = _factory.Identify($"add {task}");

            command.Run();

            _board.Verify(x=>x.Add(task));
        }

        [Fact]
        public void Add_Command_NamesMustBeUnique()
        {
            // Arrange
            _board.Setup(x => x.Add(It.IsAny<string>())).Throws<TaskAlreadyExistsException>();
            var command = _factory.Identify("add abc");

            // Act
            var result = command.Run();

            // Assert
            result.Single().Expect("Task abc already exists", Format.Negative);

        }
    }
}