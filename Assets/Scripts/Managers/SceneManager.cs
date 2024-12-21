using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }

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

        // Load a scene by name
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

        // Reload the current scene
        public void ReloadScene()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            LoadScene(currentSceneName);
        }

        // Load the next scene in the build settings
        public void LoadNextScene()
        {
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                LoadScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(nextSceneIndex).name);
            }
            else
            {
                Debug.LogWarning("No more scenes to load!");
            }
        }

        // Load a scene asynchronously
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                Debug.Log($"Loading {sceneName}: {operation.progress * 100}%");
                yield return null;
            }

            Debug.Log($"Scene {sceneName} loaded successfully!");
        }

        // Quit the game (only works in built applications)
        public void QuitGame()
        {
            Debug.Log("Quitting the game...");
            Application.Quit();
        }
    }
}
