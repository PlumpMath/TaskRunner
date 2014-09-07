using System.Collections;
using TaskRunner.Unity;
using UnityEngine;

namespace TaskRunner.Examples
{
    public class TaskRunnerExample : MonoBehaviour
    {
        private int   _n = 0;
        private float _runningForSeconds = 0;

        private void Awake()
        {
            ITaskRunner runner1 = new Unity.TaskRunner(this);
            ITaskRunner runner2 = new Unity.TaskRunner(this);

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
