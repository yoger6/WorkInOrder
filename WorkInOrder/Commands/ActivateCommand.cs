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
                    return OutputMessage.NeutralFormat(Messages.TaskActivated, _taskName);
                }

                _board.Activate(_taskName);
                return OutputMessage.NeutralFormat(Messages.TaskActivated, _taskName);
            }
            catch (TaskNotFoundException)
            {
                return OutputMessage.NegativeFormat(Messages.TaskNotFound, _taskName);
            }
            catch (TaskAlreadyActiveException)
            {
                return OutputMessage.NegativeFormat(Messages.TaskAlreadyActive, _taskName);
            }
            catch (AnotherTaskAlreadyActiveException e)
            {
                return OutputMessage.NegativeFormat(Messages.AnotherTaskAlreadyActive, _taskName, e.TaskName);
            }
            catch (NonUniqueNameException e)
            {
                var information = OutputMessage.NeutralFormat(Messages.MoreThanOneTaskFound, _taskName);
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