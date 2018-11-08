using System.Threading.Tasks;

namespace PartialTrustInstaller.Utils
{
    internal static class TaskExtensions
    {
        public static TaskAwaiter GetAwaiter(this Task task)
        {
            return new TaskAwaiter(task);
        }

        public static TaskAwaiter<T> GetAwaiter<T>(this Task<T> task)
        {
            return new TaskAwaiter<T>(task);
        }
    }
}