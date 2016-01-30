using System;
using System.Collections.Generic;
using System.Linq;


namespace CB.Xaml.Commands
{
    public class TextProcessor
    {
        public static string TrimAllLines(string text, TrimmingPosition trimmingPosition)
        {
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var trimmedLines = TrimLines(lines, trimmingPosition);
            return string.Join(Environment.NewLine, trimmedLines);
        }

        public static IEnumerable<string> TrimLines(IEnumerable<string> lines, TrimmingPosition trimmingPosition)
        {
            IEnumerable<string> trimmedLines;

            switch (trimmingPosition)
            {
                case TrimmingPosition.TrimEnd:
                    trimmedLines = lines.Select(line => line.TrimEnd());
                    break;

                case TrimmingPosition.TrimStart:
                    trimmedLines = lines.Select(line => line.TrimStart());
                    break;

                default:
                    trimmedLines = lines.Select(line => line.Trim());
                    break;
            }

            return trimmedLines.Where(line => !string.IsNullOrWhiteSpace(line));
        }
    }
}