using System;
using System.Linq;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    [Collection("Storage sequence")]
    public class UpdateStatusTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

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
        public void ThrowsWhenTaskDoesNotExist()
        {
            Assert.Throws<TaskNotFoundException>(() => _storage.UpdateStatus("nope", Status.Done));
        }

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}