using System;

namespace WorkInOrder
{
    internal class AddCommand : ICommand
    {
        private readonly ITaskStorage _storage;
        private readonly string _command;

        public AddCommand(ITaskStorage storage, string command)
        {
            _storage = storage;
            _command = command;
        }

        public OutputMessage[] Run()
        {
            var taskParts = _command.Split(' ');

            if (taskParts.Length < 2)
            {
                return OutputMessage.Negative("Missing task description");
            }

            _storage.Create(DateTime.Today, taskParts[1]);

            return OutputMessage.Neutral($"{taskParts[1]} has been added to current todo list");
        }
    }
}