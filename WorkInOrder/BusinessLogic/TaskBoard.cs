using System;
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

        public ITask GetActiveTask()
        {
            return _taskStorage.Find(Status.Current);
        }

        public ITask[] ListTasks()
        {
            return _taskStorage.GetAll()
                .OrderBy(x=>x.CreatedOn)
                .ToArray();
        }

        /// <summary>
        /// Stores a new task, activates it if there's no other.
        /// </summary>
        /// <exception cref="MissingContentException">Task needs content to exist</exception>
        /// <exception cref="TaskAlreadyExistsException">Task content must be unique</exception>
        /// <param name="content">What is the task about</param>
        public void Add(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new MissingContentException();
            }

            if (GetActiveTask() == null)
            {
                _taskStorage.Create(DateTime.Now, content, Status.Current);
            }
            else
            {
                _taskStorage.Create(DateTime.Now, content, Status.Pending);
            }
        }

        /// <summary>
        /// Skips current task, activating next one if there is one in the future.
        /// </summary>
        /// <exception cref="TaskNotFoundException">When there's no active task</exception>
        /// <returns></returns>
        public (string Skipped, string Activated) Skip()
        {
            var activeTask = GetActiveTask();
            if (activeTask == null)
            {
                throw new TaskNotFoundException();
            }
            
            var nextTask = _taskStorage.FindFirstAvailableSince(activeTask.CreatedOn);
            activeTask.Skip();

            if (nextTask != null)
            {
                nextTask.Activate();
            }

            return (activeTask.Name, nextTask?.Name);
        }
    }
}
