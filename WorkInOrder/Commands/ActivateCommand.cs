using System.Collections.Generic;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class ActivateCommand : ICommand
    {
        private readonly ITaskBoard _board;
        private readonly string _taskName;

        public ActivateCommand(ITaskBoard board, string taskName)
        {
            _board = board;
            _taskName = taskName;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                _board.Activate(_taskName);
                return OutputMessage.Neutral($"{_taskName} is now active");
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative($"{_taskName} not found");
            }
            catch (TaskAlreadyActiveException)
            {
                return OutputMessage.Negative($"{_taskName} is already active");
            }
            catch (AnotherTaskAlreadyActiveException e)
            {
                return OutputMessage.Negative($"Cannot activate {_taskName} as there's another active task: {e.TaskName}. To switch active task use the switch command.");
            }
        }
    }
}