using System.Collections.Generic;

namespace WorkInOrder.Commands
{
    public interface ICommand
    {
        IList<OutputMessage> Run();
    }
}