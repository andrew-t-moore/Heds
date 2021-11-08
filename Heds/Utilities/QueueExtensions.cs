using System.Collections.Generic;

namespace Heds.Utilities
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, params T[] items)
        {
            EnqueueRange(queue, (IEnumerable<T>)items);
        }
        
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }
    }
}