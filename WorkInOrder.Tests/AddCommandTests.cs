using System;
using System.Linq;
using Moq;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests
{
    public class AddCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public AddCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
        }

        [Fact]
        public void AddCommand_AddsNewTaskToDo()
        {
            const string something = "Something";
            var command = _factory.Identify($"add {something}");

            var result = command.Run().Single();

            _storage.Verify(x => x.Create(It.IsAny<DateTime>(), something));
            result.Expect($"{something} has been added to current todo list", Format.Neutral);
        }

        [Fact]
        public void AddCommand_ErrorWhenEmptyName()
        {
            var command = _factory.Identify("add");

            var result = command.Run().Single();

            result.Expect("Missing task description", Format.Negative);
        }

        [Fact]
        public void AddCommand_ActivatesTask_WhenNoneIsActiveYet()
        {
            const string task = "task";
            var command = _factory.Identify($"add {task}");
            
            command.Run();

            _storage.Verify(x=>x.UpdateStatus(task, Status.Current));
        }


        [Fact]
        public void AddCommand_AllowWhitespacesInTaskDescription()
        {
            const string task = "this is at test";
            var command = _factory.Identify($"add {task}");

            command.Run();

            _storage.Verify(x=>x.Create(It.IsAny<DateTime>(), task));
        }
    }
}