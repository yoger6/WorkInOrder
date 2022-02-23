using System;
using Xunit;

namespace WorkInOrder.Tests.Storage
{
    public class GetAllTests : IDisposable
    {
        private readonly TaskStorage _storage = new TestTaskStorage();

        [Fact]
        public void GetsTaskList()
        {
            _storage.Create(DateTime.Now, Guid.NewGuid().ToString());
            _storage.Create(DateTime.Now, Guid.NewGuid().ToString());

            var tasks = _storage.GetAll();

            Assert.Equal(2, tasks.Length);
        }

        public void Dispose()
        {
            _storage.Clear();
        }
    }
}