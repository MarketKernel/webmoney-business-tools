using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PartialTrustInstaller.Utils
{
    internal class TaskAwaiter<T> : INotifyCompletion
    {
        private readonly Task<T> _task;

        public bool IsCompleted => _task.IsCompleted;

        public TaskAwaiter(Task<T> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public void OnCompleted(Action continuation)
        {
            if (null == continuation)
                throw new ArgumentNullException(nameof(continuation));

            var synchronizationContext = SynchronizationContext.Current;

            _task.ContinueWith(t =>
            {
                if (null != synchronizationContext)
                    synchronizationContext.Post(state => { continuation(); }, null);
                else continuation();
            });
        }

        public T GetResult()
        {
            if (_task.IsCanceled)
                throw new TaskCanceledException();

            if (_task.IsFaulted)
            {
                var taskException = _task.Exception;
                var innerException = taskException?.InnerException;

                if (null != innerException)
                    throw innerException;

                if (null == taskException)
                    throw new InvalidOperationException("null == taskException");

                throw taskException;
            }

            return _task.Result;
        }
    }
}
