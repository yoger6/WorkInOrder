using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkInOrder.Commands
{
    internal class SwitchCommand : ICommand
    {
        private readonly ITaskStorage _storage;
        private readonly string _message;

        public SwitchCommand(ITaskStorage storage, string message)
        {
            _storage = storage;
            _message = message;
        }

        public IList<OutputMessage> Run()
        {
            var task = _storage.GetAll().SingleOrDefault(x => x.Name == _message);

            if (task == null)
            {
                return OutputMessage.Negative($"{_message} does not exist");
            }

            var currentTask = _storage.GetAll().SingleOrDefault(x => x.Status == Status.Current);
            if (currentTask != null)
            {
                _storage.UpdateStatus(currentTask.Name, Status.Pending);
            }

            _storage.UpdateStatus(task.Name, Status.Current);

            return OutputMessage.Neutral($"Switched to {_message}");
        }
    }
}