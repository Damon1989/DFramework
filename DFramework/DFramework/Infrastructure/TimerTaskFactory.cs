using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFramework.Infrastructure
{
    public static class TimerTaskFactory
    {
        public static readonly TimeSpan DoNotRepeat = TimeSpan.FromMilliseconds(-1), Infinite = DoNotRepeat;

        public static Task Timeout(this Task task, TimeSpan timeout)
        {
            if (task.IsCompleted || timeout == Infinite)
            {
                return task;
            }

            var tcs = new TaskCompletionSource<object>();

            if (timeout == TimeSpan.Zero)
            {
                tcs.SetException(new TimeoutException());
                return tcs.Task;
            }

            Timer timer = null;
            timer = new Timer(_ =>
              {
                  timer.Dispose();
                  tcs.TrySetException(new TimeoutException());
              }, null, timeout, DoNotRepeat);

            task.ContinueWith(a =>
            {
                timer.Dispose();
                MarshalTaskResults(a, tcs);
            });

            return tcs.Task;
        }

        public static Task<TResult> Timeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            if (task.IsCompleted || timeout == Infinite)
            {
                return task;
            }

            var tcs = new TaskCompletionSource<TResult>();

            if (timeout == TimeSpan.Zero)
            {
                tcs.SetException(new TimeoutException());
                return tcs.Task;
            }

            Timer timer = null;
            timer = new Timer(_ =>
            {
                timer.Dispose();
                tcs.TrySetException(new TimeoutException());
            }, null, timeout, DoNotRepeat);

            task.ContinueWith(antecedent =>
            {
                timer.Dispose();
                MarshalTaskResults(antecedent, tcs);
            });

            return tcs.Task;
        }

        public static Task<T2> ContinueWith<T1, T2>(this Task<T1> antecedentTask,
    Func<Task<T1>, T2> continuationFunc,
    TimeSpan timeout)
        {
            if (timeout == Infinite)
                return antecedentTask.ContinueWith(t => continuationFunc(t));

            TaskCompletionSource<T2> taskCompletionSource = null;
            Timer timer = null;
            timer = new Timer(_ =>
            {
                timer.Dispose();

                // Fault our proxy Task with a TimeoutException
                taskCompletionSource.TrySetException(new TimeoutException());
            });
            taskCompletionSource = new TaskCompletionSource<T2>(timer);

            antecedentTask.ContinueWith(t =>
            {
                timer.Change(timeout, DoNotRepeat);
                return continuationFunc(t);
            })
                .ContinueWith(t =>
                {
                    timer.Dispose();
                    MarshalTaskResults(t, taskCompletionSource);
                });
            return taskCompletionSource.Task;
        }

        public static Task<T2> ContinueWith<T1, T2>(this Task<T1> antecedentTask,
            Func<Task<T1>, T2> continuationFunc,
            Func<Task<T1>, T2, bool> predicate,
            TimeSpan pollInterval,
            TimeSpan timeout)
        {
            Timer timer = null;
            TaskCompletionSource<T2> taskCompletionSource = null;
            var expirationTime = DateTime.MinValue;

            timer =
                new Timer(_ =>
                {
                    try
                    {
                        if (DateTime.UtcNow > expirationTime)
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(default(T2));
                            return;
                        }

                        var result = continuationFunc(antecedentTask);

                        if (predicate(antecedentTask, result))
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(result);
                        }
                        else
                        {
                            // try again
                            timer.Change(pollInterval, DoNotRepeat);
                        }
                    }
                    catch (Exception e)
                    {
                        timer.Dispose();
                        taskCompletionSource.SetException(e);
                    }
                });

            taskCompletionSource = new TaskCompletionSource<T2>(timer);

            antecedentTask.ContinueWith(t =>
            {
                expirationTime = DateTime.UtcNow.Add(timeout);
                timer.Change(pollInterval, DoNotRepeat);
            });

            return taskCompletionSource.Task;
        }

        internal static void MarshalTaskResults<TResult>(Task source, TaskCompletionSource<TResult> proxy)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    proxy.TrySetException(source.Exception);
                    break;

                case TaskStatus.Canceled:
                    proxy.TrySetCanceled();
                    break;

                case TaskStatus.RanToCompletion:
                    var castedSource = source as Task<TResult>;
                    proxy.TrySetResult(
                        castedSource == null
                            ? default(TResult)
                            : // source is a Task
                            castedSource.Result); // source is a Task<TResult>
                    break;
            }
        }
    }
}