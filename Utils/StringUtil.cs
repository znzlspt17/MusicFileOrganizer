using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MusicFileOrganizer.Utils
{
    class StringUtil
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();
        private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();
        private static readonly char InvalidTail = '.';
        private static readonly Stack<int> invalidCharIndices = new Stack<int>();
        private const string Default = "Unknown";

        public static string CreateFolderTreePreview(Dictionary<string, List<string>> pairs)
        {
            stringBuilder.Clear();
            foreach (var left in pairs)
            {
                stringBuilder.AppendLine($"{left.Key}:");

                foreach (var right in left.Value)
                {
                    stringBuilder.AppendLine($"  - {right}");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
        public static string InvalidDirCharCleaner(char[] original)
        {
            var chars = InvalidCharCleaner(original);

            return string.Intern(new string(chars).Trim());
        }

        private static char[] InvalidCharCleaner(char[] original)
        {
            int tail = original.Length - 1;

            for (int i = 0; i < original.Length; i++)
            {
                if (Array.IndexOf(InvalidChars, original[i]) >= 0)
                {
                    invalidCharIndices.Push(i);
                }
            }
            if (TailDotChecker(original[tail]))
            {
                invalidCharIndices.Push(tail);
            }
            InvalidCharReplacer(invalidCharIndices, original);
            invalidCharIndices.Clear();

            return original;
        }
        private static bool TailDotChecker(char tail)
        {
            if (InvalidTail == tail)
            {
                return true;
            }

            return false;
        }
        private static void InvalidCharReplacer(Stack<int> indices, char[] original)
        {
            Span<char> invalidChar = original;
            foreach (int i in indices)
            {
                invalidChar[i] = '_';
            }
        }

        public static string IsNullOrEmpty(string? str)
        {
            return string.IsNullOrEmpty(str) ? Default : string.Intern(str);
        }

    }
}

