using System;
using System.IO;
using WorkInOrder.Commands;

namespace WorkInOrder.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userStoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WorkInOrder", "WorkInOrder.sqlite");
            var storage = new TaskStorage($"DataSource={userStoragePath}");
            var factory = new CommandFactory(storage);
            while (true)
            {
                var defaultColor = System.Console.ForegroundColor;
                var command = factory.Identify(System.Console.ReadLine());
                System.Console.Clear();

                foreach (var outputMessage in command.Run())
                {
                    var color = GetColor(outputMessage.Format, defaultColor);
                    System.Console.ForegroundColor = color;
                    System.Console.WriteLine(outputMessage.Content);
                    System.Console.ForegroundColor = defaultColor;
                }
            }
        }

        private static ConsoleColor GetColor(Format format, ConsoleColor defaultColor)
        {
            switch (format)
            {
                default: return defaultColor;
                case Format.Negative: return ConsoleColor.Red;
                case Format.Positive: return ConsoleColor.Green;
                case Format.Highlight: return ConsoleColor.Cyan;
            }
        }
    }
}
