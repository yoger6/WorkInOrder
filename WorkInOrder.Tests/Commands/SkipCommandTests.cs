using System.Collections.Generic;
using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class SkipCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public SkipCommandTests()
        {
            _factory = new CommandFactory(_board.Object);
        }

        [Fact]
        public void CannotSkipIfThereIsNoCurrentTask()
        {
            _board.Setup(x => x.Skip()).Throws<NoActiveTaskException>();

            var result = Run();

            result.Single().Expect("There's no active task to skip", Format.Negative);
        }

        [Fact]
        public void MarksCurrentTaskAsSkipped()
        {
            
            Run();

            _board.Verify(x => x.Skip());
        }

        private IList<OutputMessage> Run()
        {
            var command = _factory.Identify("skip");

            return command.Run();
        }
    }
}