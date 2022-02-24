using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using WorkInOrder.Tests.BusinessLogic;
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
                TestTask.Done(DateTime.Now),
                TestTask.Skipped(DateTime.Now.AddMinutes(1)),
                TestTask.Active(DateTime.Now.AddMinutes(2)),
                TestTask.Pending(DateTime.Now.AddMinutes(3)),
            };
            _board.Setup(x => x.ListTasks()).Returns(tasks);

            var result = Run();

            _board.Verify(x => x.ListTasks(), Times.Once);
            Assert.StartsWith($"+ {tasks[0].Name}", result[0].Content);
            Assert.Equal(Format.Positive, result[0].Format);
            Assert.StartsWith($"- {tasks[1].Name}", result[1].Content);
            Assert.Equal(Format.Negative, result[1].Format);
            Assert.StartsWith($"@ {tasks[2].Name}", result[2].Content);
            Assert.Equal(Format.Highlight, result[2].Format);
            Assert.StartsWith($"? {tasks[3].Name}", result[3].Content);
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
                TestTask.Done(DateTime.Now, DateTime.Now.AddDays(1)),
                TestTask.Done(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)),
            };
            _board.Setup(x => x.ListTasks()).Returns(tasks);

            var result = Run("d");

            result[0].Expect($"+ {tasks[0].Name} {tasks[0].CreatedOn.ToOutputFormat()}-{tasks[0].CompletedOn.Value.ToOutputFormat()}", Format.Positive);
            result[1].Expect($"+ {tasks[1].Name} {tasks[1].CreatedOn.ToOutputFormat()}-{tasks[1].CompletedOn.Value.ToOutputFormat()}", Format.Positive);
        }

        private IList<OutputMessage> Run(string parameter = null)
        {
            var command = _factory.Identify("list " + parameter);

            return command.Run();
        }
    }
}