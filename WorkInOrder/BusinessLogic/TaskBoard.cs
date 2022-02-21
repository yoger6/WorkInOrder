namespace WorkInOrder.BusinessLogic
{
    public class TaskBoard
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
            return _taskStorage.GetAll();
        }
    }
}
