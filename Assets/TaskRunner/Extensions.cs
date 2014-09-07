using System;
using System.Collections;

namespace TaskRunner
{
    public static class Extensions
    {
        public static ITask Forever(
              this ITaskRunner runner
            , Action action
            , bool start=true
            )
        {
            return runner.Run(_Forever(action), start);
        }

        private static IEnumerator _Forever(Action action)
        {
            while (true)
            {
                action();

                yield return null;
            }
        }
    }
}
