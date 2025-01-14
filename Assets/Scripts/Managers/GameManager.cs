using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private Player.PlayerMovement _playerMovement;
    
    private bool _isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _isGameOver = false;
        
    }
    

    public void GameOver()
    {
        if(_isGameOver)
            return;
        _isGameOver = true;
    }

    public void RestartGame()
    {
        SceneHandler.Instance.ReloadScene();
    }

    public void LoadScene(string sceneName)
    {
      //  SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
