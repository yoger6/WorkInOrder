using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class ListCommandTests
    {

        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public ListCommandTests()
        {
            _factory = new CommandFactory(_storage.Object, _board.Object);
        }

        [Fact]
        public void ListCommand_DisplaysTasksForToday()
        {
            var tasks = new[]
            {
                new Task(DateTime.Now, "Code", Status.Done, DateTime.Now),
                new Task(DateTime.Now.AddMinutes(1), "Compile", Status.Skipped),
                new Task(DateTime.Now.AddMinutes(2), "Test", Status.Current),
                new Task(DateTime.Now.AddMinutes(3), "Deploy", Status.Pending),
            };
            _board.Setup(x => x.ListTasks()).Returns(tasks);

            var result = Run();

            _board.Verify(x => x.ListTasks(), Times.Once);
            Assert.StartsWith("+ Code", result[0].Content);
            Assert.Equal(Format.Positive, result[0].Format);
            Assert.StartsWith("- Compile", result[1].Content);
            Assert.Equal(Format.Negative, result[1].Format);
            Assert.StartsWith("@ Test", result[2].Content);
            Assert.Equal(Format.Highlight, result[2].Format);
            Assert.StartsWith("? Deploy", result[3].Content);
            Assert.Equal(Format.Neutral, result[3].Format);
        }
        
        [Fact]
        public void InformsThereAreNoTasksToDisplay()
        {
            var result = Run();

            result.Single().Expect("Nothing to display", Format.Neutral);
        }

        [Fact]
        public void IncludesCreationAndCompletionDate()
        {
            var tasks = new[]
            {
                new Task(DateTime.Now, "First", Status.Done, DateTime.Now.AddDays(1)),
                new Task(DateTime.Now.AddDays(1), "Second", Status.Done, DateTime.Now.AddDays(2)),
            };
            _board.Setup(x => x.ListTasks()).Returns(tasks);

            var result = Run("d");

            result[0].Expect($"+ First {tasks[0].CreatedOn.ToOutputFormat()}-{tasks[0].CompletedOn.Value.ToOutputFormat()}", Format.Positive);
            result[1].Expect($"+ Second {tasks[1].CreatedOn.ToOutputFormat()}-{tasks[1].CompletedOn.Value.ToOutputFormat()}", Format.Positive);
        }

        private IList<OutputMessage> Run(string parameter = null)
        {
            var command = _factory.Identify("list " + parameter);

            return command.Run();
        }
    }
}