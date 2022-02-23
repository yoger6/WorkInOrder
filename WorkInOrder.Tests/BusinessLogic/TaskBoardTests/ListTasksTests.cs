using System;
using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class ListTasksTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public ListTasksTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void ListsAllTheTasks()
        {
            // Arrange
            var expected = new[]
            {
                TestTask.Active(),
                TestTask.Pending()
            };
            _storage.Setup(x => x.GetAll()).Returns(expected);

            // Act
            var actual = _board.ListTasks();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ListIsOrderedByCreationDate()
        {
            var tasks = new[]
            {
                new Task(DateTime.Now.AddDays(1), "Second", Status.Pending),
                new Task(DateTime.Now, "First", Status.Pending),
            };
            _storage.Setup(x => x.GetAll()).Returns(tasks);

            var result = _board.ListTasks();

            Assert.StartsWith("First", result[0].Name);
            Assert.StartsWith("Second", result[1].Name);
        }

        [Fact]
        public void ListCanBeEmpty()
        {
            // Act
            var list = _board.ListTasks();

            // Assert
            Assert.NotNull(list);
            Assert.Empty(list);
        }
    }
}