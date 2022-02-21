using System;
using System.Linq;
using Moq;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests
{
    public class SwitchCommandTests
    {
        private const string ExistingTask = "task";
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public SwitchCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
            _storage.Setup(x => x.GetAll()).Returns(new[] { new Task(DateTime.Now, ExistingTask, Status.Current) });
        }

        [Fact]
        public void CannotSwitchToTaskThatDoesNotExist()
        {
            const string task = "abcd";
            
            var result = Run(task);

            result.Single().Expect($"{task} does not exist", Format.Negative);
        }

        [Fact]
        public void SwitchesToNewTask()
        {
            var result = Run(ExistingTask);

            _storage.Verify(x => x.UpdateStatus(ExistingTask, Status.Current));
        }

        [Fact]
        public void PreviousTaskBecomesPending()
        {
            const string newTask = "abcd";
            _storage.Setup(x => x.GetAll()).Returns(new[] {new Task(DateTime.Now, ExistingTask, Status.Current), new Task(DateTime.Now, newTask, Status.Pending)});

            var result = Run(newTask);

            _storage.Verify(x => x.UpdateStatus(ExistingTask, Status.Pending));
        }

        private OutputMessage[] Run(string task)
        {
            var command = _factory.Identify($"switch {task}");

            return command.Run();
        }
    }
}