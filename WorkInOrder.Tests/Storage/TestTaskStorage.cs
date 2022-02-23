namespace WorkInOrder.Tests.Storage
{
    internal class TestTaskStorage : TaskStorage
    {
        public TestTaskStorage() 
            : base("DataSource=WorkInOrder.sqlite")
        {
        }
    }
}