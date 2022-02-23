using System;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    public class UpdateCompletionDateTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

        [Fact]
        public void UpdatesTheDateTaskWasCompleted()
        {
            // Arrange
            const string name = "test";
            var completionDate = DateTime.Now.AddDays(1);
            _storage.Create(DateTime.Now, name);

            // Act
            _storage.UpdateCompletionDate(name, completionDate);

            // Assert
            var updatedTask = _storage.Find(name);
            Assert.Equal(completionDate, updatedTask.CompletedOn);
        }

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}