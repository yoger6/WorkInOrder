using System;
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

        [Fact]
        public void CannotSkipIfNoTaskIsActive()
        {
            // Assert
            Assert.Throws<TaskNotFoundException>(() => _board.Skip());
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
            taskMock.Verify(x=>x.Skip());
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

        [Fact]
        public void CompletesActiveTask()
        {
            // Arrange
            var taskMock = TestTask.Mocked();
            _storage.Setup(x => x.Find(Status.Current)).Returns(taskMock.Object);

            // Act
            _board.Done();

            // Assert
            taskMock.Verify(x=>x.Complete());
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

        [Fact]
        public void ActivatesSpecifiedTask()
        {
            // Arrange
            var task = TestTask.Mocked();
            _storage.Setup(x => x.Find(task.Name)).Returns(task.Object);

            // Act
            _board.Activate(task.Name);

            // Assert
            task.Verify(x=>x.Activate());
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
            var task = TestTask.Active();
            _storage.Setup(x => x.Find(task.Name)).Returns(task);

            // Assert
            Assert.Throws<TaskAlreadyActiveException>(() => _board.Activate(task.Name));
        }
    }
}
