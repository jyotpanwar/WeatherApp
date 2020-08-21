using System;
using System.Linq;

namespace Core.Models
{
    public static class StringExtensions
    {
        public static string FirstCharUpper(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
            {
                return text.First().ToString().ToUpper() + text.Substring(1);
            }
        }
    }
}