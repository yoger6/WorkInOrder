using System;

namespace WorkInOrder
{
    public class NonUniqueNameException : Exception
    {
        public NonUniqueNameException(string name, int itemsFound) : base($"Search for {name} yielded {itemsFound} results, please narrow down the search")
        {
            
        }
    }
}