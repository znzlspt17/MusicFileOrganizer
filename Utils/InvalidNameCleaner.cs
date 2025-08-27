using System.Collections.Generic;

namespace MusicFileOrganizer.Utils
{
    public static class InvalidNameCleaner
    {
        private static readonly HashSet<char> InvalidChars = ['\\', '/', ':', '*', '?', '"', '<', '>', '', '|'];

        public static string ReplaceInvalidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Unknown";

            char[] chars = name.ToCharArray();
            int length = chars.Length;
            for (int i = 0; i < length; ++i)
            {
                if (InvalidChars.Contains(chars[i]))
                {
                    chars[i] = '_';
                }
            }
            ReplaceTrailingDot(chars);

            return new string(chars);
        }

        private static void ReplaceTrailingDot(char[] input)
        {
            if (input[^1] == '.')
            {
                input[^1] = '-';
            }
        }
    }
}
