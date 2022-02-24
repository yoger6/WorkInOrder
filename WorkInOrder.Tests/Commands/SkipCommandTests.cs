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

            result.Single().Expect(Messages.NoTaskToSkip, Format.Negative);
        }

        [Fact]
        public void SkipsTaskInformingAboutWhatWasSkippedAndActivated()
        {
            const string skippedTaskName = "skipped";
            const string activatedTaskName = "activated";
            _board.Setup(x => x.Skip()).Returns((skippedTaskName, activatedTaskName));
            
            var result = Run();

            _board.Verify(x => x.Skip());
            result[0].Expect(string.Format(Messages.TaskSkipped, skippedTaskName), Format.Neutral);
            result[1].Expect(string.Format(Messages.TaskActivated, activatedTaskName), Format.Neutral);
        }

        private IList<OutputMessage> Run()
        {
            var command = _factory.Identify("skip");

            return command.Run();
        }
    }
}