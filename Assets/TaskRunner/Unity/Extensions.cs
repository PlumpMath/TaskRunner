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
    }
}
