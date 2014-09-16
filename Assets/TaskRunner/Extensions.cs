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

        public static ITask While(
              this ITaskRunner runner
            , Func<bool> condition
            , Action action
            , Action then=null
            , bool start = true
            )
        {
            return runner.Run(_While(condition, action, then), start);
        }

        private static IEnumerator _While(
              Func<bool> condition
            , Action action
            , Action then=null
            )
        {
            while (condition())
            {
                action();

                yield return null;
            }

            if (then != null)
            {
                then();
            }
        }
    }
}
