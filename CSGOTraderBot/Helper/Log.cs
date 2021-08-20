using System;
using System.IO;

namespace CSGOTraderBot.Helper
{
    public static class Log
    {
        public const string dir = @"Logs/error.txt";

        public static void SaveError(string message)
        {
            using (StreamWriter text = new StreamWriter(dir, append: true))
            {
                text.WriteLine($"[{DateTime.Now}] {message}\n");
            }
        }
    }
}
