using System;
using System.Linq;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    [Collection("Storage sequence")]
    public class CreateTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

        [Fact]
        public void CreatesNewTask()
        {
            var date = DateTime.Now;
            var content = Guid.NewGuid().ToString();

            _storage.Create(date, content);

            var tasks = _storage.GetAll();
            var task = tasks.Single(x => x.Name == content);
            Assert.Equal(date, task.CreatedOn);
        }

        [Fact]
        public void ThrowsWhenAttemptingToCreateTaskWithSameName()
        {
            var action = new Action(() => _storage.Create(DateTime.Now, "123"));
            action.Invoke();

            Assert.Throws<TaskAlreadyExistsException>(action);
        }

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}
