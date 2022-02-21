namespace WorkInOrder.Commands
{
    internal class UnknownCommand : ICommand
    {
        private readonly string _input;

        public UnknownCommand(string input)
        {
            _input = input;
        }

        public OutputMessage[] Run()
        {
            return new[] { new OutputMessage($"Unknown command: {_input}", Format.Negative), };
        }
    }
}