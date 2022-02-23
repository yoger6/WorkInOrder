using System;
using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class AddTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public AddTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void CreatesPendingTaskWhenThereIsAlreadyActiveOne()
        {
            // Arrange
            _storage.Setup(x => x.Find(Status.Current)).Returns(TestTask.Active);
            var expected = TestTask.Pending();

            // Act
            _board.Add(expected.Name);

            // Assert
            _storage.Verify(x =>
                x.Create(It.IsAny<DateTime>(), expected.Name, Status.Pending));
        }

        [Fact]
        public void CreatesActiveTask()
        {
            // Arrange
            var expected = TestTask.Pending();

            // Act
            _board.Add(expected.Name);

            // Assert
            _storage.Verify(x =>
                x.Create(It.IsAny<DateTime>(), expected.Name, Status.Current));
        }

        [Fact]
        public void TaskMustHaveContent()
        {
            // Assert
            Assert.Throws<MissingContentException>(() => _board.Add(string.Empty));
        }
    }
}