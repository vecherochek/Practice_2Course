using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace problem_2
{
    class Program
    {
        public enum severity
        {
            trace,
            debug,
            information,
            warning,
            error,
            critical
        }
        public class Logger : IDisposable
        {
            private readonly StreamWriter writer;

            public Logger(string path)
            {
                writer = new StreamWriter(path);
            }

            public void Log(severity _severity, string data)
            {
                writer.WriteLine($"[{DateTime.Now}] [{_severity}]: {data}");
            }

            public void Dispose()
            {
                writer.Dispose();
            }
        }
        static void Main(string[] args)
        {
            using (var logger = new Logger(Path.Combine(Environment.CurrentDirectory, "logfile.txt")))
            for (severity i = severity.trace; i <= severity.critical; ++i)
            {
                Console.WriteLine($"Enter a message line for '{i}': ");
                logger.Log(i, Console.ReadLine());
            }
            Console.WriteLine($"The path to the logfile: {Path.Combine(Environment.CurrentDirectory, "logfile.txt")}");
        }       
    }
}
