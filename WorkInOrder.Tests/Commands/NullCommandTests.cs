using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class NullCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskStorage> _storage = new Mock<ITaskStorage>();
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public NullCommandTests()
        {
            _factory = new CommandFactory(_board.Object);
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
