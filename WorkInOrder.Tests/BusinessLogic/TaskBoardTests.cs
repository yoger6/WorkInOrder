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
            var task = _board.GetActiveTask();

            // Assert
            
            Assert.NotNull(task);
        }

        [Fact]
        public void NoActiveTaskFound()
        {
            // Act
            var task = _board.GetActiveTask();

            // Assert
            Assert.Null(task);
        }
    }
}
