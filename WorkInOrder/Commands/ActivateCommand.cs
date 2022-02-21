using System.Linq;

namespace WorkInOrder.Commands
{
    internal class ActivateCommand : ICommand
    {
        private readonly ITaskStorage _storage;
        private readonly string _taskName;

        public ActivateCommand(ITaskStorage storage, string taskName)
        {
            _storage = storage;
            _taskName = taskName;
        }

        public OutputMessage[] Run()
        {
            var tasks = _storage.GetTasks();
            var taskToActivate = tasks.SingleOrDefault(x => x.Name == _taskName);
            if (taskToActivate == null)
            {
                return OutputMessage.Negative($"{_taskName} not found");
            }

            if (taskToActivate.Status == Status.Current)
            {
                return OutputMessage.Neutral($"{_taskName} is already active");
            }

            var activeTask = tasks.FirstOrDefault(x => x.Status == Status.Current);
            if (activeTask != null)
            {
                _storage.UpdateStatus(activeTask.Name, Status.Skipped);
            }

            _storage.UpdateStatus(_taskName, Status.Current);

            return OutputMessage.Neutral($"{_taskName} is now active");
        }
    }
}