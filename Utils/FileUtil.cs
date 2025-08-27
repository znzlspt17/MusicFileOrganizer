using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Utils
{
    public class FileUtil
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();

        public static IEnumerable<string> LoadFileNames(string sourcePath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
            IEnumerable<string> names = dirInfo
                .EnumerateFiles("*.*", SearchOption.AllDirectories)
                .Select(fi => fi.Name)
                .ToList();
            return names;
        }

        //파일 옮김
        public static async Task CopyFileAsync(DirectoryInfo directoryInfo, string srcPath, string fileName)
        {
            string originalPath = Path.Combine(srcPath, fileName);
            string targetPath = Path.Combine(directoryInfo.FullName, fileName);
            targetPath = AddSuffixToFileNameAuto(targetPath);
            await Task.Run(() => File.Copy(originalPath, targetPath, overwrite: false));
        }

        // 중복시 숫자 서픽스 삽입
        private static string AddSuffixToFileNameAuto(string targetPath)
        {
            if (File.Exists(targetPath))
            {
                ReadOnlySpan<char> pathSpan = targetPath.AsSpan();
                int lastSlash = pathSpan.LastIndexOf('\\');
                int lastDot = pathSpan.LastIndexOf('.');

                ReadOnlySpan<char> dirSpan = pathSpan.Slice(0, lastSlash);
                ReadOnlySpan<char> nameSpan = pathSpan.Slice(lastSlash + 1, lastDot - lastSlash - 1);
                ReadOnlySpan<char> extSpan = pathSpan.Slice(lastDot); // includes dot

                int suffix = 1;

                Span<char> suffixBuffer = stackalloc char[11]; // int 최대 길이
                Span<char> fileNameBuffer = stackalloc char[nameSpan.Length + 2 + 10 + 1 + extSpan.Length];
                // name + " (" + suffix + ")" + ext (suffix 최대 10자리)

                while (File.Exists(targetPath))
                {
                    // suffix → 문자열로 변환
                    suffixBuffer.Clear();
                    suffix.TryFormat(suffixBuffer, out int suffixLength);

                    // 파일 이름 버퍼 채우기
                    nameSpan.CopyTo(fileNameBuffer);
                    int pos = nameSpan.Length;

                    fileNameBuffer[pos++] = ' ';
                    fileNameBuffer[pos++] = '(';
                    suffixBuffer.Slice(0, suffixLength).CopyTo(fileNameBuffer.Slice(pos));
                    pos += suffixLength;
                    fileNameBuffer[pos++] = ')';
                    extSpan.CopyTo(fileNameBuffer.Slice(pos));

                    string fileName = new string(fileNameBuffer.Slice(0, pos + extSpan.Length));

                    // ✅ dirSpan을 string으로 변환해서 Path.Combine 사용
                    targetPath = Path.Combine(dirSpan.ToString(), fileName);

                    suffix++;
                }

            }
            return targetPath;
        }
    }
}
