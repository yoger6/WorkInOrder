namespace WorkInOrder.BusinessLogic
{
    public interface ITaskBoard
    {
        ITask[] ListTasks();
        void Add(string name);
        (string Skipped, string Activated) Skip();
        (string Completed, string Activated) Done();
        void Activate(string name);
        (string Activated, string Deactivated) Switch(string name);
    }
}