using System;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    [Collection("Storage sequence")]
    public class FindFirstAvailableSinceTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

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