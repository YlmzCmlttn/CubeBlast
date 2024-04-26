using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplash : MonoBehaviour
{
    private GameData gameData;
    private Board board;

    private void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }
    public void WinOK()
    {
        if (gameData != null)
        {
            
            gameData.saveData.currentLevel += 1;
            if (gameData.saveData.currentLevel > board.m_World.levels.Length)
            {
                gameData.saveData.finished = true;
            }
            gameData.Save();
        }
        SceneManager.LoadScene("SplashScene");
    }

    public void LoseOK()
    {
        SceneManager.LoadScene("SplashScene");
    }
}
