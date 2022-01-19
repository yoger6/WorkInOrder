namespace WorkInOrder
{
    public class CommandFactory
    {
        private readonly ITaskStorage _storage;

        public CommandFactory(ITaskStorage storage)
        {
            _storage = storage;
        }

        public ICommand Identify(string input)
        {
            var inputParts = input.Split(' ');

            switch (inputParts[0])
            {
                default:
                    return new UnknownCommand(input);
                case "add":
                    return new AddCommand(_storage, input);
                case "list":
                    return new ListCommand(_storage);
            }
        }
    }
}
