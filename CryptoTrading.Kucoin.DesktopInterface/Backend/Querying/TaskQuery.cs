using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Querying
{
    public class TaskQuery<T>
    {
        private readonly IEnumerable<Task<T>> m_Tasks;

        private TaskQuery(IEnumerable<Task<T>> tasks)
        {
            m_Tasks = tasks;
        }

        public void OnCompletedAsync(Action<Task<T>> onCompletion)
        {
            var completingTasks = m_Tasks.Select(t => t.ContinueWith(onCompletion)).ToArray();
            Task.WaitAll(completingTasks);
        }

        public static TaskQuery<T> ForAll(IEnumerable<Task<T>> tasks)
        {
            return new TaskQuery<T>(tasks);
        }
    }
}