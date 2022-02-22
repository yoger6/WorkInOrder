namespace WorkInOrder.BusinessLogic
{
    public interface ITaskBoard
    {
        Task GetActiveTask();
        Task[] ListTasks();
        void Add(string content);
    }
}