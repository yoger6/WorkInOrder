﻿using System;
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

        public ITask[] ListTasks()
        {
            return _taskStorage.GetAll()
                .OrderBy(x => x.CreatedOn)
                .ToArray();
        }

        /// <summary>
        /// Stores a new task, activates it if there's no other.
        /// </summary>
        /// <exception cref="MissingContentException">Task needs content to exist</exception>
        /// <exception cref="TaskAlreadyExistsException">Task content must be unique</exception>
        /// <param name="name">What is the task about</param>
        public void Add(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new MissingContentException();
            }

            if (GetActiveTask() == null)
            {
                _taskStorage.Create(DateTime.Now, name, Status.Current);
            }
            else
            {
                _taskStorage.Create(DateTime.Now, name, Status.Pending);
            }
        }

        /// <summary>
        /// Skips current task, activating next one if there is one in the future.
        /// </summary>
        /// <exception cref="TaskNotFoundException">When there's no active task</exception>
        /// <returns>Name of skipped task and optionally activated one</returns>
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

        /// <summary>
        /// Completes task that's currently active
        /// </summary>
        /// <exception cref="TaskNotFoundException">When there's no active task</exception>
        /// <returns>Name of completed task and optionally activated one</returns>
        public (string Completed, string Activated) Done()
        {
            var activeTask = GetActiveTask();
            if (activeTask == null)
            {
                throw new TaskNotFoundException();
            }

            var nextTask = _taskStorage.FindFirstAvailableSince(activeTask.CreatedOn);
            activeTask.Complete();

            if (nextTask != null)
            {
                nextTask.Activate();
            }

            return (activeTask.Name, nextTask?.Name);
        }

        /// <summary>
        /// Activates given task, this can be used only when no other task is active.
        /// For that use Switch method.
        /// </summary>
        /// <exception cref="TaskNotFoundException">No task with given name</exception>
        /// <exception cref="TaskAlreadyActiveException">Task is already active</exception>
        /// <param name="name">Name of task to activate</param>
        public void Activate(string name)
        {
            var task = _taskStorage.Find(name);
            var activeTask = _taskStorage.Find(Status.Current);

            if (task == null)
            {
                throw new TaskNotFoundException();
            }

            if (task.Equals(activeTask))
            {
                throw new TaskAlreadyActiveException();
            }
            else if (activeTask != null)
            {
                throw new AnotherTaskAlreadyActiveException(activeTask.Name);
            }

            task.Activate();
        }

        private ITask GetActiveTask()
        {
            return _taskStorage.Find(Status.Current);
        }
    }
}
