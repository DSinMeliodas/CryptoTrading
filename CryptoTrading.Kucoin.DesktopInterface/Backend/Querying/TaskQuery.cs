using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;

internal class TaskQuery<T>
{
    private readonly IEnumerable<Task<T>> m_Tasks;

    private TaskQuery(IEnumerable<Task<T>> tasks)
    {
        m_Tasks = tasks;
    }

    public TaskContinueQuery<T> ContinueWith()
    {
        return TaskContinueQuery<T>.ForAll(m_Tasks);
    }

    public static TaskQuery<T> ForAll(IEnumerable<Task<T>> tasks)
    {
        return new TaskQuery<T>(tasks);
    }
}
