using System;
using System.Linq;

namespace WorkInOrder.Commands
{
    internal class AddCommand : ICommand
    {
        private readonly ITaskStorage _storage;
        private readonly string _message;

        public AddCommand(ITaskStorage storage, string message)
        {
            _storage = storage;
            _message = message;
        }

        public OutputMessage[] Run()
        {
            if (string.IsNullOrWhiteSpace(_message))
            {
                return OutputMessage.Negative("Missing task description");
            }

            _storage.Create(DateTime.Now, _message);

            if (!IsThereAnActiveTaskPresent())
            {
                _storage.UpdateStatus(_message, Status.Current);
            }

            return OutputMessage.Neutral($"{_message} has been added to current todo list");
        }

        private bool IsThereAnActiveTaskPresent()
        {
            return _storage.GetTasks().Any(x => x.Status == Status.Current);
        }
    }
}