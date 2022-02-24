using System.Collections.Generic;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class SkipCommand : ICommand
    {
        private readonly ITaskBoard _board;

        public SkipCommand(ITaskBoard board)
        {
            _board = board;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                var result = _board.Skip();
                var outcome = new List<OutputMessage>();
                outcome.AddRange(OutputMessage.NeutralFormat(Messages.TaskSkipped, result.Skipped));
                if (!string.IsNullOrWhiteSpace(result.Activated))
                {
                    outcome.AddRange(OutputMessage.NeutralFormat(Messages.TaskActivated, result.Activated));
                }

                return outcome;
            }
            catch (NoActiveTaskException)
            {
                return OutputMessage.Negative(Messages.NoTaskToSkip);
            }
        }
    }
}