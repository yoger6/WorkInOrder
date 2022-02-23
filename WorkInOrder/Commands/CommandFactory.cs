using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    public class CommandFactory
    {
        private readonly ITaskStorage _storage;
        private readonly ITaskBoard _board;

        public CommandFactory(ITaskStorage storage, ITaskBoard board)
        {
            _storage = storage;
            _board = board;
        }

        public ICommand Identify(string input)
        {
            var firstWhitespaceIndex = input.IndexOf(' ');
            var command = firstWhitespaceIndex == -1 ? input : input.Substring(0, firstWhitespaceIndex);
            var message = input == command ? string.Empty : input.Substring(firstWhitespaceIndex, input.Length - firstWhitespaceIndex).Trim();
            switch (command)
            {
                default:
                    return new UnknownCommand(input);
                case "add":
                    return new AddCommand(_board, message);
                case "list":
                    return new ListCommand(_board, message);
                case "activate":
                    return new ActivateCommand(_board, message);
                case "skip":
                    return new SkipCommand(_board);
                case "done":
                    return new DoneCommand(_board);
                case "switch":
                    return new SwitchCommand(_storage, message);
            }
        }
    }
}
