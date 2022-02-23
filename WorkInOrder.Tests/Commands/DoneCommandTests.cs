using System.Collections.Generic;
using System.Linq;
using Moq;
using WorkInOrder.BusinessLogic;
using WorkInOrder.Commands;
using Xunit;

namespace WorkInOrder.Tests.Commands
{
    public class DoneCommandTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<ITaskBoard> _board = new Mock<ITaskBoard>();

        public DoneCommandTests()
        {
            _factory = new CommandFactory(Mock.Of<ITaskStorage>(), _board.Object);
        }

        [Fact]
        public void CannotCompleteTaskWhenThereIsNoActiveOne()
        {
            _board.Setup(x => x.Done()).Throws<TaskNotFoundException>();

            var result = Complete();

            result.Single().Expect("There's not active task to complete", Format.Negative);
        }

        [Fact]
        public void MarkCurrentTaskAsDone()
        {
            Complete();

            _board.Verify(x=>x.Done());
        }

        private IList<OutputMessage> Complete()
        {
            var command = _factory.Identify("done");
            return command.Run();
        }
    }
}