namespace WorkInOrder.BusinessLogic
{
    public interface ITaskBoard
    {
        Task GetActiveTask();
        Task[] ListTasks();
    }
}