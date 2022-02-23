using System;
using WorkInOrder.Tests.BusinessLogic;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    [Collection("Storage sequence")]
    public class FindTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

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

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}