using System.Collections;

namespace TaskRunner
{
    public delegate void FinishedH(bool cancelled);

    public interface ITask
    {
        void Start  ();
        void Cancel ();
        void Pause  ();
        void Resume ();

        bool IsRunning();

        event FinishedH Finished;
    }

    public interface ITaskInternal : ITask
    {
        IEnumerator GetEnumerator();
    }
}
