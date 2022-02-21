using System;

namespace WorkInOrder
{
    public static class DateTimeExtensions
    {
        public static string ToOutputFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }
    }
}