using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class ActivateTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public ActivateTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void ActivatesSpecifiedTask()
        {
            // Arrange
            var task = TestTask.Mocked();
            _storage.Setup(x => x.Find(task.Name)).Returns(task.Object);

            // Act
            _board.Activate(task.Name);

            // Assert
            task.Verify(x => x.Activate());
        }

        [Fact]
        public void CannotActivateTaskThatDoesNotExist()
        {
            // Assert
            Assert.Throws<TaskNotFoundException>(() => _board.Activate("test"));
        }

        [Fact]
        public void CannotActivateTaskThatIsAlreadyActive()
        {
            // Arrange
            var task = TestTask.Active();
            _storage.Setup(x => x.Find(task.Name)).Returns(task);
            _storage.Setup(x => x.Find(Status.Current)).Returns(task);

            // Assert
            Assert.Throws<TaskAlreadyActiveException>(() => _board.Activate(task.Name));
        }

        [Fact]
        public void CannotActivateTaskWhenAnotherOneIsActive()
        {
            // Arrange
            var task = TestTask.Pending();
            var activeTask = TestTask.Active();
            _storage.Setup(x => x.Find(task.Name)).Returns(task);
            _storage.Setup(x => x.Find(Status.Current)).Returns(activeTask);

            // Assert
            Assert.Throws<AnotherTaskAlreadyActiveException>(() => _board.Activate(task.Name));
        }
    }
}