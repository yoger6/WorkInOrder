using System.Linq;

namespace WorkInOrder.BusinessLogic
{
    public class TaskBoard : ITaskBoard
    {
        private readonly ITaskStorage _taskStorage;

        public TaskBoard(ITaskStorage taskStorage)
        {
            _taskStorage = taskStorage;
        }

        public Task GetActiveTask()
        {
            return _taskStorage.Find(Status.Current);
        }

        public Task[] ListTasks()
        {
            return _taskStorage.GetAll()
                .OrderBy(x=>x.CreatedOn)
                .ToArray();
        }
    }
}
