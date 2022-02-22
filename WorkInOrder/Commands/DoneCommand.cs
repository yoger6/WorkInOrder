using System.Collections.Generic;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class DoneCommand : ICommand
    {
        private readonly ITaskBoard _board;

        public DoneCommand(ITaskBoard board)
        {
            _board = board;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                var result = _board.Done();
                var output = new List<OutputMessage>();
                output.AddRange(OutputMessage.Neutral($"{result.Completed} completed"));

                if (!string.IsNullOrWhiteSpace(result.Activated))
                {
                    output.AddRange(OutputMessage.Neutral($"{result.Activated} is now active"));
                }

                return output;
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative("There's not active task to complete");
            }
        }
    }
}