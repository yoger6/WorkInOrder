using System;
using System.Linq;
using WorkInOrder.Tests.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests
{
    public class TaskStorageTests : IDisposable
    {
        private const string ConnectionString = "DataSource=WorkInOrder.sqlite";
        private readonly TaskStorage _storage = new TaskStorage(ConnectionString);

        [Fact]
        public void CreatesNewTask()
        {
            var date = DateTime.Now;
            var content = Guid.NewGuid().ToString();

            _storage.Create(date, content);

            var tasks = _storage.GetAll();
            var task = tasks.Single(x=>x.Name == content);
            Assert.Equal(date, task.CreatedOn);
        }

        [Fact]
        public void GetsTaskList()
        {
            _storage.Create(DateTime.Now, Guid.NewGuid().ToString());
            _storage.Create(DateTime.Now, Guid.NewGuid().ToString());

            var tasks = _storage.GetAll();

            Assert.Equal(2, tasks.Length);
        }

        [Fact]
        public void ThrowsWhenAttemptingToCreateTaskWithSameName()
        {
            var action = new Action(() => _storage.Create(DateTime.Now, "123"));
            action.Invoke();

            Assert.Throws<TaskAlreadyExistsException>(action);
        }

        [Fact]
        public void UpdatesStatusOfGivenTask()
        {
            const string name = "Test";
            _storage.Create(DateTime.Now, name);

            _storage.UpdateStatus(name, Status.Done);

            var task = _storage.GetAll().Single();
            Assert.Equal(Status.Done, task.Status);
        }

        [Fact]
        public void AssignsCompletionDateToCompletedTasks()
        {
            const string name = "Test";
            _storage.Create(DateTime.Now, name);

            _storage.UpdateStatus(name, Status.Done);

            var task = _storage.GetAll().Single();
            Assert.NotNull(task.CompletedOn);

        }

        [Fact]
        public void ThrowsWhenTaskDoesNotExist()
        {
            Assert.Throws<TaskNotFoundException>(() => _storage.UpdateStatus("nope", Status.Done));
        }

        [Fact]
        public void FindsExistingTaskWithPartialName()
        {
            _storage.Create(DateTime.Now, "Bob1");

            var task = _storage.Find("ob");

            Assert.NotNull(task);
        }

        [Fact]
        public void TooManyTasksFound()
        {
            _storage.Create(DateTime.Now, "Bob1");
            _storage.Create(DateTime.Now, "Bob2");

            Assert.Throws<NonUniqueNameException>(() => _storage.Find("Bob"));
        }

        [Fact]
        public void FindsTaskWithGivenStatus()
        {
            // Arrange
            var expected = TestTask.Active();
            _storage.Create(expected.CreatedOn, expected.Name, expected.Status);

            // Act
            var actual = _storage.Find(Status.Current);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(Status.Current, actual.Status);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public void FindsAvailableTaskExactlyOnTheDate()
        {
            // Arrange
            var date = DateTime.Now;
            _storage.Create(date, "test");

            // Act
            var task = _storage.FindFirstAvailableSince(date);

            // Assert
            Assert.NotNull(task);
        }

        [Fact]
        public void FindsSkippedTasksFromTheFuture()
        {
            // Arrange
            var date = DateTime.Now;
            _storage.Create(date.AddDays(1), "test", Status.Skipped);

            // Act
            var task = _storage.FindFirstAvailableSince(date);

            // Assert
            Assert.NotNull(task);
        }

        [Fact]
        public void DoesNotFindTaskPriorToDate()
        {
            // Arrange
            var date = DateTime.Now;
            _storage.Create(date.AddDays(-1), "test");

            // Act
            var task = _storage.FindFirstAvailableSince(date);

            // Assert
            Assert.Null(task);
        }

        [Theory]
        [InlineData(Status.Current)]
        [InlineData(Status.Done)]
        public void DoesNotFindActiveOrCompletedTasks(Status status)
        {
            // Arrange
            var date = DateTime.Now;
            _storage.Create(date.AddDays(-1), "test", status);

            // Act
            var task = _storage.FindFirstAvailableSince(date);

            // Assert
            Assert.Null(task);
        }

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}