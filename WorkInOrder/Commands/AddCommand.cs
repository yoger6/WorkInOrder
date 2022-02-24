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
                return OutputMessage.Negative(Messages.MissingDescription);
            }
            catch (TaskAlreadyExistsException)
            {
                return OutputMessage.NegativeFormat(Messages.TaskAlreadyExists, _message);
            }

            return OutputMessage.NeutralFormat(Messages.TaskAdded, _message);
        }
    }
}