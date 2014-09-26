using System;
using System.Collections;
using UnityEngine;

namespace TaskRunner.Unity
{
    public static class Extensions
    {
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
    }
}
