using System;
using System.Collections.Generic;
using System.Linq;
using WorkInOrder.BusinessLogic;

namespace WorkInOrder.Commands
{
    internal class ActivateCommand : ICommand
    {
        private readonly ITaskBoard _board;
        private readonly string _taskName;
        private readonly Session _session;

        public ActivateCommand(ITaskBoard board, string taskName, Session session)
        {
            _board = board;
            _taskName = taskName;
            _session = session;
        }

        public IList<OutputMessage> Run()
        {
            try
            {
                if (_session.SearchResults.Any())
                {
                    var narrowedDownResult = _session.SearchResults.FirstOrDefault(x => x.Key.Equals(_taskName, StringComparison.InvariantCultureIgnoreCase));
                    _session.SearchResults.Clear();
                    _board.Activate(narrowedDownResult.Value);
                    return OutputMessage.Neutral($"{_taskName} is now active");
                }

                _board.Activate(_taskName);
                return OutputMessage.Neutral($"{_taskName} is now active");
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.Negative($"{_taskName} not found");
            }
            catch (TaskAlreadyActiveException)
            {
                return OutputMessage.Negative($"{_taskName} is already active");
            }
            catch (AnotherTaskAlreadyActiveException e)
            {
                return OutputMessage.Negative($"Cannot activate {_taskName} as there's another active task: {e.TaskName}. To switch active task use the switch command.");
            }
            catch (NonUniqueNameException e)
            {
                var information = OutputMessage.Neutral($"More than one task found when looking for {_taskName}, you can pick one from list below using its number or more specific name:");
                for (var i = 0; i < e.TasksFound.Length; i++)
                {
                    _session.SearchResults.Add($"{i + 1}", e.TasksFound[i]);
                }

                var tasks = _session.SearchResults.SelectMany(x => OutputMessage.Neutral($"{x.Key}. {x.Value}"));

                return information.Concat(tasks).ToList();
            }
        }
    }
}