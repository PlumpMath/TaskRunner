using System;
using System.Collections;
using UnityEngine;

namespace Morfel.TaskR
{
    public class TaskRunner : ITaskRunner, ITaskHost
    {
        private readonly MonoBehaviour _host;

        private bool _paused;
        private bool _disposed;

        private TaskRunner(MonoBehaviour host)
        {
            _host = host;
        }

        public static ITaskRunner Create(MonoBehaviour host)
        {
            return new TaskRunner(host);
        }

        public ITask Run(IEnumerator enumerator, bool start)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Cannot use disposed TaskRunner"
                    );
            }
            var task = new Task(
                  host       : this
                , enumerator : enumerator
                );

            if (start == true)
            {
                task.Start();
            }

            return task;
        }

        public void Pause()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Cannot use disposed TaskRunner"
                    );
            }
            _paused = true;
        }

        public void Resume()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Cannot use disposed TaskRunner"
                    );
            }
            _paused = false;
        }

        public void Start(ITaskInternal task)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Cannot use disposed TaskRunner"
                    );
            }
            _host.StartCoroutine(WrapTask(task.GetEnumerator()));
        }

        private IEnumerator WrapTask(IEnumerator enumerator)
        {
            while (enumerator.MoveNext() && _disposed == false)
            {
                while (_paused)
                {
                    yield return null;
                }
                yield return enumerator.Current;
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
