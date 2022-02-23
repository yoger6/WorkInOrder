using Moq;
using WorkInOrder.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.BusinessLogic.TaskBoardTests
{
    public class SwitchTests
    {
        private readonly Mock<ITaskStorage> _storage;
        private readonly TaskBoard _board;

        public SwitchTests()
        {
            _storage = new Mock<ITaskStorage>();
            _board = new TaskBoard(_storage.Object);
        }

        [Fact]
        public void TaskHasToExist()
        {
            // Assert
            Assert.Throws<TaskNotFoundException>(() => _board.Switch("abc"));
        }

        [Fact]
        public void ActiveTaskHasToExist()
        {
            // Arrange
            var desiredTask = TestTask.Pending();
            SetupStorage(null, desiredTask);

            // Assert
            Assert.Throws<NoActiveTaskException>(() => _board.Switch(desiredTask.Name));
        }

        [Fact]
        public void CannotSwitchToTaskThatIsAlreadyActive()
        {
            // Arrange
            var task = TestTask.Active();
            SetupStorage(task, task);

            // Assert
            Assert.Throws<TaskAlreadyActiveException>(() => _board.Switch(task.Name));
        }

        [Fact]
        public void DeactivatesCurrentTask()
        {
            // Arrange
            var activeTask = TestTask.Mocked();
            var desiredTask = TestTask.Mocked();
            SetupStorage(activeTask.Object, desiredTask.Object);

            // Act
            _board.Switch(desiredTask.Object.Name);

            // Assert
            activeTask.Verify(x=>x.Deactivate());
        }

        [Fact]
        public void ActivateDesiredTask()
        {
            // Arrange
            var activeTask = TestTask.Mocked();
            var desiredTask = TestTask.Mocked();
            SetupStorage(activeTask.Object, desiredTask.Object);

            // Act
            _board.Switch(desiredTask.Object.Name);

            // Assert
            desiredTask.Verify(x => x.Activate());
        }

        [Fact]
        public void ReturnsNamesOfBothTasks()
        {
            // Arrange
            var activeTask = TestTask.Mocked();
            const string activeName = "Task 1";
            activeTask.Setup(x => x.Name).Returns(activeName);
            
            var desiredTask = TestTask.Mocked();
            const string desiredName = "Task 2";
            desiredTask.Setup(x => x.Name).Returns(desiredName);
            SetupStorage(activeTask.Object, desiredTask.Object);

            // Act
            var (activated, deactivated) = _board.Switch(desiredTask.Object.Name);

            // Assert
            Assert.Equal(desiredName, activated);
            Assert.Equal(activeName, deactivated);

        }

        private void SetupStorage(ITask active, ITask desired)
        {
            _storage.Setup(x => x.Find(Status.Current)).Returns(active);
            _storage.Setup(x => x.Find(desired.Name)).Returns(desired);
        }
    }
}