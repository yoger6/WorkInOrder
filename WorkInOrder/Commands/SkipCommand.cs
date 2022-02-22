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
                outcome.AddRange(OutputMessage.Neutral($"{result.Skipped} skipped"));
                if (!string.IsNullOrWhiteSpace(result.Activated))
                {
                    outcome.AddRange(OutputMessage.Neutral($"{result.Activated} is now active"));
                }

                return outcome;
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative("There's no active task to skip");
            }
        }
    }
}