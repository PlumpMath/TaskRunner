using System;
using System.Collections;

namespace Morfel.TaskR
{
    public interface ITaskRunner : IDisposable
    {
        ITask Run(IEnumerator enumerator, bool start=true);

        void Pause ();
        void Resume();
    }

    public interface ITaskHost : ITaskRunner
    {
        void Start(ITaskInternal start);
    }
}