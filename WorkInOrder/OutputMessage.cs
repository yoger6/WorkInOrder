using System.Collections.Generic;
using System.Linq;

namespace WorkInOrder
{
    public class OutputMessage
    {
        public static OutputMessage[] Positive(params string[] contents)
        {
            return contents.Select(x => new OutputMessage(x, Format.Positive)).ToArray();
        }

        public static OutputMessage[] Neutral(params string[] contents)
        {
            return contents.Select(x => new OutputMessage(x, Format.Neutral)).ToArray();
        }

        public static OutputMessage[] Negative(params string[] contents)
        {
            return contents.Select(x => new OutputMessage(x, Format.Negative)).ToArray();
        }

        public static OutputMessage[] NegativeFormat(string message, params object[] parameters)
        {
            return FormatMessage(message, parameters, Format.Negative);
        }

        public static OutputMessage[] NeutralFormat(string message, params object[] parameters)
        {
            return FormatMessage(message, parameters, Format.Neutral);
        }

        private static OutputMessage[] FormatMessage(string message, object[] parameters, Format format)
        {
            return new[] { new OutputMessage(string.Format(message, parameters), format) };
        }


        public OutputMessage(string content, Format format)
        {
            Content = content;
            Format = format;
        }

        public string Content { get; }
        public Format Format {get;}
    }
}