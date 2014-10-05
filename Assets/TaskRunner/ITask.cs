using System.Collections;

namespace Morfel.TaskR
{
    public delegate void StartedH();
    public delegate void FinishedH(bool cancelled);

    public interface ITask
    {
        void Start  ();
        void Cancel ();
        void Pause  ();
        void Resume ();

        bool IsRunning();

        event StartedH  Started;
        event FinishedH Finished;
    }

    public interface ITaskInternal : ITask
    {
        IEnumerator GetEnumerator();
    }
}
