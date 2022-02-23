using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class SkipTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public SkipTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void CannotSkipIfNoTaskIsActive()
        {
            // Assert
            Assert.Throws<NoActiveTaskException>(() => _board.Skip());
        }

        [Fact]
        public void SkipsActiveTask()
        {
            // Arrange
            var taskMock = new Mock<ITask>();
            _storage.Setup(x => x.Find(Status.Current)).Returns(taskMock.Object);

            // Act
            _board.Skip();

            // Assert
            taskMock.Verify(x => x.Skip());
        }

        [Fact]
        public void ActivatesSubsequentTaskAfterSkipping()
        {
            // Arrange
            var activeTask = TestTask.Active(_storage.Object);
            var subsequentTask = TestTask.Mocked();
            _storage.Setup(x => x.Find(Status.Current)).Returns(activeTask);
            _storage.Setup(x => x.FindFirstAvailableSince(activeTask.CreatedOn)).Returns(subsequentTask.Object);

            // Act
            _board.Skip();

            // Assert
            subsequentTask.Verify(x => x.Activate());
        }
    }
}