using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public static class TaskUtil
    {
        internal static Task Canceled => Cached<bool>.CanceledTask;
        public static Task Completed => Cached.CompletedTask;
        public static Task<bool> False => Cached.FalseTask;
        public static Task<bool> True => Cached.TrueTask;

        public static Task<T> Cancelled<T>()
        {
            return Cached<T>.CanceledTask;
        }

        public static void Await(Func<Task> taskFactory, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (taskFactory == null)
                throw new ArgumentNullException(nameof(taskFactory));

            var previousContext = SynchronizationContext.Current;
            try
            {
                var syncContext = new SingleThreadSynchronizationContext(cancellationToken);
                SynchronizationContext.SetSynchronizationContext(syncContext);

                var t = taskFactory();
                if (t == null)
                    throw new InvalidOperationException("The taskFactory must return a Task");

                var awaiter = t.GetAwaiter();

                while (!awaiter.IsCompleted)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException("The task was not completed before being cancelled");

                    syncContext.RunOnCurrentThread(cancellationToken);
                }

                syncContext.SetComplete();

                awaiter.GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        public static void Await(Task task, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var previousContext = SynchronizationContext.Current;
            try
            {
                var syncContext = new SingleThreadSynchronizationContext(cancellationToken);
                SynchronizationContext.SetSynchronizationContext(syncContext);

                var awaiter = task.GetAwaiter();

                while (!awaiter.IsCompleted)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException("The task was not completed before being cancelled");

                    syncContext.RunOnCurrentThread(cancellationToken);
                }

                syncContext.SetComplete();

                awaiter.GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        public static T Await<T>(Func<Task<T>> taskFactory, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (taskFactory == null)
                throw new ArgumentNullException(nameof(taskFactory));

            var previousContext = SynchronizationContext.Current;
            try
            {
                var syncContext = new SingleThreadSynchronizationContext(cancellationToken);
                SynchronizationContext.SetSynchronizationContext(syncContext);

                Task<T> t = taskFactory();
                if (t == null)
                    throw new InvalidOperationException("The taskFactory must return a Task");

                TaskAwaiter<T> awaiter = t.GetAwaiter();

                while (!awaiter.IsCompleted)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException("The task was not completed before being cancelled");

                    syncContext.RunOnCurrentThread(cancellationToken);
                }

                syncContext.SetComplete();

                return awaiter.GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        /// Sets the result of the continuation source and forces the continuations to run on the background threadpool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="result"></param>
        public static void TrySetResultWithBackgroundContinuations<T>(this TaskCompletionSource<T> source, T result)
        {
            Task.Run(() => source.TrySetResult(result));
        }

        /// <summary>
        /// Sets the result of the continuation source and forces the continuations to run on the background threadpool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void TrySetExceptionWithBackgroundContinuations<T>(this TaskCompletionSource<T> source, Exception exception)
        {
            Task.Run(() => source.TrySetException(exception));
        }

        static class Cached
        {
            public static readonly Task CompletedTask = Task.FromResult(true);
            public static readonly Task<bool> TrueTask = Task.FromResult(true);
            public static readonly Task<bool> FalseTask = Task.FromResult(false);
        }


        static class Cached<T>
        {
            public static readonly Task<T> CanceledTask = GetCanceledTask();

            static Task<T> GetCanceledTask()
            {
                var source = new TaskCompletionSource<T>();
                source.SetCanceled();
                return source.Task;
            }
        }


        sealed class SingleThreadSynchronizationContext :
            SynchronizationContext
        {
            readonly CancellationToken _cancellationToken;

            readonly BlockingCollection<Tuple<SendOrPostCallback, object>> _queue;
            bool _completed;

            public SingleThreadSynchronizationContext(CancellationToken cancellationToken)
            {
                _cancellationToken = cancellationToken;
                _queue = new BlockingCollection<Tuple<SendOrPostCallback, object>>();
            }

            public override void Post(SendOrPostCallback callback, object state)
            {
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));

                if (_completed)
                    throw new TaskSchedulerException("The synchronization context was already completed");

                try
                {
                    _queue.Add(Tuple.Create(callback, state), _cancellationToken);
                }
                catch (InvalidOperationException)
                {
                }
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("Synchronously sending is not supported.");
            }

            public void RunOnCurrentThread(CancellationToken cancellationToken)
            {
                Tuple<SendOrPostCallback, object> callback;
                while (_queue.TryTake(out callback, 50, cancellationToken))
                    callback.Item1(callback.Item2);
            }

            public void SetComplete()
            {
                _completed = true;
            }
        }
    }
}
