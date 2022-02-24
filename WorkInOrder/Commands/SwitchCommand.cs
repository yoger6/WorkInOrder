using System.Collections.Generic;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class SwitchCommand : ICommand
    {
        private readonly ITaskBoard _board;
        private readonly string _message;

        public SwitchCommand(ITaskBoard board, string message)
        {
            _board = board;
            _message = message;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                _board.Switch(_message);
                return OutputMessage.Neutral($"Switched to {_message}");
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative($"{_message} does not exist");
            }
            catch (NoActiveTaskException)
            {
                return OutputMessage.Negative($"Cannot switch to {_message} as there's no active task to switch from. Rather use Activate command.");
            }
            catch (TaskAlreadyActiveException)
            {
                return OutputMessage.Neutral($"{_message} is already active");
            }
        }
    }
}