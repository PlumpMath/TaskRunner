using System.Collections;
using UnityEngine;

namespace Morfel.TaskR.TaskR.Examples
{
    public class TaskRunnerExample : MonoBehaviour
    {
        private int   _n = 0;
        private float _runningForSeconds = 0;

        private void Awake()
        {
            var runner1 = TaskRunner.Create(this);

            var runner2 = TaskRunner.Create(this);

            var recursiveTask = runner1.Run(
                  RecursiveIncrement()
                , start: true
                );

            var updateTask   = runner1.Forever(CustomUpdate);

            var scheduleTask = runner1.Schedule(2f, updateTask.Cancel);

            updateTask.Finished += cancelled =>
            {
                UnityEngine.Debug.Log(
                    "Update has finished. Was it cancelled? - " + cancelled
                    );
            };


            runner1.Schedule(1f, runner1.Pause);

            runner2.Schedule(3f, runner1.Resume);
        }

        /// <summary>
        /// This function will be called forever.
        /// </summary>
        private void CustomUpdate()
        {
            _runningForSeconds += Time.deltaTime;
            UnityEngine.Debug.Log(
                "Still Updating after " + _runningForSeconds + " seconds."
                );
        }

        /// <summary>
        /// Recursive evaluation of a task.
        /// Use with care, as there is no tail call optimization in place.
        /// </summary>
        private IEnumerator RecursiveIncrement()
        {
            var n = _n;

            while (_n < 30)
            {
                _n += 1;

                UnityEngine.Debug.Log("n: " + _n + " / 100");

                UnityEngine.Debug.Log("Before recursive call");

                yield return RecursiveIncrement();

                UnityEngine.Debug.Log("After recursive call (" + n + ")");
            }
        }
    }
}
