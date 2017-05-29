using System;

namespace EasyFlexibilityTool.Web.Infrastructure
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Run(valueFactory)) {}

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap()) {}

        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}