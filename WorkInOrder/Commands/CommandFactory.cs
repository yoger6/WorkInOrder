using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    public class CommandFactory
    {
        private readonly ITaskBoard _board;
        private readonly Session _session;

        public CommandFactory(ITaskBoard board, Session session = null)
        {
            _board = board;
            _session = session ?? new Session();
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
                    return new ActivateCommand(_board, message, _session);
                case "skip":
                    return new SkipCommand(_board);
                case "done":
                    return new DoneCommand(_board);
                case "switch":
                    return new SwitchCommand(_board, message);
            }
        }
    }
}
