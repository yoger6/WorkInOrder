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
            _factory = new CommandFactory(_board.Object);
        }

        [Fact]
        public void CannotCompleteTaskWhenThereIsNoActiveOne()
        {
            _board.Setup(x => x.Done()).Throws<TaskNotFoundException>();

            var result = Complete();

            result.Single().Expect(Messages.NoTaskToComplete, Format.Negative);
        }

        [Fact]
        public void CompletesTheTaskInformingWhichOneWasCompletedAndActivated()
        {
            const string completedTaskName = "completed";
            const string activatedTaskName = "activated";
            _board.Setup(x => x.Done()).Returns((completedTaskName, activatedTaskName));

            var result = Complete();

            _board.Verify(x=>x.Done());
            result[0].Expect(string.Format(Messages.TaskCompleted, completedTaskName), Format.Neutral);
            result[1].Expect(string.Format(Messages.TaskActivated, activatedTaskName), Format.Neutral);
        }

        private IList<OutputMessage> Complete()
        {
            var command = _factory.Identify("done");
            return command.Run();
        }
    }
}