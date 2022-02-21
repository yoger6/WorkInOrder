using System;
using System.Collections.Generic;
using System.Linq;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    public class ListCommand : ICommand
    {
        private readonly ITaskBoard _board;
        private readonly string _parameter;

        public ListCommand(ITaskBoard board, string parameter)
        {
            _board = board;
            _parameter = parameter;
        }

        public OutputMessage[] Run()
        {
            var tasks = _board.ListTasks();

            if (!tasks.Any())
            {
                return OutputMessage.Neutral("Nothing to display");
            }

            return FormatTasks(tasks).ToArray();
        }

        private IEnumerable<OutputMessage> FormatTasks(IEnumerable<Task> tasks)
        {
            var prefixFactory = new Func<Status, string>(
                s =>
                {
                    switch (s)
                    {
                        case Status.Done:
                            return "+";
                        case Status.Skipped:
                            return "-";
                        case Status.Current:
                            return "@";
                        default:
                            return "?";
                    }
                });

            var formatFactory = new Func<Status, Format>(
                s =>
                {
                    switch (s)
                    {
                        case Status.Done:
                            return Format.Positive;
                        case Status.Skipped:
                            return Format.Negative;
                        case Status.Current:
                            return Format.Highlight;
                        default:
                            return Format.Neutral;
                    }
                });

            foreach (var task in tasks)
            {
                var message = $"{prefixFactory.Invoke(task.Status)} {task.Name}";
                if (_parameter.ToLower().Contains("d"))
                {
                    message += $" {task.CreatedOn.ToOutputFormat()}";

                    if (task.IsCompleted)
                    {
                        message += $"-{task.CompletedOn.Value.ToOutputFormat()}";
                    }
                }

                yield return
                    new OutputMessage(
                        message,
                        formatFactory.Invoke(task.Status));
            }
        }
    }
}