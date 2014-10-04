using System.Collections;

namespace TaskRunner
{
    public class Task : ITaskInternal
    {
        public event StartedH  Started;
        public event FinishedH Finished;

        private readonly ITaskHost   _host;
        private readonly IEnumerator _enumerator;

        private bool _running;
        private bool _cancelled;
        private bool _paused;

        public Task(
              ITaskHost   host
            , IEnumerator enumerator
            )
        {
            _host       = host;
            _enumerator = enumerator;
        }

        public bool IsRunning()
        {
            return _running;
        }

        public void Start()
        {
            if (_cancelled == false && _running == false)
            {
                _running = true;

                _host.Start(this);

                var startedH = Started;
                if (startedH != null)
                {
                    startedH();
                }
            }
        }

        public void Cancel()
        {
            _cancelled = true;
            _running   = false;
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public IEnumerator GetEnumerator()
        {
            while (_running)
            {
                while (_paused)
                {
                    yield return null;
                }

                if (_enumerator.MoveNext())
                {
                    // ------------------------------
                    // Recursively evaluate this task
                    // ------------------------------
                    if (_enumerator.Current is IEnumerator)
                    {
                        var task = _host.Run(_enumerator.Current as IEnumerator);

                        while (task.IsRunning())
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        yield return _enumerator.Current;
                    }
                }
                else
                {
                    _running = false;
                }
            }

            var finishedH = Finished;
            if (finishedH != null)
            {
                finishedH(_cancelled);
            }
        }
    }
}
