using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ClassicMvc.Infrastructure.Loggers
{
    public class StreamFileIO
    {
        private static readonly Mutex MutexObj = new Mutex();

        public async Task WriteAsync(string path, string message)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                MutexObj.WaitOne();
                await sw.WriteLineAsync(message);
                MutexObj.ReleaseMutex();
            }
        }
    }
}