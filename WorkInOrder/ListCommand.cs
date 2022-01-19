using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkInOrder
{
    public class ListCommand : ICommand
    {
        private readonly ITaskStorage _storage;

        public ListCommand(ITaskStorage storage)
        {
            _storage = storage;
        }

        public OutputMessage[] Run()
        {
            var tasks = _storage.GetTasks(DateTime.Today).OrderBy(x=>x.CreatedOn);

            return FormatTasks(tasks).ToArray();
        }

        private static IEnumerable<OutputMessage> FormatTasks(IEnumerable<Task> tasks)
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
                yield return
                    new OutputMessage(
                        $"{prefixFactory.Invoke(task.Status)} {task.Name}",
                        formatFactory.Invoke(task.Status));
            }
        }
    }
}