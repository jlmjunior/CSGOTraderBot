using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Helper
{
    public static class Log
    {
        public const string dir = @"Logs/error.txt";

        public static void SaveError(string message)
        {
            using (StreamWriter text = new StreamWriter(dir))
            {
                text.WriteLine($"[{DateTime.Now}] {message}\n");
            }
        }
    }
}
