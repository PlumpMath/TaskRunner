using System;
using System.Collections;
using UnityEngine;

namespace Morfel.TaskR
{
    public static class Extensions
    {
        public static ITask Run(
              this ITaskRunner runner
            , ref ITask task
            , IEnumerator enumerator
            )
        {
            if (task != null && task.IsRunning())
            {
                task.Cancel();
            }
            task = runner.Run(enumerator);

            return task;
        }

        public static ITask Forever(
              this ITaskRunner runner
            , Action action
            , bool start=true
            )
        {
            return runner.Run(_Forever(action), start);
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

        public static ITask Schedule(
              this ITaskRunner runner
            , float t
            , Action action
            )
        {
            return runner.Run(
                _Schedule(t, action: action)
                );
        }

        public static ITask When(
              this ITaskRunner runner
            , Func<bool> condition
            , Action action
            )
        {
            return runner.Run(
                _When(condition, action: action)
                );
        }

        # region Implementations

        private static IEnumerator _When(
              Func<bool> condition
            , Action action
            )
        {
            while (condition() == false)
            {
                yield return null;
            }

            action();
        }

        private static IEnumerator _Forever(Action action)
        {
            while (true)
            {
                action();

                yield return null;
            }
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

        private static IEnumerator _Schedule(
              float t
            , Action action
            )
        {
            var x = 0f;

            while (x < t)
            {
                x += Time.deltaTime;

                yield return null;
            }

            action();
        }

        # endregion
    }
}
