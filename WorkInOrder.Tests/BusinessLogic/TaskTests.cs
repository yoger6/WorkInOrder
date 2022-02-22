using Moq;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic
{
    public class TaskTests
    {
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        [Fact]
        public void MarksItselfAsSkipped()
        {
            // Arrange
            var task = TestTask.Active(_storage.Object);

            // Act
            task.Skip();

            // Assert
            _storage.Verify(x => x.UpdateStatus(task.Name, Status.Skipped));
        }

        [Fact]
        public void ActivatesSubsequentTask()
        {
            // Arrange
            var activeTask = TestTask.Active(_storage.Object);
            var subsequentTask = TestTask.Mocked();
            _storage.Setup(x => x.FindFirstAvailableSince(activeTask.CreatedOn)).Returns(subsequentTask.Object);

            // Act
            activeTask.Skip();

            // Assert
            subsequentTask.Verify(x=>x.Activate());
        }
    }
}