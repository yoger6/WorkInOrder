using System.Linq;
using Moq;
using Xunit;

namespace WorkInOrder.Tests
{
    public class NullCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();

        public NullCommandTests()
        {
            _factory = new CommandFactory(_storage.Object);
        }

        [Fact]
        public void NullCommandReturnsUnknownMessage()
        {
            var command = _factory.Identify("acbd");

            var result = command.Run().Single();

            Assert.Equal("Unknown command: acbd", result.Content);
            Assert.Equal(Format.Negative, result.Format);
        }
    }
}
