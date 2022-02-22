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
        public void TaskActivatesItself()
        {
            // Arrange
            var task = TestTask.Pending(_storage.Object);

            // Act
            task.Activate();

            // Assert
            _storage.Verify(x=>x.UpdateStatus(task.Name, Status.Current));
        }
    }
}