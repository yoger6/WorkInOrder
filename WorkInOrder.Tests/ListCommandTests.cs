using System;
using Moq;
using Xunit;

namespace WorkInOrder.Tests
{
    public class ListCommandTests
    {

        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public ListCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
        }

        [Fact]
        public void ListCommand_DisplaysTasksForToday()
        {
            var tasks = new[]
            {
                new Task(DateTime.Now, "Code", Status.Done),
                new Task(DateTime.Now.AddMinutes(1), "Compile", Status.Skipped),
                new Task(DateTime.Now.AddMinutes(2), "Test", Status.Current),
                new Task(DateTime.Now.AddMinutes(3), "Deploy", Status.Pending),
            };
            _storage.Setup(x => x.GetTasks(DateTime.Today)).Returns(
                tasks);
            var command = _factory.Identify("list");

            var result = command.Run();

            _storage.Verify(x => x.GetTasks(DateTime.Today), Times.Once);

            Assert.Equal("+ Code", result[0].Content);
            Assert.Equal(Format.Positive, result[0].Format);
            Assert.Equal("- Compile", result[1].Content);
            Assert.Equal(Format.Negative, result[1].Format);
            Assert.Equal("@ Test", result[2].Content);
            Assert.Equal(Format.Highlight, result[2].Format);
            Assert.Equal("? Deploy", result[3].Content);
            Assert.Equal(Format.Neutral, result[3].Format);
        }

        [Fact]
        public void ListCommand_DisplaysTasksInCorrectOrder()
        {
            var tasks = new[]
            {
                new Task(DateTime.Now.AddDays(1), "Second", Status.Pending),
                new Task(DateTime.Now, "First", Status.Pending),
            };
            _storage.Setup(x => x.GetTasks(DateTime.Today)).Returns(tasks);
            var command = _factory.Identify("list");

            var result = command.Run();

            Assert.Equal("? First", result[0].Content);
            Assert.Equal("? Second", result[1].Content);
        }
    }
}