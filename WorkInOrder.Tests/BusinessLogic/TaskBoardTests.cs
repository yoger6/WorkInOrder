using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic
{
    public class TaskBoardTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;
        
        public TaskBoardTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void CanGetActiveTask()
        {
            // Arrange
            var expected = TestTask.Active();
            _storage.Setup(x => x.Find(Status.Current)).Returns(expected);

            // Act
            var actual = _board.GetActiveTask();

            // Assert
            
            Assert.Same(expected, actual);
        }

        [Fact]
        public void NoActiveTaskFound()
        {
            // Act
            var task = _board.GetActiveTask();

            // Assert
            Assert.Null(task);
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
            Assert.Same(expected, actual);
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
