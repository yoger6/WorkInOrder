using System.Collections.Generic;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class AddCommand : ICommand
    {
        private readonly ITaskBoard _board;
        private readonly string _message;

        public AddCommand(ITaskBoard board, string message)
        {
            _message = message;
            _board = board;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                _board.Add(_message);
            }
            catch (MissingContentException)
            {
                return OutputMessage.Negative("Missing task description");
            }
            catch (TaskAlreadyExistsException)
            {
                return OutputMessage.Negative($"Task {_message} already exists");
            }

            return OutputMessage.Neutral($"{_message} has been added to current todo list");
        }
    }
}