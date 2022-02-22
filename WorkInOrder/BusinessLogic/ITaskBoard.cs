namespace WorkInOrder.BusinessLogic
{
    public interface ITaskBoard
    {
        ITask GetActiveTask();
        ITask[] ListTasks();
        void Add(string content);
        (string Skipped, string Activated) Skip();
    }
}