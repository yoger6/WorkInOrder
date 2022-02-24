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
                output.AddRange(OutputMessage.NeutralFormat(Messages.TaskCompleted, result.Completed));

                if (!string.IsNullOrWhiteSpace(result.Activated))
                {
                    output.AddRange(OutputMessage.NeutralFormat(Messages.TaskActivated, result.Activated));
                }

                return output;
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative(Messages.NoTaskToComplete);
            }
        }
    }
}