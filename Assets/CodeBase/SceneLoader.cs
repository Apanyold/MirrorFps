using UnityEngine.SceneManagement;
using RSG;
using System;

namespace CodeBase
{
    public class SceneLoader
    {
        public static IPromise LoadScene(string sceneName)
        {
            var promise = new Promise();
            try
            {
                SceneManager.LoadSceneAsync(sceneName).completed += (operation) =>
                {
                    if (operation.isDone)
                        promise.Resolve();
                    else
                        promise.Reject(new Exception("Error while loading scene"));
                };
            }
            catch (Exception ex)
            {
                var msg = $"Error while loading scene with name: {sceneName} \n" + ex.Message;
                promise.Reject(new Exception(msg));
            }

            return promise;
        }
    }
}