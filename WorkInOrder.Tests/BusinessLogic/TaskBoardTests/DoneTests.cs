using System;
using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class DoneTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public DoneTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void CompletesActiveTask()
        {
            // Arrange
            var taskMock = TestTask.Mocked();
            _storage.Setup(x => x.Find(Status.Current)).Returns(taskMock.Object);

            // Act
            _board.Done();

            // Assert
            taskMock.Verify(x => x.Complete());
        }

        [Fact]
        public void CannotCompleteTaskWhenNoneIsActive()
        {
            // Assert
            Assert.Throws<TaskNotFoundException>(() => _board.Done());
        }

        [Fact]
        public void ActivatesSubsequentTaskAfterCompletion()
        {
            // Arrange
            var activeTaskCreationDate = DateTime.Today;
            var activeTask = TestTask.Mocked();
            activeTask.Setup(x => x.CreatedOn).Returns(activeTaskCreationDate);
            var subsequentTask = TestTask.Mocked();
            _storage.Setup(x => x.Find(Status.Current)).Returns(activeTask.Object);
            _storage.Setup(x => x.FindFirstAvailableSince(activeTaskCreationDate)).Returns(subsequentTask.Object);

            // Act
            _board.Done();

            // Assert
            subsequentTask.Verify(x => x.Activate());
        }
    }
}