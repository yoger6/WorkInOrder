using System.Linq;

namespace WorkInOrder.Commands
{
    internal class DoneCommand : ICommand
    {
        private readonly ITaskStorage _storage;

        public DoneCommand(ITaskStorage storage)
        {
            _storage = storage;
        }

        public OutputMessage[] Run()
        {
            var tasks = _storage.GetTasks();
            var currentTask = tasks.SingleOrDefault(x => x.Status == Status.Current);

            if (currentTask == null)
            {
                return OutputMessage.Negative("There's not active task to complete");
            }

            _storage.UpdateStatus(currentTask.Name, Status.Done);

            var nextTask = tasks.FirstOrDefault(x => x.CreatedOn > currentTask.CreatedOn);
            var outcome = OutputMessage.Neutral($"{currentTask.Name} completed");
            if (nextTask != null)
            {
                var command = new ActivateCommand(_storage, nextTask.Name);
                var activationResult = command.Run();
                return outcome.Concat(activationResult).ToArray();
            }

            return outcome;
        }
    }
}