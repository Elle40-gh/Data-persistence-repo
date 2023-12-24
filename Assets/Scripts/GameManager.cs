using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private string playerName;

    public string PlayerName { get { return playerName; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Don't destroy this object when loading a new scene
        }
        else
        {
            Destroy(this.gameObject); // Destroy this object if another one already exists
        }
    }

    public void UpdatePlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public void StartGame()
    {
        if (playerName == null || playerName == "")
        {
            playerName = "Player";
        }   
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }

    public void Exit()
    {
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
