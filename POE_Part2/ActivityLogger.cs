using System;
using System.Collections.Generic;
using System.Linq;

namespace POE_Part2
{
    public static class ActivityLogger
    {
        private static List<string> _log = new List<string>();

        public static void Log(string action)
        {
            string timestamp = DateTime.Now.ToString("[HH:mm]");
            _log.Add($"{timestamp} {action}");

            // Keep only last 100 entries to save memory
            if (_log.Count > 100)
                _log.RemoveAt(0);
        }

        public static string GetRecentLog(int count = 10)
        {
            if (_log.Count == 0)
                return "No actions logged yet.";

            var recent = _log.Skip(Math.Max(0, _log.Count - count)).ToList();
            string result = "Here's a summary of recent actions:\n";

            for (int i = 0; i < recent.Count; i++)
            {
                result += $"{i + 1}. {recent[i]}\n";
            }

            if (_log.Count > count)
                result += $"\n(Showing {count} of {_log.Count} entries. Type 'show more' for full log.)";

            return result;
        }

        public static string GetFullLog()
        {
            if (_log.Count == 0)
                return "No actions logged yet.";

            string result = "Complete Activity Log:\n";
            for (int i = 0; i < _log.Count; i++)
            {
                result += $"{i + 1}. {_log[i]}\n";
            }
            return result;
        }
    }
}