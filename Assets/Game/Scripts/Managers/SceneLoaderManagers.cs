using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Managers
{
    public class SceneLoaderManagers : MonoBehaviour
    {
        public static SceneLoaderManagers Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void LoadScene(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
            else
            {
                Debug.LogError("Scene name is null or empty!");
            }
        }
        
        public void ReloadScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            LoadScene(currentSceneName);
        }
        
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                LoadScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex).name);
            }
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                Debug.Log($"Loading {sceneName}: {operation.progress * 100}%");
                yield return null;
            }

            Debug.Log($"Scene {sceneName} loaded successfully!");
        }
        
    }
}
