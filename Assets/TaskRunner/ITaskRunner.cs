using System.Collections;

namespace TaskRunner
{
    public interface ITaskRunner
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