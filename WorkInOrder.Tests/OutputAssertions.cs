using Xunit;

namespace WorkInOrder.Tests
{
    internal static class OutputAssertions
    {
        public static void Expect(this OutputMessage message, string content, Format format)
        {
            Assert.Equal(content, message.Content);
            Assert.Equal(format, message.Format);
        }
    }
}