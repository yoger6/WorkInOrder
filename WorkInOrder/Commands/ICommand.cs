namespace WorkInOrder.Commands
{
    public interface ICommand
    {
        OutputMessage[] Run();
    }
}