using System.Linq;

namespace WorkInOrder.Commands
{
    internal class SkipCommand : ICommand
    {
        private readonly ITaskStorage _storage;

        public SkipCommand(ITaskStorage storage)
        {
            _storage = storage;
        }

        public OutputMessage[] Run()
        {
            var tasks = _storage.GetAll().OrderBy(x=>x.CreatedOn).ToArray();
            var task = tasks.SingleOrDefault(x => x.Status == Status.Current);

            if (task == null)
            {
                return OutputMessage.Negative("There's not active task to skip.");
            }

            _storage.UpdateStatus(task.Name, Status.Skipped);

            var nextTask = tasks.FirstOrDefault(x => x.CreatedOn > task.CreatedOn);
            var outcome = OutputMessage.Neutral($"{task.Name} skipped");
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