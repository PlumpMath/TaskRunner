using System.Collections;
using UnityEngine;

namespace TaskRunner.Unity
{
    public class TaskRunner : ITaskRunner, ITaskHost
    {
        private readonly MonoBehaviour _host;

        private bool _paused;

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
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public void Start(ITaskInternal task)
        {
            _host.StartCoroutine(WrapTask(task.GetEnumerator()));
        }

        private IEnumerator WrapTask(IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                while (_paused)
                {
                    yield return null;
                }
                yield return enumerator.Current;
            }
        }
    }
}
